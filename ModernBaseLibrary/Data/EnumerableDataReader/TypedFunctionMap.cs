namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class TypedFunctionMap<T>
    {
        public TypedFunctionMap()
        {
            InitializeMapFromType(typeof(T));
        }

        public Type ClassType { get; set; }
        public Dictionary<int, Func<T, object>> ObjectGetters { get; private set; } = new Dictionary<int, Func<T, object>>();
        public Dictionary<int, Func<T, char>> CharGetters { get; private set; } = new Dictionary<int, Func<T, char>>();
        public Dictionary<int, Func<T, int>> IntGetters { get; private set; } = new Dictionary<int, Func<T, int>>();
        public Dictionary<int, Func<T, long>> LongGetters { get; private set; } = new Dictionary<int, Func<T, long>>();
        public Dictionary<int, Func<T, bool>> BoolGetters { get; private set; } = new Dictionary<int, Func<T, bool>>();
        public Dictionary<int, Func<T, short>> ShortGetters { get; private set; } = new Dictionary<int, Func<T, short>>();
        public Dictionary<int, Func<T, byte>> ByteGetters { get; private set; } = new Dictionary<int, Func<T, byte>>();
        public Dictionary<int, Func<T, float>> FloatGetters { get; private set; } = new Dictionary<int, Func<T, float>>();
        public Dictionary<int, Func<T, double>> DoubleGetters { get; private set; } = new Dictionary<int, Func<T, double>>();
        public Dictionary<int, Func<T, DateTime>> DateGetters { get; private set; } = new Dictionary<int, Func<T, DateTime>>();
        public Dictionary<int, Func<T, decimal>> DecimalGetters { get; private set; } = new Dictionary<int, Func<T, decimal>>();
        public Dictionary<int, Func<T, Guid>> GuidGetters { get; set; } = new Dictionary<int, Func<T, Guid>>();
        public Dictionary<int, string> IndexNameMapping { get; private set; }
        public Dictionary<string, int> NameIndexMapping { get; private set; }
        public Dictionary<string, Type> MemberTypeMapping { get; private set; } = new Dictionary<string, Type>();
        public int FieldNum { get; private set; }

        private Func<T, T1> GetObjectFunction<T1>(Type t, MemberInfo pi)
        {
            var lambdaParam = Expression.Parameter(t, "x");
            return Expression.Lambda<Func<T, T1>>(Expression.Convert(Expression.MakeMemberAccess(lambdaParam, pi), typeof(T1)), lambdaParam).Compile();
        }

        private void AddPropertyMap(int i, Type t, Type memberType, MemberInfo pi)
        {
            MemberTypeMapping[pi.Name] = memberType;
            ObjectGetters[i] = GetObjectFunction<object>(t, pi);

            if (memberType == typeof(long) || memberType == typeof(long?))
            {
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (memberType == typeof(int) || memberType == typeof(int?))
            {
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (memberType == typeof(short) || memberType == typeof(short?))
            {
                ShortGetters[i] = GetObjectFunction<short>(t, pi);
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (memberType == typeof(byte) || memberType == typeof(byte?))
            {
                ByteGetters[i] = GetObjectFunction<byte>(t, pi);
                ShortGetters[i] = GetObjectFunction<short>(t, pi);
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (memberType == typeof(decimal) || memberType == typeof(decimal?))
            {
                ShortGetters[i] = GetObjectFunction<short>(t, pi);
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DoubleGetters[i] = GetObjectFunction<double>(t, pi);
                FloatGetters[i] = GetObjectFunction<float>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (memberType == typeof(char) || memberType == typeof(char?))
            {
                CharGetters[i] = GetObjectFunction<char>(t, pi);
            }
            else if (memberType == typeof(bool) || memberType == typeof(bool?))
            {
                BoolGetters[i] = GetObjectFunction<bool>(t, pi);
            }
            else if (memberType == typeof(DateTime) || memberType == typeof(DateTime?))
            {
                DateGetters[i] = GetObjectFunction<DateTime>(t, pi);
            }
            else if (memberType == typeof(double) || memberType == typeof(double?))
            {
                DoubleGetters[i] = GetObjectFunction<double>(t, pi);
            }
            else if (memberType == typeof(float) || memberType == typeof(float?))
            {
                DoubleGetters[i] = GetObjectFunction<double>(t, pi);
                FloatGetters[i] = GetObjectFunction<float>(t, pi);
            }
            else if (memberType == typeof(Guid) || memberType == typeof(Guid?))
            {
                GuidGetters[i] = GetObjectFunction<Guid>(t, pi);
            }
        }

        private void InitializeMapFromType(Type t)
        {
            var ti = t.GetTypeInfo();
            var properties = ti.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead && (x.GetCustomAttribute(typeof(IgnoreFieldAttribute)) == null))
                .ToArray();

            var fields = ti.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute(typeof(IgnoreFieldAttribute)) == null).ToArray();
            FieldNum = properties.Length + fields.Length;
            IndexNameMapping = new Dictionary<int, string>();
            NameIndexMapping = new Dictionary<string, int>();
            MemberTypeMapping = new Dictionary<string, Type>();
            ObjectGetters = new Dictionary<int, Func<T, object>>();

            for (int i = 0; i < properties.Length; i++)
            {
                var pi = properties[i];
                var customNameAttribute = pi.GetCustomAttribute(typeof(FieldNameAsAttribute)) as FieldNameAsAttribute;
                if (customNameAttribute != null)
                {
                    IndexNameMapping[i] = customNameAttribute.Name;
                    NameIndexMapping[customNameAttribute.Name] = i;
                }
                else
                {
                    IndexNameMapping[i] = pi.Name;
                    NameIndexMapping[pi.Name] = i;
                }
                AddPropertyMap(i, t, pi.PropertyType, pi);
            }

            for (int i = 0;i < fields.Length; i++)
            {
                var fi = fields[i];
                var idx = i + properties.Length;
                var customNameAttribute = fi.GetCustomAttribute(typeof(FieldNameAsAttribute)) as FieldNameAsAttribute;
                if (customNameAttribute != null)
                {
                    IndexNameMapping[idx] = customNameAttribute.Name;
                    NameIndexMapping[customNameAttribute.Name] = idx;
                }
                else
                {
                    IndexNameMapping[idx] = fi.Name;
                    NameIndexMapping[fi.Name] = idx;
                }
                AddPropertyMap(i + properties.Length, t, fields[i].FieldType, fields[i]);
            }
        }
    }
}
