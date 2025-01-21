
//-----------------------------------------------------------------------
// <copyright file="EMailAddress.cs" company="Lifeprojects.de">
//     Class: EMailAddress
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.12.2020</date>
//
// <summary>
// Class for Custom Value Type EMail
// https://stackoverflow.com/questions/3436101/create-custom-string-class
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Text.RegularExpressions;

    public class EMail : IEquatable<EMail>
    {
        public EMail(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            if (!IsValidAddress(value))
            {
                throw new ArgumentException();
            }

            this.MailAddress = value;
        }

        public EMail(string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            if (!IsValidAddress(value))
            {
                throw new ArgumentException();
            }

            this.MailAddress = value;
            this.Name = name;
        }

        public EMail(Uri uri)
        {
            if (uri.Scheme != "mailto")
            {
                throw new ArgumentException();
            }

            string extracted = $"{uri.UserInfo}@{uri.Host}";

            if (IsValidAddress(extracted) == false)
            {
                throw new ArgumentException();
            }

            this.MailAddress = extracted;
        }

        public string MailAddress { get; private set; }

        public string Name { get; private set; }


        #region Equals
        public bool Equals(EMail other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.MailAddress == other.MailAddress && this.Name == other.Name;
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

            if (obj.GetType() != typeof(EMail))
            {
                return false;
            }

            return Equals((EMail)obj);
        }

        public static bool operator ==(EMail a, EMail b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(EMail a, EMail b)
        {
            return !(a == b);
        }
        #endregion

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Name) == false)
            {
                return $"{this.MailAddress}<{this.Name}>";
            }
            else
            {
                return this.MailAddress;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.MailAddress != null ? this.MailAddress.GetHashCode() : 0) * 397) ^ Name.GetHashCode();
            }
        }

        public static implicit operator EMail(string value)
        {
            return value == null ? null : new EMail(value);
        }

        public static implicit operator EMail(Uri uri)
        {
            return uri == null ? null : new EMail(uri);
        }

        private static bool IsValidAddress(string value)
        {
            bool result = true;

            if (string.IsNullOrEmpty(value) == true)
            {
                result = false;
            }
            else
            {
                Regex _pattern = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.Compiled);
                result = _pattern.IsMatch(value);
            }

            return result;
        }
    }
}
