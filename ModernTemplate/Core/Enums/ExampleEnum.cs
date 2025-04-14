//-----------------------------------------------------------------------
// <copyright file="MyClass.cs" company="Lifeprojects.de">
//     Class: MyClass
//     Copyright © Lifeprojects.de yyyy
// </copyright>
//
// <author>Developer</author>
// <email>developerMail@irgendwas.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Kurze Beschreibung der Klasse
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core.Enums
{
    using System;
    using System.ComponentModel;

    public enum ExampleEnum : int
    {
        [Description("Keine Festlegung")]
        None = 0,
    }
}
