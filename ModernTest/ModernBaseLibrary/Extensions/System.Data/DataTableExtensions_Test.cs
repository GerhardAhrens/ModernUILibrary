//-----------------------------------------------------------------------
// <copyright file="DataTableExtensions_Test.cs" company="Lifeprojects.de">
//     Class: DataTableExtensions_Test
//     Copyright � Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>17.06.2025 09:53:00</date>
//
// <summary>
// Klasse f�r 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
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

        [TestMethod]
        public void GetRow_True()
        {
            DataTable dt = CreateStruktur();
            dt = FillWithOverRows(dt);

            DataRow dr = dt.GetRow(0);
            Assert.IsNotNull(dr);
            Assert.IsTrue(dr.ItemArray.Length > 0);
        }

        [TestMethod]
        public void GetRows_True()
        {
            DataTable dt = CreateStruktur();
            dt = FillWithOverRows(dt);

            DataRow[] dr = dt.GetRows();
            Assert.IsNotNull(dr);
            Assert.IsTrue(dr.Count() > 0);
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

        private static DataTable FillWithOverRows(DataTable dt)
        {
            DataRow dr1 = dt.NewRow();
            dr1["Id"] = new Guid("{4BB4C40A-71E6-42B0-A73A-1320066D28AC}");
            dr1["Author"] = "Otto Osterhase";
            dr1["Birthday"] = new DateTime(2017, 4, 16);
            dr1["IsActive"] = true;
            dr1["Country"] = "DE";
            dt.Rows.Add(dr1);
            dt.AcceptChanges();

            DataRow dr2 = dt.NewRow();
            dr2["Id"] = new Guid("{78AE10E0-373B-41B9-8463-43CC5048F850}");
            dr2["Author"] = "Max Osterhase";
            dr2["Birthday"] = new DateTime(2016, 4, 16);
            dr2["IsActive"] = false;
            dr2["Country"] = "DE";

            int yourPosition = 0;
            dt.Rows.InsertAt(dr2, yourPosition);
            dt.AcceptChanges();

            return dt;
        }
    }
}
