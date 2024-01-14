namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        int id = item.Id;
        if (DataSource.Engineers.Any(e => e.Id == id))
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {

        var engineerToDelete = Read(e => e.Id == id);
        if (engineerToDelete is null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        if (DataSource.Tasks.Find(t => t?.EngineerId == id) is not null)
            throw new Exception($"Engineer with ID={id} has a task to do");
        DataSource.Engineers.Remove(engineerToDelete);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        // 
        //Engineer? engineer = DataSource.Engineers.Find(e => e?.Id == id);
        ////if (engineer == null)
        ////{
        ////    throw new DalNotExistException($"Engineer Id: {id} not exists");
        ////}
        //return engineer;
        return DataSource.Engineers.Where(filter!).FirstOrDefault();
    }


    //public List<Engineer> ReadAll()
    //{
    //    return new List<Engineer>(DataSource.Engineers!);
    //}
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter is null)
            return DataSource.Engineers.Select(e => e);
        return DataSource.Engineers.Where(filter!);
      
    }

    //public List<Engineer> ReadAll()
    //{
    //    return new List<Engineer>(DataSource.Engineers!);
    //}
    public void Update(Engineer item)
    {
        var existingEngineer = Read(e => e.Id == item.Id);
        if (existingEngineer is null)
            throw new Exception($"Engineer with ID={item.Id} does not exist");
        Delete(item.Id);
        DataSource.Engineers.Add(item);
    }
}
