//-----------------------------------------------------------------------
// <copyright file="GuardNotEqualTo_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotEqualTo_Test
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
    public class GuardNotEqualTo_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardNotEqualTo_Test"/> class.
        /// </summary>
        public GuardNotEqualTo_Test()
        {
        }

        [TestMethod]
        public void NotEqualTo_InvalidInput_ThrowsException()
        {
            int input = 11;
            int @value = 10;
            Exception exception = new Exception();

            try
            {
                Guard.NotEqualTo(input, @value, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow(11, "11")]
        [DataRow(null, "11")]
        [DataRow("11", null)]
        [TestMethod]
        public void NotEqualTo_InvalidInput2_ThrowsException(object input, object @value)
        {
            Exception exception = new Exception();

            try
            {
                Guard.NotEqualTo(input, @value, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [TestMethod]
        public void NotEqualTo_ValidInput_DoesNotThrowException()
        {
            int input = 10;
            int @value = 10;
            Exception exception = new Exception();

            Guard.NotEqualTo(input, @value, exception);
        }

        [TestMethod]
        public void NotEqualTo_ValidInput2_DoesNotThrowException()
        {
            string input = "11";
            string @value = "11";
            Exception exception = new Exception();

            Guard.NotEqualTo(input, @value, exception);
        }

        [DataRow(null, null)]
        [DataRow(11, 11)]
        [DataRow("11", "11")]
        [TestMethod]
        public void NotEqualTo_ValidInput3_DoesNotThrowException(object input, object @value)
        {
            Exception exception = new Exception();

            Guard.NotEqualTo(input, @value, exception);
        }

        [TestMethod]
        public void NotEqualTo_InvalidInputNullCustomException_ThrowsException()
        {
            int input = 11;
            int @value = 10;
            Exception exception = null;

            try
            {
                Guard.NotEqualTo(input, @value, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void NotEqualTo_InvalidInputNullCustomException_ThrowsException2()
        {
            int input = 11;
            int @value = 10;
            Exception exception = null;

            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotEqualTo(input, @value, exception));
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
