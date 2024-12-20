//-----------------------------------------------------------------------
// <copyright file="GuardNotNullOrWhitespace_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotNullOrWhitespace_Test
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ArgumentNotNullOrWhitespace_Test
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
        /// Initializes a new instance of the <see cref="ArgumentNotNullOrWhitespace_Test"/> class.
        /// </summary>
        public ArgumentNotNullOrWhitespace_Test()
        {
        }

        [TestMethod]
        [DataRow(null, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(null, "paramName", null, "[paramName] cannot be Null, empty or white-space." + PARAMNAMEMESSAGE)]
        [DataRow(null, "", null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow(null, " ", null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow(null, null, null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow("", "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow("", "paramName", null, "[paramName] cannot be Null, empty or white-space." + PARAMNAMEMESSAGE)]
        [DataRow("", "", null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow("", " ", null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow("", null, null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow(" ", "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(" ", "paramName", null, "[paramName] cannot be Null, empty or white-space." + PARAMNAMEMESSAGE)]
        [DataRow(" ", "", null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow(" ", " ", null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        [DataRow(" ", null, null, "[parameter] cannot be Null, empty or white-space." + PARAMETERMESSAGE)]
        public void NotNullOrWhitespace_InvalidInputDefaultException_ThrowsException(
            string input,
            string paramName,
            string errorMessage,
            string expectedErrorMessage)
            {

            try
            {
                Argument.NotNullOrWhitespace(input, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void NotNullOrWhitespace_InvalidInputCustomException_ThrowsException()
        {
            string input = null;
            var expectedErrorMessage = "error message" + PARAMETERMESSAGE;
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Argument.NotNullOrWhitespace(input, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [TestMethod]
        public void NotNullOrWhitespace_InvalidInputNullCustomException_ThrowsException()
        {
            string input = null;
            Exception exception = null;

            try
            {
                Argument.NotNullOrWhitespace(input, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow(null, "custom message")]
        [DataRow("", "custom message")]
        [TestMethod]
        public void NotNullOrWhitespace_InvalidNullCustomException2_ThrowsException(string input, string message)
        {
            try
            {
                if (message == null)
                {
                    Argument.NotNullOrWhitespace<InvalidOperationException>(input);
                }
                else
                {
                    Argument.NotNullOrWhitespace<InvalidOperationException>(input, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void NotNullOrWhitespace_ValidInput_DoesNotThrowException()
        {
            var input = "input";

            try
            {
                Argument.NotNullOrWhitespace(input, nameof(input), null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
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
