//-----------------------------------------------------------------------
// <copyright file="IFileParser.cs" company="Lifeprojects.de">
//     Class: IFileParser
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// Interface to File Provider
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Collections.Generic;

    public interface IFileParser<T> : IEnumerable<T>, IDisposable where T : new()
    {
    }
}