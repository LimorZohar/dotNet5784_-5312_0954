namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Task with ID={id} does not exist");
        if (DataSource.Dependencies.Find(d => d.DependsOnTask == id) is not null)
            DataSource.Tasks.Remove(Read(id));
        throw new Exception($"Task with ID={id} has a depends task");
    }

    public Task? Read(int id)
    {
        if (DataSource.Tasks.Exists(t => t.Id == id))
        {
            Task? task = DataSource.Tasks.Find(t => t.Id == id);
            return task;
        }
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks!);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Task with ID={item.Id} does not exist");
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}
