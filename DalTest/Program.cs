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
        //private static IDependency? s_dalDependency = new DependnecyImplementation(); //stage 1
        //private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        //private static ITask? s_dalTask = new TaskImplementation(); //stage 1

        static readonly IDal s_dal = new DalList(); //stage 2

        /// Enums for main menu and submenus

        enum MainMenu { EXIT, DEPENDENCY, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }

        /// Engineer menu for CRUD operations

        private static void EngineerMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new Exception("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        /// Create engineer

                        Console.WriteLine("Enter id, name, email, cost and a number to choose experience");
                        int idEngineer, currentNum;
                        string nameEngineer, emailEngineer;
                        EngineerExperience levelEngineer;
                        double costEngineer;

                        /// Input values
                        idEngineer = int.Parse(Console.ReadLine()!);
                        nameEngineer = (Console.ReadLine()!);
                        emailEngineer = Console.ReadLine()!;
                        costEngineer = double.Parse(Console.ReadLine()!);
                        currentNum = int.Parse(Console.ReadLine()!);

                        /// Map number to EngineerExperience enum
                        switch (currentNum)
                        {
                            case 1: levelEngineer = EngineerExperience.Expert; break;
                            case 2: levelEngineer = EngineerExperience.Junior; break;
                            case 3: levelEngineer = EngineerExperience.Rookie; break;
                            default: levelEngineer = EngineerExperience.Expert; break;
                        }
                        /// Create and add new engineer

                        s_dal.Engineer.Create(new Engineer(idEngineer, nameEngineer, emailEngineer, levelEngineer, costEngineer));

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
                            Console.WriteLine(engineer.ToString());
                        break;
                    case 4:
                        // Update engineer

                        int idEngineerUpdate, currentNumUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate;
                        EngineerExperience levelEngineerUpdate;
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

                        // Map number to EngineerExperience enum

                        switch (currentNumUpdate)
                        {
                            case 1: levelEngineerUpdate = EngineerExperience.Expert; break;
                            case 2: levelEngineerUpdate = EngineerExperience.Junior; break;
                            case 3: levelEngineerUpdate = EngineerExperience.Rookie; break;
                            default: levelEngineerUpdate = EngineerExperience.Expert; break;
                        }
                        // Create and update engineer

                        s_dal.Engineer.Update(new Engineer(idEngineerUpdate, nameEngineerUpdate, emailEngineerUpdate, levelEngineerUpdate, costEngineerUpdate));
                        break;
                    case 5:

                        // Delete engineer

                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine()!);
                        s_dalEngineer!.Delete(idDelete);
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
                            Console.WriteLine(dependency.ToString());
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

                        Console.WriteLine("Enter  description, alias,deriverables, remarks,milestone, dates and task's level");
                        int taskEngineerId, currentTaskNum;
                        string taskDescription, taskAlias, taskDeliverables, taskRemarks;
                        bool taskMilestone;
                        DateTime taskCreateAt, taskStart, taskForecastDate, taskDeadline, taskComplete;
                        EngineerExperience taskLevel;
                        taskMilestone = bool.Parse(Console.ReadLine()!);
                        taskEngineerId = int.Parse(Console.ReadLine()!);
                        taskDescription = Console.ReadLine()!;
                        taskAlias = Console.ReadLine()!;
                        taskDeliverables = Console.ReadLine()!;
                        taskRemarks = Console.ReadLine()!;
                        taskCreateAt = DateTime.Parse(Console.ReadLine()!);
                        taskStart = DateTime.Parse(Console.ReadLine()!);
                        taskForecastDate = DateTime.Parse(Console.ReadLine()!);
                        taskDeadline = DateTime.Parse(Console.ReadLine()!);
                        taskComplete = DateTime.Parse(Console.ReadLine()!);
                        currentTaskNum = int.Parse(Console.ReadLine()!);
                        switch (currentTaskNum)
                        {
                            case 1: taskLevel = EngineerExperience.Expert; break;
                            case 2: taskLevel = EngineerExperience.Junior; break;
                            case 3: taskLevel = EngineerExperience.Rookie; break;
                            default: taskLevel = EngineerExperience.Expert; break;
                        }
                        s_dal.Task.Create(new DO.Task(0, taskDescription, taskAlias, taskMilestone, taskCreateAt, taskStart, taskForecastDate, taskDeadline, taskComplete, taskDeliverables, taskRemarks, taskEngineerId, taskLevel));
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
                            Console.WriteLine(task.ToString());
                        break;

                    // Update task

                    case 4:
                        int idTaskUpdate, currentTaskNumUpdate, taskEngineerIdUpdate;
                        string taskDescriptionUpdate, taskAliasUpdate, taskDeliverablesUpdate, taskRemarksUpdate;
                        bool taskMilestoneUpdate;
                        DateTime taskCreateAtUpdate, taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate;
                        EngineerExperience taskLevelUpdate;
                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dalTask!.Read(idTaskUpdate)!.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskMilestoneUpdate = bool.Parse(Console.ReadLine()!);
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine()!);
                        taskDescriptionUpdate = Console.ReadLine()!;
                        taskAliasUpdate = Console.ReadLine()!;
                        taskDeliverablesUpdate = Console.ReadLine()!;
                        taskRemarksUpdate = Console.ReadLine()!;
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskStartUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskForecastDateUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine()!);
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine()!);
                        currentTaskNumUpdate = int.Parse(Console.ReadLine()!);
                        switch (currentTaskNumUpdate)
                        {
                            case 1: taskLevelUpdate = EngineerExperience.Expert; break;
                            case 2: taskLevelUpdate = EngineerExperience.Junior; break;
                            case 3: taskLevelUpdate = EngineerExperience.Rookie; break;
                            default: taskLevelUpdate = EngineerExperience.Expert; break;
                        }
                        s_dal.Task.Update(new(idTaskUpdate, taskDescriptionUpdate, taskAliasUpdate, taskMilestoneUpdate, taskCreateAtUpdate, taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate, taskDeliverablesUpdate, taskRemarksUpdate, taskEngineerIdUpdate, taskLevelUpdate));
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

