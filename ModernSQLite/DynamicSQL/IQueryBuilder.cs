//-----------------------------------------------------------------------
// <copyright file="IQueryBuilder.cs" company="Lifeprojects.de">
//     Class: IQueryBuilder
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>Definition of IQueryBuilder Interface Class</summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public interface IQueryBuilder
    {
        string BuildQuery();
    }
}
