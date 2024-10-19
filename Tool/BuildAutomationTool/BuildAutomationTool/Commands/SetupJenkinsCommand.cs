using Spectre.Console.Cli;

namespace BuildAutomationTool.Commands;

/// <summary>
///     
/// </summary>
public class SetupJenkinsCommand : AsyncCommand<SetupTeamCityCommand.Settings>
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
        [CommandOption("-t|--token")]
        public string? Token { get; init; }

    }
    
    /// <summary>
    ///     
    /// </summary>
    /// <param name="context"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public override Task<int> ExecuteAsync(CommandContext context, SetupTeamCityCommand.Settings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(settings);
        return Task.FromResult(0);
    }
}