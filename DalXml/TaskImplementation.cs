namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation : ITask
{
    readonly string s_Task_xml = "students";

    public int Create(DO.Task item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public DO.Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public DO.Task? Read(Func<DO.Task, bool> filter = null)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool> filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(DO.Task item)
    {
        throw new NotImplementedException();
    }
}