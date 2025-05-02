//-----------------------------------------------------------------------
// <copyright file="Base64.cs" company="Lifeprojects.de">
//     Class: Base64
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <Framework>8.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>01.05.2025 20:04:44</date>
//
// <summary>
// Struct Class for Custom DataType Base64
// </summary>
// <Links>
// https://medium.com/c-sharp-programming/mastering-base64-encoding-and-decoding-in-c-803805c388d0
// </Links>
//-----------------------------------------------------------------------

namespace System
{
    public struct Base64 : IEquatable<Base64>, IComparable<Base64>
    {
        private readonly string _value;

        public Base64(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get
            {
                return this._value;
            }
        }

        public static string Default
        {
            get { return string.Empty; }
        }

        public bool IsNullOrEmpty
        {
            get { return string.IsNullOrEmpty(this.Value); }
        }

        public int Length
        {
            get
            {
                if (string.IsNullOrEmpty(this.Value) == true)
                {
                    return 0;
                }
                else
                {
                    return this.Value.Length;
                }
            }
        }

        public bool IsNull
        {
            get
            {
                if (this.Value == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static implicit operator Base64(string value)
        {
            return new Base64(value);
        }

        public static implicit operator Base64(byte[] value)
        {
            if (value == null || value.Length == 0)
            {
                throw new ArgumentException("Der Wert für byte[] muß <> null bzw. größer 0 sein.");
            }
            else
            {
                string fromByteArray = Convert.ToBase64String(value);
                return new Base64(fromByteArray);
            }
        }

        public static bool operator ==(Base64 left, Base64 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Base64 left, Base64 right)
        {
            return !left.Equals(right);
        }

        public static Base64 Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            string base64String = System.Convert.ToBase64String(plainTextBytes);
            return new Base64(base64String);
        }

        public static string Base64Decode(Base64 base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData.Value);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #region Check Funktionen
        public bool IsBase64String()
        {
            bool result = false;

            if (string.IsNullOrEmpty(this.Value) || this.Value.Length % 4 != 0 || this.Value.Contains(" ")
                || this.Value.Contains("\t") || this.Value.Contains("\r") || this.Value.Contains("\n"))
            {
                return result;
            }

            Span<byte> buffer = new Span<byte>(new byte[this.Value.Length]);
            result = Convert.TryFromBase64String(this.Value, buffer, out int bytesParsed);
            return result;
        }
        #endregion Check Funktionen

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(this.Value);
            return hashCode.ToHashCode();
        }

        #region Implementation of IEquatable<Base64>
        public override bool Equals(object obj)
        {
            if ((obj is Base64) == false)
            {
                return false;
            }

            Base64 other = (Base64)obj;
            return Equals(other);
        }

        public bool Equals(Base64 other)
        {
            return this.Value == other.Value;
        }
        #endregion Implementation of IEquatable<Base64>

        #region Konvertierung nach To...
        public override string ToString()
        {
            return this.Value.ToString();
        }

        public byte[] ToByteArray()
        {
            if (string.IsNullOrEmpty(this.Value) == false)
            {
                return System.Convert.FromBase64String(this.Value);
            }
            else
            {
                return null;
            }
        }
        #endregion Konvertierung nach To...

        #region Implementation of IComparable<Base64>

        public int CompareTo(Base64 other)
        {
            int valueCompare = this.Value.CompareTo(other.Value);

            return valueCompare;
        }
        #endregion Implementation of IComparable<Base64>

    }
}