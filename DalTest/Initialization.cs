namespace DalTest;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public static class Initialization
{
    private static IDependency? s_dalDependency; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static ITask? s_dalTask; //stage 1

    private static readonly Random s_rand = new();

    public static void Do(IDependency? _s_dalDependency, IEngineer? _s_dalEngineer, ITask? _s_dalTask)
    {   ///Placing all parameters into access variables
        s_dalDependency = _s_dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = _s_dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = _s_dalTask ?? throw new NullReferenceException("DAL can not be null!");
        createEngineers();
        createTasks();
        createDependencies();
    }

    private static void createEngineers()
    {
        int min_id = 200000000, max_id = 400000000;  /// Define the range of IDs for new engineers
        int _id;
        string _name, _email;
        EngineerExperience _level;
        EngineerExperience[] _levels = new EngineerExperience[3];
        _levels[0] = EngineerExperience.Expert;
        _levels[1] = EngineerExperience.Rookie;
        _levels[2] = EngineerExperience.Junior;

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

                    _id = s_rand.Next(min_id, max_id);
                }
                while (s_dalEngineer?.Read(_id) != null);

               /// Extract details from the tuple
                _name = _details.Item1;
                _email = _details.Item2;
                _level = _levels[s_rand.Next(0, 3)];

                // Create a new engineer object and add it to the data store

                Engineer newEngineer = new( _id, _email, s_rand.Next(2000, 9000),_name,_level);
                s_dalEngineer!.Create(newEngineer);
            }
        }

    }
    
    /// Method to create a set of tasks

    private static void createTasks()
    {
        EngineerExperience _level;/// Variable to store the experience level of an engineer
        EngineerExperience[] _levels = new EngineerExperience[3];/// Array to hold engineer experience levels
        _levels[0] = EngineerExperience.Expert;
        _levels[1] = EngineerExperience.Rookie;
        _levels[2] = EngineerExperience.Junior;

        int _id = 0;/// Variable to store the task ID
        bool _milestone = false;/// Variable indicating whether the task is a milestone
        
        /// Retrieve a list of all engineers

        List<Engineer?> myEngineers = s_dalEngineer!.ReadAll()!;
        int maxEngineer = myEngineers.Count();///Calculate the total number of engineers

        // Loop to create 100 tasks

        for (int i = 0; i < 100; i++)
        {
            string _description = "Task " + (i + 1).ToString();
            string _alias = (i + 1).ToString();
            _level = _levels[s_rand.Next(0, 3)];/// Randomly select an engineer experience level
            int currentEngineerId = myEngineers[s_rand.Next(0, maxEngineer)]!.Id;/// Randomly select a current engineer ID

            //Year _year = (Year)s_rand.Next((int)Year.FirstYear, (int)Year.ExtraYear + 1);

            //    DateTime start = new DateTime(1995, 1, 1);
            //    int range = (DateTime.Today - start).Days;
            //    DateTime _bdt = start.AddDays(s_rand.Next(range));

            DO.Task task = new(_id, _description, _alias, _milestone,/* _createAt*/ DateTime.Today,/* _start=*/null!, /*_forecastDate*/ null!, /*_deadline*/ value3: null!, /*_complete*/ value4: null!, /*_deliverables*/ value5: null!,/*_remarks*/ value6: null!, currentEngineerId: currentEngineerId, level: _level);
            /// Create a new task object and add it to the data store
            DO.Task newTask = task;
            s_dalTask!.Create(newTask);
        }
    }

    /// Method to create a set of dependencies

    private static void createDependencies()

    {   /// Retrieve a list of all tasks
        List<DO.Task?> myTasks = s_dalTask?.ReadAll()!;
        int maxTask = myTasks.Count();///Calculate the total number of tasks
        for (int i = 0; i < 250; i++)
        {
            int _ependentTask = myTasks[s_rand.Next(0, maxTask)]!.Id;///Randomly select a dependent task ID
/// Randomly select a task to depend on

            /// Create a new dependency object and add it to the data store

            Dependency neWDependency = new(0, _ependentTask, myTasks[s_rand.Next(0, maxTask)]!.Id);
            s_dalDependency!.Create(neWDependency);
        }
    }
