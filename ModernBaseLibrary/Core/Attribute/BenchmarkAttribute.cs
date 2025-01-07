//-----------------------------------------------------------------------
// <copyright file="BenchmarkAttribute.cs" company="Lifeprojects.de">
//     Class: BenchmarkAttribute
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>11.09.2019</date>
//
// <summary>Class for BenchmarkAttribute</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class BenchmarkAttribute : Attribute
    {
    }
}
