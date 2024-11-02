using System.Text.RegularExpressions;
using BuildAutomationTool.Attributes;
using LibGit2Sharp;
using Spectre.Console;

namespace BuildAutomationTool.Tasks;

/// <summary>
///     The Stage 4 task.
/// </summary>
[DependsOn(typeof(Stage3ATask))]
[DependsOn(typeof(Stage3BTask))]
[DependsOn(typeof(Stage3CTask))]
[BuildTaskName("Stage 4")]
public sealed class Stage4Task : BuildTask
{
    /// <summary>
    ///     The pattern for validating the versioning information.
    /// </summary>
    private static readonly Regex VersionPattern =
        new Regex(@"^(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<patch>[0-9]+)$", RegexOptions.Compiled);

    /// <summary>
    ///     Determine whether the task can run
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override bool CanRun(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return (!string.IsNullOrEmpty(context.Settings.GitToken) &&
                !string.IsNullOrEmpty(context.Settings.GitUserName));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void UpdateVersion(BuildContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        string? versionFilePath =
            context.GetScriptsPath("gamedata", "version.txt") ?? throw new ArgumentNullException(nameof(context));

        if (string.IsNullOrEmpty(versionFilePath))
        {
            throw new ArgumentNullException(nameof(versionFilePath));
        }

        int major = 1, minor = 0, patch = 0;
        if (File.Exists(versionFilePath))
        {
            context.LogWarning($"Found version file: {versionFilePath}");
            string versionFileRaw = File.ReadAllText(versionFilePath);
            if (VersionPattern.IsMatch(versionFileRaw))
            {
                context.LogWarning($"Detected Version: {versionFileRaw}");
                Match versionMatch = VersionPattern.Match(versionFileRaw);
                major = int.Parse(versionMatch.Groups["major"].Value);
                minor = int.Parse(versionMatch.Groups["minor"].Value);
                patch = int.Parse(versionMatch.Groups["patch"].Value);

                patch++;
            }
        }

        string versionContent = $"{major}.{minor}.{patch}";
        context.LogWarning($"New Version: {versionContent}");

        File.WriteAllText(versionFilePath, versionContent);
    }

    /// <summary>
    ///     Get the versioning information
    /// </summary>
    /// <param name="context">The runtime context</param>
    /// <returns>Returns the versioning information</returns>
    private string? GetVersion(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        string? versionFilePath =
            context.GetScriptsPath("gamedata", "version.txt") ?? throw new ArgumentNullException(nameof(context));

        if (!File.Exists(versionFilePath)) return null;

        string? versionFileRaw = File.ReadAllText(versionFilePath);
        return VersionPattern.IsMatch(versionFileRaw) ? versionFileRaw : null;
    }

    /// <summary>
    ///     Runs before the task launches
    /// </summary>
    /// <param name="context"></param>
    public override void BeforeExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    ///     Helper method to convert SSH URL to HTTPS URL
    /// </summary>
    /// <param name="sshUrl"></param>
    /// <returns>Returns the newly fixed remote URL</returns>
    private static string ConvertSshToHttps(string sshUrl)
    {
        ArgumentNullException.ThrowIfNull(sshUrl);
        // Example conversion from SSH to HTTPS:
        // From: git@github.com:username/repository.git
        // To:   https://github.com/username/repository.git

        if (sshUrl.StartsWith("git@"))
        {
            // Parse the SSH URL to create an HTTPS URL
            var httpsUrl = sshUrl.Replace("git@", "https://").Replace(":", "/");
            return httpsUrl;
        }

        if (sshUrl.StartsWith("ssh://"))
        {
            // Handle SSH URLs that start with "ssh://"
            var httpsUrl = sshUrl.Replace("ssh://git@", "https://").Replace(":", "/");
            return httpsUrl;
        }

        // Return the original URL if no changes were made (unlikely to happen)
        return sshUrl;
    }

    /// <summary>
    ///     Executes the task asynchronously, checking the repository state, adding changes, committing, and pushing to GitHub.
    /// </summary>
    /// <param name="context">The runtime context of the build.</param>
    /// <param name="progressTask">The task context.</param>
    /// <returns>Returns the exit code for executing the task</returns>
    protected override async Task<int> OnExecuteAsync(BuildContext context, ProgressTask progressTask)
    {
        ArgumentNullException.ThrowIfNull(progressTask);
        ArgumentNullException.ThrowIfNull(context);

        // ValidatePaths(context);
        //
        // UpdateVersion(context);
        //
        // var version = GetVersion(context);
        //
        // if (string.IsNullOrEmpty(version))
        // {
        //     version = "1.0.0";
        // }
        //
        // LogInformation($"Game Data Version: {version}");
        //
        // string commitMessage = $"Game Data Version: \"{version}\"";
        // string repositoryRoot = DiscoverRepository(context);
        //
        // using (var repo = new Repository(repositoryRoot))
        // {
        //     // Retrieve the remote
        //     Remote remote = repo.Network.Remotes["origin"];
        //     string remoteUrl = remote.Url;
        //     if (remoteUrl.StartsWith("ssh://") || remoteUrl.StartsWith("git@"))
        //     {
        //         context.LogWarning("Detected SSH remote URL. Converting to HTTPS.");
        //         string httpsUrl = ConvertSshToHttps(remoteUrl);
        //         repo.Network.Remotes.Update("origin", r => r.Url = httpsUrl);
        //     }
        //
        //     if (HasChangesToCommit(repo))
        //     {
        //         AddChangesToIndex(repo, context);
        //         CommitAndPushChanges(repo, commitMessage, context.Settings.GitUserName, context.Settings.GitToken);
        //     }
        // }
        int delayTime = new Random().Next(1000, 5000);
        await Task.Delay(delayTime);
        return await Task.FromResult(0);
    }

    /// <summary>
    ///     Validate the paths.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void ValidatePaths(BuildContext context)
    {
        var paths = new[]
        {
            context.GetScriptsPath("stage_3a_files"),
            context.GetScriptsPath("stage_3b_files"),
            context.GetScriptsPath("stage_3c_files")
        };

        foreach (string path in paths)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            }

            if (!Path.IsPathRooted(path))
            {
                throw new InvalidOperationException($"Invalid absolute path: \"{path}\"");
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private string DiscoverRepository(BuildContext context)
    {
        string repoRoot = Repository.Discover(context.Settings.ScriptsPath);
        if (string.IsNullOrEmpty(repoRoot))
        {
            throw new InvalidOperationException("Unable to locate the repository.");
        }

        return repoRoot;
    }

    /// <summary>
    ///     Determine if there are any changes to commit
    /// </summary>
    /// <param name="repository"></param>
    /// <returns></returns>
    private bool HasChangesToCommit(Repository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        RepositoryStatus status = repository.RetrieveStatus();
        return status.IsDirty;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="context">The runtime build context.</param>
    private void AddChangesToIndex(Repository repository, BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(context);
        var status = repository.RetrieveStatus();
        foreach (var file in status)
        {
            if (file.FilePath.EndsWith(".txt") && file.FilePath.StartsWith("Scripts"))
            {
                if (file.State is FileStatus.ModifiedInWorkdir or FileStatus.NewInWorkdir
                    or FileStatus.DeletedFromWorkdir)
                {
                    repository.Index.Add(file.FilePath.Trim(Path.PathSeparator));
                    // LogInformation($"File added to index: {file.FilePath}");
                }
            }
        }

        repository.Index.Write();
    }

    /// <summary>
    ///     Commit and push the changes.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="commitMessage"></param>
    /// <param name="userName"></param>
    /// <param name="token"></param>
    private void CommitAndPushChanges(Repository repository, string commitMessage, string userName, string token)
    {
        var author = new Signature(userName, $"{userName}", DateTime.Now);
        Commit commit = repository.Commit(commitMessage, author, author);

        var options = new PushOptions
        {
            CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
            {
                Username = userName,
                Password = token
            }
        };

        LogInformation($"Commit created: {commit.Id}");
        repository.Network.Push(repository.Network.Remotes["origin"], @"refs/heads/main", options);
        LogInformation($"Pushed commit: {commit.Id} to remote.");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context">The context</param>
    public override void AfterExecute(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}