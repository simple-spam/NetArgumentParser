using System;
using System.Linq;
using System.Reflection;
using NetArgumentParser.Options;

namespace NetArgumentParser.Attributes.Extensions;

internal static class PropertyInfoExtensions
{
    internal static bool HasAttribute<T>(this PropertyInfo propertyInfo)
        where T : Attribute
    {
        ExtendedArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));
        return propertyInfo.GetCustomAttribute<T>() is not null;
    }

    internal static bool HasOptionAttribute(this PropertyInfo propertyInfo)
    {
        ExtendedArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));
        return propertyInfo.HasAttribute<CommonOptionAttribute>();
    }

    internal static bool HasOptionGroupAttribute(this PropertyInfo propertyInfo)
    {
        ExtendedArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));
        return propertyInfo.HasAttribute<OptionGroupAttribute>();
    }

    internal static bool HasMutuallyExclusiveOptionGroupAttribute(this PropertyInfo propertyInfo)
    {
        ExtendedArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));
        return propertyInfo.HasAttribute<MutuallyExclusiveOptionGroupAttribute>();
    }

    internal static bool HasSubcommandAttribute(this PropertyInfo propertyInfo)
    {
        ExtendedArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        return propertyInfo.CustomAttributes
            .Any(t => t.AttributeType == typeof(SubcommandAttribute));
    }

    internal static ICommonOption? CreateOption(this PropertyInfo propertyInfo, object source)
    {
        ExtendedArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));
        ExtendedArgumentNullException.ThrowIfNull(source, nameof(source));

        CommonOptionAttribute? attribute = propertyInfo.GetCustomAttribute<CommonOptionAttribute>();

        return attribute?.CreateOption(source, propertyInfo);
    }
}
