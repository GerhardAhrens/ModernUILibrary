//-----------------------------------------------------------------------
// <copyright file="Enumeration.cs" company="Lifeprojects.de">
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
    using System.Diagnostics;

    [DebuggerDisplay("{Name} = {Value}")]
    public abstract partial class Enumeration : IConvertible, IComparable, IFormattable
    {
        protected Enumeration(int id, string name)
        {
            this.Value = id;
            this.Name = name;
            this.Description = string.Empty;
        }

        protected Enumeration(int id, string name, string description)
        {
            this.Value = id;
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; }

        public int Value { get; }

        public string Description { get; }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;
            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);
            return typeMatches && valueMatches;
        }

        protected bool Equals(Enumeration other)
        {
            return string.Equals(Name, other.Name) && Value == other.Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Value;
            }
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }

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

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)  => Convert.ChangeType(this, conversionType, provider);
    }
}
