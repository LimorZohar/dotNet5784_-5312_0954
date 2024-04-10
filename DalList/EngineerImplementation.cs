namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static Dal.DataSource;

internal class EngineerImplementation : IEngineer
{   // create a new engineer in the list of engineers
    public int Create(Engineer item)
    {
        int id = item.Id;
        if (Engineers.Any(e => e!.Id == id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        Engineers.Add(item);
        return item.Id;
    }
    // reset the list of engineers
    public void Reset() => Engineers.Clear();

    // delete the engineer in the list of engineers that has the same id as the id in the parameter
    public void Delete(int id) => Engineers.RemoveAll(x => x!.Id == id);

    // return the engineer in the list of engineers that matches the filter in the parameter
    public Engineer? Read(Func<Engineer, bool> filter = null!) => Engineers.FirstOrDefault(x => x!.Equals(filter));

    // return the engineer in the list of engineers that has the same id as the id in the parameter
    public Engineer? Read(int id) => Engineers.FirstOrDefault(x => x!.Id == id);

    // return all the engineers in the list of engineers that match the filter 
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {   
        if (filter is null)
            return Engineers.Select(e => e)!;
        return Engineers.Where(filter!)!;

    }
    //update the engineer in the list of engineers with the same id as the engineer in the parameter
    public void Update(Engineer item)
    {
        int index = Engineers.FindIndex(x => x!.Id == item.Id);

        if (index == -1)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");

        Engineers[index] = item;

    }
}
