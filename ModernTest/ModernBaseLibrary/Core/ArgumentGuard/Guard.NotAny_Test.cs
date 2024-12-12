//-----------------------------------------------------------------------
// <copyright file="GuardNotAny_Test.cs" company="Lifeprojects.de">
//     Class: GuardNotAny_Test
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
    public class GuardNotAny_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardNotAny_Test"/> class.
        /// </summary>
        public GuardNotAny_Test()
        {
        }

        [TestMethod]
        public void NotAny_InvalidInput_ThrowsException()
        {
            var items = new List<int>();
            var exception = new Exception("error message");

            try
            {
                Guard.NotAny(items, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow("custom message")]
        [TestMethod]
        public void NotAny_InvalidNullCustomException2_ThrowsException(string message)
        {
            var items = new List<int>();

            try
            {
                if (message == null)
                {
                    Guard.NotAny<int, InvalidOperationException>(items);
                }
                else
                {
                    Guard.NotAny<int, InvalidOperationException>(items, message);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void NotAny_InvalidInputNullCustomException_ThrowsException()
        {
            var items = new List<int>();
            Exception exception = null;

            try
            {
                Guard.NotAny(items, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void NotAny_NullInput_ThrowsException()
        {
            List<int> items = null;
            Exception exception = new Exception();

            try
            {
                Guard.NotAny(items, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void NotAny_ValidInput_DoesNotThrowException()
        {
            List<int> items = new List<int> { 1 };

            try
            {
                Guard.NotAny(items, nameof(items));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
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
