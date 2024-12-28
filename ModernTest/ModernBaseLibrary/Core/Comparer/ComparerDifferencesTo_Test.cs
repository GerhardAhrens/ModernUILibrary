namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ComparerDifferencesTo_Test
    {
        [TestMethod]
        public void ObjectWithNullObject_EqualsTrue()
        {
            Person CurrentPerson = null;

            Person oldPerson = null;

            DataTable compareResult = CompareDifferences.ToDataTable(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Rows.Count == 0);
        }

        [TestMethod]
        public void ObjectWithOneObjectToDataTable_EqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 58;
            CurrentPerson.MeetingDate = DateTime.Now;

            Person oldPerson = null;

            DataTable compareResult = CompareDifferences.ToDataTable(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Rows.Count == 3);
        }

        [TestMethod]
        public void ObjectWithOneObjectToList_EqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 58;
            CurrentPerson.MeetingDate = DateTime.Now;

            Person oldPerson = null;

            List<CompareResult> compareResult = CompareDifferences.ToList(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 4);

            int countNull = compareResult.Count(c => c.SecondValue == null);
            Assert.IsTrue(countNull == compareResult.Count);
        }

        [TestMethod]
        public void ObjectWithOneObjectAddValues_EqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 58;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = null;

            DataTable compareResult = CompareDifferences.ToDataTable(CurrentPerson, oldPerson, CurrentPerson.PersonId, "Hauptdaten");
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Rows.Count == 3);
        }

        [TestMethod]
        public void WithTwoObjectToDataTable_EqualsTrue()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 58;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            DataTable compareResult = CompareDifferences.ToDataTable(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Rows.Count == 0);
        }

        [TestMethod]
        public void WithTwoObjectToDataTable_EqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard Ahrens";
            CurrentPerson.Age = 60;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            DataTable compareResult = CompareDifferences.ToDataTable(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Rows.Count == 2);
        }

        [TestMethod]
        public void WithTwoObjectToDataTableddValues_EqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard Ahrens";
            CurrentPerson.Age = 60;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            DataTable compareResult = CompareDifferences.ToDataTable(CurrentPerson, oldPerson,CurrentPerson.PersonId, "Hauptdaten");
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Rows.Count == 2);
        }

        [TestMethod]
        public void WithTwoObjectToList_EqualsTrue()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 12;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 58;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            List<CompareResult> compareResult = CompareDifferences.ToList(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 0);
        }

        [TestMethod]
        public void WithTwoObjectToList_EqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 13;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 58;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard Ahrens";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = new DateTime(2021,3,24);

            List<CompareResult> compareResult = CompareDifferences.ToList(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 3);
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