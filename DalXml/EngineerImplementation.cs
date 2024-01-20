namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;
using static XMLTools;

internal class EngineerImplementation : IEngineer
{
    readonly string s_Engineer_xml = "students";
  
        public int Create(Engineer item)
    {
            XElement engineerXML = new XElement("Enginner",
            new XElement("Id", item.Id),
            new XElement("Email", item.Email),
            new XElement("Cost", item.Cost),
            new XElement("Name", item.Name),
            new XElement("Level", item.Level));


        return item.Id;
    }

    public void Delete(int id)
    {

        var engineers = LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);

        int index = engineers.FindIndex(x => x.Id == id);
        if (index == -1)
            throw new DalDoesNotExistException("");

        engineers.RemoveAt(index);
        SaveListToXMLSerializer<Engineer>(engineers, s_Engineer_xml);
    }

    public Engineer? Read(int id)=>
                LoadListFromXMLSerializer<Engineer>(s_Engineer_xml).FirstOrDefault(x => x.Id == id);


    public Engineer? Read(Func<Engineer, bool> filter = null)=>
                LoadListFromXMLSerializer<Engineer>(s_Engineer_xml).FirstOrDefault(filter);


    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool> filter = null)=>
         LoadListFromXMLSerializer<Engineer>(s_Engineer_xml).Where(filter);

    public void Update(Engineer item)
    {
        // Load the existing Engineers XML
        XElement engineersXML = LoadListFromXMLElement(s_Engineer_xml);

        // Find the index of the Engineer element with the specified id
        int index = engineersXML.Elements("Engineer").ToList().FindIndex(e => e.Element("Id").Value == item.Id.ToString());

        // Update the Engineer element at the specified index
        engineersXML.Elements("Engineer").ElementAt(index).ReplaceWith(
            new XElement("Engineer",
                new XElement("Id", item.Id),
                new XElement("Email", item.Email),
                new XElement("Cost", item.Cost),
                new XElement("Name", item.Name),
                new XElement("Level", item.Level)
            )
        );

        // Save the updated Engineers XML
        SaveListToXMLElement(engineersXML, s_Engineer_xml);
    }
}