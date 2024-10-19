using Spectre.Console.Cli;

namespace BuildAutomationTool.Commands;

/// <summary>
///     
/// </summary>
public class SetupTeamCityCommand : AsyncCommand<SetupTeamCityCommand.Settings>
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Settings : CommandSettings
    {
        /// <summary>
        ///     
        /// </summary>
        [CommandOption("-h|--host")]
        public string? Host { get; init; }
        /// <summary>
        ///     
        /// </summary>
        [CommandOption("-u|--username")]
        public string? User { get; init; }
        /// <summary>
        ///     
        /// </summary>
        [CommandOption("-p|--password")]
        public string? Password { get; init; }

    }
    
    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        return Task.FromResult(0);
    }
}