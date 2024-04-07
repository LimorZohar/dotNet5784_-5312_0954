using DalApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal sealed class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime Clock { get; set; } = DateTime.Now;

    public DateTime? StartDate
    {
        get { return Config.StartDate; }
        set { Config.StartDate = value; }

    }
    public DateTime? EndDate
    {
        get { return Config.EndDate; }
        set
        {
            Config.EndDate = value;
        }
    }
}
