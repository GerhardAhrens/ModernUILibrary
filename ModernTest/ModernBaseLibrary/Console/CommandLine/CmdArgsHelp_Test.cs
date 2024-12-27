//-----------------------------------------------------------------------
// <copyright file="CmdArgsHelp_Test.cs" company="Lifeprojects.de">
//     Class: CmdArgsHelp_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <Framework>6.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.11.2022 09:45:29</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Console.CommandLine
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.CommandLine;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CmdArgsHelp_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdArgsHelp_Test"/> class.
        /// </summary>
        public CmdArgsHelp_Test()
        {
        }

        [TestMethod]
        [TestDescription("program --help")]
        public void AttributeHelpLong()
        {
            string[] args = CommandManager.CommandLineToArgs("program --help");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<CmdLineModel>();
            StringAssert.Contains(helpText, "== This is a Test Model for Help-Attribute ==");
        }

        [TestMethod]
        [TestDescription("program -h")]
        public void AttributeHelpShort()
        {
            string[] args = CommandManager.CommandLineToArgs("program -h");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<CmdLineModel>();
            StringAssert.Contains(helpText, "== This is a Test Model for Help-Attribute ==");
        }

        [TestMethod]
        [TestDescription("program --quelle --help", "Usage: [-u --quelle (This is the Quellverzeichnis property)]")]
        public void HelpPropertyLongOption()
        {
            string[] args = CommandManager.CommandLineToArgs("program --quelle --help");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<CmdLineModel>();
            Assert.AreEqual(helpText, "Usage: [-q --quelle (Quellverzeichnis)]");
        }

        [TestMethod]
        [TestDescription("program -q -h", "Usage: [-u --quelle (This is the Quellverzeichnis property)]")]
        public void HelpPropertyShortOption()
        {
            string[] args = CommandManager.CommandLineToArgs("program --quelle --help");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<CmdLineModel>();
            Assert.AreEqual(helpText, "Usage: [-q --quelle (Quellverzeichnis)]");
        }

        [TestMethod]
        [TestDescription("program -h")]
        public void Help_PrintHelpText()
        {
            string[] args = CommandManager.CommandLineToArgs("program -h");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(parser.GetHelpInfo<CmdLineModelNoHelpText>());
        }

        [TestMethod]
        [TestDescription("program -n -h")]
        public void Help_CheckWhenNoHelpTextExists()
        {
            string[] args = CommandManager.CommandLineToArgs("program -n -h");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<CmdLineModelNoHelpText>();
            StringAssert.Equals(helpText, "No Help Attribute found on Property [UserName]");
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

        [Help("== This is a Test Model for Help-Attribute ==")]
        private class CmdLineModel
        {
            [Flag("quelle", "q")]
            [Help("Quellverzeichnis")]
            public string source { get; set; }

            [Flag("ziel", "z")]
            [Help("Zielverzeichnis")]
            public string destination { get; set; }
        }

        private class CmdLineModelNoHelpText
        {
            [Flag("quelle", "q")]
            public string source { get; set; }

            [Flag("ziel", "z")]
            public string destination { get; set; }

            [Flag("name", "n")]
            public string UserName { get; set; }
        }
    }
}
