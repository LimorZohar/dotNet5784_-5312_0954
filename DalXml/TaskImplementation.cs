namespace Dal;
using DalApi;
using DO;
using System.Linq;
using static XMLTools;


internal class TaskImplementation : ITask
{
    readonly string s_Task_xml = "students";

    public int Create(DO.Task item)
    {
        var tasks = LoadListFromXMLSerializer<Task>(s_Task_xml);

        Task copy = item with { Id = item.Id };
        tasks.Add(copy);
        SaveListToXMLSerializer<Task>(tasks, s_Task_xml);
        return copy.Id;
    }

    public void Delete(int id)
    {
        {
            var tasks = LoadListFromXMLSerializer<Task>(s_Task_xml);

            int index = tasks.FindIndex(x => x.Id == id);
            if (index == -1)
                throw new DalDoesNotExistException("");

            tasks.RemoveAt(index);
            SaveListToXMLSerializer<Task>(tasks, s_Task_xml);
        }
    }

    public DO.Task? Read(int id) => 
         LoadListFromXMLSerializer<Task>(s_Task_xml).FirstOrDefault(x => x.Id == id);
    

    public DO.Task? Read(Func<DO.Task, bool> filter = null)=>
          LoadListFromXMLSerializer<Task>(s_Task_xml).FirstOrDefault(filter);


    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool> filter = null)=>
                LoadListFromXMLSerializer<Task>(s_Task_xml).Where(filter);



    public void Update(DO.Task item)
    {
        var tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);

        int index = tasks.FindIndex(x => x.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException("");

        tasks[index] = item;
        SaveListToXMLSerializer<Task>(tasks, s_Task_xml);
    }
}