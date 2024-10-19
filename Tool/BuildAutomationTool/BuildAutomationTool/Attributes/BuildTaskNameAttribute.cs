namespace BuildAutomationTool.Attributes;

public sealed class BuildTaskNameAttribute : Attribute
{
    public BuildTaskNameAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string? Name { get; private set; }
    
}