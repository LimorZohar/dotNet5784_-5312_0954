using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public record Engineer // Engineer personal details
    {
        /// <summary>
        /// A unique identifier for the engineer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The engineer's email address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The engineer's hourly rate.
        /// </summary>
        public double? Cost { get; set; }

        /// <summary>
        /// The engineer's full name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The engineer's expertise level.
        /// </summary>
        public Expertise? Level { get; set; }
        public object? Task { get; internal set; }

        // public Roles? Role { get; init; }
        public TaskInEngineer? Task { get; set; } = null;

        public override string ToString() => this.ToStringProperty();
    }
}

