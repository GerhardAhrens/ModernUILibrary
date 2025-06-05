//-----------------------------------------------------------------------
// <copyright file="AddBracket.cs" company="Lifeprojects.de">
//     Class: AddBracket
//     Copyright © PTA GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>10.06.2021</date>
//
// <summary>
// Setzen von runden Klammern in der Where-Anweisung
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System.ComponentModel;

    public enum AddBracket
    {
        [Description("Keine")]
        None = 0,
        [Description("Klammer links")]
        BracketLeft = 1,
        [Description("Klammer rechts")]
        BracketRight = 2
    }
}
