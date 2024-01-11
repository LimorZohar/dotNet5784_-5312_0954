namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Engineer with ID={id} does not exist");
        if (DataSource.Tasks.Find(t => t?.EngineerId == id) is not null)
            throw new Exception($"Engineer with ID={id} has a task to do");
        DataSource.Engineers.Remove(Read(id));
    }

    public Engineer? Read(int id)
    {
        // 
        Engineer? engineer = DataSource.Engineers.Find(e => e?.Id == id);
        //if (engineer == null)
        //{
        //    throw new DalNotExistException($"Engineer Id: {id} not exists");
        //}
        return engineer;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers!);
    }

    public void Update(Engineer item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} does not exist");
        Delete(item.Id);
        DataSource.Engineers.Add(item);
    }
}
