namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static DataSource;

// the class that implements the ITask interface
internal class TaskImplementation : ITask
{ // create a new task in the list of tasks
    public int Create(DO.Task item)
    {
        int id = Config.NextTaskId;
        DO.Task copy = item with { Id = id };
        Tasks.Add(copy);
        return id;
    }
    // delete the task in the list of tasks that has the same id as the id in the parameter
    public void Delete(int id) => Tasks.RemoveAll(x => x!.Id == id);

    // reset the list of tasks
    public void Reset() => Tasks.Clear();

    // return the task in the list of tasks that matches the filter in the parameter
    public Task? Read(Func<Task, bool> filter = null!) => Tasks.FirstOrDefault(x => x!.Equals(filter));

    // return the task in the list of tasks that has the same id as the id in the parameter
    public DO.Task? Read(int id) => Tasks.FirstOrDefault(x => x!.Id == id);

    // return all the tasks in the list of tasks that match the filter 
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) =>

        (filter == null)
        ? from item in DataSource.Tasks
          select item
        : from item in DataSource.Tasks
          where filter(item)
          select item;

    //update the task in the list of tasks with the same id as the task in the parameter 
    public void Update(DO.Task item)
    {
        int index = Engineers.FindIndex(x => x!.Id == item.Id);

        if (index == -1)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        Tasks[index] = item;

    }
}
