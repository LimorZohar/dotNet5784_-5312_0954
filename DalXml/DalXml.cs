using DalApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
//singleton class that implements the IDal interface
//the class is internal so it cannot be accessed from outside the assembly
internal sealed class DalXml : IDal
    //the class is sealed so it cannot be inherited
{   // singleton so the constructor is private and the only instance is created here and is public
    public static IDal Instance { get; } = new DalXml();
    // private constructor so the class cannot be instantiated from outside
    private DalXml() { }
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    // the clock is set to the current time
    public DateTime Clock { get; set; } = DateTime.Now;

    // the start date is set to the start date in the config
    public DateTime? StartDate
    {
        get { return Config.StartDate; }
        set { Config.StartDate = value; }

    }
    // the end date is set to the end date in the config
    public DateTime? EndDate
    {
        get { return Config.EndDate; }
        set
        {
            Config.EndDate = value;
        }
    }
}
