namespace ModernBaseLibrary.Core
{
    using System.Collections;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Text.Json;

    using Microsoft.Extensions.ObjectPool;

    using ModernBaseLibrary.Extension;

    /// <summary>
    /// Class TypeHelper.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class TypeHelper
    {
        public static object ChangeType(object obj, Type type)
        {
            if (IsList(obj))
            {
                List<object> objs = ((IEnumerable)obj).Cast<object>().ToList();
                Type containedType = type.GenericTypeArguments.First();
                return objs.Select(item => Convert.ChangeType(item, containedType)).ToList();
            }
            return Convert.ChangeType(obj, type);
        }

        public static bool IsList(object @this)
        {
            if (@this == null)
            {
                return false;
            }

            return @this is IList &&
                @this.GetType().IsGenericType &&
                @this.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsIEnumerable(object @this)
        {
            if (@this == null)
            {
                return false;
            }

            if (@this is IEnumerable value)
            {
                return value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
            }
            else
            {
                return false;
            }
        }

        public static bool IsDictionary(object @this)
        {
            if (@this == null)
            {
                return false;
            }

            if (@this is IDictionary value)
            {
                return value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The string builder pool
        /// </summary>
        private static readonly ObjectPool<StringBuilder> _stringBuilderPool = new DefaultObjectPoolProvider().CreateStringBuilderPool();


        /// <summary>
        /// Processes the type of the generic.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="type">The type.</param>
        /// <param name="genericArguments">The generic arguments.</param>
        /// <param name="length">The length.</param>
        /// <param name="options">The options.</param>
        private static void ProcessGenericType([NotNull] StringBuilder builder, [NotNull] Type type, Type[] genericArguments, int length, DisplayNameOptions options)
        {
            var offset = 0;

            if (type.IsNested)
            {
                offset = type.DeclaringType.GetGenericArguments().Length;
            }

            if (options.FullName)
            {
                if (type.IsNested)
                {
                    ProcessGenericType(builder, type.DeclaringType, genericArguments, offset, options);
                    _ = builder.Append(options.NestedTypeDelimiter);
                }
                else if (!string.IsNullOrEmpty(type.Namespace))
                {
                    _ = builder.Append(type.Namespace);
                    _ = builder.Append(ControlChars.Dot);
                }
            }

            var genericPartIndex = type.Name.IndexOf('`', StringComparison.Ordinal);
            if (genericPartIndex <= 0)
            {
                _ = builder.Append(type.Name);
                return;
            }

            _ = builder.Append(type.Name, 0, genericPartIndex);

            if (options.IncludeGenericParameters)
            {
                _ = builder.Append(ControlChars.StartAngleBracket);

                for (var typeCount = offset; typeCount < length; typeCount++)
                {
                    ProcessType(builder, genericArguments[typeCount], options);

                    if (typeCount + 1 == length)
                    {
                        continue;
                    }

                    _ = builder.Append(ControlChars.Comma);
                    if (options.IncludeGenericParameterNames || !genericArguments[typeCount + 1].IsGenericParameter)
                    {
                        _ = builder.Append(ControlChars.Space);
                    }
                }

                _ = builder.Append(ControlChars.EndAngleBracket);
            }
        }
        internal static IEnumerable<PropertyInfo> GetAllProperties([NotNull] this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            while (typeInfo is not null)
            {
                foreach (var propertyInfo in typeInfo.DeclaredProperties)
                {
                    yield return propertyInfo;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }

        /// <summary>
        /// Processes the type.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="type">The type.</param>
        /// <param name="options">The options.</param>
        internal static void ProcessType([NotNull] StringBuilder builder, [NotNull] Type type, DisplayNameOptions options)
        {
            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                ProcessGenericType(builder, type, genericArguments, genericArguments.Length, options);
            }
            else if (type.IsArray)
            {
                ProcessType(builder, type, options);
            }
            else if (BuiltInTypeNames.TryGetValue(type, out var builtInName))
            {
                _ = builder.Append(builtInName);
            }
            else if (type.IsGenericParameter)
            {
                if (options.IncludeGenericParameterNames)
                {
                    _ = builder.Append(type.Name);
                }
            }
            else
            {
                var name = options.FullName ? type.FullName : type.Name;
                _ = builder.Append(name);

                if (options.NestedTypeDelimiter != ControlChars.Plus)
                {
                    _ = builder.Replace(ControlChars.Plus, options.NestedTypeDelimiter, builder.Length - name.Length, name.Length);
                }
            }
        }

        /// <summary>
        /// Creates type instance.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <returns>T.</returns>
        /// <remarks>Original code by: Jeremy Clark</remarks>
        public static T Create<T>()
            where T : class
        {
            var instance = Activator.CreateInstance<T>();

            return instance is not null ? instance : null;
        }

        /// <summary>
        /// Creates the specified parameter array.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="paramArray">The parameter array.</param>
        /// <returns>T.</returns>
        public static T Create<T>([NotNull] params object[] paramArray)
        {
            var instance = (T)Activator.CreateInstance(typeof(T), args: paramArray);

            return instance;
        }

        /// <summary>
        /// Does the object equal instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DoesObjectEqualInstance([NotNull] object value, [NotNull] object instance)
        {
            var result = ReferenceEquals(value, instance);

            return result;
        }


        /// <summary>
        /// Creates object from Json.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns>T.</returns>
        public static T FromJson<T>(string json)
            where T : class
        {
            return JsonSerializer.Deserialize<T>(json.IsArgumentNullOrEmpty(nameof(json),string.Empty));
        }

        /// <summary>
        /// Creates object from a Json file.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>T.</returns>
        /// <exception cref="FileNotFoundException">The exception.</exception>
        public static T FromJsonFile<T>(string fileName)
            where T : class
        {
            fileName = fileName.IsArgumentNullOrEmpty(nameof(fileName),string.Empty);

            if (File.Exists(fileName) is false)
            {
                throw new IOException($"File '{fileName}' not found");
            }

            var json = File.ReadAllText(fileName, Encoding.UTF8);

            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Gets the default type.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <returns>T.</returns>
        public static T GetDefault<T>()
        {
            return default;
        }

        /// <summary>
        /// Gets the instance hash code.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Int32.</returns>
        public static int GetInstanceHashCode([NotNull] object instance)
        {
            var hash = instance.IsArgumentNotNull().GetType().GetRuntimeProperties().Where(p => p is not null).Select(prop => prop.GetValue(instance)).Where(value => value is not null).Aggregate(-1, (accumulator, value) => accumulator ^ value.GetHashCode());

            return hash;
        }

        public static ImmutableDictionary<string, string> GetPropertyValues<T>([NotNull] T input)
        {
            var returnValue = new Dictionary<string, string>();

            var properties = input.GetType().GetAllProperties().Where(p => p.CanRead).OrderBy(p => p.Name).ToArray();

            for (var propertyCount = 0; propertyCount < properties.Length; propertyCount++)
            {
                var propertyInfo = properties[propertyCount];

                if (string.Equals(propertyInfo.PropertyType.Name, "IDictionary", StringComparison.Ordinal))
                {
                    var propertyValue = propertyInfo.GetValue(input) as IDictionary;

                    if (propertyValue?.Count > 0)
                    {
                        _ = returnValue.AddIfNotExists(new KeyValuePair<string, string>(propertyInfo.Name, propertyValue.ToDelimitedString()));
                    }
                }
                else
                {
                    // Get property value
                    var propertyValue = propertyInfo.GetValue(input);

                    if (propertyValue is not null)
                    {
                        _ = returnValue.AddIfNotExists(new KeyValuePair<string, string>(propertyInfo.Name, propertyValue.ToString()));
                    }
                }
            }

            return returnValue.ToImmutableDictionary();
        }

        /// <summary>
        /// Gets the display name of the type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fullName">if set to <c>true</c> [full name].</param>
        /// <returns>System.String.</returns>
        public static string GetTypeDisplayName([NotNull] object item, bool fullName = true)
        {
            return item is null ? null : GetTypeDisplayName(item.GetType(), fullName);
        }

        /// <summary>
        /// Pretty print a type name.
        /// </summary>
        /// <param name="type">The <see cref="Type" />.</param>
        /// <param name="fullName"><c>true</c> to print a fully qualified name.</param>
        /// <param name="includeGenericParameterNames"><c>true</c> to include generic parameter names.</param>
        /// <param name="includeGenericParameters"><c>true</c> to include generic parameters.</param>
        /// <param name="nestedTypeDelimiter">Character to use as a delimiter in nested type names</param>
        /// <returns>The pretty printed type name.</returns>
        /// <exception cref="ArgumentNullException">type</exception>
        public static string GetTypeDisplayName([NotNull] Type type, bool fullName = true, bool includeGenericParameterNames = false, bool includeGenericParameters = true, char nestedTypeDelimiter = ControlChars.Plus)
        {
            type = type.IsArgumentNotNull();

            var sb = _stringBuilderPool.Get();
            try
            {
                ProcessType(
                    sb,
                    type,
                    new DisplayNameOptions(
                        fullName,
                        includeGenericParameterNames,
                        includeGenericParameters,
                        nestedTypeDelimiter));

                return sb.ToString();
            }
            finally
            {
                _stringBuilderPool.Return(sb);
            }
        }

        /// <summary>
        /// Gets the built in type names.
        /// </summary>
        /// <value>The built in type names.</value>
        public static Dictionary<Type, string> BuiltInTypeNames { get; } = new()
    {
        { typeof(DateTime), "datetime" },
        { typeof(DateTimeOffset), "datetimeoffset" },
        { typeof(TimeSpan), "timespan" },
        { typeof(bool), "bool" },
        { typeof(byte), "byte" },
        { typeof(char), "char" },
        { typeof(decimal), "decimal" },
        { typeof(double), "double" },
        { typeof(float), "float" },
        { typeof(int), "int" },
        { typeof(long), "long" },
        { typeof(object), "object" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(string), "string" },
        { typeof(uint), "uint" },
        { typeof(ulong), "ulong" },
        { typeof(ushort), "ushort" },
        { typeof(void), "void" },
    };

        /// <summary>
        /// Struct DisplayNameOptions.
        /// </summary>
        internal struct DisplayNameOptions
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DisplayNameOptions" /> struct.
            /// </summary>
            /// <param name="fullName">if set to <c>true</c> [full name].</param>
            /// <param name="includeGenericParameterNames">if set to <c>true</c> [include generic parameter names].</param>
            /// <param name="includeGenericParameters">if set to <c>true</c> [include generic parameters].</param>
            /// <param name="nestedTypeDelimiter">The nested type delimiter.</param>
            public DisplayNameOptions(bool fullName, bool includeGenericParameterNames, bool includeGenericParameters, char nestedTypeDelimiter)
            {
                this.FullName = fullName;
                this.IncludeGenericParameters = includeGenericParameters;
                this.IncludeGenericParameterNames = includeGenericParameterNames;
                this.NestedTypeDelimiter = nestedTypeDelimiter;
            }

            /// <summary>
            /// Gets a value indicating whether [full name].
            /// </summary>
            /// <value><c>true</c> if [full name]; otherwise, <c>false</c>.</value>
            public bool FullName { get; }

            /// <summary>
            /// Gets a value indicating whether [include generic parameter names].
            /// </summary>
            /// <value><c>true</c> if [include generic parameter names]; otherwise, <c>false</c>.</value>
            public bool IncludeGenericParameterNames { get; }

            /// <summary>
            /// Gets a value indicating whether [include generic parameters].
            /// </summary>
            /// <value><c>true</c> if [include generic parameters]; otherwise, <c>false</c>.</value>
            public bool IncludeGenericParameters { get; }

            /// <summary>
            /// Gets the nested type delimiter.
            /// </summary>
            /// <value>The nested type delimiter.</value>
            public char NestedTypeDelimiter { get; }
        }
    }
}

