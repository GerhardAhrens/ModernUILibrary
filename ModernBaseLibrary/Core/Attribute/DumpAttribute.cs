//-----------------------------------------------------------------------
// <copyright file="DumpAttribute.cs" company="Lifeprojects.de">
//     Class: DumpAttribute
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>
// Class for DumpAttribute
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public class DumpAttribute : Attribute
    {
        public DumpAttribute([CallerMemberName] string description = "")
        {
            this.Description = description;
        }

        public string Description
        {
            get;
            set;
        }
    }
}