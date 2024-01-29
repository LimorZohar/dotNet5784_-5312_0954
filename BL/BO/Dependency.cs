using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Task
    {
        /// <summary>
        /// A unique identifier for the task.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// An optional short name or abbreviation for the task.
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// A detailed explanation of the task's purpose and requirements.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The date when the task was first created.
        /// </summary>
        public DateTime? CreatedAtDate { get; set; }

        /// <summary>
        /// The estimated time needed to complete the task.
        /// </summary>
        public TimeSpan? RequiredEffortTime { get; set; }

        /// <summary>
        /// Whether the task is a significant milestone within a larger project.
        /// </summary>
        public bool IsMilestone { get; set; }

        /// <summary>
        /// The task's level of difficulty or complexity.
        /// </summary>
        public TComplexity Complexity { get; set; }

        /// <summary>
        /// The date when work on the task is intended to begin.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The date when the task is scheduled to be worked on.
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// The date by which the task must be completed.
        /// </summary>
        public DateTime? DeadlineDate { get; set; }

        /// <summary>
        /// The date when the task was actually completed.
        /// </summary>
        public DateTime? CompleteDate { get; set; }

        /// <summary>
        /// The tangible outcomes or outputs expected upon task completion.
        /// </summary>
        public string? Deliverables { get; set; }

        /// <summary>
        /// Additional notes or comments about the task.
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// The ID of the engineer responsible for the task.
        /// </summary>
        public int? EngineerId { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
