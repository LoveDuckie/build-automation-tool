using System.Reflection;
using BuildAutomationTool.Attributes;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     The base build task
/// </summary>
public abstract class BuildTask
{
    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<string, Type> BuildTaskTypes = new();

    public virtual string? TaskName => GetType().IsDefined(typeof(BuildTaskNameAttribute))
        ? GetType().GetCustomAttribute<BuildTaskNameAttribute>()?.Name
        : GetType().Name;

    /// <summary>
    ///     The base static constructor
    /// </summary>
    static BuildTask()
    {
        foreach (Type type in typeof(BuildTask).Assembly.GetTypes().Where(n => n.IsSubclassOf(typeof(BuildTask))))
        {
            if (type.FullName != null)
            {
                BuildTaskTypes?.Add(type.Name, type);
            }
        }
    }

    /// <summary>
    ///     Log the contents of the message.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    private void Log(LogTypes type, string? message, params object?[] parameters)
    {
        if (message == null) return;
        string formatted = string.Format(message, parameters);

        if (string.IsNullOrEmpty(TaskName))
        {
            throw new ArgumentNullException(nameof(TaskName));
        }

        string logTypeName = EnumHelpers.GetEnumDescription(type);
        string logColor = EnumHelpers.GetEnumLogColor(type);

        AnsiConsole.MarkupLine("[bold {3}][[{2}]][/] [bold white]Task: \"{1}\":[/][white] {0}[/]", formatted,
            TaskName.ToUpper(), logTypeName,
            logColor);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    protected void LogInformation(string? message, params object?[] parameters)
    {
        Log(LogTypes.Information, message, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    protected void LogWarning(string? message, params object?[] parameters)
    {
        Log(LogTypes.Warning, message, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    protected void LogError(string? message, params object?[] parameters)
    {
        Log(LogTypes.Error, message, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="exc"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    protected void LogError(Exception exc, string? message, params object?[] parameters)
    {
        Log(LogTypes.Error, message, parameters);
        Log(LogTypes.Error, exc.Message);
        Log(LogTypes.Error, exc.StackTrace ?? string.Empty);
    }

    /// <summary>
    ///     Get the build task types.
    /// </summary>
    /// <returns>Returns the container of task types.</returns>
    public static Dictionary<string, Type> GetBuildTaskTypes() => BuildTaskTypes;

    /// <summary>
    ///     Determines whether this build task can run
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Returns boolean value indicating if it can run</returns>
    public abstract bool CanRun(BuildContext context);

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    public abstract void BeforeExecute(BuildContext context);

    /// <summary>
    ///
    /// </summary>
    /// <param name="context">The runtime build context</param>
    /// <param name="progressTask"></param>
    /// <returns>Returns the awaitable task</returns>
    public async Task<int> ExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (progressTask == null) throw new ArgumentNullException(nameof(progressTask));
        LogWarning("Running");
        try
        {
            var result = await OnExecuteAsync(context, progressTask);
            LogInformation("Done!");
            progressTask.Increment(100D);
            return result;
        }
        catch (Exception e)
        {
            progressTask.Increment(100D);
            LogError(e, "Error");
            throw;
        }
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    /// <param name="progressTask"></param>
    /// <returns></returns>
    protected abstract Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask);

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    public abstract void AfterExecute(BuildContext context);
}