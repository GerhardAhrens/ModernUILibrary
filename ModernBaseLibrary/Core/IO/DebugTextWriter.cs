//-----------------------------------------------------------------------
// <copyright file="DebugTextWriter.cs" company="Lifeprojects.de">
//     Class: DebugTextWriter
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
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// A TextWriter that writes to the Debug window.
    /// </summary>
    public class DebugTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Debug.Write(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            Debug.Write(value);
        }
    }
}