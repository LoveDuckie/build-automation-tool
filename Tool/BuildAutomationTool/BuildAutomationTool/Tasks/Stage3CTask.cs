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
        return File.Exists(context.GetScriptsPath(ScriptPath));
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
        string scriptPath = context.GetScriptsPath(ScriptPath);
        ProcessRunnerResult result =
            await new ProcessRunner(new ProcessRunnerParameters()
            {
                WorkingDirectory = Path.GetDirectoryName(scriptPath),
                FilePath = scriptPath,
                EnvironmentVariables = new Dictionary<string, string>(),
                Arguments = "build",
            });

        if (!string.IsNullOrEmpty(result.StandardError))
        {
            if (!string.IsNullOrEmpty(result.StandardError))
            {
                using var reader = new StringReader(result.StandardError.Trim());
                while (await reader.ReadLineAsync() is { } line)
                {
                    LogError(line);
                }
            }
        }

        if (!string.IsNullOrEmpty(result.StandardOutput))
        {
            using var reader = new StringReader(result.StandardOutput.Trim());
            while (await reader.ReadLineAsync() is { } line)
            {
                LogInformation(line);
            }
        }

        return await Task.FromResult(result.ExitCode);
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