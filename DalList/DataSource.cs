﻿using System.Collections.Generic;

namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal static DateTime? StartDate;
        internal static DateTime? EndDate;
    }

    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();
    internal static List<DO.Task?> Tasks { get; } = new();
}