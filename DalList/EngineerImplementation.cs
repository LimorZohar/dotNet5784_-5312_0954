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

    public Engineer? Read(Func<Engineer, bool> filter = null!) => Engineers.FirstOrDefault(x => x!.Equals(filter));

    public Engineer? Read(int id) => Engineers.FirstOrDefault(x => x.Id == id);

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter is null)
            return Engineers.Select(e => e);
        return Engineers.Where(filter!);

    }
    public void Update(Engineer item)
    {
        int index = Engineers.FindIndex(x => x!.Id == item.Id);

        if (index == -1)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");

        Engineers[index] = item;

    }
}
