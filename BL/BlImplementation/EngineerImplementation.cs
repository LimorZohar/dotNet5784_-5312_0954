namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{   
    private DalApi.IDal _dal = DalApi.Factory.Get;

    //    private readonly Bl _bl;

    public int Create(BO.Engineer boEngineer)// Create a engineer in do to bo
    {
        try
        {   // Validate the data
            Tools.ValidatePositiveId(boEngineer.Id, nameof(boEngineer.Id));
            Tools.ValidateNonEmptyString(boEngineer.Name, nameof(boEngineer.Name));
            Tools.ValidateEmail(boEngineer.Email, nameof(boEngineer.Email));
            Tools.ValidatePositiveNumber(boEngineer.Cost, nameof(boEngineer.Cost));

            //create the engineer in the data layer
            DO.Engineer doEngineer = new DO.Engineer
                (boEngineer.Id, boEngineer.Email, boEngineer.Cost
                , boEngineer.Name, (DO.Expertise)boEngineer.Level!);
            //return the id of the engineer
            int idEng = _dal.Engineer.Create(doEngineer);
            return idEng;
        }
        //if the engineer already exists throw exception that the engineer already exists from the data layer
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }



    public BO.Engineer? Read(int id)//looking for engineer with the same id
    {
        
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Email = doEngineer.Email,
            Cost = doEngineer.Cost,
            Name = doEngineer.Name,
            Level = (Expertise)doEngineer.Level!
        };
    }



    public void Delete(int id)// delete the engineer
    {
        try
        {
            _dal.Engineer.Delete(id);
        }

        catch (Exception ex)
        {
            throw new BlFailedToDelete("$fail in delete the Id={id}", ex);
        }
    }

    //read all the engineers in the data layer and return them in the business layer 
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
        //return the engineers that the filter is true
        return filter != null ? engineers.Where(filter) : engineers;
    }

    //update the engineer in the data layer 
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

            if(boEngineer.Task is not null && boEngineer.Task.Id != 0)
            {
                DO.Task t = _dal.Task.Read(boEngineer.Task.Id) ?? throw new BlDoesNotExistException("the task don't exist");
                _dal.Task.Update(t with { EngineerId = boEngineer.Id });
            }

                
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

