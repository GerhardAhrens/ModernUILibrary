//-----------------------------------------------------------------------
// <copyright file="GuardNotIsMatch_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotIsMatch_Test
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.05.2023</date>
//
// <summary>
// Test Klasse für Argument Validation
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.ArgumentGuard
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuardNotIsMatch_Test
    {
        private const string PARAMNAMEMESSAGE = " (Parameter 'paramName')";
        private const string PARAMETERMESSAGE = " (Parameter 'parameter')";

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardNotIsMatch_Test"/> class.
        /// </summary>
        public GuardNotIsMatch_Test()
        {
        }

        [DataRow("123", "123")]
        [DataRow("", "")]
        [DataRow("  ", " ")]
        [DataRow(" ", " ")]
        [DataRow("123", @"\d+")]
        [DataRow("12", @"\d{2}")]
        [DataRow("12asdasd", @"\.*")]
        [DataRow("my-us3r_n4m3", @"^[a-z0-9_-]{3,16}$")]
        [DataRow("john@doe.com", @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$")]
        [DataRow("Aa", @"aa", RegexOptions.IgnoreCase)]
        [TestMethod]
        public void NotIsMatch_ValidInput_DoesNotThrowException(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            Exception exception = new Exception();

            Argument.NotIsMatch(input, pattern, exception, options);
        }

        [DataRow("123", "1234")]
        [DataRow("", " ")]
        [DataRow("  ", "   ")]
        [DataRow("123", @"\d{4}")]
        [DataRow("th1s1s-wayt00_l0ngt0beausername", @"^[a-z0-9_-]{3,16}$")]
        [DataRow("john@doe.something", @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$")]
        [DataRow("Aa", @"aa")]
        [TestMethod]
        public void NotIsMatch_InvalidInput_ThrowsException(string input, string pattern)
        {
            Exception exception = new Exception();

            try
            {
                Argument.NotIsMatch(input, pattern, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow("paramName", "custom error message", $"custom error message " + PARAMNAMEMESSAGE)]
        [DataRow("paramName", null, "[paramName] does not match the pattern [1234]." + PARAMNAMEMESSAGE)]
        [DataRow("", null, "[parameter] does not match the pattern [1234]." + PARAMETERMESSAGE)]
        [DataRow(" ", null, "[parameter] does not match the pattern [1234]." + PARAMETERMESSAGE)]
        [DataRow(null, null, "[parameter] does not match the pattern [1234]." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotIsMatch_InvalidInputDefaultException_ThrowsException(
                    string paramName,
                    string errorMessage,
                    string expectedErrorMessage)
        {
            string input = "123";
            string pattern = "1234";

            try
            {
                Argument.NotIsMatch(input, paramName, pattern, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [DataRow(null)]
        [DataRow("custom message")]
        [TestMethod]
        public void NotIsMatch_InvalidNullCustomException2_ThrowsException(string message)
        {
            string input = "123";
            string pattern = "1234";

            try
            {
                if (message == null)
                {
                    Argument.NotIsMatch<InvalidOperationException>(input, pattern);
                }
                else
                {
                    Argument.NotIsMatch<InvalidOperationException>(input, pattern, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void NotIsMatch_InvalidInputNullException_ThrowsException()
        {
            string input = "";
            string pattern = "";
            Exception exception = null;

            Argument.NotIsMatch(input, pattern, exception);
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
