namespace BlImplementation;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using BlApi;
using BO;
using DO;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;


    public int Create(BO.Task boTask)
    {
        Tools.ValidateNonEmptyString(boTask.Alias, nameof(boTask.Alias));
        Tools.ValidatePositiveId(boTask.Id, nameof(boTask.Id));
        //create the task in the data layer 
        DO.Task doTask = new DO.Task
        {
            Id = boTask.Id,
            Alias = boTask.Alias,
            Description = boTask.Description,
            CreatedAtDate = boTask.CreatedAtDate,
            RequiredEffortTime = boTask.RequiredEffortTime,
            IsMilestone = boTask.IsMilestone,
            Complexity = (DO.TComplexity)boTask.Complexity!,
            StartDate = boTask.StartDate,
            ScheduledDate = boTask.ScheduledDate,
            DeadlineDate = boTask.DeadlineDate,
            CompleteDate = boTask.CompleteDate,
            Deliverables = boTask.Deliverables,
            Remarks = boTask.Remarks,
            EngineerId = boTask.EngineerId
        };
        //create the dependencies in the data layer
        try
        {   
            var dependenciesToCreate = boTask.Dependencies!
                .Select(task => new DO.Dependency
                {
                    DependentTask = boTask.Id,
                    DependsOnTask = task.Id
                })
                .ToList();
            
            dependenciesToCreate.ForEach(dependency => _dal.Dependency.Create(dependency));

            int key = _dal.Task.Create(doTask);

            return key;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }
    //read the task with the same id 
    public BO.Task? Read(int id)
    {
        try
        {
            DO.Task task = _dal.Task.Read(id) ?? throw new BlDoesNotExistException("The task does not exist");

            IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(x => x.DependentTask == id) ?? new List<DO.Dependency>();

            DO.Engineer engineer = _dal.Engineer.Read((int)task.EngineerId!) ?? new();
            //return the task with the same id
            return new BO.Task()
            {
                Id = task.Id,
                Alias = task.Alias,
                Description = task.Description,
                CreatedAtDate = task.CreatedAtDate,
                RequiredEffortTime = task.RequiredEffortTime,
                Complexity = (BO.TComplexity)task.Complexity!,
                Deliverables = task.Deliverables,
                Remarks = task.Remarks,
                StartDate = task.StartDate,
                ScheduledDate = task.ScheduledDate,
                DeadlineDate = task.StartDate > task.ScheduledDate ? task.StartDate + task.RequiredEffortTime :
                     task.ScheduledDate + task.RequiredEffortTime,
                CompleteDate = task.CompleteDate,
                Engineer = new EngineerInTask()
                {
                    Id = engineer.Id,
                    Name = engineer.Name
                },
                Dependencies = (from dep in dependencies
                                let depTask = _dal.Task.Read(dep.DependsOnTask)
                                select new BO.TaskInList()
                                {
                                    Id = depTask.Id,
                                    Alias = depTask.Alias,
                                    Description = depTask.Description,
                                    Status = GetStatus(depTask)
                                }).ToList()
            };

        }
        catch (DO.DalDoesNotExistException exe)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists", exe);//to check
        }


    }
    //delete the task with the same id
    public void Delete(int id)
    {
        // Validate that the provided ID is a positive number
        Tools.ValidatePositiveId(id, nameof(id));

        // Read the existing task with the given ID
        var existingTask = _dal.Task.Read(id);

        // If the task does not exist, throw an exception
        if (existingTask == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");
        }

        try
        {
            // Delete dependencies associated with the task
            _dal.Dependency.Delete(id);

            // Delete the task itself
            _dal.Task.Delete(id);
        }
        catch (Exception ex)
        {
            // Throw a Business Logic Failed to Delete Exception if an error occurs during deletion
            throw new BO.BlFailedToDelete($"Failed to delete the task with ID={id}", ex);
        }
    }

    public void Update(BO.Task item)
    {
        // Validate that the task ID is a positive number
        Tools.ValidatePositiveNumber(item.Id, nameof(item.Id));

        // Validate that the task alias is a non-empty string
        Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

        try
        {
            // If the task is not a milestone, update its dependencies
            if (item.Milestone == null)
            {
                // Delete existing dependencies for the task
                _dal.Dependency
                    .ReadAll(d => d.DependentTask == item.Id)
                    .ToList()
                    .ForEach(existingDependency => _dal.Dependency.Delete(existingDependency.Id));

                // Create new dependencies based on the provided Business Object dependencies
                item.Dependencies?.ForEach(boDependency =>
                {
                    DO.Dependency doDependency = new DO.Dependency(0, item.Id, boDependency.Id);
                    int idDependency = _dal.Dependency.Create(doDependency);
                });
            }

            // Create a Data Object representing the updated task
            DO.Task doTask = new DO.Task(
                item.Id,
                item.Alias,
                item.Description,
                item.CreatedAtDate,
                item.RequiredEffortTime,
                item.IsMilestone,
                (DO.TComplexity)item.Complexity!,
                item.StartDate,
                item.ScheduledDate,
                item.DeadlineDate,
                item.CompleteDate,
                item.Deliverables,
                item.Remarks,
                item.EngineerId
            );

            // Update the task in the data storage
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            // Throw a Business Logic Already Exists Exception if the task with the given ID does not exist
            throw new BlAlreadyExistsException($"Task with ID={item.Id} does not exist", ex);
        }
    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        return from task in _dal.Task.ReadAll()
               select new TaskInList()
               {
                   Id = task.Id,
                   Alias = task.Alias,
                   Status = GetStatus(task),
                   Description = task.Description
               };
    }

    public void BuildGantt()
    {
        var boTask = from task in _dal.Task.ReadAll()
                     select Read(task.Id);

        var level1 = boTask.Where(x => x.Dependencies is null);
        level1.ToList().ForEach(t => Update(t));

    }

    public void ScheduleTasks(DateTime startDate)
    {
        Dictionary<int, DO.Task> tasks = _dal.Task.ReadAll().ToList().ToDictionary(task => task.Id);
        List<Dependency> dependencies = _dal.Dependency.ReadAll().ToList();


        // Initialize the schedule with tasks that have no dependencies
        Dictionary<int, DO.Task> schedule = tasks.Where(task => !dependencies.Any(dep => dep.DependentTask == task.Key)).
            Select(task => task.Value).ToList().ToDictionary(task => task.Id);

        foreach (int key in schedule.Keys)
        {
            DO.Task old = schedule[key];
            TimeSpan? lenghTask = old.RequiredEffortTime;
            old = old with { ScheduledDate = startDate, DeadlineDate = startDate + lenghTask };
            schedule[key] = old;
        }

        foreach (int task in tasks.Keys)
        {
            if (schedule.ContainsKey(task))
                tasks.Remove(task);
        }


        while (tasks.Count > 0)
        {
            foreach (int newTask in tasks.Keys)
            {
                bool canSchedule = true;

                foreach (Dependency dep in dependencies.Where(dep => dep.DependentTask == newTask))
                {
                    if (!schedule.ContainsKey(dep.DependsOnTask))
                    {
                        canSchedule = false;
                        break;
                    }
                }

                if (canSchedule)
                {
                    DateTime? earlyStart = DateTime.MinValue;
                    DateTime? lastDepDate = DateTime.MinValue;

                    foreach (Dependency dep in dependencies.Where(dep => dep.DependentTask == newTask))
                    {
                        lastDepDate = schedule[dep.DependsOnTask].DeadlineDate;
                        if (lastDepDate > earlyStart)
                            earlyStart = lastDepDate;
                    }
                    tasks[newTask] = tasks[newTask] with { ScheduledDate = earlyStart,DeadlineDate = earlyStart + tasks[newTask].RequiredEffortTime};

                    schedule.Add(newTask, tasks[newTask]);
                    tasks.Remove(newTask);
                }
            }
        }

        schedule.Values.ToList().ForEach(task => { _dal.Task.Update(task); });
        _dal.StartDate = startDate;
    }

    public IEnumerable<BO.TaskInGantt> GantList()
    {
        return (from task in ReadAll()
                let t = _dal.Task.Read(task.Id)
                let start = (int)(t.ScheduledDate - _dal.StartDate)!.Value.TotalHours
                select new BO.TaskInGantt()
                {
                    ID = task.Id,
                    Name = task.Alias,
                    TasksDays = (double)t.RequiredEffortTime.Value.TotalHours*10,
                    StartOffset = start * 10,
                    EndOffset = (int)(start + t.RequiredEffortTime.Value.TotalHours) * 10,
                    Dependencies = StringDependencies(task.Id),
                    Status = task.Status,
                }).ToList().OrderBy(x => x.StartOffset);
    }

    private string StringDependencies(int id)
    {
        var x = _dal.Dependency.ReadAll(x => x.DependentTask == id)
                                    .Where(dependency => dependency.DependentTask == id)
                                    .Select(dependency => dependency.DependsOnTask.ToString() + " ");
        string dep = "";
        foreach (string tmp in x)
        {
            dep += tmp.ToString();
        }
        return dep;
    }

    private Status GetStatus(DO.Task item)
    {
        return item.ScheduledDate is null ? Status.Unscheduled :
            item.StartDate is null ? Status.Scheduled :
            item.CompleteDate is null ? Status.OnTrack :
            Status.Completed;
    }

}
