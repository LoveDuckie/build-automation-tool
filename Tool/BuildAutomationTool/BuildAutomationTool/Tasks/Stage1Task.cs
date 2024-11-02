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
    ///     Get the relative path to the script
    /// </summary>
    private string ScriptPath => "build_stage_1.bat";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Returns if the scripts path exists.</returns>
    public override bool CanRun(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        // return File.Exists(context.GetScriptsPath(ScriptPath));
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
        if (context == null) throw new ArgumentNullException(nameof(context));
    }
}