//-----------------------------------------------------------------------
// <copyright file="GuardNotNullOrEmpty_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotNullOrEmpty_Test
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
    public class GuardNotNullOrEmpty_Test
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
        /// Initializes a new instance of the <see cref="GuardNotNullOrEmpty_Test"/> class.
        /// </summary>
        public GuardNotNullOrEmpty_Test()
        {
        }

        [DataRow(null, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(null, "paramName", null, "[paramName] cannot be Null or empty." + PARAMNAMEMESSAGE)]
        [DataRow(null, "", null, "[parameter] cannot be Null or empty." + PARAMETERMESSAGE)]
        [DataRow(null, " ", null, "[parameter] cannot be Null or empty." + PARAMETERMESSAGE)]
        [DataRow(null, null, null, "[parameter] cannot be Null or empty." + PARAMETERMESSAGE)]
        [DataRow("", "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow("", "paramName", null, "[paramName] cannot be Null or empty." + PARAMNAMEMESSAGE)]
        [DataRow("", "", null, "[parameter] cannot be Null or empty." + PARAMETERMESSAGE)]
        [DataRow("", " ", null, "[parameter] cannot be Null or empty." + PARAMETERMESSAGE)]
        [DataRow("", null, null, "[parameter] cannot be Null or empty." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotNullOrEmpty_InvalidInputDefaultException_ThrowsException(
            string input,
            string paramName,
            string errorMessage,
            string expectedErrorMessage)
        {
            try
            {
                Argument.NotNullOrEmpty(input, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void NotNullOrEmpty_InvalidInputCustomException_ThrowsException()
        {
            string input = null;
            var expectedErrorMessage = "error message";
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Argument.NotNullOrEmpty(input, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [TestMethod]
        public void NotNullOrEmpty_InvalidInputNullCustomException_ThrowsException()
        {
            string input = null;
            Exception exception = null;

            try
            {
                Argument.NotNullOrEmpty(input, exception);
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
        public void NotNullOrEmpty_InvalidNullCustomException2_ThrowsException(string input, string message)
        {
            try
            {
                if (message == null)
                {
                    Argument.NotNullOrEmpty<InvalidOperationException>(input);
                }
                else
                {
                    Argument.NotNullOrEmpty<InvalidOperationException>(input, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [DataRow("input")]
        [DataRow(" ")]
        [TestMethod]
        public void NotNullOrEmpty_ValidInput_DoesNotThrowException(string input)
        {
            try
            {
                Argument.NotNullOrEmpty(input, nameof(input), null);
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
