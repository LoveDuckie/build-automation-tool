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
        return File.Exists(context.GetScriptsPath(ScriptPath));
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