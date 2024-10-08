﻿namespace Dal;
using DalApi;
using DO;
using static XMLTools;
internal class DependencyImplementation : IDependency
{
    readonly string s_Dependency_xml = "dependencys";

    public int Create(Dependency item)
    {
        var dependecies = LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        int id = Config.NextLinkId;
        Dependency copy = item with { Id = id };
        dependecies.Add(copy);
        SaveListToXMLSerializer<Dependency>(dependecies, s_Dependency_xml);
        return copy.Id;
    }

    public void Delete(int id)
    {
        var dependecies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);

        int index = dependecies.FindIndex(x => x.Id == id);
        if (index == -1)
            throw new DalDoesNotExistException("");

        dependecies.RemoveAt(index);
        SaveListToXMLSerializer<Dependency>(dependecies, s_Dependency_xml);
    }

    public void Reset() => XMLTools.SaveListToXMLSerializer<Dependency>(new List<Dependency>(), s_Dependency_xml);


    public Dependency? Read(int id) =>
        LoadListFromXMLSerializer<Dependency>(s_Dependency_xml).FirstOrDefault(x => x.Id == id);

    public Dependency? Read(Func<Dependency, bool> filter = null!) =>
        LoadListFromXMLSerializer<Dependency>(s_Dependency_xml).FirstOrDefault(filter);

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool> filter = null!) =>
        LoadListFromXMLSerializer<Dependency>(s_Dependency_xml).Where(task => filter is null ? true : filter(task));

    public void Update(Dependency item)
    {
        var dependecies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);

        int index = dependecies.FindIndex(x => x.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException("");

        dependecies[index] = item;
        SaveListToXMLSerializer<Dependency>(dependecies, s_Dependency_xml);
    }
}