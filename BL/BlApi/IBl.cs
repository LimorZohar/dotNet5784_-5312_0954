
using BO;
using DalApi;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public IMileStone MileStone { get; }
    public ITask Task { get; }
    public DateTime Clock { get; }
    public void  AddDay();
    public void AddMonth();
    public void AddYear();
    public void ResetClock();

}
