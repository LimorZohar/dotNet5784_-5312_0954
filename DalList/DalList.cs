using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    private DalList() { }
    public IDependency Dependency => new DependnecyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime Clock { get; set; } = DateTime.Now;

    public DateTime? StartDate
    {
        get { return DataSource.Config.StartDate; }
        set { DataSource.Config.StartDate = value; }
    }
    public DateTime? EndDate
    {
        get { return DataSource.Config.EndDate; }
        set { DataSource.Config.EndDate = value; }
    }
}