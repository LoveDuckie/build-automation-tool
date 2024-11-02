using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     Stage 3B Task
/// </summary>
[DependsOn(typeof(Stage1Task))]
[DependsOn(typeof(Stage2BTask))]
[BuildTaskName("Stage 3B")]
public sealed class Stage3BTask : BuildTask
{
    /// <summary>
    ///     Get the relative path to the script
    /// </summary>
    private string ScriptPath => "build_stage_3b.bat";

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public override bool CanRun(BuildContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        return true;
    }

    /// <summary>
    /// Method invoked before the execution of the build task.
    /// </summary>
    /// <param name="context">The build context providing necessary settings and logging.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provided build context is null.</exception>
    public override void BeforeExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="progressTask"></param>
    /// <returns></returns>
    protected override async Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        ArgumentNullException.ThrowIfNull(context);
        int delayTime = new Random().Next(1000, 10000);
        await Task.Delay(delayTime);
        return await Task.FromResult(0);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public override void AfterExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}