using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     Stage 1 Task
/// </summary>
[BuildTaskName("Stage 1")]
public sealed class Stage1Task : BuildTask
{
    /// <summary>
    /// Determines if the task can be executed.
    /// </summary>
    /// <param name="context">The build context.</param>
    /// <returns>Returns if the scripts path exists.</returns>
    public override bool CanRun(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        // return File.Exists(context.GetScriptsPath(ScriptPath));
        return true;
    }

    /// <summary>
    /// Executes logic before the main execution of the task.
    /// </summary>
    /// <param name="context">The build context.</param>
    public override void BeforeExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    /// Executes the task with a delay before completion.
    /// </summary>
    /// <param name="context">The build context.</param>
    /// <param name="progressTask">The progress task to report progress.</param>
    /// <returns>Returns the result code of the task execution.</returns>
    protected override async Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        ArgumentNullException.ThrowIfNull(context);
        int delayTime = new Random().Next(1000, 5000);
        await Task.Delay(delayTime);
        return await Task.FromResult(0);
    }

    /// <summary>
    /// Executes logic after the main task execution is completed.
    /// </summary>
    /// <param name="context">The build context containing relevant data and state information.</param>
    public override void AfterExecute(BuildContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
    }
}