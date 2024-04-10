using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
//singleton class that implements the IDal interface
//the class is sealed so it cannot be inherited
//the class is internal so it cannot be accessed from outside the assembly
sealed internal class DalList : IDal
{   // singleton so the constructor is private and the only instance is created here and is public
    public static IDal Instance { get; } = new DalList();

    // private constructor so the class cannot be instantiated from outside
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