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
            EngineerInTask? engineerInTask = engineer != null ? new EngineerInTask() { Id = engineer.Id, Name = engineer.Name } : null;

            Expertise? level = doTask.Level != null ? (Expertise)doTask.Level : null;

            return new BO.Task()
            {
                Id = doTask.Id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                CreatedAtDate = doTask.CreatedAtDate,
                RequiredEffortTime = doTask.RequiredEffortTime,
                IsMilestone = doTask.IsMilestone,
                Complexity = (TComplexity)doTask.Complexity,
                StartDate = doTask.StartDate,
                DeadlineDate = doTask.DeadlineDate,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                EngineerId = doTask.EngineerId,
                Status = Tools.CalculateStatus(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
            };
        }

        public IEnumerable<BO.TaskInList> ReadAll()
        {
            return _dal.Task.ReadAll().Select(doTask => new BO.TaskInList());
        }

        public void Delete(int id)
        {
            Tools.ValidatePositiveId(id, nameof(id));

            var existingTask = _dal.Task.Read(id);

            if (existingTask == null)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");
            }

            try
            {
                _dal.Dependency.Delete(id);
                _dal.Task.Delete(id);
            }
            catch (Exception ex)
            {
                throw new BO.BlFailedToDelete($"Fail to delete the Id = {id}", ex);
            }
        }

        public void Update(BO.Task item)
        {
            Tools.ValidatePositiveNumber(item.Id, nameof(item.Id));
            Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

            try
            {
                if (item.Milestone == null)
                {
                    _dal.Dependency
                        .ReadAll(d => d.DependentTask == item.Id)
                        .ToList()
                        .ForEach(existingDependency => _dal.Dependency.Delete(existingDependency.Id));

                    item.Dependencies?.ForEach(boDependency =>
                    {
                        DO.Dependency doDependency = new DO.Dependency(0, item.Id, boDependency.Id);
                        int idDependency = _dal.Dependency.Create(doDependency);
                    });
                }

                DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Description,
                    item.CreatedAtDate, item.RequiredEffortTime, item.IsMilestone, (DO.TComplexity)item.Complexity!, item.StartDate,
                    item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId);

                _dal.Task.Update(doTask);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BlAlreadyExistsException($"Engineer with ID={item.Id} not exists", ex);
            }
        }

        public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
