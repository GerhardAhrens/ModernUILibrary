//-----------------------------------------------------------------------
// <copyright file="GuardFor_Test.cs" company="Lifeprojects.de">
//     Class: GuardFor_Test
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
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ArgumentFor_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentFor_Test"/> class.
        /// </summary>
        public ArgumentFor_Test()
        {
        }

        [TestMethod]
        public void For_InvalidInput_ThrowsException()
        {
            var items = new List<int>();
            bool predicate() => !items.Any();
            var exception = new Exception("error message");

            try
            {
                Argument.For(predicate, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [TestMethod]
        public void For_InvalidInputNullException_ThrowsException()
        {
            var items = new List<int>();
            bool predicate() => !items.Any();
            Exception exception = null;

            try
            {
                Argument.For(predicate, exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        [DataRow("custom message")]
        [TestMethod]
        public void For_InvalidInput2_ThrowsException(string message)
        {
            var items = new List<int>();
            bool predicate() => !items.Any();

            try
            {
                if (message == null)
                {
                    Argument.For<InvalidOperationException>(predicate);
                }
                else
                {
                    Argument.For<InvalidOperationException>(predicate, message);
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
        public void For_ValidInput_DoesNotThrowException(int input, int threshold)
        {
            var items = new List<int> { 1 };
            bool predicate() => !items.Any();
            var exception = new Exception("error message");

            try
            {
                Argument.For(predicate, exception);
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
