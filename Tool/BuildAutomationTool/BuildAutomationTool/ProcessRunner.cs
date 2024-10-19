using System.Diagnostics;
using BuildAutomationTool.Interfaces;

namespace BuildAutomationTool;

/// <summary>
///     
/// </summary>
public sealed class ProcessRunnerAwaiter : IAwaiter<ProcessRunnerResult>
{
    #region Properties

    /// <summary>
    ///     The process runner
    /// </summary>
    public ProcessRunner Runner { get; private set; }

    /// <summary>
    ///     The process
    /// </summary>
    public Process Process { get; }


    /// <summary>
    ///
    /// </summary>
    private Action? Continuation { get; set; }


    /// <summary>
    ///     Read-only property for indicating if the process has terminated.
    /// </summary>
    public bool IsCompleted => Process?.HasExited ?? false;

    #endregion


    /// <summary>
    ///     
    /// </summary>
    /// <param name="processRunner"></param>
    /// <param name="process"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProcessRunnerAwaiter(ProcessRunner processRunner, Process process)
    {
        Runner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        Process = process ?? throw new ArgumentNullException(nameof(process));
        Process.EnableRaisingEvents = true;
        Process.Exited += OnProcessExited;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnProcessExited(object? sender, EventArgs e)
    {
        Continuation?.Invoke();
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="continuation"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void OnCompleted(Action continuation)
    {
        Continuation = continuation ?? throw new ArgumentNullException(nameof(continuation));
    }

    /// <summary>
    ///     
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ProcessRunnerResult GetResult()
    {
        var runnerResult = new ProcessRunnerResult(Process.ExitCode, Process.StandardOutput.ReadToEnd(),
            Process.StandardError.ReadToEnd());

        Process?.Dispose();

        return runnerResult;
    }
}

/// <summary>
///
/// </summary>
public struct ProcessRunnerParameters
{
    /// <summary>
    ///
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <summary>
    ///
    /// </summary>
    public string Arguments { get; init; }

    /// <summary>
    /// /
    /// </summary>
    public string FilePath { get; init; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, string> EnvironmentVariables { get; init; }

    /// <summary>
    ///
    /// </summary>
    public Action<string> StandardOutputHandler { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Action<string> StandardErrorHandler { get; set; }
}

/// <summary>
///     The process runner.
/// </summary>
public sealed class ProcessRunner : IAwaitable<ProcessRunnerResult>, IDisposable
{
    /// <summary>
    ///
    /// </summary>
    private string FilePath { get; }

    /// <summary>
    ///
    /// </summary>
    private string? CurrentWorkingDirectory { get; }

    /// <summary>
    ///     
    /// </summary>
    private string[]? Arguments { get; }

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, string>? EnvironmentVariables { get; }

    /// <summary>
    ///
    /// </summary>
    private Action<string> StandardOutputHandler { get; set; }

    /// <summary>
    ///
    /// </summary>
    private Action<string> StandardErrorHandler { get; set; }

    /// <summary>
    ///     
    /// </summary>
    /// <returns>Returns the awaiter</returns>
    public IAwaiter<ProcessRunnerResult> GetAwaiter() => new ProcessRunnerAwaiter(this, CreateProcess());

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameters"></param>
    public ProcessRunner(ProcessRunnerParameters parameters)
    {
        FilePath = parameters.FilePath;
        if (parameters.WorkingDirectory != null) CurrentWorkingDirectory = parameters.WorkingDirectory;
        Arguments = parameters.Arguments.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        EnvironmentVariables = parameters.EnvironmentVariables;
        StandardErrorHandler = parameters.StandardErrorHandler;
        StandardOutputHandler = parameters.StandardOutputHandler;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <returns></returns>
    private Process CreateProcess()
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            WorkingDirectory = CurrentWorkingDirectory,
            FileName = FilePath,
            Arguments = Arguments != null
                ? string.Join(' ', Arguments)
                : string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        if (EnvironmentVariables != null)
        {
            foreach (var kv in EnvironmentVariables)
            {
                startInfo.EnvironmentVariables.Add(kv.Key, kv.Value);
            }
        }

        process.EnableRaisingEvents = true;
        process.OutputDataReceived += (_, args) =>
        {
            if (args.Data != null) this.StandardOutputHandler?.Invoke(args.Data);
        };
        process.ErrorDataReceived += (_, args) =>
        {
            if (args.Data != null) this.StandardErrorHandler?.Invoke(args.Data);
        };
        process.StartInfo = startInfo;


        process.Start();
        return process;
    }

    /// <summary>
    ///     Dispose the process if it still exists.
    /// </summary>
    public void Dispose()
    {
    }
}