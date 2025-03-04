//-----------------------------------------------------------------------
// <copyright file="SimpleFormatter.cs" company="Lifeprojects.de">
//     Class: SimpleFormatter
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Definition of Logger SimpleFormatter Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Basic Fomatter.
    /// </summary>
    [DebuggerStepThrough()]
    public sealed class SimpleFormatter : IFormatter
    {
        public string FormatMessage(Record record)
        {
            string formatedMsg = string.Format("===============================\n[{0}] [{1}]:\n==============================={2}",DateTime.Now.ToString(),record.Message);
            if (record.Exception != null)
            {
                formatedMsg += "\n[Exception]===============================" + record.Exception.ToString();
            }

            return record.Message;
        }
    }
}
