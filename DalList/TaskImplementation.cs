namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static DataSource;

internal class TaskImplementation : ITask
{
    public int Create(DO.Task item)
    {
        int id = Config.NextTaskId;
        DO.Task copy = item with { Id = id };
        Tasks.Add(copy);
        return id;
    }

    public void Delete(int id) => Tasks.RemoveAll(x => x!.Id == id);
 

    public Task? Read(Func<Task, bool> filter = null!) => Tasks.FirstOrDefault(x => x!.Equals(filter));

    public DO.Task? Read(int id) => Tasks.FirstOrDefault(x => x!.Id == id);

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) =>

        filter == null ? DataSource.Tasks.Select(item => item) : DataSource.Tasks.Where(filter!);


    public void Update(DO.Task item)
    {
        int index = Engineers.FindIndex(x => x!.Id == item.Id);

        if (index == -1)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        Tasks[index] = item;

    }
}
