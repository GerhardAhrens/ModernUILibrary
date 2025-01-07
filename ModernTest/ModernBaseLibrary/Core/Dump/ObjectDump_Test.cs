namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectDump_Test
    {
        [TestMethod]
        public void DumpObjectFullMember()
        {
            Person p1 = new Person();
            p1.PersonId = 12;
            p1.Name = "Gerhard";
            p1.Age = 58;
            p1.MeetingDate = DateTime.Now;

            var aa = ObjectDump.Dump(p1);
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