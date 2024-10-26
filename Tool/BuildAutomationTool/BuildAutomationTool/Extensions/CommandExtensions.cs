using BuildAutomationTool.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace BuildAutomationTool.Extensions;

/// <summary>
///
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="command"></param>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    private static void Log(this ICommand command, LogTypes type, string message, params string[] parameters)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        string formatted = string.Format(message, parameters);
        string logTypeName = EnumHelpers.GetEnumDescription(type);
        string logColor = EnumHelpers.GetEnumLogColor(type);
        AnsiConsole.MarkupLine("[bold {2}][[{1}]][/] [white]{0}[/]", formatted, logTypeName, logColor);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="command"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public static void LogInformation(this ICommand command, string message, params string[] parameters)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        Log(command, LogTypes.Information, message, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="command"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public static void LogWarning(this ICommand command, string message, params string[] parameters)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        Log(command, LogTypes.Warning, message, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="command"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public static void LogError(this ICommand command, string message, params string[] parameters)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        Log(command, LogTypes.Error, message, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="command"></param>
    /// <param name="exc"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    public static void LogError(this ICommand command, Exception exc, string message = "", params string[] parameters)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (exc == null) throw new ArgumentNullException(nameof(exc));
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        Log(command, LogTypes.Error, message, parameters);
        Log(command, LogTypes.Error, exc.Message);
        Log(command, LogTypes.Error, exc.StackTrace ?? string.Empty);
    }
}