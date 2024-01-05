/// <summary>
/// Engineer Entity represents an engineer with all its properties
/// </summary>
/// <param name="Id">Personal unique ID of the engineer (as in national id card)</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Cost">Cost associated with the engineer</param>
/// <param name="Name">Private Name of the engineer</param>
/// <param name="Level">Level of the engineer</param>
namespace DO;

public record Engineer //engineer personal details 
(
    int Id,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    Enum? Level = null
)
{
    public Engineer() : this(0, " ", 0, " ", null) { } // empty ctor for stage 3
}
