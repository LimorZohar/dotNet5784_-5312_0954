using Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
namespace DalTest
{
   
    internal class Program
    {
       static readonly IDal s_dal = new DalList(); //stage 2
        //static readonly IDal s_dal = new DalXml(); //stage 3
        // Enums for main menu and submenus

        enum MainMenu { EXIT, DEPENDENCY, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }

        /// Engineer menu for CRUD operations

        private static void EngineerMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new DalDoesNotExistException("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        /// Create engineer

                        Console.WriteLine("Enter id, name, email, cost and a number to choose experience");
                        int idEngineer;
                        string nameEngineer, emailEngineer;
                        Expertise levelEngineer;
                        double costEngineer;

                        /// Input values
                        idEngineer = Console.Read();
                        nameEngineer = (Console.ReadLine()!);
                        emailEngineer = Console.ReadLine()!;
                        costEngineer = double.Parse(Console.ReadLine()!);
                        levelEngineer = (Expertise)Console.Read();

                        /// Map number to EngineerExperience enum
                        /// Create and add new engineer

                        s_dal.Engineer.Create(new Engineer(Id: idEngineer, Name: nameEngineer, Email: emailEngineer,
                            Level: (Expertise)levelEngineer, Cost: costEngineer));

                        break;
                    case 2:
                        // Read engineer

                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine()!);

                        // Check if engineer exists and print details

                        if (s_dal.Engineer!.Read(id) is null)
                            Console.WriteLine("no engineer found");
                        Console.WriteLine(s_dal.Engineer!.Read(id)!.ToString());
                        break;
                    case 3:

                        // Read all engineers

                        foreach (var engineer in s_dal.Engineer!.ReadAll())
                            Console.WriteLine(engineer?.ToString());
                        break;
                    case 4:
                        // Update engineer

                        int idEngineerUpdate, currentNumUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate;
                        //Expertise levelEngineerUpdate;
                        double costEngineerUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dal.Engineer!.Read(idEngineerUpdate)!.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details

                        // Input values for update
                        nameEngineerUpdate = (Console.ReadLine()!);
                        emailEngineerUpdate = Console.ReadLine()!;
                        costEngineerUpdate = double.Parse(Console.ReadLine()!);
                        currentNumUpdate = int.Parse(Console.ReadLine()!);


                        s_dal.Engineer.Update(new Engineer(Id: idEngineerUpdate, Name: nameEngineerUpdate,
                            Email: emailEngineerUpdate, Level: (Expertise)currentNumUpdate, Cost: costEngineerUpdate));
                        break;
                    case 5:

                        // Delete engineer

                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine()!);
                        s_dal.Engineer!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        /// Dependency menu for CRUD operations

        private static void DependencyMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {    // Create dependency

                    case 1:
                        Console.WriteLine("Enter details for all the characteristics");
                        int dependentTask, dependsOnTask;
                        dependentTask = int.Parse(Console.ReadLine()!);
                        dependsOnTask = int.Parse(Console.ReadLine()!);
                        s_dal.Dependency.Create(new Dependency(0, dependentTask, dependsOnTask));
                        break;
                    case 2:
                        // Read dependency

                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine()!);
                        // Check if dependency exists and print details

                        if (s_dal.Dependency!.Read(id) is null)
                            Console.WriteLine("no dependency found");
                        Console.WriteLine(s_dal.Dependency!.Read(id)!.ToString());
                        break;
                    case 3:
                        // Read all dependencies

                        foreach (var dependency in s_dal.Dependency!.ReadAll())
                            Console.WriteLine(dependency!.ToString());
                        break;
                    case 4:
                        // Update dependency

