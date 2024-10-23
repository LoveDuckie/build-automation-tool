using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace BuildAutomationTool.Commands;

/// <summary>
/// 
/// </summary>
public sealed class CleanCommand : AsyncCommand<CleanCommand.Settings>
{
    /// <summary>
    ///     Get the relative path to the script
    /// </summary>
    public string ScriptPath { get; } = Path.Combine("clean.bat");

    /// <summary>
    ///     
    /// </summary>
    public sealed class Settings : CommandSettings
    {
        /// <summary>
        ///     
        /// </summary>
        [Description("The absolute path to where the scripts are located.")]
        [CommandOption("-p|--scripts-path")]
        public string? ScriptsPath { get; init; }
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context">The build context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>Returns the validation result</returns>
    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        if (settings.ScriptsPath != null && !Path.IsPathFullyQualified(settings.ScriptsPath))
        {
            return ValidationResult.Error("The script path is invalid. It must be an absolute path.");
        }


        return base.Validate(context, settings);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(settings);

        await Task.Delay(new Random().Next(1000, 10000));
        return await Task.FromResult(0);
    }
}