using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class MileStone
    {   // A property that contains the milestone's ID.
        public int Id { get; init; }

        // A property that contains the milestone's description.
        public string? Description { get; set; }

        // A property that contains the milestone's alias.
        public string? Alias { get; set; }

        // A property that contains the milestone's creation date.
        public DateTime CreateAt { get; set; }

        // A property that contains the milestone's status.
        public Status? Status { get; set; } = null;

        // A property that contains the milestone's forecast date (internal forecast of the estimate).
        public DateTime? ForecastDate { get; set; } = null;

        // A property that contains the milestone's deadline.
        public DateTime? Deadline { get; se t; } = null;

        // A property that contains the milestone's completion date.
        public DateTime? Complete { get; set; } = null;

        // A property that contains the milestone's completion percentage.
        public double? CompletionPercentage { get; set; } = null;

        // A property that contains additional remarks about the milestone.
        public string? Remarks { get; set; } = null;

        // A property that contains a list of the milestone's dependencies.
        public List<TaskInList?>? Dependencies { get; set; } = null;

        // A function that returns a string representing the Milestone object.
        public override string ToString() => this.ToStringProperty();

    }
}
