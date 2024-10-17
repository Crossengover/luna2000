using System.ComponentModel;

namespace luna2000.Extensions;

public static class EntityExtensions
{
    public static string? GetDescription(this Enum en)
    {
        var type = en.GetType().GetField(en.ToString());
        var attributes = type.GetCustomAttributes(false);

        var descriptionAttribute = Array.Find(attributes, a => a.GetType().Name == "DescriptionAttribute");
        return (descriptionAttribute as DescriptionAttribute)?.Description;
    }
}