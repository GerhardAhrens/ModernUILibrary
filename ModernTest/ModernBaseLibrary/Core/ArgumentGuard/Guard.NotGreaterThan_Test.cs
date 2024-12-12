//-----------------------------------------------------------------------
// <copyright file="GuardNotGreaterThan_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotGreaterThan_Test
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
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuardNotGreaterThan_Test
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
        /// Initializes a new instance of the <see cref="GuardNotGreaterThan_Test"/> class.
        /// </summary>
        public GuardNotGreaterThan_Test()
        {
        }

        [DataRow(11, 10, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(11, 10, "paramName", null, "[paramName] cannot be greater than 10." + PARAMNAMEMESSAGE)]
        [DataRow(11, 10, "", null, "[parameter] cannot be greater than 10." + PARAMETERMESSAGE)]
        [DataRow(11, 10, " ", null, "[parameter] cannot be greater than 10." + PARAMETERMESSAGE)]
        [DataRow(11, 10, null, null, "[parameter] cannot be greater than 10." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotGreaterThan_InvalidInputDefaultException_ThrowsException(int input, int threshold, string paramName, string errorMessage, string expectedErrorMessage)
        {
            try
            {
                Guard.NotGreaterThan(input, threshold, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void NotGreaterThan_InvalidInputCustomException_ThrowsException()
        {
            int input = 11;
            int threshold = 10;
            var expectedErrorMessage = "error message" + PARAMNAMEMESSAGE;
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Guard.NotGreaterThan(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [TestMethod]
        public void NotGreaterThan_InvalidInputNullCustomException_ThrowsException()
        {
            int input = 11;
            int threshold = 10;
            Exception exception = null;

            try
            {
                Guard.NotGreaterThan(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow(null)]
        [DataRow("custom message")]
        [TestMethod]
        public void NotGreaterThan_InvalidNullCustomException2_ThrowsException(string message)
        {
            int input = 11;
            int threshold = 10;

            try
            {
                if (message == null)
                {
                    Guard.NotGreaterThan<int, InvalidOperationException>(input, threshold);
                }
                else
                {
                    Guard.NotGreaterThan<int, InvalidOperationException>(input, threshold, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [DataRow(9, 10)]
        [DataRow(10, 10)]
        [TestMethod]
        public void NotGreaterThan_ValidInput_DoesNotThrowException(int input, int threshold)
        {
            Guard.NotGreaterThan(input, threshold, nameof(input), null);
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
