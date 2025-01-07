//-----------------------------------------------------------------------
// <copyright file="ConverterSimpleBase.cs" company="Lifeprojects.de">
//     Class: ConverterSimpleBase
//     Copyright © Lifeprojects.de GmbH 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2022</date>
//
// <summary>
// Basisklasse für WPF Value Converter
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Windows.Markup;

    public abstract class ConverterSimpleBase : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
