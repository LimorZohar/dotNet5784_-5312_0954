namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using static XMLTools;


internal class EngineerImplementation : IEngineer
{
    readonly string s_Engineer_xml = "engineers";

    public int Create(Engineer item)
    {
        int newId = Config.NextLinkId;
        Engineer newengineer = item with { Id = newId };
        XElement engineerRootelement = LoadListFromXMLElement(s_Engineer_xml);
        XElement engineer = ItemToXelement<Engineer>(newengineer, "Engineer");
        engineerRootelement.Add(engineer);
        SaveListToXMLElement(engineerRootelement, s_Engineer_xml);

        return newId;

    }

    public void Delete(int id)
    {
        XElement list = LoadListFromXMLElement(s_Engineer_xml);

        foreach (var element in list.Elements())
        {
            if (element.Element("Id")!.Value == id.ToString())
            {
                element.Remove();
                SaveListToXMLElement(list, s_Engineer_xml);
                return;
            }
        }
        throw new DalDoesNotExistException("ID dosnt exist");
    }


    public Engineer? Read(int id)
    {
        XElement? engineerElem = LoadListFromXMLElement(s_Engineer_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return engineerElem == null ? null : GetEngineer(engineerElem);
    }

    public Engineer? Read(Func<Engineer, bool> filter = null!)
    {
        return LoadListFromXMLElement(s_Engineer_xml).Elements().Select(st => GetEngineer(st)).FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool> filter = null!)
    {
        if (filter == null)
            return LoadListFromXMLElement(s_Engineer_xml).Elements().Select(st => GetEngineer(st));
        else
            return LoadListFromXMLElement(s_Engineer_xml).Elements().Select(st => GetEngineer(st)).Where(filter);

    }

    public void Update(Engineer item)
    {
        XElement xelementEng = ItemToXelement<Engineer>(item, "Engineer");

        XElement list = LoadListFromXMLElement(s_Engineer_xml);

        foreach (var element in list.Elements())
        {
            if (element.Element("Id")!.Value == item.Id.ToString())
            {
                element.ReplaceWith(xelementEng);
                SaveListToXMLElement(list, s_Engineer_xml);
                return;
            }
        }
        throw new DalDoesNotExistException("ID dosnt exist");
    }
}


