

namespace BlImplementation;
 using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;



internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    //private object boEngineer;
    public int Create(BO.Engineer boEngineer)// Create a engineer in do to bo
    {
        Tools.ValidatePositiveId(boEngineer.Id, nameof(boEngineer.Id));
        Tools.ValidateNonEmptyString(boEngineer.Name, nameof(boEngineer.Name));
        Tools.ValidateEmail(boEngineer.Email, nameof(boEngineer.Email));
        Tools.ValidatePositiveNumber(boEngineer.Cost, nameof(boEngineer.Cost));


        DO.Engineer doEngineer = new DO.Engineer
            (boEngineer.Id, boEngineer.Email, boEngineer.Cost
            , boEngineer.Name, (DO.Expertise)boEngineer.Level!);
        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
            return idEng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={boEngineer.Id} already exists", ex);
        }
    }



    public BO.Engineer? Read(int id)//looking for engineer with the same id
    {

        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Email = doEngineer.Email,
            Cost = doEngineer.Cost,
            Name = doEngineer.Name,
            Level =(Expertise) doEngineer.Level! 
        };
    }



    public void Delete(int id)// delete the engineer
    {
        try
        {
            _dal.Engineer.Delete(id);
        }

        catch(Exception ex)
        {
            throw new BlFailedToDelete("$fail in delete the Id={id}", ex);
        }
    }


    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        var engineers = from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        let boEngineer = new Engineer
                        {
                            Id = doEngineer.Id,
                            Email = doEngineer.Email,
                            Cost = doEngineer.Cost,
                            Name = doEngineer.Name,
                            Level = (Expertise)(doEngineer.Level!)
                        }
                        select boEngineer;

        return filter != null ? engineers.Where(filter) : engineers;
    }


    public void Update(Engineer boEngineer)
    {
        // Validate the data
        Tools.ValidatePositiveId(boEngineer.Id, nameof(boEngineer.Id));
        Tools.ValidateNonEmptyString(boEngineer.Name, nameof(boEngineer.Name));
        Tools.ValidateEmail(boEngineer.Email, nameof(boEngineer.Email));
        Tools.ValidatePositiveNumber(boEngineer.Cost, nameof(boEngineer.Cost));

        // Check Engineer level
        if (boEngineer.Level <= Expertise.Beginner)
        {
            throw new BO.BlInvalidDataException("Invalid Engineer level. Must be above Junior.");
        }

        // Check for assigned Task
        if (GetTask(boEngineer) == null)
        {
            throw new BO.BlInvalidDataException("Invalid Task. Engineer must be assigned to a task.");
        }

        // Update in the data layer
        try
        {
            DO.Engineer doEngineer = new DO.Engineer
            {
                Id = boEngineer.Id,
                Name = boEngineer.Name,
                Email = boEngineer.Email,
                Level = (DO.Expertise)boEngineer.Level!,
                Cost = boEngineer.Cost,
            };

            _dal.Engineer.Update(doEngineer);
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} not exists", ex);
        }
    }

    private static object GetTask(Engineer boEngineer)
    {
        return boEngineer.Task!;
    }
}

