//-----------------------------------------------------------------------
// <copyright file="GuardNotNull_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotNull_Test
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
    public class GuardNotNull_Test
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
        /// Initializes a new instance of the <see cref="GuardNotNull_Test"/> class.
        /// </summary>
        public GuardNotNull_Test()
        {
        }

        [DataRow("paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow("paramName", null, "[paramName] cannot be Null." + PARAMNAMEMESSAGE)]
        [DataRow("", null, "[parameter] cannot be Null." + PARAMETERMESSAGE)]
        [DataRow(" ", null, "[parameter] cannot be Null." + PARAMETERMESSAGE)]
        [DataRow(null, null, "[parameter] cannot be Null." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotNull_InvalidInputDefaultException_ThrowsException(
            string paramName,
            string errorMessage,
            string expectedErrorMessage)
        {
            object input = null;

            try
            {
                Guard.NotNull(input, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void NotNull_InvalidInputCustomException_ThrowsException()
        {
            object input = null;
            var expectedErrorMessage = "error message" + PARAMNAMEMESSAGE;
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Guard.NotNull(input, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [TestMethod]
        public void NotNull_InvalidNullCustomException_ThrowsException()
        {
            object input = null;
            Exception exception = null;

            try
            {
                Guard.NotNull(input, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [DataRow(null)]
        [DataRow("custom message")]
        [TestMethod]
        public void NotNull_InvalidNullCustomException2_ThrowsException(string message)
        {
            object input = null;

            try
            {
                if (message == null)
                {
                    Guard.NotNull<object, InvalidOperationException>(input);
                }
                else
                {
                    Guard.NotNull<object, InvalidOperationException>(input, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void NotNull_ValidInput_DoesNotThrowException()
        {
            var input = new object();

            try
            {
                Guard.NotNull(input, nameof(input), null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
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
