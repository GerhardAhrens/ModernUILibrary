//-----------------------------------------------------------------------
// <copyright file="GuardNotGreaterThanOrEqualTo_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotGreaterThanOrEqualTo_Test
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
    public class GuardNotGreaterThanOrEqualTo_Test
    {
        const string PARAMNAMEMESSAGE = " (Parameter 'paramName')";
        const string PARAMETERMESSAGE = " (Parameter 'parameter')";

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardNotGreaterThanOrEqualTo_Test"/> class.
        /// </summary>
        public GuardNotGreaterThanOrEqualTo_Test()
        {
        }

        [DataRow(11, 10, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(11, 10, "paramName", null, "[paramName] cannot be greater than or equal to 10." + PARAMNAMEMESSAGE)]
        [DataRow(11, 10, "", null, "[parameter] cannot be greater than or equal to 10." + PARAMETERMESSAGE)]
        [DataRow(11, 10, " ", null, "[parameter] cannot be greater than or equal to 10." + PARAMETERMESSAGE)]
        [DataRow(11, 10, null, null, "[parameter] cannot be greater than or equal to 10." + PARAMETERMESSAGE)]
        [DataRow(10, 10, null, null, "[parameter] cannot be greater than or equal to 10." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotGreaterThanOrEqualTo_InvalidInputDefaultException_ThrowsException(
                    int input,
                    int threshold,
                    string paramName,
                    string errorMessage,
                    string expectedErrorMessage)
        {
            try
            {
                Guard.NotGreaterThanOrEqualTo(input, threshold, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [DataRow(11, 10)]
        [DataRow(10, 10)]
        [TestMethod]
        public void NotGreaterThanOrEqualTo_InvalidInputCustomException_ThrowsException(int input, int threshold)
        {
            var expectedErrorMessage = "error message" + PARAMETERMESSAGE;
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Guard.NotGreaterThanOrEqualTo(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow(11, 10)]
        [DataRow(10, 10)]
        [TestMethod]
        public void NotGreaterThanOrEqualTo_InvalidInputNullCustomException_ThrowsException(int input, int threshold)
        {
            Exception exception = null;

            try
            {
                Guard.NotGreaterThanOrEqualTo(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow(11, 10, null)]
        [DataRow(10, 10, null)]
        [DataRow(11, 10, "custom message")]
        [DataRow(10, 10, "custom message")]
        [TestMethod]
        public void NotGreaterThanOrEqualTo_InvalidNullCustomException2_ThrowsException(int input, int threshold, string message)
        {
            try
            {
                if (message == null)
                {
                    Guard.NotGreaterThanOrEqualTo<int, InvalidOperationException>(input, threshold);
                }
                else
                {
                    Guard.NotGreaterThanOrEqualTo<int, InvalidOperationException>(input, threshold, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [DataRow(9, 10)]
        [TestMethod]
        public void NotGreaterThanOrEqualTo_ValidInput_DoesNotThrowException(int input, int threshold)
        {
            Guard.NotGreaterThanOrEqualTo(input, threshold, nameof(input), null);
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
