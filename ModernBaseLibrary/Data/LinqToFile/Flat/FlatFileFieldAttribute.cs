//-----------------------------------------------------------------------
// <copyright file="FlatFileFieldAttribute.cs" company="Lifeprojects.de">
//     Class: FlatFileFieldAttribute
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// Attributes Class for Flat File Field
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class FlatFileFieldAttribute : FileFieldAttributeBase
    {
        public FlatFileFieldAttribute(int start, int length)
        {
            this.Start = start;
            this.Length = length;
        }

        public int Start { get; private set; }

        public int Length { get; private set; }
    }
}