                        int idUpdate, dependentTaskUpdate, dependsOnTaskUpdate;
                        Console.WriteLine("Enter id for reading");
                        idUpdate = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dal.Dependency!.Read(idUpdate)!.ToString());
                        Console.WriteLine("Enter details to update");

                        // Input values for update

                        dependentTaskUpdate = int.Parse(Console.ReadLine()!);
                        dependsOnTaskUpdate = int.Parse(Console.ReadLine()!);

                        // Create and update dependency

                        s_dal.Dependency!.Update(new(idUpdate, dependentTaskUpdate, dependsOnTaskUpdate));

                        break;
                    case 5:

                        // Delete dependency

                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine()!);
                        s_dal.Dependency!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        // Task menu for CRUD operations

        private static void TaskMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {
                    case 1:
                        // Create task
                        Console.WriteLine("Enter Task id:");
                        int Tid = Console.Read();
                        Console.WriteLine("Enter  TaskId");
                        int taskEngineerId = 0;
                        taskEngineerId = int.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter alias");
                        string? taskAlias = Console.ReadLine()!;
                        Console.WriteLine("Enter  description");
                        string? taskDescription = Console.ReadLine()!;
                        Console.WriteLine("Enter deriverables");
                        string? taskDeliverables = Console.ReadLine()!;
                        Console.WriteLine("Enter  remarks");
                        string? taskRemarks = Console.ReadLine()!;
                        Console.WriteLine("Enter milestone");
                        bool taskMilestone;
                        taskMilestone = bool.Parse(Console.ReadLine()!);
                        // Console.WriteLine("Enter dates");
                        Console.WriteLine("Enter  task's num days");
                        int currentTaskNum = 0;
                        currentTaskNum = int.Parse(Console.ReadLine()!);

                        //DateTime taskCreateAt, taskStart, taskForecastDate, taskDeadline, taskComplete;
                        //taskCreateAt = DateTime.Parse(Console.ReadLine()!);
                        //taskStart = DateTime.Parse(Console.ReadLine()!);
                        //taskForecastDate = DateTime.Parse(Console.ReadLine()!);
                        //taskDeadline = DateTime.Parse(Console.ReadLine()!);
                        //taskComplete = DateTime.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter  task's level from 1-3");
                        TComplexity taskLevel;
                        taskLevel = (TComplexity)Console.Read();
                        int num = int.Parse(Console.ReadLine()!);
                        DateTime newT = DateTime.Now;

                        s_dal.Task.Create(new DO.Task(Id: taskEngineerId, Alias: taskAlias, Description: taskDescription, CreatedAtDate: null, RequiredEffortTime: null, taskMilestone, Complexity: taskLevel, StartDate: null, ScheduledDate: null, DeadlineDate: null, CompleteDate: null, Deliverables: taskDeliverables, Remarks: taskRemarks));
                        break;

                    // Read task

                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine()!);
                        if (s_dal.Task!.Read(id) is null)
                            Console.WriteLine("no task found");
                        Console.WriteLine(s_dal.Task!.Read(id)!.ToString());
                        break;
                    // Read all tasks

                    case 3:
                        foreach (var task in s_dal.Task!.ReadAll())
                            Console.WriteLine(task!.ToString());
                        break;

                    // Update task

                    case 4:
                        int idTaskUpdate, currentTaskNumUpdate, taskEngineerIdUpdate;
                        string taskDescriptionUpdate, taskAliasUpdate, taskDeliverablesUpdate, taskRemarksUpdate;
                        bool taskMilestoneUpdate;
                        DateTime taskCreateAtUpdate, taskStartUpdate, taskDeadlineUpdate, taskCompleteUpdate;
                        TComplexity taskLevelUpdate;
                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dal.Task!.Read(idTaskUpdate)!.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskMilestoneUpdate = bool.Parse(Console.ReadLine()!);
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine()!);
                        taskDescriptionUpdate = Console.ReadLine()!;
                        taskAliasUpdate = Console.ReadLine()!;
                        taskDeliverablesUpdate = Console.ReadLine()!;
                        taskRemarksUpdate = Console.ReadLine()!;
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskStartUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine()!);
                        currentTaskNumUpdate = int.Parse(Console.ReadLine()!);
                        taskLevelUpdate = (TComplexity)currentTaskNumUpdate;

                        s_dal.Task.Update(
                            new
                            (Id: idTaskUpdate,
                            Description: taskDescriptionUpdate,
                            Alias: taskAliasUpdate,
                            IsMilestone: taskMilestoneUpdate,
                            CreatedAtDate: taskCreateAtUpdate,
                            StartDate: taskStartUpdate,
                            DeadlineDate: taskDeadlineUpdate,
                            CompleteDate: taskCompleteUpdate,
                            Deliverables: taskDeliverablesUpdate,
                            Remarks: taskRemarksUpdate,
                            EngineerId: taskEngineerIdUpdate,
                            Complexity: taskLevelUpdate));
                        break;

                    // Delete task

                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine()!);
                        s_dal.Task!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        static void Main(string[] args)
        {
            try
            {
                // Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);//stage 1
                //Initialization.Do(s_dal); //stage 2
                Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                if (ans == "Y") //stage 3
                    Initialization.Do(s_dal); //stage 2

                int chooseEntity;
                do
                {
                    // Display main menu options to the user

                    Console.WriteLine("enum MainMenu { EXIT, DEPENDENCY, ENGINEER, TASK }");
                    chooseEntity = int.Parse(Console.ReadLine()!);

                    // Switch based on the user's choice of entity

                    switch (chooseEntity)
                    {
                        case 1:
                            // Invoke DependencyMenu for CRUD operations on dependencies

                            DependencyMenu();
                            break;
                        case 2:
                            // Invoke EngineerMenu for CRUD operations on engineers

                            EngineerMenu();
                            break;
                        case 3:
                            // Invoke TaskMenu for CRUD operations on tasks

                            TaskMenu();
                            break;
                    }
                } while (chooseEntity > 0 && chooseEntity < 4);
            }

            catch (Exception ex)

            {
                // Handle exceptions by printing the exception details to the console

                Console.WriteLine(ex.ToString());
            }
        }
    }
}

