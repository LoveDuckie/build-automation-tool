using Spectre.Console;

namespace BuildAutomationTool.Tasks;

public sealed class GenericBuildTask : BuildTask
{
    #region Properties

    /// <summary>
    ///     
    /// </summary>
    public Func<bool, BuildContext> OnCanRun { get; private set; }

    /// <summary>
    ///     
    /// </summary>
    public Action<BuildContext> OnExecute { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public Action<BuildContext> OnBeforeExecute { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public Action<BuildContext> OnAfterExecute { get; private set; }

    #endregion

    /// <inheritdoc />
    public GenericBuildTask(string? taskName, Func<bool, BuildContext> onCanRun, Action<BuildContext> onExecute,
        Action<BuildContext> onBeforeExecute, Action<BuildContext> onAfterExecute)
    {
        TaskName = taskName;
        OnCanRun = onCanRun;
        OnExecute = onExecute;
        OnBeforeExecute = onBeforeExecute;
        OnAfterExecute = onAfterExecute;
    }


    public override string? TaskName { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override bool CanRun(BuildContext context)
    {
        return false;
    }

    public override void BeforeExecute(BuildContext context)
    {
    }


    protected override Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        return Task.FromResult(0);
    }

    public override void AfterExecute(BuildContext context)
    {
    }
}