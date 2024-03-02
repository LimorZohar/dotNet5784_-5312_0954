using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.ComponentModel.Design.Serialization;

namespace BlImplementation
{
    internal class MilestoneImplementation : IMileStone
    {
        private DalApi.IDal _dal = DalApi.Factory.Get;
        private readonly Bl _bl;
        internal MilestoneImplementation(Bl bl) => _bl = bl;


        // Function Create: Creates milestones in the system, starting and ending with unique tasks
        public IEnumerable<DO.Dependency> Create()
        {
            // Set dependencies between tasks in the system
            var groupedDependencies = _dal.Dependency.ReadAll()
                .GroupBy(d => d.DependentTask)
                .OrderBy(group => group.Key)
                .Select(group => (group.Key, group.Select(d => d!.DependsOnTask).ToList()))
                .ToList();

            // Select unique lists of dependencies
            var uniqueLists = groupedDependencies
                .Select(group => group.Item2.ToList())
                .DistinctBy(list => string.Join(",", list))
                .ToList();

            int milestoneAlias = 1;

            List<DO.Dependency> dependencies = new List<DO.Dependency>();

            // Create milestones and dependencies
            foreach (var tasksList in uniqueLists)
            {
                if (tasksList != null)
                {
                    DO.Task doTask = new DO.Task
                        (0, $"M{milestoneAlias}",
                        $"a milestone with Id: {milestoneAlias}",
                        DateTime.Now,
                        null,
                        true,
                        DO.TComplexity.Novice,
                        null, null, null, null, null, null, null);
                    try
                    {
                        int milestoneId = _dal.Task.Create(doTask);

                        foreach (var taskId in tasksList)
                        {
                            dependencies.Add(new DO.Dependency
                            {
                                DependentTask = milestoneId,
                                DependsOnTask = taskId
                            });
                        }

                        foreach (var dependencyGroup in groupedDependencies)
                        {
                            if (dependencyGroup.Item2.SequenceEqual(tasksList))
                            {
                                dependencies.Add(new DO.Dependency
                                {
                                    DependentTask = dependencyGroup.Item1,
                                    DependsOnTask = milestoneId
                                });
                            }
                        }

                        milestoneAlias++;
                    }
                    catch (DO.DalAlreadyExistsException ex)
                    {
                        throw new BO.BlFailedToCreate($"Failed to create Milestone with Alias = M{milestoneAlias}", ex);
                    }
                }
            }

            // Tasks not dependent on any milestone
            var independentOnTasks = _dal.Task.ReadAll(t => !t.IsMilestone)
                .Where(task => !dependencies.Any(d => d.DependentTask == task!.Id))
                .Select(task => task!.Id)
                .ToList();

            DO.Task startMilestone = new DO.Task
                   (0,
                   $"Start",
                   $"a milestone with Id: {0}",
                    DateTime.Now,
                        null,
                        true,
                        DO.TComplexity.Novice,
                        null, null, null, null, null, null, null);

            // Tasks that no milestone depends on
            var independentTasks = _dal.Task.ReadAll(t => !t.IsMilestone)
                .Where(task => !dependencies.Any(d => d.DependsOnTask == task!.Id))
                .Select(task => task!.Id)
                .ToList();

            DO.Task endMilestone = new DO.Task
                   (0,
                   $"End",
                   $"a milestone with Id: {milestoneAlias}",
                   DateTime.Now,
                        null,
                        true,
                        DO.TComplexity.Novice,
                        null, null, null, null, null, null, null);

            // Delete all previous dependencies
            _dal.Dependency.ReadAll().ToList().ForEach(d => _dal.Dependency.Delete(d!.Id));

            try
            {
                // Create start and end milestones
                int startMilestoneId = _dal.Task.Create(startMilestone);
                int endMilestoneId = _dal.Task.Create(endMilestone);

                foreach (var task in independentOnTasks)
                {
                    dependencies.Add(new DO.Dependency
                    {
                        DependentTask = task,
                        DependsOnTask = startMilestoneId
                    });
                }

                foreach (var task in independentTasks)
                {
                    dependencies.Add(new DO.Dependency
                    {
                        DependentTask = endMilestoneId,
                        DependsOnTask = task
                    });
                }
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlFailedToCreate("Failed to create END or START milestone", ex);
            }

            foreach (var dependency in dependencies.ToList())
            {
                if (dependency != null)
                    _dal.Dependency.Create(dependency);
            }

            // Calculate dates for all tasks and milestones
            CalculateDatesOfTasksAndMilestones();

            return _dal.Dependency.ReadAll()!;
        }

