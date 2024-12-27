/*
 * https://craigwatson1962.wordpress.com/2012/02/15/linq-extension-method-to-dump-any-ienumerable/
*/

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IEnumerableExtensions_Test
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
        }

        [TestMethod]
        public void CompareSortedCollections()
        {
            List<string> left = new List<string> { "Alice", "Charles", "Derek" };
            List<string> right = new List<string> { "Bob", "Charles", "Ernie" };

            IEnumerableExtensions.CompareSortedCollections(left, right, StringComparer.CurrentCultureIgnoreCase,
                s => System.Console.WriteLine("Left: " + s), s => System.Console.WriteLine("Right: " + s), (x, y) => System.Console.WriteLine("Both: " + x + y));
        }

        [TestMethod]
        public void GetEnumeratedType()
        {
            List<string> listeString = new List<string>() { "A", "B" };
            Type result = listeString.GetType().GetEnumeratedType();
            Assert.IsTrue(result.Name == typeof(string).Name);
        }


        [DataRow("Test1", "[0] T [1] e [2] s [3] t [4] 1")]
        [TestMethod]
        public void ToPrintWith_String(string input, string expected)
        {
            string result = input.AsEnumerable().ToPrint();
            Assert.That.StringEquals(result, expected);
        }

        [TestMethod]
        public void ToPrintWith_SimpleArray()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            string result = simpleArray.AsEnumerable().ToPrint();
            Assert.That.StringEquals(result, "[0] 1 [1] 2 [2] 3 [3] 4");
        }

        [TestMethod]
        public void ToPrintWith_SimpleArrayWhereClause1()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            string result = simpleArray.AsEnumerable().ToPrint((val, pos) => val % 2 == 0);
            Assert.That.StringEquals(result, "[0] 2 [1] 4");
        }

        [TestMethod]
        public void ToPrintWith_SimpleArrayWhereClause2()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };

            Func<int, int, bool> wherePredicate =
                    (InputObject, Position) =>
                    {
                        if (Position % 2 == 0)
                        {
                            return false;
                        }

                        return true;
                    }; 

            string result = simpleArray.AsEnumerable().ToPrint(wherePredicate);

            Assert.That.StringEquals(result, "[0] 2 [1] 4");
        }

        [TestMethod]
        public void ToPrintWith_SimpleArrayConcatenat1()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };

            string result = simpleArray.AsEnumerable().ToPrint(ConcatenateFunction: (carry, val) => carry.AppendFormat("{0}, ", val));
            Assert.That.StringEquals(result, "[0] 1, [1] 2, [2] 3, [3] 4,");
        }

        [TestMethod]
        public void ToPrintWith_SimpleArrayConcatenat2()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };

            Func<StringBuilder, string, StringBuilder> concatenate = 
                (carry, val) =>
                {
                    return carry.AppendFormat("{0}, ", val);
                };

            string result = simpleArray.AsEnumerable().ToPrint(ConcatenateFunction: concatenate);
            Assert.That.StringEquals(result, "[0] 1, [1] 2, [2] 3, [3] 4,");
        }

        [TestMethod]
        public void ToPrintWith_SimpleObject()
        {
            List<Tuple<int, string>> simpleObjects = new List<Tuple<int, string>>()
                {
                  Tuple.Create(1, "Test"),
                  Tuple.Create(200, "String Test"),
                  Tuple.Create(-21, "Testing"),
                  Tuple.Create(0, "the quick brown")
                };

            Func<Tuple<int, string>, int, bool> wherePredicate =
                (obj, pos) =>
                {
                    if (obj.Item1 > 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };

            Func<Tuple<int, string>, int, string> format =
                (obj, pos) => $"[{pos}] int value={obj.Item1} string value ={obj.Item2}\n";

            string result = simpleObjects.ToPrint(wherePredicate, format);
            Assert.That.StringEquals(result, "[0] int value=200 string value =String Test");
        }

        [TestMethod]
        public void ToPrintWith_Dictionary1()
        {
            Dictionary<long, int?> dictTest = new Dictionary<long, int?>()
            {
                { 234L, null },
                {-44345L, 65742 },
                { -5644, null },
                {6799032L, 8765464 }
            };

            string result = dictTest.ToPrint((dictObj, pos) => dictObj.Value.HasValue, (dictObj, pos) => $"Key={dictObj.Key} Value={dictObj.Value.Value};");
            Assert.That.StringEquals(result, "Key=-44345 Value=65742; Key=6799032 Value=8765464;");
        }

        [TestMethod]
        public void ToPrintWith_Dictionary2()
        {
            Dictionary<long, int?> dictTest = new Dictionary<long, int?>()
            {
                { 234L, null },
                {-44345L, 65742 },
                { -5644, null },
                {6799032L, 8765464 }
            };

            Func<KeyValuePair<long, int?>, int, string> format =
                (dictObj, position) =>
                {
                    if (dictObj.Value.HasValue)
                    {
                        return $"Object {position} of {dictTest.Count()} Key = {dictObj.Key} Value = {dictObj.Value};";
                    }
                    else
                    {
                        return $"Object {position} of {dictTest.Count()} Key = {dictObj.Key} Value = null;";
                    }
                };

            string result = dictTest.ToPrint(formatFunction: format);
            Assert.That.StringEquals(result, "Object 0 of 4 Key = 234 Value = null; Object 1 of 4 Key = -44345 Value = 65742; Object 2 of 4 Key = -5644 Value = null; Object 3 of 4 Key = 6799032 Value = 8765464;");
        }

        [TestMethod]
        public void Pivot_Variante1()
        {
            var l = new List<EmployeeForPivot>() {
            new EmployeeForPivot() { Name = "Fons", Department = "R&D", Function = "Trainer", Salary = 2000 },
            new EmployeeForPivot() { Name = "Jim", Department = "R&D", Function = "Trainer", Salary = 3000 },
            new EmployeeForPivot() { Name = "Ellen", Department = "Dev", Function = "Developer", Salary = 4000 },
            new EmployeeForPivot() { Name = "Mike", Department = "Dev", Function = "Consultant", Salary = 5000 },
            new EmployeeForPivot() { Name = "Jack", Department = "R&D", Function = "Developer", Salary = 6000 },
            new EmployeeForPivot() { Name = "Demy", Department = "Dev", Function = "Consultant", Salary = 2000 }};

            var result1 = l.Pivot(emp => emp.Department, emp2 => emp2.Function, lst => lst.Sum(emp => emp.Salary));

            foreach (var row in result1)
            {
                Assert.IsTrue(row.Key == "R&D");

                foreach (var column in row.Value)
                {
                    Assert.IsTrue(column.Key == "Trainer");
                    Assert.IsTrue(column.Value == 5000);
                    break;
                }

                break;
            }
        }

        [TestMethod]
        public void Pivot_Variante2()
        {
            var l = new List<EmployeeForPivot>() {
            new EmployeeForPivot() { Name = "Fons", Department = "R&D", Function = "Trainer", Salary = 2000 },
            new EmployeeForPivot() { Name = "Jim", Department = "R&D", Function = "Trainer", Salary = 3000 },
            new EmployeeForPivot() { Name = "Ellen", Department = "Dev", Function = "Developer", Salary = 4000 },
            new EmployeeForPivot() { Name = "Mike", Department = "Dev", Function = "Consultant", Salary = 5000 },
            new EmployeeForPivot() { Name = "Jack", Department = "R&D", Function = "Developer", Salary = 6000 },
            new EmployeeForPivot() { Name = "Demy", Department = "Dev", Function = "Consultant", Salary = 2000 }};

            var result2 = l.Pivot(emp => emp.Function, emp2 => emp2.Department, lst => lst.Count());

            foreach (var row in result2)
            {
                Assert.IsTrue(row.Key == "Trainer");

                foreach (var column in row.Value)
                {
                    Assert.IsTrue(column.Key == "R&D");
                    Assert.IsTrue(column.Value == 2);
                    break;
                }

                break;
            }
        }

        [TestMethod]
        public void IEnumerable_IsNullOrEmpty()
        {
            var l = new List<EmployeeForPivot>() {
            new EmployeeForPivot() { Name = "Fons", Department = "R&D", Function = "Trainer", Salary = 2000 },
            new EmployeeForPivot() { Name = "Jim", Department = "R&D", Function = "Trainer", Salary = 3000 },
            new EmployeeForPivot() { Name = "Ellen", Department = "Dev", Function = "Developer", Salary = 4000 },
            new EmployeeForPivot() { Name = "Mike", Department = "Dev", Function = "Consultant", Salary = 5000 },
            new EmployeeForPivot() { Name = "Jack", Department = "R&D", Function = "Developer", Salary = 6000 },
            new EmployeeForPivot() { Name = "Demy", Department = "Dev", Function = "Consultant", Salary = 2000 }};

            Assert.IsTrue(l.IsNotNullOrEmpty<EmployeeForPivot>());
            Assert.IsFalse(l.IsNullOrEmpty<EmployeeForPivot>());

        }

        [TestMethod]
        public void IEnumerable_ConvertList()
        {
            var values = new[] { "1", "2", "3" };
            var result = values.ConvertList<string, int>();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoopIndex()
        {
            List<string> listeString = new List<string>() { "A", "B", "C", "D" };
            IEnumerable<Tuple<int,string>> result = listeString.LoopIndex();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoopIndexIsNull()
        {
            List<string> listeString = null;

            try
            {
                IEnumerable<Tuple<int, string>> result = listeString.LoopIndex().ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void ContainsOrDefaultContainsValueAndReturnsIt()
        {
            var expected = unsorted[2];

            var actual = unsorted.ContainsOrDefault(expected);

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void ContainsOrDefaultDoesNotContainReturnsDefault()
        {
            var actual = unsorted.ContainsOrDefault(new Simple { Id = 999, Name = "Unknown" });

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PageFirstReturnsFirstPage()
        {
            var expected = unsorted.Take(2).ToList();
            var actual = unsorted.Page(1, 2).ToList();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PageLastReturnsSubsetIfRangeExceeded()
        {
            var actual = unsorted.Page(1, 20).ToList();
            Assert.AreEqual(unsorted, actual);
        }

        [TestMethod]
        public void PagePastEndOfRangeReturnsNoResults()
        {
            var actual = unsorted.Page(10, 10).ToList();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PageSecondReturnsSecondPage()
        {
            var expected = unsorted.Skip(2).Take(2).ToList();
            var actual = unsorted.Page(2, 2).ToList();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PageZeroPageIndexThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => unsorted.Page(0, 12));
        }

        [TestMethod]
        public void PageZeroPageSizeThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => unsorted.Page(1, 0).ToList());
        }

        [TestMethod]
        public void SortBySequenceByProperty()
        {
            var sequence = new[] { 123, 910, 234, 100, 1001 };

            var sorted = unsorted.OrderBySequence(u => u.Id, sequence);

            var expected = new[] { "Four", "Score", "And", "Seven", "Years" };
            Assert.AreEqual(expected, sorted.Select(s => s.Name).ToArray());
        }

        private class EmployeeForPivot
        {
            public string Name { get; set; }
            public string Department { get; set; }
            public string Function { get; set; }
            public decimal Salary { get; set; }
        }

        private class Simple
        {
            public int Id;
            public string Name;
        }

        private readonly List<Simple> unsorted = new List<Simple> {
            new Simple { Id = 234, Name = "And" },
            new Simple { Id = 100, Name = "Seven" },
            new Simple { Id = 910, Name = "Score" },
            new Simple { Id = 123, Name = "Four" },
            new Simple { Id = 1001, Name = "Years" }
        };
    }
}