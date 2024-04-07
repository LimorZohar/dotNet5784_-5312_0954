
using BO;
using DalApi;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public DateTime Clock { get; set; }
    public DateTime? StartDate { set; get; }
    public DateTime? EndDate { set; get; }

}
