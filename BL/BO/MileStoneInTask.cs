using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class MileStoneInTask
    {   // A property that contains the milestone's ID.
        public int Id { get; init; }

        // A property that contains the milestone's alias.
        public string? Alias { get; set; }

        // A function that returns a string representing the MilestoneInTask object.
        public override string ToString() => this.ToStringProperty();

    }
}
