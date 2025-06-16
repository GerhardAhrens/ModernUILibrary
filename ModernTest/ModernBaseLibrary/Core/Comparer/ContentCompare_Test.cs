namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ContentCompare_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }


        [TestMethod]
        public void ObjectAreEqual_A()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            DataRow p2 = new PersonDataRow().GetData();

            Assert.IsTrue(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }

        [TestMethod]
        public void ObjectAreEqual_B()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            p1.AgeDec = 64.1M;
            DataRow p2 = new PersonDataRow().GetData();
            p2["AGEDEC"] = 64.1M;

            Assert.IsTrue(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }

        [TestMethod]
        public void ObjectAreEqual_C()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            p1.AgeDec = 64M;
            DataRow p2 = new PersonDataRow().GetData();
            p2["AGEDEC"] = 64.0M;

            Assert.IsTrue(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }

        [TestMethod]
        public void ObjectAreEqual_D()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            p1.AgeDec = 64.0M;
            DataRow p2 = new PersonDataRow().GetData();
            p2["AGEDEC"] = 64M;

            Assert.IsTrue(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }

        [TestMethod]
        public void ObjectAreNotEqual_A()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            p1.Author = "Charlie";
            DataRow p2 = new PersonDataRow().GetData();

            Assert.IsFalse(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }

        [TestMethod]
        public void ObjectAreNotEqual_B()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            p1.Birthday = new DateTime(1960, 6, 1);
            DataRow p2 = new PersonDataRow().GetData();

            Assert.IsFalse(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }

        [TestMethod]
        public void ObjectAreNotEqual_C()
        {
            PersonDTO p1 = new PersonDTO().GetData();
            p1.AgeDec = 64.1m;
            DataRow p2 = new PersonDataRow().GetData();

            Assert.IsFalse(ContentCompare<PersonDTO>.IsObjectEqual(p1, p2));
        }
        private class PersonDTO
        {
            public Guid Id { get; set; }
            public string Author { get; set; }
            public DateTime Birthday { get; set; }
            public bool IsActive { get; set; }
            public string Country { get; set; }
            public int AgeInt { get; set; }
            public decimal AgeDec { get; set; }

            public PersonDTO GetData()
            {
                this.Id = new Guid("{32BA406E-CBDF-49C2-8540-0FD169C628C6}");
                this.Author = "Gerhard Ahrens";
                this.Birthday = new DateTime(1960, 6, 28);
                this.IsActive = true;
                this.Country = "DE";
                this.AgeInt = 64;
                this.AgeDec = 64.0m;
                return this;
            }
        }

        private class PersonDataRow
        {
            DataTable dt = new DataTable("DataTableDemo");

            public DataRow GetData()
            {
                DataColumn dc = new DataColumn();
                dc.Caption = "Country";
                dc.ColumnName = "Country";
                dc.MaxLength = 3;
                dc.Unique = false;
                dc.AllowDBNull = false;

                dt.Columns.Add("ID", typeof(Guid));
                dt.Columns.Add("AUTHOR", typeof(string));
                dt.Columns.Add("BIRTHDAY", typeof(DateTime));
                dt.Columns.Add("ISACTIVE", typeof(bool));
                dt.Columns.Add(dc);
                dt.Columns.Add("AGEINT", typeof(int));
                dt.Columns.Add("AGEDEC", typeof(decimal));

                dt.Rows.Add(new Guid("{32BA406E-CBDF-49C2-8540-0FD169C628C6}"), "Gerhard Ahrens", new DateTime(1960, 6, 28), true, "DE", 64, 64.0m);

                return dt.Rows[0];
            }
        }
    }
}