namespace System
{
    using System.Collections.Generic;
    using System.Globalization;

    public sealed class Effort : IEquatable<Effort>, IComparable, IComparable<Effort>
    {        
        public Effort(decimal value, float effortHour = 8)
        {
            this.Value = value;
            this.EffortHour = effortHour;
        }

        public Effort(double value, float effortHour = 8)
        {
            this.Value = (decimal)value;
            this.EffortHour = effortHour;
        }

        public Effort(float value, float effortHour = 8)
        {
            this.Value = (decimal)value;
            this.EffortHour = effortHour;
        }

        public Effort(int value, float effortHour = 8)
        {
            this.Value = (decimal)value;
            this.EffortHour = effortHour;
        }

        public decimal Value { get; private set; }

        public float EffortHour { get; private set; }

        #region Equatable and Operator ==, !=

        public bool Equals(Effort other)
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

            if (obj.GetType() != typeof(Effort))
            {
                return false;
            }

            return Equals((Effort)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Value.GetHashCode() * 397;
            }
        }

        public static bool operator ==(Effort left, Effort right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Effort left, Effort right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Comparable and Operator >, <, >=, <=

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Effort))
            {
                throw new ArgumentException("Object is not Effort object");
            }

            return CompareTo((Effort)obj);
        }

        public int CompareTo(Effort other)
        {
            if (other == null)
            {
                return 1;
            }

            if (this < other)
            {
                return -1;
            }

            if (this > other)
            {
                return 1;
            }

            return 0;
        }

        public static bool operator >(Effort left, Effort right)
        {
            AssertSameEffort(left, right);
            return left.Value > right.Value;
        }

        public static bool operator >=(Effort left, Effort right)
        {
            AssertSameEffort(left, right);
            return left.Value >= right.Value;
        }

        public static bool operator <(Effort left, Effort right)
        {
            AssertSameEffort(left, right);
            return left.Value < right.Value;
        }

        public static bool operator <=(Effort left, Effort right)
        {
            AssertSameEffort(left, right);
            return left.Value <= right.Value;
        }

        #endregion

        #region Operator +, -
        public static Effort operator +(Effort left, Effort right)
        {
            AssertSameEffort(left, right);
            return new Effort(left.Value + right.Value);
        }

        public static Effort operator +(Effort left, decimal right)
        {
            AssertNotNull(left);
            return new Effort(left.Value + right);
        }

        public static Effort operator -(Effort left, Effort right)
        {
            AssertSameEffort(left, right);
            return new Effort(left.Value - right.Value);
        }

        public static Effort operator -(Effort left, decimal right)
        {
            AssertNotNull(left);
            return new Effort(left.Value - right);
        }

        #endregion

        #region Operator *, /

        public static Effort operator *(Effort left, decimal right)
        {
            AssertNotNull(left);
            return new Effort(left.Value * right);
        }

        public static Effort operator /(Effort left, decimal right)
        {
            AssertNotNull(left);
            return new Effort(left.Value / right);
        }

        #endregion

        #region implicit operator
        public static implicit operator Effort(decimal value)
        {
            return new Effort(value);
        }

        public static implicit operator Effort(double value)
        {
            return new Effort((decimal)value);
        }

        public static implicit operator Effort(float value)
        {
            return new Effort((decimal)value);
        }

        public static implicit operator Effort(long value)
        {
            return new Effort((decimal)value);
        }

        public static implicit operator Effort(int value)
        {
            return new Effort((decimal)value);
        }
        #endregion implicit operator

        #region Helper functions
        public static void AssertNotNull(Effort value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Effort Is Null");
            }
        }

        public static void AssertSameEffort(Effort first, Effort second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException("Any Effort Is Null");
            }
        }
        #endregion

        #region Extension Methodes
        public decimal ToHours()
        {
            return this.Value * Convert.ToDecimal(this.EffortHour);
        }

        public IEnumerable<object> GetValues()
        {
            yield return this.Value;
            yield return this.EffortHour;
        }
        #endregion Extension Methodes

        /// <summary>
        /// Use the decorated interal Currency object to display the string
        /// </summary>
        /// 
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        public string ToString(CultureInfo cultureInfo)
        {
            return this.Value.ToString(cultureInfo);
        }
    }
}