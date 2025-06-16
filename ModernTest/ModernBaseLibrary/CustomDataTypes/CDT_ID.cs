//-----------------------------------------------------------------------
// <copyright file="CDT_Base64.cs" company="Lifeprojects.de">
//     Class: CDT_Base64
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.05.2025 11:12:02</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class CDT_ID
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CDT_Base64"/> class.
        /// </summary>
        public CDT_ID()
        {
        }

        [TestMethod]
        public void CreateNewID()
        {
            ID id = Guid.Empty;
            Assert.IsTrue(id.Status == ID.IDStatus.New);
        }

        [TestMethod]
        public void CreateEditID()
        {
            ID id = Guid.NewGuid();
            Assert.IsTrue(id.Status == ID.IDStatus.Edit);
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
