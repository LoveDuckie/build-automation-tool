using Spectre.Console;

namespace BuildAutomationTool;

/// <summary>
///
/// </summary>
public enum TaskRunnerResultTypes
{
    None = 0,
    Failed,
    Success
}

/// <summary>
///
/// </summary>
public sealed class TaskRunnerResult
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="taskTree"></param>
    /// <param name="resultTypes"></param>
    /// <param name="exception"></param>
    public TaskRunnerResult(Tree taskTree, TaskRunnerResultTypes resultTypes, Exception? exception = null)
    {
        ResultTypes = resultTypes;
        TaskTree = taskTree ?? throw new ArgumentNullException(nameof(taskTree));
        Exception = exception;
    }

    /// <summary>
    ///     
    /// </summary>
    public Exception? Exception { get; private set; }

    /// <summary>
    ///     
    /// </summary>
    public TaskRunnerResultTypes ResultTypes { get; private set; }

    /// <summary>
    ///     
    /// </summary>
    public Tree TaskTree { get; private set; }
}