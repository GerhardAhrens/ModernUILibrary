//-----------------------------------------------------------------------
// <copyright file="ExpandoObjectExtensions.cs" company="Lifeprojects.de">
//     Class: ExpandoObjectExtensions
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>12.02.2024</date>
//
// <summary>
// Extensions Klasse für den Typ ExpandoObject
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Dynamic;

    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Verifies whether the target instances contains a property with the given name.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="propertyName"></param>
        /// <returns>True if property exists; false otherwise.</returns>
        public static bool ContainsProperty(this ExpandoObject @this, string propertyName)
        {
            return ((IDictionary<string, object>)@this).ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets the property value. 
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="this">Target instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property value. Default type value if not found.</returns>
        public static T GetPropertyValue<T>(this ExpandoObject @this, string propertyName)
        {
            if (((IDictionary<string, object>)@this).TryGetValue(propertyName, out var value))
            {
                return (T)value;
            }

            return default;
        }

        /// <summary>
        /// Tries t o get the value for the named property.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="this">Target instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">Out: The value.</param>
        /// <returns>True if a property is found</returns>
        public static bool TryGetPropertyValue<T>(this ExpandoObject @this, string propertyName, out T value)
        {
            if (((IDictionary<string, object>)@this).TryGetValue(propertyName, out var innerValue))
            {
                value = (T)innerValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Creates (or overrides) the dynamic property. 
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="this">Target instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        public static void CreateProperty<T>(this ExpandoObject @this, string propertyName)
        {
            ((IDictionary<string, object>)@this)[propertyName] = default(T);
        }

        /// <summary>
        /// Sets the value to the dynamic property. If the property does not exists, it will be created.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="this">Target instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value of the property</param>
        public static void SetPropertyValue<T>(this ExpandoObject @this, string propertyName, T value)
        {
            ((IDictionary<string, object>)@this)[propertyName] = value;
        }

        /// <summary>
        /// Checks whether the property value is null.
        /// </summary>
        /// <param name="this">Target instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True if null, false otherwise.</returns>
        public static bool IsNull(this ExpandoObject @this, string propertyName)
        {
            if (@this.TryGetPropertyValue<object>(propertyName, out var value))
            {
                return value == null;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the property value is null or equals the default value for the given type.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="this">Target instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True if null, false otherwise.</returns>

        public static bool IsNullOrDefault<T>(this ExpandoObject @this, string propertyName)
        {
            if (@this.TryGetPropertyValue<T>(propertyName, out T value))
            {
                return value == null || default(T).Equals(value);
            }

            return true;
        }
    }
}