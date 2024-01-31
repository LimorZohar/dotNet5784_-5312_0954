using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TaskInList
    {
        public int Id { get; init; } // A unique identifier for a task in the list
        public string? Description { get; set; } // The task's description
        public string? Alias { get; set; } // The task's alias
        public BO.Status Status { get; set; } // The task's status from the BO status list
        public override string ToString() => this.ToStringProperty(); // A ToString() function that displays the task's details

    }
}
