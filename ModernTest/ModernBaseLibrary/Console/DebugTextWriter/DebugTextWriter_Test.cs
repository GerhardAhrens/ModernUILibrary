//-----------------------------------------------------------------------
// <copyright file="DebugTextWriter_Test.cs" company="Lifeprojects.de">
//     Class: DebugTextWriter_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.12.2022 06:47:11</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class DebugTextWriter_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugTextWriter_Test"/> class.
        /// </summary>
        public DebugTextWriter_Test()
        {
        }

        [TestMethod]
        public void ConsoleTraceListeners_Test()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.WriteLine("Hello World"); 
            Console.ReadKey();
        }

        [TestMethod]
        public void ConsoleDebugTextWriter_Test()
        {
            Console.SetOut(new DebugTextWriter());
            Console.WriteLine("Hello World.");
            Console.ReadKey();
        }

        [TestMethod]
        public void OutputDebugStringTextWriter_Test()
        {
            Console.SetOut(new OutputDebugStringTextWriter());
            Console.WriteLine("Hello World.");
            Console.ReadKey();
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}
