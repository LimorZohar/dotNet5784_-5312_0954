namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
//using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;

internal class TaskImplementation : ITask
{
    public int Create(DO.Task item)
    {
        int id = DataSource.Config.NextTaskId;
        DO.Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        var taskToDelete = Read(t => t.Id == id);
        if (taskToDelete is null)
            throw new Exception($"Task with ID={id} does not exist");
        DataSource.Tasks.RemoveAll(x => x?.Id== id);
    }

    //public DO.Task? Read(int id)
    //{
    //    if (DataSource.Tasks.Exists(t => t?.Id == id))
    //    {
    //        DO.Task task = DataSource.Tasks.Find(t => t?.Id == id)!;
    //        return task;
    //    }
    //    throw new DalNotExistException($"the item with id: {id} not found in DataBase");
    //}
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter!);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        return filter == null ? DataSource.Tasks.Select(item => item) : DataSource.Tasks.Where(filter!);
    }
    //public Task? Read(Func<Task, bool> filter)
    //{

    //    return DataSource.Tasks.Where(filter).FirstOrDefault();
    //}



    //public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    //{
    //    if (filter is null)
    //        return DataSource.Tasks.Select(e => e);
    //    return DataSource.Tasks.Where(filter);

    //}
    //public List<DO.Task> ReadAll()
    //{
    //    return new List<DO.Task>((IEnumerable<DO.Task>)DataSource.Tasks!);
    //}

    public void Update(DO.Task item)
    {
        var existingTask = Read(t => t.Id == item.Id);
        if (existingTask is null)
            throw new Exception($"Task with ID={item.Id} does not exist");
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}
