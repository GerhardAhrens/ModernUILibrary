/*
 * <copyright file="TypeExtensions.cs" company="Lifeprojects.de">
 *     Class: TypeExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>28.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Data Types
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

//-----------------------------------------------------------------------
// <copyright file="StringMaskExtensions.cs" company="Lifeprojects.de">
//     Class: StringMaskExtensions
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.07.2019</date>
//
// <summary>Extensions Class for Data Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string>
        {
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(object), "object" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(void), "void" },
        { typeof(byte?), "byte?" },
        { typeof(sbyte?), "sbyte?" },
        { typeof(short?), "short?" },
        { typeof(ushort?), "ushort?" },
        { typeof(int?), "int?" },
        { typeof(uint?), "uint?" },
        { typeof(long?), "long?" },
        { typeof(ulong?), "ulong?" },
        { typeof(float?), "float?" },
        { typeof(double?), "double?" },
        { typeof(decimal?), "decimal?" },
        { typeof(bool?), "bool?" },
        { typeof(char?), "char?" }
        };

        /// <summary>
        /// Die Methode gibt von einem Typ den Alias zurück
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns></returns>
        public static string Alias(this Type type)
        {
            return TypeAliases.ContainsKey(type) ? TypeAliases[type] : string.Empty;
        }

        /// <summary>
        /// Die Methode gibt von einem Typ den Alias oder den Namen zurück
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns></returns>
        public static string AliasOrName(this Type type)
        {
            return TypeAliases.ContainsKey(type) ? TypeAliases[type] : type.Name;
        }

        public static string GetFriendlyTypeName(this Type @this)
        {
            var typeName = @this.Name.StripStartingWith("`");
            var genericArgs = @this.GetGenericArguments();
            if (genericArgs.Length > 0)
            {
                typeName += "<";
                foreach (var genericArg in genericArgs)
                {
                    typeName += genericArg.GetFriendlyTypeName() + ", ";
                }
                typeName = typeName.TrimEnd(',', ' ') + ">";
            }

            return typeName;
        }

        public static string ToFriendlyName(this Type @this)
        {
            if (@this.IsArray)
            {
                return @this.GetFriendlyNameOfArrayType();
            }

            if (@this.IsGenericType)
            {
                return @this.GetFriendlyNameOfGenericType();
            }

            if (@this.IsPointer)
            {
                return @this.GetFriendlyNameOfPointerType();
            }

            var aliasName = default(string);
            return TypeAliases.TryGetValue(@this, out aliasName) ? aliasName : @this.Name;
        }

        public static string GetFriendlyName(this Type @this)
        {
            if (@this.IsArray)
            {
                return @this.GetFriendlyNameOfArrayType();
            }

            if (@this.IsGenericType)
            {
                return @this.GetFriendlyNameOfGenericType();
            }

            if (@this.IsPointer)
            {
                return @this.GetFriendlyNameOfPointerType();
            }

            var aliasName = default(string);
            return TypeAliases.TryGetValue(@this, out aliasName) ? aliasName : @this.Name;
        }

        public static bool IsCollection<T>(this T @this)
        {
            bool result = false;

            if (@this == null)
            {
                return result;
            }

            Type valueType = @this.GetType();

            result = valueType.IsArray || typeof(IEnumerable<object>).IsAssignableFrom(valueType) || typeof(IEnumerable<T>).IsAssignableFrom(valueType);

            return result;
        }

        public static bool IsSimpleType(this Type @this)
        {
            return
               @this.IsValueType ||
               @this.IsPrimitive ||
               new[] {
               typeof(String),
               typeof(Decimal),
               typeof(DateTime),
               typeof(DateTimeOffset),
               typeof(TimeSpan),
               typeof(Guid)
               }.Contains(@this) || (Convert.GetTypeCode(@this) != TypeCode.Object);
        }

        /// <summary>
        /// Prüfen, ob ein Typ (null)-Werte annehmen kann
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType == false)
            {
                // kein Wertetyp, daher kann der Typ (null)-Werte annehmen
                return true;
            }

            // Wenn Nullable.GetUnderlyingType() einen übergeordneten Typ ermitteln kann,
            // bedeutet dies, dass dies ein Wertetyp ist, der (null)-Werte annehmen kann,
            // z.B. bool?
            return Nullable.GetUnderlyingType(type) is Type;
        }

        public static Type GetUnderlyingType(this MemberInfo @this)
        {
            switch (@this.MemberType)
            {
                case MemberTypes.Event:
                    {
                        return ((EventInfo)@this).EventHandlerType;
                    }
                case MemberTypes.Field:
                    {
                        return ((FieldInfo)@this).FieldType;
                    }
                case MemberTypes.Method:
                    {
                        return ((MethodInfo)@this).ReturnType;
                    }
                case MemberTypes.Property:
                    {
                        return ((PropertyInfo)@this).PropertyType;
                    }
                default:
                    throw new ArgumentException
                    (
                       "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }

        /// <summary>
        /// Die Methode prüft, ob der übergeben Typ nummerisch ist
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this Type @this)
        {
            var numType = typeof(INumber<>);
            var result = @this.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == numType));
            return result;
        }

        public static string GetName<T>(this Expression<Func<T>> action)
        {
            return GetNameFromMemberExpression(action.Body);
        }

        private static string GetNameFromMemberExpression(Expression expression)
        {
            if (expression is MemberExpression)
            {
                return (expression as MemberExpression).Member.Name;
            }
            else if (expression is UnaryExpression)
            {
                return GetNameFromMemberExpression((expression as UnaryExpression).Operand);
            }

            return "MemberNameUnknown";
        }

        private static string GetFriendlyNameOfArrayType(this Type @this)
        {
            var arrayMarker = string.Empty;
            while (@this.IsArray)
            {
                var commas = new string(Enumerable.Repeat(',', @this.GetArrayRank() - 1).ToArray());
                arrayMarker += $"[{commas}]";
                @this = @this.GetElementType();
            }

            return @this.GetFriendlyName() + arrayMarker;
        }

        private static string GetFriendlyNameOfGenericType(this Type type)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments().First().GetFriendlyName() + "?";
            }

            var friendlyName = type.Name;
            var indexOfBacktick = friendlyName.IndexOf('`');
            if (indexOfBacktick > 0)
            {
                friendlyName = friendlyName.Remove(indexOfBacktick);
            }

            var typeParameterNames = type
                .GetGenericArguments()
                .Select(typeParameter => typeParameter.GetFriendlyName());

            var joinedTypeParameters = string.Join(", ", typeParameterNames);

            return string.Format("{0}<{1}>", friendlyName, joinedTypeParameters);
        }

        private static string GetFriendlyNameOfPointerType(this Type type) => type.GetElementType().GetFriendlyName() + "*";
    }
}