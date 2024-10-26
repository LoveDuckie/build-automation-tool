using System.ComponentModel;
using System.Reflection;
using BuildAutomationTool.Attributes;

namespace BuildAutomationTool;

/// <summary>
///
/// </summary>
public static class EnumHelpers
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">The value to retrieve the attribute from.</param>
    /// <returns></returns>
    public static string GetEnumDescription(Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
        }
        return value.ToString();
    }

    /// <summary>
    ///     Get the color of the enumeration field
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetEnumLogColor(Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            if (Attribute.GetCustomAttribute(field, typeof(LogColorAttribute)) is LogColorAttribute attribute)
            {
                return attribute.Color;
            }
        }
        return value.ToString();
    }
}