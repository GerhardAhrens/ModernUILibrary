
namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using System.Linq.Expressions;
    using System.Reflection;

    public class FunctionMap
    {
        public FunctionMap(Type t)
        {
            InitializeMapFromType(t);
        }

        public Type ClassType { get; set; }
        public Dictionary<int, Func<object, object>> ObjectGetters { get; private set; } = new Dictionary<int, Func<object, object>>();
        public Dictionary<int, Func<object, char>> CharGetters { get; private set; } = new Dictionary<int, Func<object, char>>();
        public Dictionary<int, Func<object, int>> IntGetters { get; private set; } = new Dictionary<int, Func<object, int>>();
        public Dictionary<int, Func<object, long>> LongGetters { get; private set; } = new Dictionary<int, Func<object, long>>();
        public Dictionary<int, Func<object, bool>> BoolGetters { get; private set; } = new Dictionary<int, Func<object, bool>>();
        public Dictionary<int, Func<object, short>> ShortGetters { get; private set; } = new Dictionary<int, Func<object, short>>();
        public Dictionary<int, Func<object, byte>> ByteGetters { get; private set; } = new Dictionary<int, Func<object, byte>>();
        public Dictionary<int, Func<object, float>> FloatGetters { get; private set; } = new Dictionary<int, Func<object, float>>();
        public Dictionary<int, Func<object, double>> DoubleGetters { get; private set; } = new Dictionary<int, Func<object, double>>();
        public Dictionary<int, Func<object, DateTime>> DateGetters { get; private set; } = new Dictionary<int, Func<object, DateTime>>();
        public Dictionary<int, Func<object, decimal>> DecimalGetters { get; private set; } = new Dictionary<int, Func<object, decimal>>();
        public Dictionary<int, Func<object, Guid>> GuidGetters { get; set; } = new Dictionary<int, Func<object, Guid>>();
        public Dictionary<string, Type> MemberTypeMapping { get; private set; } = new Dictionary<string, Type>();
        public Dictionary<int, string> IndexNameMapping { get; private set; }
        public Dictionary<string, int> NameIndexMapping { get; private set; }
        public int FieldNum { get; private set; }

        Func<object, T1> GetObjectFunction<T1>(Type t, MemberInfo pi)
        {
            var lambdaParam = Expression.Parameter(typeof(object), "x");
            return Expression.Lambda<Func<object, T1>>(Expression.Convert(Expression.MakeMemberAccess(Expression.Convert(lambdaParam, t), pi), typeof(T1)), lambdaParam).Compile();
        }
        void AddPropertyMap(Type t, int i, Type propertyType, MemberInfo pi)
        {
            MemberTypeMapping[pi.Name] = propertyType;
            ObjectGetters[i] = GetObjectFunction<object>(t, pi);

            if (propertyType == typeof(long) || propertyType == typeof(long?))
            {
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (propertyType == typeof(short) || propertyType == typeof(short?))
            {
                ShortGetters[i] = GetObjectFunction<short>(t, pi);
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (propertyType == typeof(byte) || propertyType == typeof(byte?))
            {
                ByteGetters[i] = GetObjectFunction<byte>(t, pi);
                ShortGetters[i] = GetObjectFunction<short>(t, pi);
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            {
                ShortGetters[i] = GetObjectFunction<short>(t, pi);
                IntGetters[i] = GetObjectFunction<int>(t, pi);
                LongGetters[i] = GetObjectFunction<long>(t, pi);
                DoubleGetters[i] = GetObjectFunction<double>(t, pi);
                FloatGetters[i] = GetObjectFunction<float>(t, pi);
                DecimalGetters[i] = GetObjectFunction<decimal>(t, pi);
            }
            else if (propertyType == typeof(char) || propertyType == typeof(char?))
            {
                CharGetters[i] = GetObjectFunction<char>(t, pi);
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                BoolGetters[i] = GetObjectFunction<bool>(t, pi);
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                DateGetters[i] = GetObjectFunction<DateTime>(t, pi);
            }
            else if (propertyType == typeof(double) || propertyType == typeof(double?))
            {
                DoubleGetters[i] = GetObjectFunction<double>(t, pi);
            }
            else if (propertyType == typeof(float) || propertyType == typeof(float?))
            {
                DoubleGetters[i] = GetObjectFunction<double>(t, pi);
                FloatGetters[i] = GetObjectFunction<float>(t, pi);
            }
            else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
            {
                GuidGetters[i] = GetObjectFunction<Guid>(t, pi);
            }
        }

        private void InitializeMapFromType(Type t)
        {
            var ti = t.GetTypeInfo();
            var properties = ti.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead && (x.GetCustomAttribute<IgnoreFieldAttribute>() == null)).ToArray();
            var fields = ti.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<IgnoreFieldAttribute>() == null).ToArray();
            FieldNum = properties.Length + fields.Length;
            IndexNameMapping = new Dictionary<int, string>();
            NameIndexMapping = new Dictionary<string, int>();
            MemberTypeMapping = new Dictionary<string, Type>();
            ObjectGetters = new Dictionary<int, Func<object, object>>();

            for (int i = 0; i < properties.Length; i++)
            {
                var pi = properties[i];
                var customName = pi.GetCustomAttribute<FieldNameAsAttribute>();
                if (customName != null)
                {
                    NameIndexMapping[customName.Name] = i;
                    IndexNameMapping[i] = customName.Name;
                }
                else
                {
                    NameIndexMapping[pi.Name] = i;
                    IndexNameMapping[i] = pi.Name;
                }

                AddPropertyMap(t, i, pi.PropertyType, pi);
            }

            for (int i = 0; i < fields.Length; i++)
            {
                var fi = fields[i];
                var idx = i + properties.Length;
                var customName = fi.GetCustomAttribute<FieldNameAsAttribute>();
                if (customName != null)
                {
                    NameIndexMapping[customName.Name] = idx;
                    IndexNameMapping[idx] = customName.Name;
                }
                else
                {
                    NameIndexMapping[fi.Name] = idx;
                    IndexNameMapping[idx] = fi.Name;
                }

                AddPropertyMap(t, idx, fi.FieldType, fi);
            }
        }
    }
}
