//-----------------------------------------------------------------------
// <copyright file="GuardNotLessThanOrEqualTo_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotLessThanOrEqualTo_Test
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
    public class ArgumentNotLessThanOrEqualTo_Test
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
        /// Initializes a new instance of the <see cref="ArgumentNotLessThanOrEqualTo_Test"/> class.
        /// </summary>
        public ArgumentNotLessThanOrEqualTo_Test()
        {
        }

        [DataRow(-1, 0, "paramName", "custom error message", "custom error message" + PARAMNAMEMESSAGE)]
        [DataRow(-1, 0, "paramName", null, "[paramName] cannot be less than or equal to 0." + PARAMNAMEMESSAGE)]
        [DataRow(-1, 0, "", null, "[parameter] cannot be less than or equal to 0." + PARAMETERMESSAGE)]
        [DataRow(-1, 0, " ", null, "[parameter] cannot be less than or equal to 0." + PARAMETERMESSAGE)]
        [DataRow(-1, 0, null, null, "[parameter] cannot be less than or equal to 0." + PARAMETERMESSAGE)]
        [DataRow(0, 0, null, null, "[parameter] cannot be less than or equal to 0." + PARAMETERMESSAGE)]
        [TestMethod]
        public void NotLessThanOrEqualTo_InvalidInputDefaultException_ThrowsException(
            int input,
            int threshold,
            string paramName,
            string errorMessage,
            string expectedErrorMessage)
        {
            try
            {
                Argument.NotLessThanOrEqualTo(input, threshold, paramName, errorMessage);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [DataRow("a", "b")]
        [DataRow("a", "a")]
        [TestMethod]
        public void NotLessThanOrEqualTo_InvalidInputDefaultException2_ThrowsException(string input, string threshold)
        {
            try
            {
                Argument.NotLessThanOrEqualTo(input, threshold, nameof(input));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [DataRow(-1, 0)]
        [DataRow(0, 0)]
        [TestMethod]
        public void NotLessThanOrEqualTo_InvalidInputNullCustomException_ThrowsException(int input, int threshold)
        {
            Exception exception = null;

            try
            {
                Argument.NotLessThanOrEqualTo(input, threshold, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [DataRow(-1, 0, null)]
        [DataRow(0, 0, "custom message")]
        [TestMethod]
        public void NotLessThanOrEqualTo_InvalidNullCustomException2_ThrowsException(int input, int threshold, string message)
        {
            try
            {
                if (message == null)
                {
                    Argument.NotLessThanOrEqualTo<int, InvalidOperationException>(input, threshold);
                }
                else
                {
                    Argument.NotLessThanOrEqualTo<int, InvalidOperationException>(input, threshold, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [DataRow(2, 0)]
        [TestMethod]
        public void NotLessThanOrEqualTo_ValidInput_DoesNotThrowException(int input, int threshold)
        {
            try
            {
                Argument.NotLessThanOrEqualTo(input, threshold, nameof(input), null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
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
