
using System.Drawing;

namespace BO;

public class TaskInGantt
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string Dependencies { get; set; }
    public double StartOffset { get; set; }
    public double TasksDays { get; set; }
    public double EndOffset { get; set; }
    public BO.Status Status { get; set; }
    public override string ToString() => this.ToStringProperty();
}
