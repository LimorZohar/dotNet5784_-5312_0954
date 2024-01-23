namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;
using static XMLTools;


internal class EngineerImplementation : IEngineer
{
    readonly string s_Engineer_xml = "engineers";

    public int Create(Engineer item)
    {
        try
        {
            // Ensure unique ID for the new engineer
            int id = Config.NextLinkId;
            // Create the XML element for the engineer
            XElement engineerXML = new XElement("Engineer",
                new XElement("Id", id),
                new XElement("Email", item.Email),
                new XElement("Cost", item.Cost),
                new XElement("Name", item.Name),
                new XElement("Level", item.Level)
            );

            // Load the existing Engineers XML
            XElement engineersXML = LoadListFromXMLElement(s_Engineer_xml);

            // Append the new engineer to the list
            engineersXML.Add(engineerXML);

            // Save the updated Engineers XML
            SaveListToXMLElement(engineersXML, s_Engineer_xml);

            return id;
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            throw new DalXMLFileLoadCreateException("Failed to create engineer in XML: " + ex.Message);
        }
    }


    //public int Create(Engineer item)
    //{
    //    XElement engineerXML = new XElement("Enginner",
    //    new XElement("Id", item.Id),
    //    new XElement("Email", item.Email),
    //    new XElement("Cost", item.Cost),
    //    new XElement("Name", item.Name),
    //    new XElement("Level", item.Level));


    //    return item.Id;
    //}

    public void Delete(int id)
    {
        try
        {
            // Load the existing Engineers XML
            XElement engineersXML = LoadListFromXMLElement(s_Engineer_xml);

            // Find the index of the Engineer element with the specified id
            int index = engineersXML.Elements("Engineer").ToList().FindIndex(e => e.Element("Id")!.Value == id.ToString());

            // Throw exception if engineer not found
            if (index == -1)
            {
                throw new DalDoesNotExistException("Engineer with id " + id + " does not exist.");
            }

            // Remove the Engineer element at the specified index
            engineersXML.Elements("Engineer").ElementAt(index).Remove();

            // Save the updated Engineers XML
            SaveListToXMLElement(engineersXML, s_Engineer_xml);
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            throw new DalXMLFileLoadCreateException("Failed to delete engineer from XML: " + ex.Message);
        }
    }

    //public void Delete(int id)
    //{

    //    var engineers = LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);

    //    int index = engineers.FindIndex(x => x.Id == id);
    //    if (index == -1)
    //        throw new DalDoesNotExistException("");

    //    engineers.RemoveAt(index);
    //    SaveListToXMLSerializer<Engineer>(engineers, s_Engineer_xml);
    //}

    public Engineer? Read(int id)
    {
       XElement? engineerElem = XMLTools.LoadListFromXMLElement(s_Engineer_xml).Elements().FirstOrDefault(st=> (int?)st.Element("Id")==id);
        return engineerElem == null ? null : GetEngineer(engineerElem); 
    }
    public Engineer? Read(Func<Engineer, bool> filter = null!)
    {
       return XMLTools.LoadListFromXMLElement(s_Engineer_xml).Elements().Select(st =>GetEngineer(st)).FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool> filter = null!)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_Engineer_xml).Elements().Select(st => GetEngineer(st));
        else
            return XMLTools.LoadListFromXMLElement(s_Engineer_xml).Elements().Select(st => GetEngineer(st)).Where(filter);

    }

    public void Update(Engineer item)
    {
        // Load the existing Engineers XML
        XElement engineersXML = LoadListFromXMLElement(s_Engineer_xml);

        // Find the index of the Engineer element with the specified id
        int index = engineersXML.Elements("Engineer").ToList().FindIndex(e => e.Element("Id")!.Value == item.Id.ToString());

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