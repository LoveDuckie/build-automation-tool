using System.ComponentModel;
using System.Reflection;
using BuildAutomationTool.Extensions;
using BuildAutomationTool.Tasks;
using LibGit2Sharp;
using Spectre.Console;
using Spectre.Console.Cli;
using Tree = Spectre.Console.Tree;

namespace BuildAutomationTool.Commands;

/// <summary>
/// 
/// </summary>
public sealed class RunCommand : AsyncCommand<RunCommand.Settings>
{
    /// <summary>
    /// Represents the settings required to execute a command, including paths, task names, and Git credentials.
    /// </summary>
    public sealed class Settings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the absolute path to where the scripts are located.
        /// This property is used to specify the directory containing the scripts
        /// to be executed by the command.
        /// </summary>
        [Description("The absolute path to where the scripts are located.")]
        [CommandOption("-s|--scripts-path")]
        public string? ScriptsPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the target build task to be executed.
        /// This property is necessary for specifying which build task the command should run.
        /// </summary>
        [Description("The target build task to be executed.")]
        [CommandOption("-t|--task-name")]
        public string? TaskName { get; set; }

        /// <summary>
        /// Gets the Git username for authentication.
        /// This property is used to authenticate the user when accessing Git repositories.
        /// </summary>
        [Description("The Git username for authentication.")]
        [CommandOption("-u|--git-username")]
        [DefaultValue("lucshelton@gmail.com")]
        public string GitUserName { get; init; } = string.Empty;

        /// <summary>
        /// Gets or initializes the Git token or password for authentication.
        /// This property is used to authenticate the user when accessing Git repositories.
        /// </summary>
        [Description("The Git token or password for authentication.")]
        [CommandOption("-p|--git-token")]
        [DefaultValue("")]
        public string GitToken { get; init; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether to execute the task exclusively.
        /// If set to true, the task will run in isolation from other tasks.
        /// </summary>
        [Description("Whether to execute the task exclusively.")]
        [CommandOption("--exclusive")]
        [DefaultValue(true)]
        public bool Exclusive { get; init; }


        /// <summary>
        /// Gets or sets the list of library names or paths to be included in the build process.
        /// </summary>
        [CommandOption("-l|--library <VALUE>")]
        public List<string>? Libraries { get; set; }
    }

    /// <summary>
    ///     Attempt to retrieve the absolute path to the root of the repository.
    /// </summary>
    /// <param name="repositoryRootPath">Output parameter for the repository path.</param>
    /// <returns>True if repository root was found, otherwise false.</returns>
    private bool TryGetRepositoryRootPath(out string? repositoryRootPath)
    {
        string assemblyPath = Assembly.GetExecutingAssembly().Location;
        string? repoRoot = Repository.Discover(Path.GetDirectoryName(assemblyPath));
        repositoryRootPath = string.IsNullOrEmpty(repoRoot) ? null : Path.GetDirectoryName(repoRoot.TrimEnd('\\'));
        return repositoryRootPath != null;
    }

    /// <summary>
    ///     Validates the command input, including checking for valid script paths and task names.
    /// </summary>
    /// <param name="context">Command context.</param>
    /// <param name="settings">Command settings.</param>
    /// <returns>Validation result indicating success or failure.</returns>
    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(settings);

        // Validate scripts path
        if (!string.IsNullOrEmpty(settings.ScriptsPath) && !Path.IsPathFullyQualified(settings.ScriptsPath))
        {
            return ValidationResult.Error("The script path is invalid. It must be an absolute path.");
        }

        // Discover repository root if no script path is provided
        if (string.IsNullOrEmpty(settings.ScriptsPath))
        {
            this.LogWarning("The script path is empty. Attempting to locate repository root...");
            if (TryGetRepositoryRootPath(out string? repositoryRootPath))
            {
                this.LogInformation($"Repository Root found: \"{repositoryRootPath}\"");
                if (repositoryRootPath != null) settings.ScriptsPath = Path.Combine(repositoryRootPath, "Scripts");
            }
            else
            {
                return ValidationResult.Error("Could not find repository root. Provide a valid script path.");
            }
        }

        // Set default task name if not provided
        settings.TaskName ??= "Stage1Task";

        // Validate task name exists in build tasks
        if (!BuildTask.GetBuildTaskTypes().ContainsKey(settings.TaskName))
        {
            return ValidationResult.Error(
                $"The task name '{settings.TaskName}' is invalid or does not exist. " +
                "Ensure that it is properly defined and derives from BuildTask."
            );
        }

        return base.Validate(context, settings);
    }

    /// <summary>
    ///     Executes the build task asynchronously, providing progress updates to the console.
    /// </summary>
    /// <param name="context">The runtime context of the build.</param>
    /// <param name="settings">Contains the settings defined at runtime.</param>
    /// <returns>Returns an integer exit code (0 for success, non-zero for failure).</returns>
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(settings);

        // Ensure TaskName is valid
        if (settings.TaskName == null)
        {
            throw new ArgumentNullException(nameof(settings.TaskName), "Task name cannot be null.");
        }

        try
        {
            this.LogInformation($"Running Task: \"{settings.TaskName}\"");

            // Initialize build context and runner
            BuildContext buildContext = new BuildContext(settings);
            TaskRunner runner = new TaskRunnerParallel();
            // TaskRunner runner = new TaskRunnerSequential();
            runner.Initialize();
            Tree? taskTree = null;
            // Run tasks with progress display
            await AnsiConsole.Progress()
                .AutoRefresh(true)
                .AutoClear(false)
                .HideCompleted(false)
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn()
                )
                .StartAsync(async progressContext =>
                {
                    taskTree = await runner.ExecuteTasksAsync(BuildTask.GetBuildTaskTypes()[settings.TaskName],
                        buildContext, progressContext);
                });

            if (taskTree != null) AnsiConsole.Write(taskTree);

            return 0; // Return success
        }
        catch (Exception e)
        {
            this.LogError(e);
            return 1; // Return failure code
        }
    }
}