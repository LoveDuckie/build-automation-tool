namespace BuildAutomationTool.Attributes;

/// <summary>
///     
/// </summary>
public sealed class BuildTaskDescriptionAttribute : Attribute
{
    public BuildTaskDescriptionAttribute(string name)
    {
        Description = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string? Description { get; private set; }
    
}