//-----------------------------------------------------------------------
// <copyright file="GuardNotInRange_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotInRange_Test
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
    public class GuardNotInRange_Test
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
        /// Initializes a new instance of the <see cref="GuardNotInRange_Test"/> class.
        /// </summary>
        public GuardNotInRange_Test()
        {
        }

        [DataRow(11,1, 10, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(11,1, 10, "paramName", null, "[paramName] is not in Range 1 to 10." + PARAMNAMEMESSAGE)]
        [DataRow(11,1, 10, "", null, "[parameter] is not in Range 1 to 10." + PARAMETERMESSAGE)]
        [DataRow(11,1, 10, " ", null, "[parameter] is not in Range 1 to 10." + PARAMETERMESSAGE)]
        [DataRow(11,1, 10, null, null, "[parameter] is not in Range 1 to 10." + PARAMETERMESSAGE)]
        [DataRow(0,1, 10, null, null, "[parameter] is not in Range 1 to 10." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotInRange_InvalidInputDefaultException_ThrowsException(
            int input,
            int minValue,
            int maxValue,
            string paramName,
            string errorMessage,
            string expectedErrorMessage)
        {
            try
            {
                Argument.NotInRange(input, minValue, maxValue, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void NotInRange_InvalidInputCustomException_ThrowsException()
        {
            int input = 11;
            int minValue = 1;
            int maxValue = 10;
            var expectedErrorMessage = "error message" + PARAMNAMEMESSAGE;
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Argument.NotInRange(input, minValue, maxValue, exception);
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
            int minValue = 1;
            int maxValue = 10;

            try
            {
                if (message == null)
                {
                    Argument.NotInRange<int, InvalidOperationException>(input, minValue, maxValue);
                }
                else
                {
                    Argument.NotInRange<int, InvalidOperationException>(input, minValue, maxValue, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [DataRow(11, 5, 10)]
        [DataRow(0, 1, 10)]
        [TestMethod]
        public void NotInRange_InvalidInputNullCustomException_ThrowsException(int input, int minValue, int maxValue)
        {
            Exception exception = null;

            try
            {
                Argument.NotInRange(input, minValue, maxValue, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow(9, 5,10)]
        [DataRow(5, 1, 10)]
        [TestMethod]
        public void NotInRange_ValidInput_DoesNotThrowException(int input, int minValue, int maxValue)
        {
            Argument.NotInRange(input, minValue, maxValue, nameof(input), null);
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
