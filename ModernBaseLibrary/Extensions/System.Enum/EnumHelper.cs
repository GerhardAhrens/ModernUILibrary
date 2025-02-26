namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static partial class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            // if this is a Flags enum, value may contain multiple items
            var values = value.ToString().Split(',').Select(s => s.Trim()).ToList();
            var enumType = value.GetType();

            var result = string.Join(" | ", values.Select(enumValue => enumType.GetMember(enumValue)
                                                                           .FirstOrDefault()
                                                                           ?.GetCustomAttribute<DescriptionAttribute>()
                                                                           ?.Description
                                                                       ?? enumValue.ToString()));

            return result;
        }

        public static IReadOnlyCollection<T> GetValues<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            var itemType = typeof(T);

            if (!itemType.IsEnum)
            {
                throw new ArgumentException("Type '" + itemType.Name + "' is not an enum");
            }

            var fields = itemType
                .GetFields()
                .Where(field => field.IsLiteral)
                .ToList();

            return fields
                .Select(field => field.GetValue(itemType))
                .Cast<T>()
                .ToList()
                .AsReadOnly();
        }
    }
}