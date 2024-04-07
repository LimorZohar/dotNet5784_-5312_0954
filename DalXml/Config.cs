using static Dal.XMLTools;


namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int GetNextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextLinkId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextLinkId"); }

    internal static DateTime? StartDate
    {
        get { return GetDate(s_data_config_xml, "StartProject"); }
        set { SetDate(value, s_data_config_xml, "StartProject"); }
    }
    internal static DateTime? EndDate
    {
        get { return GetDate(s_data_config_xml, "EndProject"); }
        set { SetDate(value, s_data_config_xml, "EndProject"); }
    }

}
