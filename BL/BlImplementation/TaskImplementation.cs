namespace BlImplementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public BO.Task? Read(int id)
        {
            DO.Task? doTask = _dal.Task.Read(id);

            if (doTask == null)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");
            }

            List<TaskInList> tasksList = null!;
            MileStoneInTask milestone = null!;

            DO.Dependency? checkMileStone = _dal.Dependency.Read(d => d.DependsOnTask == doTask.Id);

            if (checkMileStone != null)
            {
                int milestoneId = checkMileStone.DependentTask;
                DO.Task? milestoneAsATask = _dal.Task.Read(t => t.Id == milestoneId && t.IsMilestone);

                if (milestoneAsATask != null)
                {
                    milestone = new MileStoneInTask()
                    {
                        Id = milestoneId,
                        Alias = milestoneAsATask.Alias
                    };
                }
                else
                {
                    tasksList = Tools.CalculateList(id)!;
                }
            }
            else
            {
                tasksList = Tools.CalculateList(id)!;
            }

            var engineer = _dal.Engineer.Read(e => e.Id == doTask.EngineerId);
            EngineerInTask? engineerInTask = engineer != null ?
                new EngineerInTask() { Id = engineer.Id, Name = engineer.Name } : null;

            BO.Expertise? level = null;
            if (doTask.Level != null)   
            {
                level = (BO.Expertise)doTask.Level;
            }


            return new BO.Task()
            {
                Id = doTask.Id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                CreatedAtDate = doTask.CreatedAtDate,
                RequiredEffortTime = doTask.RequiredEffortTime,
                IsMilestone = doTask.IsMilestone,
                Complexity = (BO.TComplexity)doTask.Complexity,
                StartDate = doTask.StartDate,
                DeadlineDate = doTask.DeadlineDate,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                EngineerId = doTask.EngineerId,
                Status = Tools.CalculateStatus(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
            };
        }

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


        public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
        {
            // Check if a filter function is provided, if not, set a default filter to include all items
            Func<BO.Task, bool>? filter1 = filter != null ? filter! : item => true;

            // Create a list to store the retrieved tasks
            List<BO.Task>? boTasks = _dal.Task.ReadAll()
                .Select(doTask =>
                {
                    // Initialize variables for tasksList and milestone
                    List<TaskInList>? tasksList = new List<TaskInList>();
                    MileStoneInTask? milestone = null;

                    // Check if the task is part of a milestone
                    DO.Dependency checkMilestone = _dal.Dependency.Read(d => d.DependsOnTask == doTask.Id)!;
                    if (checkMilestone != null)
                    {
                        // If there is a dependency, check if it represents a milestone
                        int milestoneId = checkMilestone.DependentTask;
                        DO.Task? milestoneAsATask = _dal.Task.Read(t => t.Id == milestoneId && t.IsMilestone);
                        milestone = milestoneAsATask != null
                            ? new MileStoneInTask() { Id = milestoneId, Alias = milestoneAsATask.Alias }
                            : null;
                    }
                    else
                    {
                        // If no milestone dependency, calculate tasksList
                        tasksList = Tools.CalculateList(doTask.Id);
                    }

                    // Read the engineer associated with the task
                    var engineer = _dal.Engineer.Read(e => e.Id == doTask.EngineerId);
                    EngineerInTask? engineerInTask = engineer != null
                        ? new EngineerInTask() { Id = engineer.Id, Name = engineer.Name }
                        : null;

                    // Convert the task's experience level
                    BO.Expertise? level = doTask.Level != null ? (BO.Expertise)doTask.Level : null;

                    // Create a new Business Object Task and add it to the list
                    return new BO.Task()
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias,
                        Description = doTask.Description,
                        CreatedAtDate = doTask.CreatedAtDate,
                        RequiredEffortTime = doTask.RequiredEffortTime,
                        IsMilestone = doTask.IsMilestone,
                        Complexity = (BO.TComplexity)doTask.Complexity,
                        StartDate = doTask.StartDate,
                        DeadlineDate = doTask.DeadlineDate,
                        CompleteDate = doTask.CompleteDate,
                        Deliverables = doTask.Deliverables,
                        Remarks = doTask.Remarks,
                        EngineerId = doTask.EngineerId,
                        Status = Tools.CalculateStatus(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
                    };
                })
                .Where(filter1)
                .ToList();

            // Return the list of tasks with the option to apply the filter
            return boTasks;
        }

    }
}
