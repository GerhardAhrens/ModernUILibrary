namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Core;

    [TestClass]
    public class ObjectDumper_Test
    {
        [TestMethod]
        public void DumpObjectDataSet()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            DataRow row = dt.NewRow();
            row.ItemArray = new Object[] { "George Washington", 44 };
            dt.Rows.Add(row);
            ds.Tables.Add(dt);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            ObjectDumper.Write(ds, 1, sw);
            File.WriteAllText(@"C:\temp\object.txt", sb.ToString());
        }

        [TestMethod]
        public void DumpObjectDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            DataRow row = dt.NewRow();
            row.ItemArray = new Object[] { "George Washington", 44 };
            dt.Rows.Add(row);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            ObjectDumper.Write(dt, 1, sw);
            File.WriteAllText(@"C:\temp\object.txt", sb.ToString());
        }

        [TestMethod]
        public void DumpObjectModelClass()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            ObjectDumper.Write(p1, 1, sw);
            File.WriteAllText(@"C:\temp\object.txt", sb.ToString());
        }

        private class Person
        {
            public int PersonId { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }

            public DateTime? MeetingDate { get; set; }

            public List<Department> Department { get; set; }
        }

        private class Department
        {
            public int DepartmentId { get; set; }

            public string DepartmentName { get; set; }
        }
    }
}