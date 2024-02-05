using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalTest;
namespace BlTest
{
    internal class program
    {
        // Static field to access the BL layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        enum MainMenu { EXIT, MILESTONE, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }
        static void Main(string[] args)
        {
            Initialization.Do();

            Console.WriteLine("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

            if (ans.ToUpper() == "Y")
                DalTest.Initialization.Do();

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Manage Engineers");
                Console.WriteLine("2. Manage Tasks");
                Console.WriteLine("3. Manage Milestones");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageEngineers();
                        break;
                    case "2":
                        ManageTasks();
                        break;
                    case "3":
                        MilestoneMenu();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static void ManageEngineers()
        {

            SubMenu chooseSubMenu;

            do
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("0. EXIT");
                Console.WriteLine("1. CREATE");
                Console.WriteLine("2. READ");
                Console.WriteLine("3. READALL ");
                Console.WriteLine("4. UPDATE");
                Console.WriteLine("5. DELETE");

                // Get user input
                int input;
                if (!int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out input))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    continue;
                }

                chooseSubMenu = (SubMenu)input;

                switch (chooseSubMenu)
                {
                    case SubMenu.CREATE:
                        // Implement create functionality
                        Console.WriteLine("Enter Id, name,TaskId,Cost, email, level");
                        int idEngineer,
                            idTask;
                        string nameEngineer,
                               emailEngineer,enumhelp;
                        double cost;
                        DO.Expertise levelEngineer;
                        try
                        {
                            int.TryParse(Console.ReadLine() ??//check the num of the enum
                                throw new BlInvalidDataException("enter a number please"), out idEngineer);
                            nameEngineer = (Console.ReadLine()!);
                            emailEngineer = Console.ReadLine()!;
                            enumhelp = Console.ReadLine()!;
                            levelEngineer = (DO.Expertise)Enum.Parse
                                (typeof(DO.Expertise), enumhelp);//change to level 
                            double.TryParse(Console.ReadLine() ?? 
                                throw new BlInvalidDataException("enter a doublenumber please"), out cost);
                            int.TryParse(Console.ReadLine() ?? 
                                throw new BlInvalidDataException("enter a number please"), out idTask);

                            BO.Engineer newEng = new BO.Engineer()
                            {
                                Id = idEngineer,
                                Name = nameEngineer,
                                Email = emailEngineer,
                                Level = (BO.Expertise)levelEngineer,
                                Cost = cost,
                                Task = new BO.TaskInEngineer()
                                {
                                    Id = idTask,
                                    Alias = s_bl.Task.Read(idTask)!.Alias
                                }
                            };
                            s_bl.Engineer.Create(newEng);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case SubMenu.READ:
                        // Implement read functionality
                        Console.WriteLine("Enter id for reading");
                        int idEng;
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idEng);
                        try
                        {
                            var engineer = s_bl.Engineer!.Read(idEng);
                            if (engineer is null)
                                Console.WriteLine("No engineer found");// if threr is not found
                            else
                            {
                          
                                Console.WriteLine(s_bl.Engineer.Read(idEng)!);//print the details
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                        break;
                    case SubMenu.READALL:
                     
                        Console.WriteLine("read all the engineers");
                        try
                        {
                            s_bl.Engineer!.ReadAll()
                            .ToList()
                            .ForEach(engineer => Console.WriteLine(engineer.ToString()));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case SubMenu.UPDATE:
                        int idEngineerUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate, inputUpdate;
                        Expertise levelEngineerUpdate;
                        double costEngineerUpdate;

                        Console.WriteLine("Enter id for updating");
                        if (!int.TryParse(Console.ReadLine(), out idEngineerUpdate))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                            continue; // Restart the loop if the input is not a valid number
                        }

                        try
                        {
                            Engineer updatedEngineer = s_bl.Engineer!.Read(idEngineerUpdate)!;

                            Console.WriteLine(updatedEngineer.ToString());

                            Console.WriteLine("Enter a Name (press Enter to keep the current value):");
                            nameEngineerUpdate = Console.ReadLine()!;
                            if (string.IsNullOrWhiteSpace(nameEngineerUpdate))
                            {
                                nameEngineerUpdate = updatedEngineer.Name!;
                            }

                            Console.WriteLine("Enter an Email (press Enter to keep the current value):");
                            emailEngineerUpdate = Console.ReadLine()!;
                            if (string.IsNullOrWhiteSpace(emailEngineerUpdate))
                            {
                                emailEngineerUpdate = updatedEngineer.Email!;
                            }

                            Console.WriteLine("1-5 to update level (press Enter to keep the current value):");
                            inputUpdate = Console.ReadLine()!;
                            levelEngineerUpdate = (Expertise)(string.IsNullOrWhiteSpace(inputUpdate)
                                ? updatedEngineer.Level
                                : (Expertise)Enum.Parse(typeof(Expertise), inputUpdate))!;

                            Console.WriteLine("Enter the cost (press Enter to keep the current value):");
                            inputUpdate = Console.ReadLine()!;
                            costEngineerUpdate = (double)(string.IsNullOrWhiteSpace(inputUpdate)
                                ? updatedEngineer.Cost
                                : double.Parse(inputUpdate))!;
                            BO.Engineer newEngUpdate = new BO.Engineer()
                            {
                                Id = idEngineerUpdate,
                                Name = nameEngineerUpdate,
                                Email = emailEngineerUpdate,
                                Level = (BO.Expertise)levelEngineerUpdate,
                                Cost = costEngineerUpdate,
                                Task = updatedEngineer.Task
                            };
                            try
                            {
                                s_bl.Engineer.Update(newEngUpdate);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case SubMenu.DELETE:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idDelete);
                        try
                        {
                            s_bl.Engineer!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                        break;
                    default: return;
                }

            } while (chooseSubMenu != SubMenu.EXIT);
        }


        public static void ManageTasks()
        {
            SubMenu chooseSubMenu;

            do
            {
                Console.WriteLine("Choose an option for Tasks:");
                Console.WriteLine("0. EXIT");
                Console.WriteLine("1. CREATE");
                Console.WriteLine("2. READ");
                Console.WriteLine("3. READALL ");
                Console.WriteLine("4. UPDATE");
                Console.WriteLine("5. DELETE");

                // Get user input
                int input;
                if (!int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out input))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    continue;
                }

                chooseSubMenu = (SubMenu)input;

                switch (chooseSubMenu)
                {
                    case SubMenu.EXIT:
                        Console.WriteLine("Exiting the Task Management. Goodbye!");
                        break;
                    case SubMenu.CREATE:
                        int taskinlistid, EngId, days;
                        string Alias, Description, delivers, remarks, inputEE;
                        //DateTime? createat, startdate, scheddate, deadlinedate, complatedate;
                        TimeSpan? Requiredeffordtime;
                        TComplexity  Complexity;
                        Status status;
                        List<BO.TaskInList?> taskInList = new List<TaskInList?>();
                        Console.WriteLine("$enter taskinlist id");
                        int.TryParse(Console.ReadLine() ?? 
                            throw new BlInvalidDataException("enter a number please"), out taskinlistid);
                        Console.WriteLine("$description");
                        Description= Console.ReadLine()!;
                        Console.WriteLine("$enter remarks");
                        remarks=Console.ReadLine()!;
                        Console.WriteLine("$enter Alias");
                        Alias=Console.ReadLine()!;
                        Console.WriteLine("$enter delivers");
                        delivers=Console.ReadLine()!;
                        Console.WriteLine("$enter engineer id");
                        int.TryParse(Console.ReadLine() ??
                            throw new BlInvalidDataException("enter a number please"), out EngId);
                        Console.WriteLine("Enter  required effort time");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out days);
                        Requiredeffordtime = TimeSpan.FromDays(days);
                        Console.WriteLine("Enter input for level");
                        inputEE = Console.ReadLine()!;
                        Complexity = (TComplexity)Enum.Parse(typeof(TComplexity), inputEE);
                        try
                        {
                            while(taskinlistid != -1)
                            {
                                taskInList!.Add(new BO.TaskInList()
                                {
                                    Id = taskinlistid,
                                    Description = s_bl.Task.Read(taskinlistid)!.Description,
                                    Alias = s_bl.Task.Read(taskinlistid)!.Alias,
                                    Status = Tools.CalculateStatus(null, null, null, null)
                                });
                                taskinlistid = int.Parse(Console.ReadLine()!);
                            }
                            BO.Task newTask = new BO.Task()
                            {
                                Id = 0,
                                Description = Description,
                                Alias = Alias,
                                CreatedAtDate = DateTime.Now,
                                StartDate = null,
                                ScheduledDate = null,
                                DeadlineDate = null, 
                                CompleteDate = null,
                                Deliverables = delivers,
                                RequiredEffortTime = Requiredeffordtime,
                                Remarks = remarks,
                                Engineer = new EngineerInTask()
                                {
                                    Id = EngId,
                                    Name = s_bl.Engineer.Read(EngId)!.Name!
                                },
                                Complexity = Complexity,
                                Status = Tools.CalculateStatus(null, null, null, null),
                                Milestone = null,
                                Dependencies = taskInList!
                            };
                            s_bl.Task.Create(newTask);//create the new task
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                        break;
                    case SubMenu.READ:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out id);
                        try
                        {
                            if (s_bl.Task!.Read(id) is null)
                                Console.WriteLine("no task found");
                            else
                                Console.WriteLine(s_bl.Task!.Read(id)!.ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case SubMenu.READALL:
                        try
                        {
                            s_bl.Task!.ReadAll()
                                      .ToList()
                                      .ForEach(task => Console.WriteLine(task.ToString()));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break; 
                        
                        
                    case SubMenu.UPDATE:
                        int idTaskUpdate,
                        taskEngineerIdUpdate,
                        taskInListIdUpdate;
                        string? taskDescriptionUpdate,
                            taskAliasUpdate,
                            taskDeliverablesUpdate,
                            taskRemarksUpdate,
                            inputEEUpdate,
                            inputUpdate;
                        TimeSpan? requiredEffortTimeUpdate;
                        TComplexity? taskLevelUpdate;
                        try
                        {
                            List<BO.TaskInList?> taskInListUpdate = new List<BO.TaskInList?>();
                            Console.WriteLine("Enter id for reading");
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idTaskUpdate);
                            BO.Task updatedTask = s_bl.Task.Read(idTaskUpdate)!;
                            Console.WriteLine(updatedTask.ToString());
                            Console.WriteLine("Enter description to update");//if null to put the same details
                            taskDescriptionUpdate = Console.ReadLine();
                            if (taskDescriptionUpdate == null || taskDescriptionUpdate == "")
                            {
                                taskDescriptionUpdate = updatedTask.Description;
                            }
                            Console.WriteLine("Enter alias to update");
                            taskAliasUpdate = Console.ReadLine();
                            if (taskAliasUpdate == null || taskAliasUpdate == "")
                            {
                                taskAliasUpdate = updatedTask.Alias;
                            }
                            Console.WriteLine("Enter required effort time to update");
                            inputUpdate = Console.ReadLine()!;
                            requiredEffortTimeUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedTask.RequiredEffortTime : TimeSpan.FromDays(int.Parse(inputUpdate));

                            Console.WriteLine("Enter deliverables to update");
                            taskDeliverablesUpdate = Console.ReadLine() ?? updatedTask.Deliverables;
                            if (taskDeliverablesUpdate == null || taskDeliverablesUpdate == "")
                            {
                                taskDeliverablesUpdate = updatedTask.Deliverables;
                            }
                            Console.WriteLine("Enter remarks to update");
                            taskRemarksUpdate = Console.ReadLine() ?? updatedTask.Remarks;
                            if (taskRemarksUpdate == null || taskRemarksUpdate == "")
                            {
                                taskRemarksUpdate = updatedTask.Remarks;
                            }
                            Console.WriteLine("Enter input 1-5 to update the level");
                            inputEEUpdate = Console.ReadLine()!;
                            taskLevelUpdate = string.IsNullOrWhiteSpace(inputEEUpdate) ? updatedTask.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputEEUpdate);
                            Console.WriteLine("entertask in list id");
                            inputUpdate = Console.ReadLine()!;

                            if (string.IsNullOrWhiteSpace(inputUpdate))
                            {
                                taskInListUpdate = updatedTask.Dependencies!;
                            }
                            else
                            {
                                taskInListIdUpdate = int.Parse(inputUpdate);
                                while (taskInListIdUpdate != -1)
                                {
                                    taskInListUpdate!.Add(new BO.TaskInList()
                                    {
                                        Id = taskInListIdUpdate,
                                        Description = s_bl.Task.Read(taskInListIdUpdate)!.Description,
                                        Alias = s_bl.Task.Read(taskInListIdUpdate)!.Alias,
                                        Status = Tools.CalculateStatus(updatedTask.StartDate, updatedTask.ScheduledDate, updatedTask.DeadlineDate, updatedTask.CompleteDate)
                                    });
                                    int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out taskInListIdUpdate);
                                }
                            }
                            Console.WriteLine("Enter ID of engineer");
                            inputUpdate = Console.ReadLine()!;
                            taskEngineerIdUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedTask.Engineer!.Id : int.Parse(inputUpdate);

                            BO.Task newTaskUpdate = new BO.Task()
                            {
                                Id = idTaskUpdate,
                                Description = taskDescriptionUpdate,
                                Alias = taskAliasUpdate,
                                CreateAtDate = updatedTask.CreateAtDate,
                                StartDate = updatedTask.StartDate,
                                ScheduledDate = updatedTask.ScheduledDate,
                                DeadlineDate = updatedTask.DeadlineDate,
                                CompleteDate = updatedTask.CompleteDate,
                                Deliverables = taskDeliverablesUpdate,
                                RequiredEffortTime = requiredEffortTimeUpdate,
                                Remarks = taskRemarksUpdate,
                                Engineer = new BO.EngineerInTask()
                                {

                                    Id = taskEngineerIdUpdate,
                                    Name = s_bl.Engineer.Read(taskEngineerIdUpdate)!.Name!
                                },
                                Complexity = (TComplexity)taskLevelUpdate,
                                Status = Tools.CalculateStatus(updatedTask.StartDate, updatedTask.ScheduledDate, updatedTask.DeadlineDate, updatedTask.CompleteDate),
                                Milestone = updatedTask.Milestone,
                                Dependencies = taskInListUpdate!,
                            };
                            s_bl.Task.Update(newTaskUpdate);
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                        break;
                    case SubMenu.DELETE:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        try
                        {
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idDelete);
                            s_bl.Task!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    default:
                        break;
                }

            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }


        public static void MilestoneMenu()
        {
            int chooseSubMenu;
            while (true)
            {
                Console.WriteLine("EXIT - click 0\r\n" +
                                  "CREATE - click 1\r\n" +
                                  "READ - click 2\r\n" +
                                  "UPDATE - click 3\r\n" +
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {
                    case 0:
                        // EXIT
                        return;

                    case 1:
                        // CREATE
                        try
                        {
                            s_bl.MileStone.Create();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during CREATE: {ex.Message}");
                        }
                        break;

                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() 
                            ?? throw new BlInvalidDataException("Enter a number please"), out id);
                        try
                        {
                            if (s_bl.MileStone!.Read(id) is null)
                                Console.WriteLine("no milestone's task found");
                            else
                            {
                                Console.WriteLine(s_bl.MileStone!.Read(id)!.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during READ: {ex.Message}");
                        }
                        break;

                    case 3:
                        // UPDATE
                        int idMilestoneUpdate;
                        string milestoneDescriptionUpdate,
                            milestoneAliasUpdate;
                        string? milestoneRemarksUpdate;
                        Console.WriteLine("Enter id for reading milestone");
                        int.TryParse(Console.ReadLine() ??
                            throw new BlInvalidDataException("Enter a number please"), out idMilestoneUpdate);
                        try
                        {
                        
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during READALL: {ex.Message}");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }
}



  



        
