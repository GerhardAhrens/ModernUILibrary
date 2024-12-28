namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ComparerObject_Test
    {
        [TestMethod]
        public void ComparerObjectWithTwoObject_EqualsTrue()
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

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == true);
        }

        [TestMethod]
        public void ComparerObjectWithTwoObjectIgnor_EqualsTrue()
        {
            Person p1 = new Person();
            p1.PersonId = 13;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = new Person();
            p2.PersonId = 12;
            p2.Name = "Gerhard";
            p2.Age = 58;
            p2.MeetingDate = null;

            bool isEqual = p1.ObjectPropertiesEqual(p2, "PersonId");
            Assert.IsTrue(isEqual == true);
        }

        [TestMethod]
        public void ComparerObjectWithTwoObject_EqualsFalse()
        {
            Person p1 = new Person();
            p1.PersonId = 13;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = null;

            Person p2 = new Person();
            p2.PersonId = 12;
            p2.Name = "Gerhard";
            p2.Age = 58;
            p2.MeetingDate = null;

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == false);
        }

        [TestMethod]
        public void ComparerObjectWithTwoObjectAndList1_EqualsTrue()
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

            Department dept = new Department();
            dept.DepartmentId = 1;
            dept.DepartmentName = "PTA";
            List<Department> deptList = new List<Department>();
            deptList.Add(dept);

            p1.Department = deptList;
            p2.Department = deptList;

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == true);
        }

        [TestMethod]
        public void ComparerObjectWithTwoObjectAndList2_EqualsFalse()
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

            Department dept = new Department();
            dept.DepartmentId = 1;
            dept.DepartmentName = "PTA";
            List<Department> deptList = new List<Department>();
            deptList.Add(dept);

            p1.Department = deptList;
            p2.Department = null;

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == false);
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

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == false);
        }

        [TestMethod]
        public void ComparerObjectWithNullObject_EqualsTrue()
        {
            Person p1 = null;

            Person p2 = null;

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == true);
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

            bool isEqual = p1.ObjectPropertiesEqual(p2);
            Assert.IsTrue(isEqual == false);
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