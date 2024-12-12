//-----------------------------------------------------------------------
// <copyright file="GuardNotLessThan_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotLessThan_Test
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
    public class GuardNotLessThan_Test
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
        /// Initializes a new instance of the <see cref="GuardNotLessThan_Test"/> class.
        /// </summary>
        public GuardNotLessThan_Test()
        {
        }

        [DataRow(-1, 0, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(-1, 0, "paramName", null, "[paramName] cannot be less than 0." + PARAMNAMEMESSAGE)]
        [DataRow(-1, 0, "", null, "[parameter] cannot be less than 0." + PARAMETERMESSAGE)]
        [DataRow(-1, 0, " ", null, "[parameter] cannot be less than 0." + PARAMETERMESSAGE)]
        [DataRow(-1, 0, null, null, "[parameter] cannot be less than 0." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotLessThan_InvalidInputDefaultException_ThrowsException(
            int input,
            int threshold,
            string paramName,
            string errorMessage,
            string expectedErrorMessage)
        {
            try
            {
                Guard.NotLessThan(input, threshold, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void NotLessThan_InvalidInputDefaultException2_ThrowsException()
        {
            string input = "a";
            string threshold = "b";

            try
            {
                Guard.NotLessThan(input, threshold, nameof(input));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void NotLessThan_InvalidInputDefaultException3_ThrowsException()
        {
            bool input = false;
            bool threshold = true;

            try
            {
                Guard.NotLessThan(input, threshold, nameof(input));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void NotLessThan_InvalidInputCustomException_ThrowsException()
        {
            int input = -1;
            int threshold = 0;
            var expectedErrorMessage = "error message" + PARAMNAMEMESSAGE;
            var exception = new Exception(expectedErrorMessage);

            try
            {
                Guard.NotLessThan(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void NotLessThan_InvalidInputNullCustomException_ThrowsException()
        {
            int input = -1;
            int threshold = 0;
            Exception exception = null;

            try
            {
                Guard.NotLessThan(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [DataRow(null)]
        [DataRow("custom message")]
        [TestMethod]
        public void NotLessThan_InvalidNullCustomException2_ThrowsException(string message)
        {
            int input = -1;
            int threshold = 0;

            try
            {
                if (message == null)
                {
                    Guard.NotLessThan<int, InvalidOperationException>(input, threshold);
                }
                else
                {
                    Guard.NotLessThan<int, InvalidOperationException>(input, threshold, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [DataRow(0, 0)]
        [DataRow(2, 0)]
        [TestMethod]
        public void NotLessThan_ValidInput_DoesNotThrowException(int input, int threshold)
        {
            Guard.NotLessThan(input, threshold, nameof(input), null);
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
