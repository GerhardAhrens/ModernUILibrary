//-----------------------------------------------------------------------
// <copyright file="ConvertTypeInfo.cs" company="Lifeprojects.de">
//     Class: ConvertTypeInfo
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.06.2023 13:34:20</date>
//
// <summary>
// Die Klasse stellt eine ValueObject vom Typ User zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.CoreBase;
    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public class User : ValueObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User(string name, string email = "")
        {
            Name = name;
            EMail = email;

            Create();
        }

        public string Name { get; private set; }

        public string EMail { get; private set; }

        public User Create()
        {
            if (Name.IsEmpty() == true && EMail.IsEmpty() == true)
            {
                return null;
            }

            return this;
        }

        public User TryValidate()
        {
            if (this.Name.IsEmpty() == true && this.EMail.IsEmpty() == true)
            {
                return null;
            }
            else
            {
                if (this.EMail.IsEmpty() == true)
                {
                    return this;
                }
                else
                {
                    if (IsValidAddress(EMail) == true)
                    {
                        return this;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private bool IsValidAddress(string value)
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
