using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalTest;

namespace BlTest
{
    internal class Program
    {
        // Static field to access the BL layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Hello and welcome,\r\n" +
                    "For engineers click 1,\r\n" +
                    "For tasks click 2,\r\n" +
                    "For MileStone click 3.\r\n" +
                    "To intialize data click 4\r\n" +
                    "EXIT click 5\r\n");
                string action = Console.ReadLine()!;
                switch (action)
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
                        Initialization.Do(); 

                        break;

                    default:
                        return;
                }
            }
        }

        public static void ManageEngineers()
        {

            int chooseSubMenu;

            while (true)
            {
                Console.WriteLine("Choose an option:");
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
                        // Implement create functionality
                        int idEngineer;
                        int idTask;
                        string nameEngineer, emailEngineer,enumhelp;
                        double cost;
                        DO.Expertise levelEngineer;
                        try
                        {   //get the details from the user

                            //get the id
                            Console.WriteLine("Enter engineer ID: ");
                            int.TryParse(Console.ReadLine() ??//check the num of the enum
                                throw new BlInvalidDataException("Enter a number please"), out idEngineer);

                            //get the name
                            Console.WriteLine("Enter engineer name: ");
                            nameEngineer = (Console.ReadLine()!);

                            //get the email
                            Console.WriteLine("Enter engineer email: ");
                            emailEngineer = Console.ReadLine()!;

                            //get the level
                            Console.WriteLine("Enter 1-5 for level");
                            enumhelp = Console.ReadLine()!;
                            levelEngineer = (DO.Expertise)Enum.Parse
                                (typeof(DO.Expertise), enumhelp);//change to level 

                            //get the cost
                            Console.WriteLine("Enter the cost");
                            double.TryParse(Console.ReadLine() ?? 
                                throw new BlInvalidDataException("enter a number please"), out cost);

                            //get the task id
                            Console.WriteLine("Enter a task id"); nameEngineer = (Console.ReadLine()!);
                            int.TryParse(Console.ReadLine() ?? 
                                throw new BlInvalidDataException("enter a number please"), out idTask);

                            BO.Engineer newEng = new BO.Engineer()
                            {
                                Id = idEngineer,
                                Name = nameEngineer,
                                Email = emailEngineer,
                                Level = (BO.Expertise)levelEngineer,
                                Cost = cost,
                                //get the task id
                                Task = new TaskInEngineer()
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


                    // Implement create functionality
                    case 2:
                        // Implement read functionality
                        Console.WriteLine("Enter id for reading");
                        int idEng;
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out idEng);
                        try
                        {
                            var engineer = s_bl.Engineer!.Read(idEng);
                            if (engineer is null)
                                Console.WriteLine("No engineer found");// if there is not found
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


                    // Implement read all functionality
                    case 3:
                     
                        Console.WriteLine("Read all the engineers");
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

                     // Implement update functionality
                    case 4:
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

                     // Implement delete functionality
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out idDelete);
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

            } 
        }

       
        public static void ManageTasks()
        {

            int chooseSubMenu;

            while (true)
            {
                Console.WriteLine("Choose an option:");
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
                        Console.WriteLine("Exiting the Task Management. Goodbye!");
                        break;
                    case 1:
                        int taskInListId, EngId, days;
                        string alias, description, delivers, remarks, inputEE;
                        //DateTime? createat, startdate, scheddate, deadlinedate, complatedate;
                        TimeSpan? RequiredEffordTime;
                        TComplexity Complexity;
                        //Status status;
                        List<BO.TaskInList?> taskInList = new List<TaskInList?>();
                        Console.WriteLine("$enter taskinlist id");
                        int.TryParse(Console.ReadLine() ??
                            throw new BlInvalidDataException("enter a number please"), out taskInListId);
                        Console.WriteLine("$description");
                        description = Console.ReadLine()!;
                        Console.WriteLine("$enter remarks");
                        remarks = Console.ReadLine()!;
                        Console.WriteLine("$enter alias");
                        alias = Console.ReadLine()!;
                        Console.WriteLine("$enter delivers");
                        delivers = Console.ReadLine()!;
                        Console.WriteLine("$enter engineer id");
                        int.TryParse(Console.ReadLine() ??
                            throw new BlInvalidDataException("enter a number please"), out EngId);
                        Console.WriteLine("Enter  required effort time");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out days);
                        RequiredEffordTime = TimeSpan.FromDays(days);
                        Console.WriteLine("Enter input for level");
                        inputEE = Console.ReadLine()!;
                        Complexity = (TComplexity)Enum.Parse(typeof(TComplexity), inputEE);
                        try
                        {
                            while (taskInListId != -1)
                            {
                                taskInList!.Add(new BO.TaskInList()
                                {
                                    Id = taskInListId,
                                    Alias = s_bl.Task.Read(taskInListId)!.Alias,
                                    Description = s_bl.Task.Read(taskInListId)!.Description,
                                    Status = Tools.CalculateStatus(null, null, null, null)
                                });

                                taskInListId = int.Parse(Console.ReadLine()!);
                            }
                            BO.Task newTask = new BO.Task()
                            {
                                Id = 0,
                                Description = description,
                                Alias = alias,
                                CreatedAtDate = DateTime.Now,
                                StartDate = null,
                                ScheduledDate = null,
                                DeadlineDate = null,
                                CompleteDate = null,
                                Deliverables = delivers,
                                RequiredEffortTime = RequiredEffordTime,
                                Remarks = remarks,
                                Engineer = new BO.EngineerInTask()
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

                    case 2:
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

                    case 3:
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


                    case 4:
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
                            taskLevelUpdate = string.IsNullOrWhiteSpace(inputEEUpdate) ? updatedTask.Complexity : (TComplexity)Enum.Parse(typeof(TComplexity), inputEEUpdate);
                            Console.WriteLine("enter task in list id");
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
                            taskEngineerIdUpdate = (int)(string.IsNullOrWhiteSpace(inputUpdate) ? updatedTask.EngineerId : int.Parse(inputUpdate))!;

                            BO.Task newTaskUpdate = new BO.Task()
                            {
                                Id = idTaskUpdate,
                                Alias = taskAliasUpdate,
                                Description = taskDescriptionUpdate,
                                CreatedAtDate = updatedTask.CreatedAtDate,
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
                    case 5:
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
            }
        }

      


        public static void MilestoneMenu()
        {
            int chooseSubMenu;
            while (true)
            {
                Console.WriteLine("EXIT - click 0\r\n" +
                                  "CREATE - click 1\r\n" +
                                  "READ - click 2\r\n" +
                                  "UPDATE - click 3\r\n");
                chooseSubMenu = int.Parse(Console.ReadLine()!);

                switch (chooseSubMenu)
                {
                    case 0:
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
                            MileStone updatedMilestone = s_bl.MileStone.Read(idMilestoneUpdate)!;
                            Console.WriteLine(updatedMilestone.ToString());
                            Console.WriteLine("Enter description, alias, remarks ");//if null to put the same details
                            milestoneDescriptionUpdate = Console.ReadLine()!;
                            if (milestoneDescriptionUpdate == null || milestoneDescriptionUpdate == "")
                            { milestoneDescriptionUpdate = updatedMilestone.Description!; }
                            milestoneAliasUpdate = Console.ReadLine()!;
                            if (milestoneAliasUpdate == null || milestoneAliasUpdate == "")
                            { milestoneAliasUpdate = updatedMilestone.Alias!; }
                            milestoneRemarksUpdate = Console.ReadLine();
                            if (milestoneRemarksUpdate == null || milestoneRemarksUpdate == "")
                            { milestoneRemarksUpdate = updatedMilestone.Remarks; }
                            BO.MileStone newMilUpdate = new BO.MileStone()
                            {
                                Id = idMilestoneUpdate,
                                Description = milestoneDescriptionUpdate,
                                Alias = milestoneAliasUpdate,
                                CreateAt = s_bl.MileStone.Read(idMilestoneUpdate)!.CreateAt,
                                Status = s_bl.MileStone.Read(idMilestoneUpdate)!.Status,
                                ForecastDate = s_bl.MileStone.Read(idMilestoneUpdate)!.ForecastDate,
                                Deadline = s_bl.MileStone.Read(idMilestoneUpdate)!.Deadline,
                                Complete = s_bl.MileStone.Read(idMilestoneUpdate)!.Complete,
                                CompletionPercentage = s_bl.MileStone.Read(idMilestoneUpdate)!.CompletionPercentage,
                                Remarks = milestoneRemarksUpdate,
                                Dependencies = s_bl.MileStone.Read(idMilestoneUpdate)!.Dependencies
                            };
                            s_bl.MileStone.Update(newMilUpdate);

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



  



        
