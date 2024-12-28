//-----------------------------------------------------------------------
// <copyright file="DynamicXmlNodeNotFoundException.cs" company="Lifeprojects.de">
//     Class: DynamicXmlNodeNotFoundException
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.06.2017</date>
//
// <summary>
// Exception for Class to read and write to XML Files
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.XML
{
    using System;

    /// <summary>
    /// The exception that is thrown when an XML node is not found.
    /// </summary>
    public class DynamicXmlNodeNotFoundException : Exception
    {
        public DynamicXmlNodeNotFoundException(string xPath)
        {
            this.XPath = xPath;
        }

        public string XPath { get; private set; }
    }
}