using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     Stage 3A Task
/// </summary>
[DependsOn(typeof(Stage1Task))]
[DependsOn(typeof(Stage2ATask))]
[DependsOn(typeof(Stage2BTask))]
[BuildTaskName("Stage 3A")]
public sealed class Stage3ATask : BuildTask
{
    /// <summary>
    ///     Get the relative path to the script
    /// </summary>
    private string ScriptPath => "build_stage_3a.bat";

    /// <summary>
    /// Determines whether the task can run within the provided build context.
    /// </summary>
    /// <param name="context">The build context in which the task is running.</param>
    /// <returns>True if the task can run; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
    public override bool CanRun(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return true;
    }

    /// <summary>
    /// Executes operations required before the task's primary execution phase.
    /// </summary>
    /// <param name="context">The build context in which the task is running.</param>
    /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
    public override void BeforeExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    /// Executes the task asynchronously within the provided build context, updating the given progress.
    /// </summary>
    /// <param name="context">The build context in which the task is executing.</param>
    /// <param name="progress">The progress task representing the progress of the execution.</param>
    /// <returns>Returns a Task representing the asynchronous operation, with the result being the execution result code.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
    protected override async Task<int> OnExecuteAsync(BuildContext context, ProgressTask progress)
    {
        ArgumentNullException.ThrowIfNull(context);
        int delayTime = new Random().Next(1000, 5000);
        await Task.Delay(delayTime);
        return await Task.FromResult(0);
    }

    /// <summary>
    /// Performs post-execution steps after the main execution of the task has completed.
    /// </summary>
    /// <param name="context">The build context in which the task was executed.</param>
    /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
    public override void AfterExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}