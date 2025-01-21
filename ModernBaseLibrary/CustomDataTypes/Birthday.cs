//-----------------------------------------------------------------------
// <copyright file="Birthday.cs" company="Lifeprojects.de">
//     Class: Birthday
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.02.2023</date>
//
// <summary>
// Klasse für für den Custom Datatype 'Birthday'
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public struct Birthday : IEquatable<Birthday>, IComparable<Birthday>, IFormattable
    {
        private readonly DateTime _value;

        public Birthday(DateTime value)
        {
            if (value > new DateTime(1900, 1, 1) && value < new DateTime(2199, 1, 1))
            {
                _value = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("value", "variable value must be between 1900/1/1 and 2199/1/1.");
            }
        }

        public DateTime Value
        {
            get
            {
                return _value;
            }
        }

        public int Day
        {
            get { return this.Value.Day; }
        }

        public int Month
        {
            get { return this.Value.Month; }
        }

        public int Year
        {
            get { return this.Value.Year; }
        }

        public static implicit operator Birthday(DateTime value)
        {
            return new Birthday(value);
        }

        public static implicit operator DateTime(Birthday value)
        {
            return new DateTime(value.Year, value.Month, value.Day);
        }

        public static bool operator ==(Birthday left, Birthday right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Birthday left, Birthday right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return 207501131 ^ this.Value.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if ((obj is Birthday) == false)
            {
                return false;
            }

            Birthday other = (Birthday)obj;
            return Equals(other);
        }

        public override string ToString()
        {
            return this.Value.ToString("d");
        }

        public string ToString(string format)
        {
            return this.Value.ToString(format);
        }

        public int AgeInYear()
        {
            return this.Value.GetAge();
        }

        public int AgeInDays()
        {
            DateTime d1 = DateTime.Now;
            DateTime d2 = this.Value;

            TimeSpan t = d1 - d2;
            double days = t.TotalDays;
            return (int)days;
        }

        #region Implementation of IEquatable<Birthday>

        public bool Equals(Birthday other)
        {
            return this.Value == other.Value;
        }

        #endregion Implementation of IEquatable<Birthday>

        #region Implementation of IComparable<Birthday>

        public int CompareTo(Birthday other)
        {
            int valueCompare = this.Value.CompareTo(other.Value);

            return valueCompare;
        }

        #endregion Implementation of IComparable<Birthday>

        #region Implementation of IFormattable

        public String ToString(String format, IFormatProvider formatProvider)
        {
            return this.Value.ToString(format, formatProvider);
        }

        #endregion Implementation of IFormattable

        #region Implementation of IConvertible
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return this.Value;
        }

        public string ToString(IFormatProvider provider)
        {
            return ((DateTime)this.Value).ToString(provider);
        }
        #endregion Implementation of IConvertibles
    }
}
