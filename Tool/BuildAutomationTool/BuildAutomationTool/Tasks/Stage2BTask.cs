using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     Stage 2B Task
/// </summary>
[DependsOn(typeof(Stage1Task))]
[BuildTaskName("Stage 2B")]
public sealed class Stage2BTask : BuildTask
{
    /// <summary>
    ///     Get the relative path to the script
    /// </summary>
    private string ScriptPath => "build_stage_2b.bat";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override bool CanRun(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return true;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
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
        await Task.Delay(new Random().Next(1000, 10000));
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