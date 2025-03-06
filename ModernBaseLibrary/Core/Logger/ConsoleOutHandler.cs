//-----------------------------------------------------------------------
// <copyright file="ConsoleOutHandler.cs" company="Lifeprojects.de">
//     Class: ConsoleOutHandler
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Output from Logger in Console</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class ConsoleOutHandler : AbstractOutHandler
    {
        public override void Push(Record record)
        {
            string formatedMsg = this.formatter.FormatMessage(record);
            Console.Write(formatedMsg);
        }

        public override Task FlushAsync()
        {
            return Task.CompletedTask;
        }

        public override void Flush()
        {
        }
    }
}
