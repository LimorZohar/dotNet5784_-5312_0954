namespace DalTest;
using DalApi;
using DO;
using System.Collections.Generic;

public static class Initialization
{

    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();
    /// Define the range of IDs for new engineers
    private const int MIN_ID = 200000000;
    private const int MAX_ID = 400000000;

    //public static void Do(IDependency? _s_dalDependency, IEngineer? _s_dalEngineer, ITask? _s_dalTask)
    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4        createEngineers();
        createTasks();
        createDependencies();
        createEngineers();
    }

    private static void createEngineers()
    {
        int _id;
        string _name, _email;
        Expertise _level;


        // Details for sample engineers
        (string, string)[] EngineersDetails =
        {
        ("John Doe","john.doe@gmail.com"),
        ("Sarah Smith","sarah.smith@gmail.com"),
        ("Michael Johnson","michael.johnson@gmail.com"),
        ("Emily Brown","emily.brown@gmail.com"),
        ("David Wilson","david.wilson@gmail.com"),
        ("Olivia Taylor","olivia.taylor@gmail.com"),
        ("Benjamin Lee","benjamin.lee@gmail.com"),
        ("Emma White","emma.white@gmail.com"),
        ("Limor Zohar","Limor.zohar@gmail.com"),
        ("Hadas Donat","Hadas.Daonat@gmail.com")
        };
        /// Create engineers based on predefined details

        for (int i = 0; i < 4; i++)
        {
            foreach (var _details in EngineersDetails)
            {
                do
                {/// Generate a unique ID within the specified range

                    _id = s_rand.Next(MIN_ID, MAX_ID);
                }
                while (s_dal!.Engineer.Read(_id) is not null);

                /// Extract details from the tuple
                _name = _details.Item1;
                _email = _details.Item2;
                _level = (Expertise)s_rand.Next(0, 4);

                // Create a new engineer object and add it to the data store

                Engineer newEngineer = new(_id, _email, s_rand.Next(2000, 9000), _name, (Expertise)_level);
                //s_dalEngineer!.Create(newEngineer); //stage 1
                s_dal!.Engineer.Create(newEngineer); //stage 2
            }
        }

    }

    /// Method to create a set of tasks

    private static void createTasks()
    {
        TComplexity _level = TComplexity.Novice;/// Variable to store the experience level of an engineer


        // Retrieve a list of all engineers
        IEnumerable<Engineer?> myEngineers = s_dal!.Engineer.ReadAll();


        // Loop to create 100 tasks

        for (int i = 0; i < 100; i++)
        {
            string _description = "Task " + (i + 1).ToString();
            string _alias = (i + 1).ToString();

            _level = (TComplexity)s_rand.Next((int)TComplexity.Novice, (int)TComplexity.Expert); /// Randomly select an engineer experience level

            var nonNullEngineers = myEngineers.Where(e => e != null).ToList();

            int currentEngineerId = s_rand.Next(0, nonNullEngineers.Count);

            DO.Task task = new Task
            {
                Description = _description,
                Alias = _alias,
                Complexity = _level,
                EngineerId = currentEngineerId,
            };

            s_dal!.Task.Create(task);//stage 2
        }
    }

    /// Method to create a set of dependencies

    private static void createDependencies()

    {   /// Retrieve a list of all tasks
        IEnumerable<Task?> myTasks = s_dal!.Task.ReadAll();
        int maxTask = myTasks.Count(), _dependentTask, _DependsOnTask; ;///Calculate the total number of tasks
        for (int i = 0; i < 250; i++)
        {
            //int _ependentTask = myTasks[s_rand.Next(0, maxTask)]!.Id;///Randomly select a dependent task ID
            //                                                         /// Randomly select a task to depend on

            //                                                         /// Create a new dependency object and add it to the data store
            var nonNullTasks = myTasks.Where(e => e != null).ToList();
            _dependentTask = nonNullTasks[s_rand.Next(0, nonNullTasks.Count)]!.Id;
            _DependsOnTask = nonNullTasks[s_rand.Next(0, nonNullTasks.Count)]!.Id;
            Dependency neWDependency = new(0, _dependentTask, _DependsOnTask);
            //s_dalDependency!.Create(neWDependency);//stage 1
            s_dal!.Dependency.Create(neWDependency);//stage 2

        }
    }

}