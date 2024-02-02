

namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        Tools.ValidateNonEmptyString(boTask.Alias, nameof(boTask.Alias));//check if the sring is not empty
        Tools.ValidatePositiveId(boTask.Id, nameof(boTask.Id));//check if the id is positive

        DO.Task doTask = new DO.Task//create new 
            (boTask.Id, boTask.Alias, boTask.Description,
            boTask.CreatedAtDate, boTask.RequiredEffortTime,
            boTask.IsMilestone, (DO.TComplexity)boTask.Complexity!,boTask.StartDate
            ,boTask.ScheduledDate,boTask.DeadlineDate,boTask.CompleteDate,boTask.Deliverables
            ,boTask.Remarks,boTask.EngineerId);
        try
        {
            var dependenciesToCreate = boTask.Dependencies!//create a dependency
                .Select(task => new DO.Dependency
                {
                    DependentTask = boTask.Id,//the key
                    DependsOnTask = task.Id// taking from the task
                })
                .ToList();

            dependenciesToCreate.ForEach(dependency => _dal.Dependency.Create(dependency));

            int key = _dal.Task.Create(doTask);//return the key

            return key;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);//if the dependency already exist
        }
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);//upload the tasks
        if (doTask== null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");


        return new BO.Task()
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description= doTask.Description,
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

    public IEnumerable<BO.StudentInList> ReadAll()
    {
        return (from DO.Student doStudent in _dal.Student.ReadAll()
                select new BO.StudentInList
                {
                    Id = doStudent.Id,
                    Name = doStudent.Name,
                    CurrentYear = (BO.Year)(DateTime.Now.Year - doStudent.RegistrationDate.Year)
                });
    }

    public int Create(Task item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}

