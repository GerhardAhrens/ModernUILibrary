namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ComparerObjectDifferences_Test
    {
        [TestMethod]
        public void ComparerObjectWithNullObject_EqualsTrue()
        {
            Person p1 = null;

            Person p2 = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(p1, p2);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 0);
        }

        [TestMethod]
        public void ComparerObjectWithOneObject_EqualsFalse()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(p1, p2);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 3);
        }

        [TestMethod]
        public void ObjectEqualsTrue()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = new Person();
            p2.PersonId = 12;
            p2.Name = "Gerhard";
            p2.Age = 58;
            p2.MeetingDate = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(p1,p2);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 0);
        }

        [TestMethod]
        public void ObjectEqualsTrue_IgnorProperty()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = new Person();
            p2.PersonId = 12;
            p2.Name = "Gerhard";
            p2.Age = 58;
            p2.MeetingDate = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(p1, p2,"Age");
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 0);
        }

        [TestMethod]
        public void ObjectEqualsFalse()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 13;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 60;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard Ahrens";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 3);
        }

        [TestMethod]
        public void ObjectEqualsFalse_IgnorProperty1()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 13;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 60;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard Ahrens";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(CurrentPerson, oldPerson,"Age");
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 2);
        }

        [TestMethod]
        public void ObjectEqualsFalse_IgnorProperty2()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 13;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 60;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard Ahrens";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            string[] ignorProperty = new IgnorAuditTrailWords().IgnorPropertiesAsArray;
            List<CompareResult> compareResult = CompareObject.GetDifferences(CurrentPerson, oldPerson, ignorProperty);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 2);
        }

        [TestMethod]
        public void ObjectEqualsFalse_IgnorProperty3()
        {
            Person CurrentPerson = new Person();
            CurrentPerson.PersonId = 13;
            CurrentPerson.Name = "Gerhard";
            CurrentPerson.Age = 60;
            CurrentPerson.MeetingDate = null;

            Person oldPerson = new Person();
            oldPerson.PersonId = 12;
            oldPerson.Name = "Gerhard Ahrens";
            oldPerson.Age = 58;
            oldPerson.MeetingDate = null;

            string[] ignorProperty = new IgnorAuditTrailWords().IgnorPropertiesAsArray;
            List<CompareResult> compareResult = CompareObject.GetDifferences(CurrentPerson, oldPerson, null);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 3);
        }

        [TestMethod]
        public void ObjectWithTwoObjectAndList1_EqualsTrue()
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

            Department dept = new Department();
            dept.DepartmentId = 1;
            dept.DepartmentName = "PTA";
            List<Department> deptList = new List<Department>();
            deptList.Add(dept);

            CurrentPerson.Department = deptList;
            oldPerson.Department = deptList;

            List<CompareResult> compareResult = CompareObject.GetDifferences(CurrentPerson, oldPerson);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 0);
        }

        [TestMethod]
        public void ComparerObjectWithTwoObjectAndList2_EqualsFalse()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard Ahrens";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = new Person();
            p2.PersonId = 12;
            p2.Name = "Gerhard";
            p2.Age = 58;
            p2.MeetingDate = null;

            Department dept = new Department();
            dept.DepartmentId = 1;
            dept.DepartmentName = "PTA";
            List<Department> deptList = new List<Department>();
            deptList.Add(dept);

            p1.Department = deptList;
            p2.Department = null;

            List<CompareResult> compareResult = CompareObject.GetDifferences(p1, p2);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 2);
        }

        [TestMethod]
        public void ComparerObjectWithTwoObjectAndList3_EqualsFalse()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = new Person();
            p2.PersonId = 12;
            p2.Name = "Gerhard";
            p2.Age = 58;
            p2.MeetingDate = null;

            Department dept1 = new Department();
            dept1.DepartmentId = 1;
            dept1.DepartmentName = "PTA";
            List<Department> deptList1 = new List<Department>();
            deptList1.Add(dept1);
            p1.Department = deptList1;

            Department dept2 = new Department();
            dept2.DepartmentId = 2;
            dept2.DepartmentName = "PTA";
            List<Department> deptList2 = new List<Department>();
            deptList2.Add(dept2);
            p2.Department = deptList2;

            List<CompareResult> compareResult = CompareObject.GetDifferences(p1, p2);
            Assert.IsNotNull(compareResult);
            Assert.IsTrue(compareResult.Count == 1);
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

        private class IgnorAuditTrailWords 
        {
            public IgnorAuditTrailWords()
            {
                if (this.IgnorProperties == null)
                {
                    this.IgnorProperties = new List<string>();
                    this.IgnorProperties.Add("Age");
                }
            }

            public List<string> IgnorProperties { get; private set; }

            public string[] IgnorPropertiesAsArray { get { return IgnorProperties.ToArray(); } }
        }
    }
}