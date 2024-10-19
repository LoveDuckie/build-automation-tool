// See https://aka.ms/new-console-template for more information

using BuildAutomationTool.Commands;
using Spectre.Console.Cli;

namespace BuildAutomationTool;

/// <summary>
///     
/// </summary>
public static class Program
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<RunCommand>("run");
            config.AddCommand<CleanCommand>("clean");
            config.AddCommand<SetupTeamCityCommand>("setup-teamcity");
            config.AddCommand<SetupJenkinsCommand>("setup-jenkins");
        });
        app.Run(args);
    }
}