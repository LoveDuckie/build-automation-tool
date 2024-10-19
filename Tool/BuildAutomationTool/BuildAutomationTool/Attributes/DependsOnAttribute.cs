namespace BuildAutomationTool.Attributes;

/// <summary>
///     
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependsOnAttribute : Attribute
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="dependsOnType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DependsOnAttribute(Type dependsOnType)
    {
        DependsOnType = dependsOnType ?? throw new ArgumentNullException(nameof(dependsOnType));
    }

    public Type DependsOnType { get; private set; }
}