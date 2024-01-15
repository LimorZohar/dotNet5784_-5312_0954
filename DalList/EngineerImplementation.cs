namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static Dal.DataSource;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        int id = item.Id;
        if (Engineers.Any(e => e.Id == id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id) => Engineers.RemoveAll(x => x!.Id == id);

    //{
    //    var engineerToDelete = Read(e => e.Id == id);
    //    if (engineerToDelete is null)
    //        throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
    //    if (DataSource.Tasks.Find(t => t?.EngineerId == id) is not null)
    //        throw new Exception($"Engineer with ID={id} has a task to do");
    //DataSource.Engineers.Remove(engineerToDelete);
    //}

    public Engineer? Read(Func<Engineer, bool> filter = null!) => Engineers.FirstOrDefault(x => x!.Equals(filter));

    public Engineer? Read(int id) => Engineers.FirstOrDefault(x => x.Id == id);


    //public List<Engineer> ReadAll()
    //{
    //    return new List<Engineer>(DataSource.Engineers!);
    //}
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter is null)
            return Engineers.Select(e => e);
        return Engineers.Where(filter!);

    }

    //public List<Engineer> ReadAll()
    //{
    //    return new List<Engineer>(DataSource.Engineers!);
    //}
    public void Update(Engineer item)
    {
        int index = Engineers.FindIndex(x => x!.Id == item.Id);

        if (index == -1)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");

        Engineers[index] = item;

        //var existingEngineer = Read(e => e.Id == item.Id);
        //if (existingEngineer is null)
        //    throw new Exception($"Engineer with ID={item.Id} does not exist");
        //Delete(item.Id);
        //DataSource.Engineers.Add(item);
    }
}
