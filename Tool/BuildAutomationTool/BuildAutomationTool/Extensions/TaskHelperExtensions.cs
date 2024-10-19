using System.Reflection;
using BuildAutomationTool.Attributes;
using BuildAutomationTool.Tasks;

namespace BuildAutomationTool.Extensions;

/// <summary>
/// 
/// </summary>
public static class TaskHelperExtensions
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="buildTask"></param>
    /// <returns>Returns the list of dependencies for this task</returns>
    public static List<DependsOnAttribute> GetDependsOn(this BuildTask buildTask)
    {
        if (buildTask == null) throw new ArgumentNullException(nameof(buildTask));
        if (!buildTask.HasDependencies())
        {
            throw new InvalidOperationException("Build task has no dependencies");
        }

        return buildTask.GetType().GetCustomAttributes<DependsOnAttribute>().ToList();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buildTask"></param>
    /// <returns>Returns boolean value indicating if it has any dependencies.</returns>
    public static bool HasDependencies(this BuildTask buildTask)
    {
        if (buildTask == null) throw new ArgumentNullException(nameof(buildTask));
        return buildTask.GetType().GetCustomAttributes<DependsOnAttribute>(false).Any();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buildTask"></param>
    /// <returns></returns>
    public static string? GetTaskName(this BuildTask buildTask)
    {
        return buildTask.GetType().IsDefined(typeof(BuildTaskNameAttribute))
            ? buildTask.GetType().GetCustomAttribute<BuildTaskNameAttribute>()?.Name
            : buildTask.GetType().Name;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buildTaskType"></param>
    /// <returns></returns>
    public static string? GetTaskName(this Type buildTaskType)
    {
        if (!buildTaskType.IsSubclassOf(typeof(BuildTask)))
        {
            throw new InvalidCastException(nameof(buildTaskType));
        }
        return buildTaskType.IsDefined(typeof(BuildTaskNameAttribute))
            ? buildTaskType.GetCustomAttribute<BuildTaskNameAttribute>()?.Name
            : buildTaskType.Name;
    }
}