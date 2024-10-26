using BuildAutomationTool.Extensions;
using BuildAutomationTool.Tasks;
using Spectre.Console;

namespace BuildAutomationTool;

/// <summary>
///     Invoke tasks from a sorted subgraph sequentially.
/// </summary>
public sealed class TaskRunnerSequential : TaskRunner
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="startTaskType">The type of the task to start from</param>
    /// <param name="context">The runtime build context.</param>
    /// <param name="progressContext">The task progress context.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>Returns the task tree.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public override async Task<Tree?> ExecuteTasksAsync(Type startTaskType, BuildContext context,
        ProgressContext progressContext)
    {
        ArgumentNullException.ThrowIfNull(startTaskType);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(progressContext);

        var taskTreeRoot = new Tree("Tasks");

        var taskTypes = TaskGraph.GetSortedSubgraphFromTask(startTaskType);
        var progressTasks = taskTypes.Select(taskType => taskType.GetTaskName()).OfType<string>()
            .ToDictionary(taskName => taskName, taskName => progressContext.AddTask(taskName));

        TreeNode? lastNode = null;

        foreach (var task in taskTypes.Select(taskType => Activator.CreateInstance(taskType) as BuildTask))
        {
            if (task == null) continue;

            lastNode = lastNode == null
                ? taskTreeRoot.AddNode(task.GetTaskName() ?? throw new InvalidOperationException())
                : lastNode.AddNode(task.GetTaskName() ?? throw new InvalidOperationException());
            if (task == null) throw new ArgumentNullException(nameof(task));

            var taskName = task.GetTaskName() ?? throw new ArgumentNullException(nameof(startTaskType));

            task.BeforeExecute(context);

            var exitCode = await task.ExecuteAsync(context, progressTasks[taskName]);

            if (exitCode != 0)
                throw new InvalidOperationException($"Task failed: {taskName}. Exit code: {exitCode}");

            task.AfterExecute(context);
        }

        return taskTreeRoot;
    }
}