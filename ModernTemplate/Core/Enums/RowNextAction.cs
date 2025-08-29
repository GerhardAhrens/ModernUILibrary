//-----------------------------------------------------------------------
// <copyright file="RowNextAction.cs" company="company">
//     Class: RowNextAction
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Enum Klasse für die Übergabe weiterer Aktionen.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;

    public enum RowNextAction : int
    {
        None = 0,
        AddRow = 1,
        InsertRow = 2,
        UpdateRow = 3,
        CopyRow = 4,
        Refresh = 5,
    }
}