        // Function Read: Read a milestone from the data storage based on its identifier
        public MileStone? Read(int id)
        {
            try
            {
                // Read the milestone from the data storage
                DO.Task? doTaskMilestone = _dal.Task.Read(t => t.Id == id && t.IsMilestone);
                if (doTaskMilestone == null)
                    throw new BO.BlDoesNotExistException($"Milestone with ID={id} does not exist");

                // Get the list of tasks dependent on the milestone
                var tasksId = _dal.Dependency.ReadAll(d => d.DependentTask == doTaskMilestone.Id)
                                             .Select(d => d.DependsOnTask);
                var tasks = _dal.Task.ReadAll(t => tasksId.Contains(t.Id)).ToList();

                // Create a list of mapped tasks
                var tasksInList = tasks.Select(t => new BO.TaskInList
                {
                    Id = t.Id,
                    Description = t.Description,
                    Alias = t.Alias,
                    Status = Tools.CalculateStatus(t.StartDate, t.ScheduledDate, t.DeadlineDate, t.CompleteDate)
                }).ToList();

                // Calculate the completion percentage of the milestone
                double CompletionPercentage = 0;
                if (tasksInList.Count > 0)
                {
                    CompletionPercentage = (tasksInList.Count(t => t.Status == Status.OnTrack) / tasksInList.Count * 0.1) * 100;
                }

                // Build a mapped milestone object
                return new BO.MileStone()
                {
                    Id = doTaskMilestone.Id,
                    Description = doTaskMilestone.Description,
                    Alias = doTaskMilestone.Alias,
                    CreateAt = doTaskMilestone.CreateAt,
                    Status = Tools.CalculateStatus(doTaskMilestone.StartDate, doTaskMilestone.ScheduledDate, doTaskMilestone.DeadlineDate, doTaskMilestone.CompleteDate),
                    ForecastDate = doTaskMilestone.ScheduledDate,
                    Deadline = doTaskMilestone.DeadlineDate,
                    Complete = doTaskMilestone.CompleteDate,
                    CompletionPercentage = CompletionPercentage,
                    Remarks = doTaskMilestone.Remarks,
                    Dependencies = tasksInList!
                };
            }
            catch (Exception ex)
            {
                throw new BlFailedToRead("Failed to build milestone ", ex);
            }
        }

        // Function Update: Update a milestone in the data storage
        public void Update(BO.MileStone item)
        {
            // Validate the ID of the milestone
            Tools.ValidatePositiveNumber(item.Id, nameof(item.Id));
            // Validate the alias of the milestone
            Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

            try
            {
                // Read the existing milestone from the storage
                DO.Task oldDoTask = _dal.Task.Read(t => t.Id == item.Id)!;
                // Build a new task based on the old model and the new model
                DO.Task doTask = new DO.Task(item.Id, item.Description
                    , item.Alias, item.CreateAt,oldDoTask.RequiredEffortTime ,true,oldDoTask.Complexity,
                    oldDoTask.StartDate, item.ForecastDate, item.Deadline, item.Complete,
                    oldDoTask.Deliverables, item.Remarks, 0);

                _dal.Task.Update(doTask);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Milestone with ID={item.Id} does not exist", ex);
            }
        }

        // Function to calculate dates for all tasks and milestones
        private void CalculateDatesOfTasksAndMilestones()
        {
            var lastMilestone = _dal.Task.Read(t => t.Alias == "End");

            if (lastMilestone == null)
                return;

            RecursionDatesForMilestones(lastMilestone);
        }

