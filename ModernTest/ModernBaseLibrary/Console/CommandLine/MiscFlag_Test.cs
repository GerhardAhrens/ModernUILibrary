/*
 * <copyright file="MiscFlag_Test.cs" company="Lifeprojects.de">
 *     Class: MiscFlag_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>21.11.2022 21:05:32</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary.Console.CommandLine
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.CommandLine;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class MiscFlag_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiscFlag_Test"/> class.
        /// </summary>
        public MiscFlag_Test()
        {
        }

        [TestMethod]
        [TestDescription("program --dataset=1 --runs=5 --switch-probability=0.7 --iterations 100 -o ../project/folder")]
        public void Test_That_Long_Names_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program --dataset=1 --runs=5 --switch-probability=0.7 --iterations 100 -o ../project/folder");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output);
        }

        [TestMethod]
        [TestDescription("program -d=1 -r=5 -x=0.7 -i 100 --output ../project/folder")]
        public void Test_That_Short_Names_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program -d=1 -r=5 -x=0.7 -i 100 --output ../project/folder");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output);
        }

        [TestMethod]
        [TestDescription("program -d=1 -r=5 -x=0.7 -i 100 -o=../project/folder")]
        public void Test_That_Short_Names_With_Complex_Joins_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program -d=1 -r=5 -x=0.7 -i 100 -o=../project/folder");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output);
        }

        [TestMethod]
        [TestDescription("program -d=1 -r=5 -x=0.7 -i 100 --output=../project/folder")]
        public void Test_That_Long_Names_With_Complex_Joins_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program -d=1 -r=5 -x=0.7 -i 100 --output=../project/folder");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output);
        }

        [TestMethod]
        [TestDescription("program -d=1 -r=5 -x=0.7 -i 100 -o=\"../project/folder\"")]
        public void Test_That_Short_Names_With_Complex_Joins_And_Quotes_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program -d=1 -r=5 -x=0.7 -i 100 -o=\"../project/folder\"");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output?.Trim('"'));
        }

        [TestMethod]
        [TestDescription("program -d=1 -r=5 -x=0.7 -i 100 --output=\"../project/folder\"")]
        public void Test_That_Long_Names_With_Complex_Joins_And_Quotes_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program -d=1 -r=5 -x=0.7 -i 100 --output=\"../project/folder\"");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output?.Trim('"'));
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

        [Help("== This is a Test Model ==")]
        private class MiscTestModel : ICmdLineModel
        {
            [Flag("dataset", "d")]
            public int Dataset { get; set; }

            [Flag("runs", "r")]
            public int Runs { get; set; }

            [Flag("switch-probability", "x")]
            public double SwitchProbability { get; set; }

            [Flag("iterations", "i")]
            public int NoOfIterations { get; set; }

            [Flag("output", "o")]
            public string Output { get; set; }

            public string[] Extras { get; set; }
            public string[] Options { get; set; }
        }
    }
}
