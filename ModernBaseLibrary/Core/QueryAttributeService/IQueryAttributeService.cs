//-----------------------------------------------------------------------
// <copyright file="IQueryAttributeService.cs" company="Lifeprojects.de">
//     Class: IQueryAttributeService
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.08.2017</date>
//
// <summary>
// Interface zum QueryAttributeService
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ModernBaseLibrary.Core
{
    public interface IQueryAttributeService : IDisposable
    {
        string GetDefaultAttribute<T>();

        IDictionary<string, object> GetExternAttributes<TAttr>(Type typ) where TAttr : Attribute;

        IDictionary<string, string> GetMetadataAttributes();
    }
}
