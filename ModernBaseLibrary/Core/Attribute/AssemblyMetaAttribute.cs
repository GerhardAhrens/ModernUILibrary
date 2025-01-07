//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaAttribute.cs" company="Lifeprojects.de">
//     Class: AssemblyMetaAttribute
//     Copyright © Gerhard Ahrens, 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.04.2022</date>
//
// <summary>Class for Assembly Meta Data</summary>
//-----------------------------------------------------------------------

namespace System
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class AssemblyMetaAttribute : Attribute
    {
        private string _Key;
        /// <summary>
        /// Beschreibung zum Property
        /// </summary>
        public string Key
        {
            get { return (this._Key); }
            set
            {
                this._Key = value;
            }
        }

        private string _Value;
        /// <summary>
        /// Beschreibung zum Property
        /// </summary>
        public string Value
        {
            get { return (this._Value); }
            set
            {
                this._Value = value;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BugFixAttribute"/> class.
        /// </summary>

        public AssemblyMetaAttribute(string pKey, string pValue)
        {
            this.Key = pKey;
            this.Value = pValue;
        }
        public override string ToString()
        {
            return string.Format("{0}~{1}", this.Key, this.Value);
        }
    }
}