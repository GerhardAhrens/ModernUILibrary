//-----------------------------------------------------------------------
// <copyright file="ActionTextWriter.cs" company="Lifeprojects.de">
//     Class: ActionTextWriter
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.01.2023</date>
//
// <summary>
// Umleiten einer Ausgabe über den TextWriter
// </summary>
// <Website>
// Originally published at http://damieng.com/blog/2008/07/30/linq-to-sql-log-to-debug-window-file-memory-or-multiple-writers
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// A TextWriter that can delegate the writing to any Action that takes a string.
    /// </summary>
    public class ActionTextWriter : TextWriter
    {
        readonly Action<string> action;

        public ActionTextWriter(Action<string> action)
        {
            this.action = action;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override void Write(string value)
        {
            action(value);
        }
    }
}