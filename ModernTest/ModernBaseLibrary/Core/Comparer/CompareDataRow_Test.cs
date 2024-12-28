//-----------------------------------------------------------------------
// <copyright file="CompareDataRow_Test.cs" company="Lifeprojects.de">
//     Class: CompareDataRow_Test
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2021</date>
//
// <summary>
// UnitTest for
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CompareDataRow_Test
    {
        private DataTable firstTable = null;
        private DataTable secondTable = null;

        [TestMethod]
        public void CompareDataRow_RowWithNull()
        {
            try
            {
                List<CompareResult> compareRow = CompareDataRow.CompareDifferences(null, null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_RowNotNull1()
        {
            InitDataFirst();
            InitDataSecond();

            try
            {
                DataRow rowTabFirst = firstTable.Rows[0];
                DataRow rowTabsecond = secondTable.Rows[0];
                List<CompareResult> compareRow = CompareDataRow.CompareDifferences(rowTabFirst, rowTabsecond);
                Assert.IsNotNull(compareRow);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_RowNotNull2()
        {
            InitDataFirst();
            InitDataSecond();

            try
            {
                foreach (DataRow currentRow in firstTable.Rows)
                {
                    DataRow secondRow = secondTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("Referenz") == currentRow.Field<int>("Referenz"));

                    List<CompareResult> compareRow = CompareDataRow.CompareDifferences(currentRow, secondRow,"Id");
                    Assert.IsNotNull(compareRow);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_RowNotNull3()
        {
            InitDataFirst();
            InitDataSecond();

            List<string> ignorColumn = new List<string>() { "Id" };

            try
            {
                foreach (DataRow currentRow in firstTable.Rows)
                {
                    DataRow secondRow = secondTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("Referenz") == currentRow.Field<int>("Referenz"));

                    List<CompareResult> compareRow = CompareDataRow.CompareDifferences(currentRow, secondRow, ignorColumn);
                    Assert.IsNotNull(compareRow);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_RowNotNull4()
        {
            InitDataFirst();
            InitDataSecond();

            List<string> ignorColumn = new List<string>() { "Id" };

            try
            {
                foreach (DataRow currentRow in firstTable.Rows)
                {
                    DataRow oldRow = secondTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("Referenz") == currentRow.Field<int>("Referenz"));

                    string result = CompareDataRow.CompareDifferences(oldRow, currentRow, ignorColumn).ToText();
                    if (string.IsNullOrEmpty(result) == false)
                    {
                        Debug.WriteLine(result);
                    }

                    string resultShort = CompareDataRow.CompareDifferences(oldRow, currentRow, ignorColumn).ToShortText();
                    if (string.IsNullOrEmpty(resultShort) == false)
                    {
                        Debug.WriteLine(resultShort);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_RowNotNull5()
        {
            InitDataFirst();
            InitDataSecond();

            List<string> ignorColumn = new List<string>() { "Id" };

            try
            {
                foreach (DataRow currentRow in firstTable.Rows)
                {
                    DataRow oldRow = secondTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("Referenz") == currentRow.Field<int>("Referenz"));

                    List<string> result = CompareDataRow.CompareDifferences(oldRow, currentRow, ignorColumn).ToList();
                    if (result != null)
                    {
                        result.ForEach(x => Debug.WriteLine(x));
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_RowWithNullToText()
        {
            try
            {
                string resultFull = CompareDataRow.CompareDifferences(null, null).ToText();
                string resultShort = CompareDataRow.CompareDifferences(null, null).ToShortText();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        private void InitDataFirst()
        {
            firstTable = new DataTable("DTAuthor");
            firstTable.Columns.Add("Id", typeof(Guid));
            firstTable.Columns.Add("Referenz", typeof(int));
            firstTable.Columns.Add("Author", typeof(string));
            firstTable.Columns.Add("Department", typeof(string));
            firstTable.Columns.Add("Factor", typeof(int));
            firstTable.Columns.Add("Birthday", typeof(DateTime));
            firstTable.Columns.Add("IsActive", typeof(bool));
            firstTable.Columns.Add("Country", typeof(string));
            firstTable.PrimaryKey = new DataColumn[] { firstTable.Columns["Author"] };

            DataRow dr1 = firstTable.NewRow();
            dr1["Id"] = Guid.NewGuid();
            dr1["Referenz"] = 1;
            dr1["Author"] = "Otto";
            dr1["Department"] = "Innen";
            dr1["Factor"] = 10;
            dr1["Birthday"] = new DateTime(2017, 4, 16);
            dr1["IsActive"] = true;
            dr1["Country"] = "DE";
            firstTable.Rows.Add(dr1);

            DataRow dr2 = firstTable.NewRow();
            dr2["Id"] = Guid.NewGuid();
            dr2["Referenz"] = 2;
            dr2["Author"] = "Karl";
            dr2["Department"] = "Innen";
            dr2["Factor"] = 10;
            dr2["Birthday"] = new DateTime(2011, 1, 24);
            dr2["IsActive"] = false;
            dr2["Country"] = "DE";
            firstTable.Rows.Add(dr2);

            DataRow dr3 = firstTable.NewRow();
            dr3["Id"] = Guid.NewGuid();
            dr3["Referenz"] = 3;
            dr3["Author"] = "RabaZamba";
            dr3["Department"] = "Innen";
            dr3["Factor"] = 10;
            dr3["Birthday"] = new DateTime(1967, 6, 30);
            dr3["IsActive"] = true;
            dr3["Country"] = "EN";
            firstTable.Rows.Add(dr3);
        }

        private void InitDataSecond()
        {
            secondTable = new DataTable("DTAuthor");
            secondTable.Columns.Add("Id", typeof(Guid));
            secondTable.Columns.Add("Referenz", typeof(int));
            secondTable.Columns.Add("Author", typeof(string));
            secondTable.Columns.Add("Department", typeof(string));
            secondTable.Columns.Add("Factor", typeof(int));
            secondTable.Columns.Add("Birthday", typeof(DateTime));
            secondTable.Columns.Add("IsActive", typeof(bool));
            secondTable.Columns.Add("Country", typeof(string));
            secondTable.PrimaryKey = new DataColumn[] { secondTable.Columns["Author"] };

            DataRow dr1 = secondTable.NewRow();
            dr1["Id"] = Guid.NewGuid();
            dr1["Referenz"] = 1;
            dr1["Author"] = "Otto";
            dr1["Department"] = "Innen";
            dr1["Factor"] = 10;
            dr1["Birthday"] = new DateTime(2017, 4, 16);
            dr1["IsActive"] = true;
            dr1["Country"] = "DE";
            secondTable.Rows.Add(dr1);

            DataRow dr2 = secondTable.NewRow();
            dr2["Id"] = Guid.NewGuid();
            dr2["Referenz"] = 2;
            dr2["Author"] = "Karl";
            dr2["Department"] = "Innen";
            dr2["Factor"] = 10;
            dr2["Birthday"] = new DateTime(2011, 1, 24);
            dr2["IsActive"] = false;
            dr2["Country"] = "DE";
            secondTable.Rows.Add(dr2);

            DataRow dr3 = secondTable.NewRow();
            dr3["Id"] = Guid.NewGuid();
            dr3["Referenz"] = 3;
            dr3["Author"] = "RabaZambaX";
            dr3["Department"] = "Extern";
            dr3["Factor"] = 20;
            dr3["Birthday"] = new DateTime(1967, 6, 30);
            dr3["IsActive"] = true;
            dr3["Country"] = string.Empty;
            secondTable.Rows.Add(dr3);
        }
    }
}