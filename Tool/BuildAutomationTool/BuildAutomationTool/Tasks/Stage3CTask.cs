using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     The Stage 3C task.
/// </summary>
[DependsOn(typeof(Stage1Task))]
[BuildTaskName("Stage 3C")]
public sealed class Stage3CTask : BuildTask
{
    /// <summary>
    ///     Get the relative path to the script
    /// </summary>
    private string ScriptPath => "build_stage_3c.bat";

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override bool CanRun(BuildContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        return true;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context">The runtime context of the build</param>
    public override void BeforeExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    ///     Execute the task asynchronously.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="progressTask"></param>
    /// <returns></returns>
    protected override async Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        ArgumentNullException.ThrowIfNull(context);
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