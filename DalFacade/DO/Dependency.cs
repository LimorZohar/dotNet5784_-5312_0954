
/// <summary>
/// Dependencies between tasks
/// </summary>
/// <param name="Id"></param>
/// <param name="DependentTask"></param>
/// <param name="DependsOnTask"></param>
namespace DO;

public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
)
{
    public Dependency(): this( Id:0,0,0) {}
}

