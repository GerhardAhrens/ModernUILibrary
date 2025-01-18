//-----------------------------------------------------------------------
// <copyright file="FluentString_Test.cs" company="Lifeprojects.de">
//     Class: FluentString_Test
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2021</date>
//
// <summary>
// UnitTest for Fluent String
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentString_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        public FluentString_Test()
        {
        }

        [TestMethod]
        public void IsLengthAndLength()
        {
            string value = "Ich bin ein String";
            bool result = value.That().IsLength(0);
            Assert.IsFalse(result);

            int resultLength = value.That().Length();
            Assert.IsTrue(resultLength == value.Length);
        }

        [TestMethod]
        public void IsMatchOrIsNotMatch()
        {
            string emailAddress = "developer@lifeprojects.de";
            bool result = emailAddress.That().IsMatch("*@*.com");
            Assert.IsTrue(result);

            emailAddress = "developer@lifeprojects.de";
            result = emailAddress.That().IsNotMatch("*@*.com");
            Assert.IsTrue(result);
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