
using BO;
using DalApi;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public IMileStone MileStone { get; }
    public ITask Task { get; }
}
