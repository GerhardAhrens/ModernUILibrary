namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Data;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CompareDataTable_Test
    {
        private DataTable firstTable = null;
        private DataTable secondTable = null;

        [TestMethod]
        public void CompareDataRow_TableWithNull()
        {
            try
            {
                DataTable compare = CompareDataTable.CompareDifferences(null, null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CompareDataRow_TableNotNull()
        {
            InitDataFirst();
            InitDataSecond();

            try
            {
                DataTable compare = CompareDataTable.CompareDifferences(firstTable, secondTable);
                Assert.IsNotNull(compare);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        private void InitDataFirst()
        {
            firstTable = new DataTable("DTAuthor_1");
            firstTable.Columns.Add("Id", typeof(Guid));
            firstTable.Columns.Add("Author", typeof(string));
            firstTable.Columns.Add("Birthday", typeof(DateTime));
            firstTable.Columns.Add("IsActive", typeof(bool));
            firstTable.Columns.Add("Country", typeof(string));
            firstTable.PrimaryKey = new DataColumn[] { firstTable.Columns["Author"] };

            DataRow dr1 = firstTable.NewRow();
            dr1["Id"] = Guid.NewGuid();
            dr1["Author"] = "Otto";
            dr1["Birthday"] = new DateTime(2017, 4, 16);
            dr1["IsActive"] = true;
            dr1["Country"] = "DE";
            firstTable.Rows.Add(dr1);

            DataRow dr2 = firstTable.NewRow();
            dr2["Id"] = Guid.NewGuid();
            dr2["Author"] = "Karl";
            dr2["Birthday"] = new DateTime(2011, 1, 24);
            dr2["IsActive"] = false;
            dr2["Country"] = "DE";
            firstTable.Rows.Add(dr2);

            DataRow dr3 = firstTable.NewRow();
            dr3["Id"] = Guid.NewGuid();
            dr3["Author"] = "RabaZamba";
            dr3["Birthday"] = new DateTime(1967, 6, 30);
            dr3["IsActive"] = true;
            dr3["Country"] = "EN";
            firstTable.Rows.Add(dr3);
        }

        private void InitDataSecond()
        {
            secondTable = new DataTable("DTAuthor_2");
            secondTable.Columns.Add("Id", typeof(Guid));
            secondTable.Columns.Add("Author", typeof(string));
            secondTable.Columns.Add("Birthday", typeof(DateTime));
            secondTable.Columns.Add("IsActive", typeof(bool));
            secondTable.Columns.Add("Country", typeof(string));
            secondTable.PrimaryKey = new DataColumn[] { secondTable.Columns["Author"] };

            DataRow dr1 = secondTable.NewRow();
            dr1["Id"] = Guid.NewGuid();
            dr1["Author"] = "Otto";
            dr1["Birthday"] = new DateTime(2017, 4, 16);
            dr1["IsActive"] = true;
            dr1["Country"] = "DE";
            secondTable.Rows.Add(dr1);

            DataRow dr2 = secondTable.NewRow();
            dr2["Id"] = Guid.NewGuid();
            dr2["Author"] = "Karl";
            dr2["Birthday"] = new DateTime(2011, 1, 24);
            dr2["IsActive"] = false;
            dr2["Country"] = "DE";
            secondTable.Rows.Add(dr2);

            DataRow dr3 = secondTable.NewRow();
            dr3["Id"] = Guid.NewGuid();
            dr3["Author"] = "RabaZamba";
            dr3["Birthday"] = new DateTime(1967, 6, 30);
            dr3["IsActive"] = true;
            dr3["Country"] = "EN";
            secondTable.Rows.Add(dr3);
        }

    }
}