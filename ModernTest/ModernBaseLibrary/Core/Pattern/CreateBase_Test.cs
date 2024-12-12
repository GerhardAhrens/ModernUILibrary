//-----------------------------------------------------------------------
// <copyright file="CreateBase_Test.cs" company="Lifeprojects.de">
//     Class: CreateBase_Test
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>12.12.2024 08:15:58</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.Pattern
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Core;

    [TestClass]
    public class CreateBase_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBase_Test"/> class.
        /// </summary>
        public CreateBase_Test()
        {
        }

        [TestMethod]
        public void CreateInstance_Test_A()
        {
            var liste = (null as List<string>).Create();
            Assert.IsNotNull(liste);
        }

        [TestMethod]
        public void CreateInstance_Test_B()
        {
            List<string> liste = null;
            liste = liste.Create();
            Assert.IsNotNull(liste);
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
