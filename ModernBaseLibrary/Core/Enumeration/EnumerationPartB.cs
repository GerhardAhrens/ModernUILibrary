//-----------------------------------------------------------------------
// <copyright file="EnumerationPartB.cs" company="Lifeprojects.de">
//     Class: Enumeration
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2023</date>
//
// <summary>
// Die Klasse stellt die basis für eine Enummeration zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("{Name} = {Value}")]
    public abstract partial class Enumeration<T> where T : Enumeration<T>
    {
        private static readonly IDictionary<int, Enumeration<T>> _valueCache = new Dictionary<int, Enumeration<T>>();
        private static readonly IDictionary<string, Enumeration<T>> _nameCache = new Dictionary<string, Enumeration<T>>(StringComparer.OrdinalIgnoreCase);

        public static IDictionary<int, Enumeration<T>> ValueCache
        {
            get
            {
                RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
                return _valueCache;
            }
        }

        public static IDictionary<string, Enumeration<T>> NameCache
        {
            get
            {
                RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
                return _nameCache;
            }
        }

        public string Name { get; }


        public int Value { get; }


        public static IEnumerable<string> Names => NameCache.Keys;


        public static IEnumerable<int> Values => ValueCache.Keys;

        protected Enumeration(string name, int value)
        {
            Name = name;
            Value = value;
            ValueCache.Add(value, this);
            NameCache.Add(name, this);
        }

        public static T Parse(string name)
        {
            if (TryParse(name, out var value))
            {
                return value;
            }

            throw new InvalidOperationException($"Requested value {name} was not found.");
        }

        public static bool TryParse(string name, out T value)
        {
            if (NameCache.TryGetValue(name, out var containedValue))
            {
                value = (T)containedValue;
                return true;
            }

            value = default(T);
            return false;
        }

        public static string Format(T value, string format)
        {
            return value.ToString(format);
        }

        public static bool IsDefined(string name)
        {
            return NameCache.ContainsKey(name);
        }
    }

    public abstract partial class Enumeration<T> : IConvertible
    {
        TypeCode IConvertible.GetTypeCode() => TypeCode.Int32;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Value, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Value, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Value, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Value;
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Value, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException("Invalid cast.");
        string IConvertible.ToString(IFormatProvider provider) => ToString();

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(this, conversionType, provider);
    }

    public abstract partial class Enumeration<T> : IComparable<Enumeration<T>>
    {
        public int CompareTo(Enumeration<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }
    }

    public abstract partial class Enumeration<T> : IFormattable
    {
        public string ToString(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }
            if (string.Compare(format, "G", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Name;
            }
            if (string.Compare(format, "D", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Value.ToString();
            }
            if (string.Compare(format, "X", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Value.ToString("X8");
            }
            throw new FormatException("Invalid format");
        }

        public override string ToString() => ToString("G");

        public string ToString(string format, IFormatProvider formatProvider) => ToString(format);
    }

    public abstract partial class Enumeration<T> : IEquatable<Enumeration<T>>
    {
        public bool Equals(Enumeration<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Enumeration<T>)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator >(Enumeration<T> item1, Enumeration<T> item2) => item1.CompareTo(item2) > 0;

        public static bool operator <(Enumeration<T> item1, Enumeration<T> item2) => item1.CompareTo(item2) < 0;

        public static explicit operator Enumeration<T>(int value)
        {
            return (Enumeration<T>)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { value.ToString(), value }, null);
        }
    }
}
