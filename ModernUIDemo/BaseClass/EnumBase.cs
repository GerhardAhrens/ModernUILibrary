//-----------------------------------------------------------------------
// <copyright file="EnumBase.cs" company="Lifeprojects.de">
//     Class: EnumBase
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Class of EnumBase Base Implemation</summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;
    using System.Runtime.Serialization;

    public class EnumBase : ISerializable
    {
        protected readonly int Value;

        [NonSerialized]
        protected readonly string Description;

        public EnumBase(SerializationInfo info, StreamingContext context)
        {
            this.Description = (string)info.GetValue("Description", typeof(string));
            this.Value = (int)info.GetValue("Value", typeof(int));
        }

        protected EnumBase(int value, string description)
        {
            this.Value = value;
            this.Description = description;
        }

        public static bool operator ==(EnumBase x, EnumBase y)
        {
            return ((object)x != null && x.Equals(y)) || ((object)x == null && (object)y == null);
        }

        public static bool operator !=(EnumBase x, EnumBase y)
        {
            return ((object)x != null && !x.Equals(y)) || ((object)x == null && (object)y != null);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EnumBase);
        }

        public bool Equals(EnumBase p)
        {
            return !object.ReferenceEquals(p, null) && this.Value.Equals(p.Value);
        }

        public string ToShortText(bool isUpper = true)
        {
            string result = string.Empty;
            if (this.Description.ToLower().Contains("develop") == true)
            {
                if (isUpper == true)
                {
                    result = this.Description.Substring(0, 3).ToUpper();
                }
                else
                {
                    result = this.Description.Substring(0, 3);
                }
            }
            else if (this.Description.ToLower().Contains("user acceptance testing") == true)
            {
                if (isUpper == true)
                {
                    result = "UAT";
                }
                else
                {
                    result = "Uat";
                }
            }
            else
            {
                if (isUpper == true)
                {
                    result = this.Description.Substring(0, 4).ToUpper();
                }
                else
                {
                    result = this.Description.Substring(0, 4);
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return this.Value;
        }

        public override string ToString()
        {
            return this.Description;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", this.Value);
            info.AddValue("Description", this.Description);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            this.GetObjectData(info, context);
        }
    }
}
