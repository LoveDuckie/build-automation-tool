namespace BuildAutomationTool;

/// <summary>
///     The resulting value.
/// </summary>
public sealed class ProcessRunnerResult
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="standardOutput"></param>
    /// <param name="exitCode"></param>
    /// <param name="standardError"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProcessRunnerResult(int exitCode, string standardOutput, string standardError)
    {
        ExitCode = exitCode;
        StandardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
        StandardError = standardError ?? throw new ArgumentNullException(nameof(standardError));
    }

    #region Properties

    /// <summary>
    ///     
    /// </summary>
    public int ExitCode { get; private set; }

    /// <summary>
    ///     
    /// </summary>
    public string StandardOutput { get; private set; }

    /// <summary>
    ///     
    /// </summary>
    public string StandardError { get; private set; }

    #endregion
}