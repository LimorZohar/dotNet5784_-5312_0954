namespace Dal;
using DalApi;
using DO;
using static XMLTools;
internal class DependencyImplementation : IDependency
{
    readonly string s_Dependency_xml = "students";

    public int Create(Dependency item)
    {
        var dependecies = LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);

        Dependency copy = item with { Id = item.Id };
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

    public Dependency? Read(int id) =>
        LoadListFromXMLSerializer<Dependency>(s_Dependency_xml).FirstOrDefault(x => x.Id == id);

    public Dependency? Read(Func<Dependency, bool> filter = null) =>
        LoadListFromXMLSerializer<Dependency>(s_Dependency_xml).FirstOrDefault(filter);

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool> filter = null) =>
        LoadListFromXMLSerializer<Dependency>(s_Dependency_xml).Where(filter);

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