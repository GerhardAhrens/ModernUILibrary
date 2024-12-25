//-----------------------------------------------------------------------
// <copyright file="EnumDescripionToDictionary.cs" company="Lifeprojects.de">
//     Class: EnumDescripionToDictionary
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>
// Die Klasse konvertiert ein Enum zu einem DictionaryOfT
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public class EnumDescripionToDictionary<TEnum> : Dictionary<TEnum, string> where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        public EnumDescripionToDictionary()
        {
            if (!typeof(TEnum).IsEnum)
            {
                // There is no BCL exception suitable for invalid generic type parameters.
                throw new NotSupportedException("Generic parameter T must be of type Enum.");
            }

            // These binding flags exclude the hidden, generated instance field named 'value__'. 
            var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var descAtt = field.GetCustomAttribute<DescriptionAttribute>();
                if (descAtt != null)
                {
                    var desc = descAtt.Description;
                    if (!string.IsNullOrEmpty(desc))
                    {
                        Add((TEnum)Enum.Parse(typeof(TEnum), field.Name), desc);
                        continue;
                    }
                }
                Add((TEnum)Enum.Parse(typeof(TEnum), field.Name), field.Name);
            }
        }

        /// <summary>
        /// This method hides the <see cref="Remove"/> method of the base <see cref="Dictionary{TKey, TValue}"/> class 
        /// to prevent the removal of items. As this dictionary describes an <see cref="Enum"/> type, its contents 
        /// may not be changed at runtime.
        /// </summary>
        /// <param name="key">Key of the dictionary item to be removed.</param>
        /// <remarks>
        /// It is not necessary to hide the <see cref="Dictionary{TKey, TValue}.Add"/> method, as using any of the enum
        /// values as a key for add will cause an <see cref="ArgumentException"/> because of the duplicate key.
        /// </remarks>
        public new void Remove(TEnum key)
        {
            throw new InvalidOperationException(string.Format("Items may not be removed from this dictionary as type '{0}' has not changed.", typeof(TEnum).Name));
        }
    }
}
