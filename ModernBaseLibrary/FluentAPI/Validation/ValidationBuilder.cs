//-----------------------------------------------------------------------
// <copyright file="ValidationBuilder.cs" company="Lifeprojects.de">
//     Class: ValidationBuilder
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>21.07.2020</date>
//
// <summary>
//      Result Class for FluentValidation Implementierung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("PropertyName={this.PropertyName}; ValidResult={this.ValidResult}")]
    public class ValidationBuilder
    {
        public object PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public object FromValue { get; set; }

        public object ToValue { get; set; }

        public Type PropertyType { get; set; }

        public bool ValidResult { get; set; }

        public string ValidMessage { get; set; }
    }
}
