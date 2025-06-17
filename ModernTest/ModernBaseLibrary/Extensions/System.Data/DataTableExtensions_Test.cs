//-----------------------------------------------------------------------
// <copyright file="DataTableExtensions_Test.cs" company="Lifeprojects.de">
//     Class: DataTableExtensions_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>17.06.2025 09:53:00</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataTableExtensions_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableExtensions_Test"/> class.
        /// </summary>
        public DataTableExtensions_Test()
        {
        }

        [TestMethod]
        public void HasColumnWith_Id()
        {
            DataTable dt = CreateStruktur();
            Assert.IsTrue(dt.HasColumn("Id"));
        }

        [TestMethod]
        public void HasColumnNot_Id()
        {
            DataTable dt = CreateStruktur();
            Assert.IsFalse(dt.HasColumn("IdX"));
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

        private static DataTable CreateStruktur()
        {
            DataTable dt = new DataTable("DataTableDemo");

            DataColumn dc = new DataColumn();
            dc.Caption = "Country";
            dc.ColumnName = "Country";
            dc.MaxLength = 3;
            dc.Unique = false;
            dc.AllowDBNull = false;

            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("Author", typeof(string));
            dt.Columns.Add("Birthday", typeof(DateTime));
            dt.Columns.Add("IsActive", typeof(bool));
            dt.Columns.Add(dc);

            return dt;
        }
    }
}
