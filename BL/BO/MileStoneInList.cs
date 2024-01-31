using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class MileStoneInList
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public Status? Status { get; set; } = null;
        public double? CompletionPercentage { get; set; } = null; public override string ToString() => this.ToStringProperty();
    }
}
