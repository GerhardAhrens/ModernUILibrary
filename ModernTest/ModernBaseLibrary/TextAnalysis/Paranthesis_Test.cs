//-----------------------------------------------------------------------
// <copyright file="Paranthesis_Test.cs" company="Lifeprojects.de">
//     Class: Paranthesis_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.02.2022 06:52:21</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Text
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Paranthesis_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paranthesis_Test"/> class.
        /// </summary>
        public Paranthesis_Test()
        {
        }

        [TestMethod]
        public void CheckAllParanthesisOK()
        {
            bool result = false;
            string input = "{ ( [ ] ) }";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckAllParanthesisEmpty()
        {
            bool result = false;
            string input = "";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckAllParanthesisWithtext()
        {
            bool result = false;
            string input = "{ Test } [ Test]";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckAllParanthesisWithtextWrong()
        {
            bool result = false;
            string input = "{ Test } Test]";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckAllParanthesisFailed1()
        {
            bool result = false;
            string input = "{)";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckAllParanthesisFailed2()
        {
            bool result = false;
            string input = "{ (";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckAllParanthesisFailed3()
        {
            bool result = false;
            string input = "(";

            using (Paranthesis p = new Paranthesis())
            {
                result = p.IsValid(input);
            }

            Assert.IsFalse(result);
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
