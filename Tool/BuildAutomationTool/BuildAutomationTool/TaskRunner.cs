using BuildAutomationTool.Tasks;
using Spectre.Console;

namespace BuildAutomationTool;

/// <summary>
///     The base abstract class.
/// </summary>
public abstract class TaskRunner
{
    /// <summary>
    ///
    /// </summary>
    protected BuildTaskGraph TaskGraph { get; } = new();

    /// <summary>
    /// Initialize the task graph.
    /// </summary>
    public void Initialize()
    {
        TaskGraph.GenerateGraph();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="startTaskType"></param>
    /// <param name="context"></param>
    /// <param name="progressContext"></param>
    /// <returns></returns>
    public abstract Task<Tree?> ExecuteTasksAsync(Type startTaskType, BuildContext context,
        ProgressContext progressContext);
}