namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependnecyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        //.....
        DataSource.Dependencies.RemoveAll(x => x?.Id ==id);
        throw new Exception($"Dependency is indelible entity");
    }

    public Dependency? Read(int id)
    {
        if (DataSource.Dependencies.Exists(d => d?.Id == id))
        {
            Dependency? dependency = DataSource.Dependencies.Find(d => d?.Id == id);
            return dependency;
        }
        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies!);
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Dependency with ID={item.Id} does not exist");
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
}