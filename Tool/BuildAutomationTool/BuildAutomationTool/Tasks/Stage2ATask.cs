using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     Stage 2A Task
/// </summary>
[DependsOn(typeof(Stage1Task))]
[BuildTaskName("Stage 2A")]
public sealed class Stage2ATask : BuildTask
{
    /// <summary>
    ///     Get the relative path to the script
    /// </summary>
    private string ScriptPath => "build_stage_2a.bat";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">The runtime context for the build.</param>
    /// <returns>Returns boolean value indicating if this task can run.</returns>
    public override bool CanRun(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context">The runtime context for the build.</param>
    public override void BeforeExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context">The runtime context for the build.</param>
    /// <param name="progressTask"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected override async Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        if (progressTask == null) throw new ArgumentNullException(nameof(progressTask));
        
        int delayTime = new Random().Next(1000, 5000);
        await Task.Delay(delayTime);
        return await Task.FromResult(0);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override void AfterExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}