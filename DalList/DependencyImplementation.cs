namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static Dal.DataSource;
internal class DependnecyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        //.....
        Dependencies.RemoveAll(x => x?.Id == id);
        throw new Exception($"Dependency is indelible entity");
    }

    public Dependency? Read(Func<Dependency, bool> filter = null!) => Dependencies.FirstOrDefault(x => x!.Equals(filter));

    public Dependency? Read(int id) => Dependencies.FirstOrDefault(x => x!.Id == id);

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter is null)
            return Dependencies.Select(e => e)!;
        return Dependencies.Where(filter!)!;
        // return new List<Dependency>(DataSource.Dependencies!);
    }

    public void Update(Dependency item)
    {
        int index = Dependencies.FindIndex(x => x!.Id == item.Id);

        if (index == -1)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");

        Dependencies[index] = item;

        //var existingDependency = Read(d => d.Id == item.Id);
        //if (existingDependency is null)
        //    throw new Exception($"Dependency with ID={item.Id} does not exist");
        //Delete(item.Id);
        //DataSource.Dependencies.Add(item);
    }
}