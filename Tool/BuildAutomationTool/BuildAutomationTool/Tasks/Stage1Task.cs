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
        return File.Exists(context.GetScriptsPath(ScriptPath));
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
        // var scriptPath = context.GetScriptsPath(ScriptPath);
        // var result =
        //     await new ProcessRunner(new ProcessRunnerParameters()
        //     {
        //         WorkingDirectory = Path.GetDirectoryName(scriptPath),
        //         FilePath = scriptPath,
        //         EnvironmentVariables = new Dictionary<string, string>(),
        //         Arguments = "build",
        //     });
        // if (!string.IsNullOrEmpty(result.StandardError))
        // {
        //     if (!string.IsNullOrEmpty(result.StandardError))
        //     {
        //         using var reader = new StringReader(result.StandardError.Trim());
        //         while (await reader.ReadLineAsync() is { } line)
        //         {
        //             LogError(line);
        //         }
        //     }
        // }
        //
        // if (!string.IsNullOrEmpty(result.StandardOutput))
        // {
        //     using var reader = new StringReader(result.StandardOutput.Trim());
        //     while (await reader.ReadLineAsync() is { } line)
        //     {
        //         LogInformation(line);
        //     }
        // }

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