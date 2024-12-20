//-----------------------------------------------------------------------
// <copyright file="ObjectExtension.cs" company="Lifeprojects.de">
//     Class: ObjectExtension
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Extension Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Text.Json;

    [SupportedOSPlatform("windows")]
    public static partial class ObjectExtension
    {
        /*
        public static long GetObjectSize(this object @this)
        {
            long size = 0;
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011 // Typ oder Element ist veraltet
                formatter.Serialize(s, @this);
#pragma warning restore SYSLIB0011 // Typ oder Element ist veraltet
                size = s.Length;
            }

            return size;
        }
        */

        /// <summary>
        /// A T extension method that that return the first not null value (including the @this).
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value.</returns>
        public static T Coalesce<T>(this T @this, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (T value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// A T extension method that that return the first not null value (including the @this) or a default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value or a default value.</returns>
        public static T CoalesceOrDefault<T>(this T @this, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (T value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return default(T);
        }

        /// <summary>
        ///     A T extension method that that return the first not null value (including the @this) or a default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value or a default value.</returns>
        public static T CoalesceOrDefault<T>(this T @this, Func<T> defaultValueFactory, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (T value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return defaultValueFactory();
        }

        /// <summary>
        /// A T extension method that that return the first not null value (including the @this) or a default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value or a default value.</returns>
        public static T CoalesceOrDefault<T>(this T @this, Func<T, T> defaultValueFactory, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (T value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return defaultValueFactory(@this);
        }

        /// <summary>
        /// 	Converts an object to the specified target type or returns the default value if
        ///     those 2 types are not convertible.
        ///     <para>
        ///     If the <paramref name="value"/> can't be convert even if the types are 
        ///     convertible with each other, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "@this">The @this.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object @this)
        {
            return @this.ConvertTo(default(T));
        }

        /// <summary>
        /// 	Converts an object to the specified target type or returns the default value if
        ///     those 2 types are not convertible.
        ///     <para>
        ///     If the <paramref name="value"/> can't be convert even if the types are 
        ///     convertible with each other, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "@this">The @this.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object @this, T defaultValue)
        {
            if (@this != null)
            {
                var targetType = typeof(T);

                if (@this.GetType() == targetType) return (T)@this;

                var converter = TypeDescriptor.GetConverter(@this);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return (T)converter.ConvertTo(@this, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return (T)converter.ConvertFrom(@this);
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Converts an object to the specified target type or returns the default value if
        /// those 2 types are not convertible.
        /// <para>Any exceptions are optionally ignored (<paramref name="ignoreException"/>).</para>
        /// <para>
        /// If the exceptions are not ignored and the <paramref name="value"/> can't be convert even if 
        /// the types are convertible with each other, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <param name = "ignoreException">if set to <c>true</c> ignore any exception.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value, T defaultValue, bool ignoreException)
        {
            if (ignoreException)
            {
                try
                {
                    return value.ConvertTo<T>();
                }
                catch
                {
                    return defaultValue;
                }
            }
            return value.ConvertTo<T>();
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentNullException"/> 
        /// if the the value is null.
        /// </summary>
        /// <param name="this">The value to test.</param>
        /// <param name="message">The message to display if the value is null.</param>
        /// <param name="name">The name of the parameter being tested.</param>
        public static void ExceptionIfNullOrEmpty(this object @this, string message, string name)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        public static bool ObjectPropertiesEqual<T>(this T @this, T to, params string[] ignore) where T : class
        {
            if (@this != null && to != null)
            {
                var type = @this.GetType();
                var ignoreList = new List<string>(ignore);
                foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (ignoreList.Contains(pi.Name))
                    {
                        continue;
                    }

                    var selfValue = type.GetProperty(pi.Name).GetValue(@this, null);
                    var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                    if (pi.PropertyType.IsClass && !pi.PropertyType.Module.ScopeName.Equals("CommonLanguageRuntimeLibrary"))
                    {
                        // Check of "CommonLanguageRuntimeLibrary" is needed because string is also a class
                        if (ObjectPropertiesEqual(selfValue, toValue, ignore))
                        {
                            continue;
                        }

                        return false;
                    }

                    if (selfValue.IsCollection() == true && toValue.IsCollection() == true)
                    {
                        List<object> selfList = (selfValue as IEnumerable<object>).Cast<object>().ToList();
                        List<object> toList = (toValue as IEnumerable<object>).Cast<object>().ToList();
                        return selfList.SequenceEqual(toList);
                    }
                    else
                    {
                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            return false;
                        }
                    }
                }
            }
            else if (@this == null && to != null)
            {
                return false;
            }
            else if (@this != null && to == null)
            {
                return false;
            }

            return true;
        }

        public static string Quote(this object o)
        {
            if (o == null || o == DBNull.Value)
            {
                return "NULL";
            }

            if (o is int || o is long || o is double)
            {
                return o.ToString();
            }

            if (o is decimal)
            {
                return ((decimal)o).ToString("0.00");
            }

            if (o is double)
            {
                return (Math.Round((decimal)o, 4)).ToString();
            }

            if (o is double)
            {
                return ((decimal)o).ToString("0");
            }

            if (o is bool)
            {
                return (bool)o ? "1" : "0";
            }

            if (o is DateTime)
            {
                return "'" + ((DateTime)o).ToString("yyyy-MM-dd") + "'";
            }

            return "'" + o.ToString().Replace("'", "''") + "'";
        }

        public static bool IsNullOrEmpty(this object @this)
        {
            return @this == null;
        }

        public static string ObjectName<T>(this T @this)
        {
            if (default(T) == null && Equals(@this, default(T)))
            {
                return $"(null {typeof(T)})";
            }

            return $"{@this.GetType()} : {@this.ToString()} declared as {{{typeof(T)}}}";
        }

        public static bool IsEmpty(this object @this)
        {
            if (@this is object)
            {
                return @this == null || @this == DBNull.Value || Convert.IsDBNull(@this) == true;
            }
            else
            {
                return string.IsNullOrEmpty(@this.ToString());
            }
        }

        public static bool IsNumeric(this object @this)
        {
            if (@this == null || @this is DateTime)
            {
                return false;
            }

            if (@this is short || @this is int || @this is long || @this is decimal || @this is float || @this is double || @this is bool)
            {
                return true;
            }

            try
            {
                if (@this is string)
                {
                    double outValue;
                    if (double.TryParse(@this as string, out outValue) == true)
                    {
                        return true;
                    }
                }
                else
                {
                    double outValue;
                    if (double.TryParse(@this as string, out outValue) == true)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
            }

            return false;
        }

        public static int ToInt(this object @this, int defaultInt = 0)
        {
            int resultNum = defaultInt;
            try
            {
                if (!string.IsNullOrEmpty(@this.ToString()))
                {
                    resultNum = Convert.ToInt32(@this);
                }
            }
            catch
            {
            }

            return resultNum;
        }

        public static decimal ToDecimal(this object @this, decimal defaultDec = 0M)
        {
            decimal resultNum = defaultDec;
            try
            {
                if (!string.IsNullOrEmpty(@this.ToString()))
                {
                    bool isDec = decimal.TryParse(@this.ToString(), out resultNum);
                    if (isDec == true)
                    {
                        resultNum = Convert.ToDecimal(@this);
                    }
                }
            }
            catch
            {
            }

            return resultNum;
        }

        public static DateTime ToDateTime(this object @this)
        {
            DateTime resultDeteTime = new DateTime(1900, 1, 1);
            try
            {
                if (!string.IsNullOrEmpty(@this.ToString()))
                {
                    bool isDateTime = DateTime.TryParse(@this.ToString(), out resultDeteTime);
                    if (isDateTime == true)
                    {
                        resultDeteTime = Convert.ToDateTime(@this);
                    }
                }
            }
            catch
            {
            }

            return resultDeteTime;
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="typeCode">The type of object to return.</param>
        /// <returns>
        ///     An object whose underlying type is  and whose value is equivalent to .-or-A null reference (Nothing in Visual
        ///     Basic), if  is null and  is , , or .
        /// </returns>
        public static Object ChangeType(this Object value, TypeCode typeCode)
        {
            return Convert.ChangeType(value, typeCode);
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object. A parameter
        ///     supplies culture-specific formatting information.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="typeCode">The type of object to return.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        ///     An object whose underlying type is  and whose value is equivalent to .-or- A null reference (Nothing in
        ///     Visual Basic), if  is null and  is , , or .
        /// </returns>
        public static Object ChangeType(this Object value, TypeCode typeCode, IFormatProvider provider)
        {
            return Convert.ChangeType(value, typeCode, provider);
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or-A null reference (Nothing in Visual Basic), if
        ///     is null and  is not a value type.
        /// </returns>
        public static Object ChangeType(this Object value, Type conversionType)
        {
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object. A parameter
        ///     supplies culture-specific formatting information.
        /// </summary>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or- , if the  of  and  are equal.-or- A null
        ///     reference (Nothing in Visual Basic), if  is null and  is not a value type.
        /// </returns>
        public static Object ChangeType(this Object value, Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(value, conversionType, provider);
        }

        /// <summary>
        ///     Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">An object that implements the  interface.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or-A null reference (Nothing in Visual Basic), if
        ///     is null and  is not a value type.
        /// </returns>
        public static Object ChangeType<T>(this Object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        ///     Returns an object of the specified type whose value is equivalent to the specified object. A parameter
        ///     supplies culture-specific formatting information.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">An object that implements the  interface.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        ///     An object whose type is  and whose value is equivalent to .-or- , if the  of  and  are equal.-or- A null
        ///     reference (Nothing in Visual Basic), if  is null and  is not a value type.
        /// </returns>
        public static Object ChangeType<T>(this Object value, IFormatProvider provider)
        {
            return (T)Convert.ChangeType(value, typeof(T), provider);
        }

        public static object CloneObject(this object @this)
        {
            Type typeSource = @this.GetType();
            object objTarget = Activator.CreateInstance(typeSource);

            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.CanWrite == true)
                {
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(string)))
                    {
                        property.SetValue(objTarget, property.GetValue(@this, null), null);
                    }
                    else
                    {
                        object objPropertyValue = property.GetValue(@this, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                        }
                    }
                }
            }

            return objTarget;
        }

        public static string ToDump(this object @this)
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy;
            var propInfos = @this.GetType().GetProperties(flags);

            var sb = new StringBuilder();

            string typeName = @this.GetType().Name;
            sb.AppendLine(typeName);
            sb.AppendLine("".PadRight(typeName.Length + 5, '='));

            foreach (System.Reflection.PropertyInfo propInfo in propInfos)
            {
                sb.Append(propInfo.Name);
                sb.Append(": ");
                if (propInfo.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    object value = propInfo.GetValue(@this, null);
                    sb.Append(value != null ? value : "null");
                }

                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "@this">The object to be compared.</param>
        /// <param name = "values">The values to compare with the object.</param>
        /// <returns></returns>
        public static bool EqualsAny<T>(this T @this, params T[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (DeepCompare(@this, values[i]) == true)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the object is equal to none of the provided values.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "@this">The object to be compared.</param>
        /// <param name = "values">The values to compare with the object.</param>
        /// <returns></returns>
        public static bool EqualsNone<T>(this T @this, params T[] values)
        {
            return (@this.EqualsAny(values) == false);
        }

        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	object someObject = GetSomeObject();
        /// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
        /// 	else { /* the someObject is not null */ }
        /// </example>
        public static bool IsNull(this object target)
        {
            var ret = IsNull<object>(target);
            return ret;
        }

        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	MyClass someObject = GetSomeObject();
        /// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
        /// 	else { /* the someObject is not null */ }
        /// </example>
        public static bool IsNull<T>(this T target)
        {
            var result = ReferenceEquals(target, null);
            return result;
        }

        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	object someObject = GetSomeObject();
        /// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
        /// 	else { /* the someObject is null */ }
        /// </example>
        public static bool IsNotNull(this object @this)
        {
            var ret = IsNotNull<object>(@this);
            return ret;
        }

        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	MyClass someObject = GetSomeObject();
        /// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
        /// 	else { /* the someObject is null */ }
        /// </example>
        public static bool IsNotNull<T>(this T @this)
        {
            var result = !ReferenceEquals(@this, null);
            return result;
        }
        /// <summary>
        /// Determines whether the object is exactly of the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "@this">The object to check.</param>
        /// <returns>
        /// <c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType<T>(this object @this)
        {
            return @this.IsOfType(typeof(T));
        }

        /// <summary>
        /// Determines whether the object is excactly of the passed type
        /// </summary>
        /// <param name = "@this">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// <c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType(this object @this, Type type)
        {
            return (@this.GetType().Equals(type));
        }

        /// <summary>
        /// Determines whether the object is of the passed generic type or inherits from it.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "@this">The object to check.</param>
        /// <returns>
        /// <c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits<T>(this object @this)
        {
            return @this.IsOfTypeOrInherits(typeof(T));
        }

        /// <summary>
        /// Determines whether the object is of the passed type or inherits from it.
        /// </summary>
        /// <param name = "@this">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// <c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits(this object @this, Type type)
        {
            var objectType = @this.GetType();

            do
            {
                if (objectType.Equals(type))
                {
                    return true;
                }

                if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
                {
                    return false;
                }

                objectType = objectType.BaseType;
            } while (true);
        }

        /// <summary>
        /// 	Determines whether the object is assignable to the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "@this">The object to check.</param>
        /// <returns>
        /// <c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo<T>(this object @this)
        {
            return @this.IsAssignableTo(typeof(T));
        }

        /// <summary>
        /// Determines whether the object is assignable to the passed type.
        /// </summary>
        /// <param name = "@this">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// <c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo(this object @this, Type type)
        {
            var objectType = @this.GetType();
            return type.IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Gets the type default value for the underlying data type, in case of reference types: null
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "@this">The value.</param>
        /// <returns>The default value</returns>
        public static T GetTypeDefaultValue<T>(this T @this)
        {
            return default(T);
        }

        private static bool DeepCompare(object @this, object another)
        {
            if (ReferenceEquals(@this, another))
            {
                return true;
            }

            if ((@this == null) || (another == null))
            {
                return false;
            }

            if (@this.GetType() != another.GetType())
            {
                return false;
            }

            var result = true;

            foreach (var property in @this.GetType().GetProperties())
            {
                var objValue = property.GetValue(@this);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 	If target is null reference, returns notNullValue.
        /// 	Othervise returns target.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "@this">Target which is maybe null. Can be null.</param>
        /// <param name = "notNullValue">Value used instead of null.</param>
        /// <example>
        /// 	const int DEFAULT_NUMBER = 123;
        /// 
        /// 	int? number = null;
        /// 	int notNullNumber1 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 123
        /// 
        /// 	number = 57;
        /// 	int notNullNumber2 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 57
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static T NotNull<T>(this T @this, T notNullValue)
        {
            return ReferenceEquals(@this, null) ? notNullValue : @this;
        }

        /// <summary>
        /// 	If target is null reference, returns result from notNullValueProvider.
        /// 	Othervise returns target.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "@this">Target which is maybe null. Can be null.</param>
        /// <param name = "notNullValueProvider">Delegate which return value is used instead of null.</param>
        /// <example>
        /// 	int? number = null;
        /// 	int notNullNumber1 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns random number from 10 to 20
        /// 
        /// 	number = 57;
        /// 	int notNullNumber2 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns 57
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static T NotNull<T>(this T @this, Func<T> notNullValueProvider)
        {
            return ReferenceEquals(@this, null) ? notNullValueProvider() : @this;
        }

        /// <summary>
        /// DeepClone method for any object.
        /// Clones all public properties.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="source">Source object</param>
        /// <remarks>Inspired by https://andreslugo.dev/how-to-deep-clone-objects-in-c</remarks>
        /// <returns>The new, cloned instance</returns>
        public static T DeepClone<T>(this T source) where T : class
        {
            if (object.ReferenceEquals(source, null))
            {
                return default;
            }

            var deserializeSettings = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };

            var serialized = JsonSerializer.Serialize(source);
            var clone = JsonSerializer.Deserialize<T>(serialized, deserializeSettings);
            return clone;
        }
    }
}