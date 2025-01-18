//-----------------------------------------------------------------------
// <copyright file="FluentObject.cs" company="Lifeprojects.de">
//     Class: FluentObject
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@pta.de</email>
// <date>21.06.2021</date>
//
// <summary>
// Die Klasse stellt Methoden zur Object Behandlung auf Basis einer
// FluentAPI (FluentObject Extension) zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Diagnostics;

    using ModernBaseLibrary.FluentAPI;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentObject : FluentObject<FluentObject>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentObject"/> class.
        /// </summary>
        public FluentObject(object value) : base(value)
        {
        }
    }

    public class FluentObject<TAssertions> : ReferenceTypeAssertions<Object, TAssertions> where TAssertions : FluentObject<TAssertions>
    {
        public FluentObject(object value) : base(value)
        {
            this.ObjectValue = value;
        }

        private object ObjectValue { get; set; }

        public bool IsNull()
        {
            return this.ObjectValue == null;
        }

        public bool IsNotNull()
        {
            return this.ObjectValue != null;
        }

        public string Quote()
        {
            if (this.ObjectValue == null || this.ObjectValue == DBNull.Value)
            {
                return "NULL";
            }
            else if (this.ObjectValue is int || this.ObjectValue is long)
            {
                return this.ObjectValue.ToString();
            }
            else if (this.ObjectValue is decimal)
            {
                return ((decimal)this.ObjectValue).ToString("0.00");
            }
            else if (this.ObjectValue is double)
            {
                return ((double)this.ObjectValue).ToString("0.00");
            }
            else if (this.ObjectValue is bool)
            {
                return (bool)this.ObjectValue ? "1" : "0";
            }
            else if (this.ObjectValue is DateTime)
            {
                return $"'{((DateTime)this.ObjectValue).ToString("yyyy-MM-dd")}'";
            }
            else
            {
                return $"'{this.ObjectValue.ToString().Replace("'", "''")}'";
            }
        }
    }
}