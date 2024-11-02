using BuildAutomationTool.Extensions;
using BuildAutomationTool.Tasks;
using Spectre.Console;

namespace BuildAutomationTool;

/// <summary>
///     The task runner for invoking the individual tasks.
/// </summary>
public sealed class TaskRunnerParallel : TaskRunner
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="startTaskType">The type of the task to start from.</param>
    /// <param name="context"></param>
    /// <param name="progressContext"></param>
    /// <returns>Returns the generated task tree.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public override async Task<Tree?> ExecuteTasksAsync(Type startTaskType, BuildContext context,
        ProgressContext progressContext)
    {
        ArgumentNullException.ThrowIfNull(startTaskType);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(progressContext);

        var taskLevels = TaskGraph.GetSortedSubgraphFromTaskAsLevels(startTaskType);
        Tree? taskTreeRoot = new Tree("[bold white]Tasks[/]");
        var taskLevelCount = 0;
        TreeNode? lastTreeLevel = null;
        foreach (var taskLevel in taskLevels)
        {
            taskLevelCount++;
            var taskLevelName = $"[bold white]Task Level[/]: {taskLevelCount}";
            var runningTasks = new List<Task>();
            lastTreeLevel = lastTreeLevel == null
                ? taskTreeRoot.AddNode(taskLevelName)
                : lastTreeLevel.AddNode(taskLevelName);

            foreach (var currentTaskInstance in taskLevel.Select(currentTaskType =>
                         Activator.CreateInstance(currentTaskType) as BuildTask))
            {
                if (currentTaskInstance == null)
                {
                    throw new InvalidOperationException("Task instance is null");
                }

                lastTreeLevel.AddNode($"[bold white]Task:[/] {currentTaskInstance.GetTaskName()}");

                if (!currentTaskInstance.CanRun(context))
                {
                    continue;
                }
                var task = Task.Run(async () =>
                {
                    currentTaskInstance.BeforeExecute(context);
                    var exitCode = await currentTaskInstance.ExecuteAsync(context,
                        progressContext.AddTask(currentTaskInstance.GetTaskName() ?? string.Empty));
                    if (exitCode != 0)
                    {
                        throw new InvalidOperationException("Task execution failed.");
                    }

                    currentTaskInstance.AfterExecute(context);
                });
                runningTasks.Add(task);
            }

            await Task.WhenAll(runningTasks);
        }

        return taskTreeRoot;
    }
}