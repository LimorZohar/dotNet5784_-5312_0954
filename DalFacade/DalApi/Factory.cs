namespace DalApi;

using static DalApi.Config;
using System.Reflection;


    public static class Factory
    {
        public static IDal Get
        {
            get
            {   // Extracting the DAL name from the configuration
                string dalType = s_dalName ?? throw new DalConfigException($"DAL name is not extracted from the configuration");
                DalImplementation dal = s_dalPackages[dalType] ?? throw new DalConfigException($"Package for {dalType} is not found in packages list in dal-config.xml");
                   // Loading the DAL package
                try { Assembly.Load(dal.Package ?? throw new DalConfigException($"Package {dal.Package} is null")); }
                catch (Exception ex) { throw new DalConfigException($"Failed to load {dal.Package}.dll package", ex); }
                // Extracting the DAL class from the configuration
                Type type = Type.GetType($"{dal.Namespace}.{dal.Class}, {dal.Package}") ??
                    throw new DalConfigException($"Class {dal.Namespace}.{dal.Class} was not found in {dal.Package}.dll");
            // return the DAL instance and check if it is a singleton
                return type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as IDal ??
                    throw new DalConfigException($"Class {dal.Class} is not a singleton or wrong property name for Instance");
            }
        }
    }
    
