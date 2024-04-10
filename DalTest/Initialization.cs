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
        ResetData();
        createTasks();
        createDependencies();
        createEngineers();
    }

    private static void ResetData()
    {
        s_dal!.Engineer.Reset();
        s_dal.Task.Reset();
        s_dal.Dependency.Reset();
    }


    private static void createEngineers()
    {   // Variables to store engineer details
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
        // Loop to create 4 engineers
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


        // Loop to create 20 tasks with random details
        for (int i = 0; i < 20; i++)
        {
            string _description = "Task " + (i + 1).ToString();
            string _alias = (i + 1).ToString();

            _level = (TComplexity)s_rand.Next((int)TComplexity.Novice, (int)TComplexity.Expert); /// Randomly select an engineer experience level

            // Remove any null values from the list of engineers
            var nonNullEngineers = myEngineers.Where(e => e != null).ToList();

            // Select a random engineer from the list
            int currentEngineerId = s_rand.Next(0, nonNullEngineers.Count);
            TimeSpan time = new(i + 1, i * 5, i * 45); 
            // Create a new task object and add it to the data store
            DO.Task task = new Task
            {
                Description = _description,
                RequiredEffortTime = time,
                Alias = _alias,
                Complexity = _level,
                EngineerId = currentEngineerId,
            };

            s_dal!.Task.Create(task);//stage 2
        }
    }

    /// Method to create a set of dependencies

    private static void createDependencies()
    {
        int depend = 0;
        int dependon = 0;
        List<Task> tasks = s_dal!.Task.ReadAll().ToList();
        Dependency dep;
        // Create a set of dependencies between tasks
        HashSet<Dependency> dependencies = new HashSet<Dependency>();

        for (int i = 5; i < tasks.Count() * 2; i++)
        {
            switch (i)
            {   // Create dependencies between tasks based on their ID
                case int x when x < 10:
                    dependon = s_rand.Next(0, 5);
                    depend = s_rand.Next(5, 11);
                    dependon = tasks[dependon].Id;
                    depend = tasks[depend].Id;
                    break;
                
                case int x when x < 20:
                    dependon = s_rand.Next(5, 11);
                    depend = s_rand.Next(11, 16);
                    dependon = tasks[dependon].Id;
                    depend = tasks[depend].Id;
                    break;

                case int x when x < 40:
                    dependon = s_rand.Next(11, 16);
                    depend = s_rand.Next(16, 20);
                    dependon = tasks[dependon].Id;
                    depend = tasks[depend].Id;
                    break;

                default:
                    return;

            }
            dep = new Dependency(
                    Id: 0,
                    DependentTask: depend,
                    DependsOnTask: dependon);

            if (depend != dependon)
                s_dal.Dependency.Create(dep);
        }
    }
}