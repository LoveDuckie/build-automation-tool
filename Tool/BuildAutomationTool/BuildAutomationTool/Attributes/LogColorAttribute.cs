namespace BuildAutomationTool.Attributes;

/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class LogColorAttribute : Attribute
{
    public LogColorAttribute(string color)
    {
        Color = color ?? throw new ArgumentNullException(nameof(color));
    }

    /// <summary>
    ///
    /// </summary>
    public string Color { get; private set; }
}