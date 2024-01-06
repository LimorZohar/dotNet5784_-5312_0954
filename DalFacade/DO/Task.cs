    /// <summary>
    /// Represents a task entity with various properties.
    /// </summary>
    /// <param name="Id">Personal unique ID of the task.</param>
    /// <param name="Alias">An alias or alternate name for the task.</param>
    /// <param name="Description">A description or details of the task.</param>
    /// <param name="CreatedAtDate">The date and time when the task was created.</param>
    /// <param name="RequiredEffortTime">The estimated time required to complete the task.</param>
    /// <param name="IsMilestone">Indicates whether the task is a milestone.</param>
    /// <param name="TaskComplexity">The complexity level of the task.</param>
    /// <param name="StartDate">The planned start date of the task.</param>
    /// <param name="ScheduledDate">The scheduled date for the task.</param>
    /// <param name="DeadlineDate">The deadline date for completing the task.</param>
    /// <param name="CompleteDate">The date when the task was completed.</param>
    /// <param name="Deliverables">Any deliverables associated with the task.</param>
    /// <param name="Remarks">Additional remarks or comments related to the task.</param>
    /// <param name="EngineerId">The unique ID of the engineer assigned to the task.</param>

using System;

namespace DO
{
    public record Task
    (
        int Id,
        string? Alias = null,
        string? Description = null,
        DateTime? CreatedAtDate = null,
        TimeSpan? RequiredEffortTime = null,
        bool IsMilestone = false,
        enum Copmlexity,
        DateTime? StartDate = null,
        DateTime? ScheduledDate = null,
        DateTime? DeadlineDate = null,
        DateTime? CompleteDate = null,
        string? Deliverables = null,
        string? Remarks = null,
        int EngineerId
    )
    {
        public Task() : this(0, "", "", DateTime.null, TimeSpan.null, false, null, DateTime.null, null, null, null, null, null, null, 0) { }

        /// <summary>
        /// RegistrationDate - registration date of the current student record
        /// </summary>
        public DateTime RegistrationDate => DateTime.Now; // get-only property with a corrected name
    }
}
