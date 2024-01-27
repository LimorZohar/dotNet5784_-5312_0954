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
        static readonly IDal s_dal = Factory.Get; //stage 4
        //static readonly IDal s_dal = new DalList(); //stage 2
        //static readonly IDal s_dal = new DalXml(); //stage 3


        /// Engineer menu for CRUD operations

        private static void EngineerMenu()
        {
            int chooseSubMenu;

            while (true)
            {
                Console.WriteLine("EXIT click 0\r\n" +
                    "CREATE click 1\r\n" +
                    "READ click 2\r\n" +
                    "READALL click 3\r\n" +
                    "UPDATE click 4\r\n" +
                    "DELETE click 5\r\n");
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {
                    case 1:
                        int idEngineer;
                        string nameEngineer, emailEngineer;
                        Expertise levelEngineer;
                        double costEngineer;

                        /// Input values
                        Console.WriteLine("Enter engineer ID: ");
                        idEngineer = Console.Read();

                        Console.WriteLine("Enter engineer name: ");
                        nameEngineer = Console.ReadLine()!;

                        Console.WriteLine("Enter engineer email: ");
                        emailEngineer = Console.ReadLine()!;

                        Console.WriteLine("Enter engineer cost: ");
                        costEngineer = double.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter engineer level (Expertise): ");
                        levelEngineer = (Expertise)Console.Read();

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
                        try
                        {
                            Console.WriteLine(s_dal.Engineer!.Read(id)!.ToString());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
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
                        double costEngineerUpdate;

                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter engineer name: ");
                        nameEngineerUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter engineer email: ");
                        emailEngineerUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter engineer cost: ");
                        costEngineerUpdate = double.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter current engineer number: ");
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
            }
        }

        /// Dependency menu for CRUD operations

        private static void DependencyMenu()
        {
            int chooseSubMenu;

            while (true)
            {
                while (true)
                {
                    Console.WriteLine("EXIT click 0\r\n" +
                        "CREATE click 1\r\n" +
                        "READ click 2\r\n" +
                        "READALL click 3\r\n" +
                        "UPDATE click 4\r\n" +
                        "DELETE click 5\r\n");
                    chooseSubMenu = int.Parse(Console.ReadLine()!);

                    switch (chooseSubMenu)
                    {
                        case 0:
                            return;

                        case 1:
                            int dependentTask, dependsOnTask;

                            Console.WriteLine("Enter dependent task: ");
                            dependentTask = int.Parse(Console.ReadLine()!);

                            Console.WriteLine("Enter task it depends on: ");
                            dependsOnTask = int.Parse(Console.ReadLine()!);

                            s_dal.Dependency.Create(new Dependency(0, dependentTask, dependsOnTask));
                            break;

                        case 2:
                            int id;
                            Console.WriteLine("Enter id for reading");
                            id = int.Parse(Console.ReadLine()!);
                            if (s_dal.Dependency!.Read(id) is null)
                                Console.WriteLine("no dependency found");
                            Console.WriteLine(s_dal.Dependency!.Read(id)!.ToString());
                            break;

                        case 3:
                            foreach (var dependency in s_dal.Dependency!.ReadAll())
                                Console.WriteLine(dependency!.ToString());
                            break;

                        case 4:
                            int idUpdate, dependentTaskUpdate, dependsOnTaskUpdate;

                            Console.WriteLine("Enter id for reading");
                            idUpdate = int.Parse(Console.ReadLine()!);
                            Console.WriteLine(s_dal.Dependency!.Read(idUpdate)!.ToString());

                            Console.WriteLine("Enter details to update\r\n");

                            Console.WriteLine("Enter dependent task for update:\r\n");
                            dependentTaskUpdate = int.Parse(Console.ReadLine()!);

                            Console.WriteLine("Enter task it depends on for update:\r\n");
                            dependsOnTaskUpdate = int.Parse(Console.ReadLine()!);

                            s_dal.Dependency!.Update(new(idUpdate, dependentTaskUpdate, dependsOnTaskUpdate));

                            break;
                        case 5:
                            int idDelete;
                            Console.WriteLine("Enter id for deleting\r\n");
                            idDelete = int.Parse(Console.ReadLine()!);
                            s_dal.Dependency!.Delete(idDelete);
                            break;

                        default:
                            return;
                    }
                }
            }
        }

        // Task menu for CRUD operations

        private static void TaskMenu()
        {
            int chooseSubMenu;

            while (true)
            {
                Console.WriteLine("EXIT click 0\r\n" +
                    "CREATE click 1\r\n" +
                    "READ click 2\r\n" +
                    "READALL click 3\r\n" +
                    "UPDATE click 4\r\n" +
                    "DELETE click 5\r\n");
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {
                    case 0:
                        return;
                    case 1:
                        int idTaskUpdate, currentTaskNumUpdate, taskEngineerIdUpdate;
                        string taskDescriptionUpdate, taskAliasUpdate, taskDeliverablesUpdate, taskRemarksUpdate;
                        bool taskMilestoneUpdate;
                        DateTime taskCreateAtUpdate, taskStartUpdate, taskDeadlineUpdate, taskCompleteUpdate;
                        TComplexity taskLevelUpdate;

                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task milestone (true/false): ");
                        taskMilestoneUpdate = bool.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task engineer ID: ");
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task description: ");
                        taskDescriptionUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task alias: ");
                        taskAliasUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task deliverables: ");
                        taskDeliverablesUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task remarks: ");
                        taskRemarksUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task creation date (yyyy-MM-dd HH:mm:ss): ");
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task start date (yyyy-MM-dd HH:mm:ss): ");
                        taskStartUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task deadline (yyyy-MM-dd HH:mm:ss): ");
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task completion date (yyyy-MM-dd HH:mm:ss): ");
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter current task number: ");
                        currentTaskNumUpdate = int.Parse(Console.ReadLine()!);
                        taskLevelUpdate = (TComplexity)currentTaskNumUpdate;


                        s_dal.Task.Create(new
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
                        try
                        {
                            foreach (var task in s_dal.Task!.ReadAll())
                                Console.WriteLine(task!.ToString());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    // Update task

                    case 4:

                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task milestone (true/false): ");
                        taskMilestoneUpdate = bool.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task engineer ID: ");
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task description: ");
                        taskDescriptionUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task alias: ");
                        taskAliasUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task deliverables: ");
                        taskDeliverablesUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task remarks: ");
                        taskRemarksUpdate = Console.ReadLine()!;

                        Console.WriteLine("Enter task creation date (yyyy-MM-dd HH:mm:ss): ");
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task start date (yyyy-MM-dd HH:mm:ss): ");
                        taskStartUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task deadline (yyyy-MM-dd HH:mm:ss): ");
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter task completion date (yyyy-MM-dd HH:mm:ss): ");
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter current task number: ");
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
                    default:
                        return;
                }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Hello and welcome,\r\n" +
                    "For engineers click 1,\r\n" +
                    "For tasks click 2,\r\n" +
                    "For dependencies click 3.\r\n" +
                    "To intialize data click 4\r\n" +
                    "EXIT click 5\r\n");
                string action = Console.ReadLine()!;
                switch (action)
                {
                    case "1":
                        EngineerMenu();
                        break;

                    case "2":
                        TaskMenu();
                        break;

                    case "3":
                        DependencyMenu();
                        break;
                    case "4":
                        //Initialization.Do(s_dal); //stage 2
                        Initialization.Do(); //stage 4

                        break;

                    default:
                        return;
                }
            }
        }

    }
}


