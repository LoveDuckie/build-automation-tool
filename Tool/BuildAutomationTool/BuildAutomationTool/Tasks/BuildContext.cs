using System.ComponentModel;
using BuildAutomationTool.Attributes;
using BuildAutomationTool.Commands;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
/// 
/// </summary>
public enum LogTypes
{
    /// <summary>
    ///     
    /// </summary>
    None = 0,

    /// <summary>
    ///     
    /// </summary>
    [Description("Debug")] [LogColor("green")]
    Debug,

    /// <summary>
    ///
    /// </summary>
    [Description("Info")] [LogColor("blue")]
    Information,

    /// <summary>
    /// 
    /// </summary>
    [Description("Warning")] [LogColor("yellow")]
    Warning,

    /// <summary>
    ///     
    /// </summary>
    [Description("Error")] [LogColor("red")]
    Error
}

/// <summary>
///     
/// </summary>
public class BuildContext
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="settings"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BuildContext(RunCommand.Settings settings)
    {
        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <summary>
    ///     
    /// </summary>
    public RunCommand.Settings Settings { get; set; }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="paths"></param>
    /// <returns>Returns the absolute script path</returns>
    public string GetScriptsPath(params string[] paths)
    {
        ArgumentNullException.ThrowIfNull(paths);
        var combinedPath = Path.Combine(paths);

        if (combinedPath == null) throw new ArgumentNullException(nameof(combinedPath));

        var settingsScriptsPath = Settings.ScriptsPath;
        if (settingsScriptsPath != null && !Path.IsPathFullyQualified(settingsScriptsPath))
            throw new ArgumentException(nameof(settingsScriptsPath));
        return Path.Combine(Settings.ScriptsPath ?? throw new InvalidOperationException(), combinedPath);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public void Log(LogTypes type, string message, params string[] parameters)
    {
        string formatted = string.Format(message, parameters);
        string logTypeName = EnumHelpers.GetEnumDescription(type);
        string logColor = EnumHelpers.GetEnumLogColor(type);
        AnsiConsole.MarkupLine("[bold {2}][[{1}]][/] [white]{0}[/]", formatted, logTypeName, logColor);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public void LogInformation(string message, params string[] parameters)
    {
        Log(LogTypes.Information, message, parameters);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public void LogWarning(string message, params string[] parameters)
    {
        Log(LogTypes.Warning, message, parameters);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public void LogError(string message, params string[] parameters)
    {
        Log(LogTypes.Error, message, parameters);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="exc"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public void LogError(Exception exc, string message, params string[] parameters)
    {
        Log(LogTypes.Error, message, parameters);
        Log(LogTypes.Error, exc.Message);
        Log(LogTypes.Error, exc.StackTrace ?? string.Empty);
    }
}