        // Recursive function to calculate dates for milestones
        private void RecursionDatesForMilestones(DO.Task milestone)
        {
            // If there is already a finish date for the milestone, exit
            if (milestone.DeadlineDate != null) return;

            // Check dependencies of the current milestone
            var dependenciesForCheck = _dal.Dependency.ReadAll(d => d.DependsOnTask == milestone.Id);
            if (dependenciesForCheck != null)
            {
                // Get dependency IDs
                var dependenciesIds = dependenciesForCheck.Select(d => d.DependentTask).ToList();
                // Get tasks dependent on the milestone
                var dependentsTask = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
                // If there is a task with an empty finish date, exit
                foreach (var task in dependentsTask)
                    if (task.DeadlineDate == null) return;
            }

            // Calculate the latest possible finish date
            DateTime? date = CalculateLatestFinishDate(milestone);

            // Get dependencies of the current milestone
            var dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id);

            if (dependencies == null)
                return;

            // Get tasks dependent on the milestone
            var dependentTasks = dependencies.Select(d => _dal.Task.Read(t => t.Id == d.DependsOnTask));

            foreach (var task in dependentTasks)
            {
                // Update the task
                _dal.Task.Update(
                    new DO.Task(
                        task!.Id,
                        task.Alias,
                        task.Description,
                        task.CreatedAtDate,
                        task.RequiredEffortTime,
                        task.IsMilestone,
                        task.Complexity,
                        task.StartDate,

                        (DateTime)(date!) - (TimeSpan)(task.RequiredEffortTime!),
                        date,
                        task.CompleteDate,
                        task.Deliverables,
                        task.Remarks, task.EngineerId));
                        
                       

                // Recursive call for the dependent task
                RecursionDatesForMilestones(_dal.Task.Read(t => t.Id == _dal.Dependency.Read(d => d.DependentTask == task.Id)!.DependsOnTask)!);
            }

            // Update the milestone itself
            _dal.Task.Update(new DO.Task(
                milestone.Id,
                milestone.Alias,
                milestone.Description,
                milestone.CreateAt,
                milestone.RequiredEffortTime,
                milestone.IsMilestone,
                    milestone.Complexity,
                            CalculateEarliestStartDate(milestone),
                            CalculateLatestFinishDate(milestone),
                            milestone.DeadlineDate,
                            milestone.CompleteDate,
                            milestone.Deliverables,
                            milestone.Remarks, milestone.EngineerId)); 
                
        }

        // Calculate the latest possible finish date for a milestone
        private DateTime? CalculateLatestFinishDate(DO.Task milestone)
        {
            // Get all dependencies of the milestone
            var dependencies = _dal.Dependency.ReadAll(d => d.DependsOnTask == milestone.Id);

            // If there are no dependencies, the latest possible date is the planned project end date
            if (dependencies == null || dependencies.Count() == 0)
                return _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id).Max(d => _dal.Task.Read(t => t.Id == d.DependsOnTask)!.DeadlineDate);

            // Set the latest possible finish date
            var dependenciesIds = dependencies.Select(d => d.DependentTask).ToList();
            var dependentTasks = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
            DateTime? latestFinishDate = dependentTasks.Max(t =>
            {
                return (DateTime)(t.DeadlineDate!) - (TimeSpan)(t.RequiredEffortTime!);
            });
            return latestFinishDate;
        }

        // Calculate the earliest possible start date for a milestone
        private DateTime? CalculateEarliestStartDate(DO.Task milestone)
        {
            // Get all dependencies of the milestone
            var dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id);

            // If there are no dependencies, the earliest possible date is the planned project start date
            if (dependencies == null || dependencies.Count() == 0)
                return _dal.Dependency.ReadAll(d => d.DependsOnTask == milestone.Id).Min(d => _dal.Task.Read(t => t.Id == d.DependsOnTask)!.StartDate);

            // Set the earliest possible start date
            var dependenciesIds = dependencies.Select(d => d.DependsOnTask).ToList();
            var dependentTasks = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
            DateTime? earliestStartDate = dependentTasks.Min(t =>
            {
                return t.CompleteDate != null ? (DateTime)t.CompleteDate : (DateTime)t.DeadlineDate!;
            });
            return earliestStartDate;
        }
    }
}