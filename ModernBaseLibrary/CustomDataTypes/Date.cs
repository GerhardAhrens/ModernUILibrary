//-----------------------------------------------------------------------
// <copyright file="Date.cs" company="Lifeprojects.de">
//     Class: Date
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.02.2023</date>
//
// <summary>
// Klasse für den Custom Type 'Date'
// </summary>
//-----------------------------------------------------------------------


namespace System
{
    public struct Date : IEquatable<Date>, IComparable<Date>, IFormattable
    {
        private readonly DateTime _value;

        public Date(DateTime value)
        {
            if (value >= new DateTime(1900, 1, 1) && value < new DateTime(2199, 1, 1))
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

        public static Date Default
        {
            get { return new DateTime(1900, 1, 1); }
        }

        public static Date Today
        {
            get { return DateTime.Today; }
        }

        public bool IsDefault
        {
            get { return this.Value == new DateTime(1900,1,1); }
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

        public static implicit operator Date(DateTime value)
        {
            return new Date(value);
        }

        public static implicit operator DateTime(Date value)
        {
            return new DateTime(value.Year,value.Month,value.Day);
        }

        public static bool operator ==(Date left, Date right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Date left, Date right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return 207501131 ^ this.Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if ((obj is Date) == false)
            {
                return false;
            }

            Date other = (Date)obj;
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

        #region Extenstion Methodes 
        /// <summary>
        /// Prüft ob ein Datum zwischen Start und End liegt
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>True, wenn innerhalb</br>False, wenn nicht innerhab</returns>
        public bool InRange(DateTime startDate, DateTime endDate)
        {
            return this.Value >= startDate && this.Value < endDate;
        }

        /// <summary>
        /// Prüft ob ein Datum zwischen Start und End liegt
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>True, wenn innerhalb</br>False, wenn nicht innerhab</returns>
        public bool InRange(Date startDate, Date endDate)
        {
            return this.Value >= startDate && this.Value < endDate;
        }

        public bool In(params DateTime[] values)
        {
            return Array.IndexOf(values, this.Value) != -1;
        }

        public bool In(params Date[] values)
        {
            return Array.IndexOf(values, this.Value) != -1;
        }

        public bool NotIn(params DateTime[] values)
        {
            return Array.IndexOf(values, this.Value) == -1;
        }

        public bool NotIn(params Date[] values)
        {
            return Array.IndexOf(values, this.Value) == -1;
        }

        public Date FirstDayInYear()
        {
            return new DateTime(this.Value.Year, 1, 1);
        }

        public Date LastDayInYear()
        {
            return new DateTime(this.Value.Year, 12, 31);
        }

        public Date FirstMonthInYear()
        {
            return new DateTime(this.Value.Year, 1, 1);
        }

        public Date LastMonthInYear()
        {
            return new DateTime(this.Value.Year, 12, 31);
        }
        #endregion Extenstion Methodes

        #region Implementation of IEquatable<Date>

        public bool Equals(Date other)
        {
            return this.Value == other.Value;
        }

        #endregion Implementation of IEquatable<Date>

        #region Implementation of IComparable<Date>

        public int CompareTo(Date other)
        {
            int valueCompare = this.Value.CompareTo(other.Value);

            return valueCompare;
        }

        #endregion Implementation of IComparable<Date>

        #region Implementation of IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
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
