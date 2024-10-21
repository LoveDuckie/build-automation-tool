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
        return File.Exists(context.GetScriptsPath(ScriptPath));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"></exception>
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
        // string scriptPath = context.GetScriptsPath(ScriptPath);
        // ProcessRunnerResult result =
        //     await new ProcessRunner(new ProcessRunnerParameters()
        //     {
        //         WorkingDirectory = Path.GetDirectoryName(scriptPath),
        //         FilePath = scriptPath,
        //         EnvironmentVariables = new Dictionary<string, string>(),
        //         Arguments = "build",
        //     });
        //
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
    /// <exception cref="ArgumentNullException"></exception>
    public override void AfterExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}