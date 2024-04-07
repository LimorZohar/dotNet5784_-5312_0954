
namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    DateTime Clock { get; set; }
    DateTime? StartDate { get; set; }
    DateTime? EndDate { get; set; }

}
