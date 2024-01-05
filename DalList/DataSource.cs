
namespace Dal;
internal static class DataSource
{
    internal static class Config
    {
        internal const int startCourseId = 1000;
        private static int nextCourseId = startCourseId;
        internal static int NextCourseId { get => nextCourseId++; }
    }
       

        internal static List<DO.Engineer> Engineers { get; } = new();//LIST OF ENGINEERS
    internal static List<DO.Task> Tasks { get; } = new();//LIST OF TASK
    internal static List<DO.Dependency> Dependencies { get; } = new();//LIST OF DEPENDENCY

    //...
}
