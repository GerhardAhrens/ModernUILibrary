namespace ModernTest.ModernBaseLibrary.Collection
{
    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ConcurrentHashSet_Test
    {
        private const int MAX = 5100;

        [TestInitialize]
        public void OnTestInitialize()
        {

        }

        [TestMethod]
        public void AddAndClear()
        {
            ConcurrentHashSet<string> set = new ConcurrentHashSet<string>()
            {
                "string",
                "testing",
                "word",
            };

            Assert.IsTrue(set.IsEmpty == false);

            set.Clear();

            Assert.IsTrue(set.IsEmpty == true);
        }

        [TestMethod]
        public void JoinContent()
        {
            ConcurrentHashSet<string> set = new ConcurrentHashSet<string>()
            {
                "string",
                "testing",
                "word",
            };

            Assert.IsTrue(set.IsEmpty == false);

            for (int i = 0; i < 10; i++)
            {
                set.Add(i.ToString());
            }

            string temp = string.Join(", ", set.OrderBy(x => x));
            Assert.IsTrue(string.IsNullOrEmpty(temp)  == false);
        }

        [TestMethod]
        public void TryGetValue()
        {
            ConcurrentHashSet<string> set = new ConcurrentHashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                set.Add(i.ToString());
            }

            Assert.IsTrue(set.IsEmpty == false);

            set.TryGetValue("7", out string str);
            Assert.AreEqual(str, "7");

            set.TryGetValue("8", out str);
            Assert.AreEqual(str, "8");
        }

        [TestMethod]
        public void ComplexInt()
        {
            Random rand = new Random(51235);

            try
            {
                TestAll<int>(() => rand.Next(), (x) => x);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void ComplexLong()
        {
            Random rand = new Random(51235);

            try
            {
                TestAll<long>(() => (((long)rand.Next()) << 32) + (long)rand.Next(), (x) => x);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        [Obsolete]
        public void ComplexString()
        {
            Random rand = new Random(51235);

            try
            {
                TestAll<string>(() => rand.Next().ToString(), (x) => string.Copy(x));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void ComplexWithTestClass()
        {
            Random rand = new Random(51235);

            try
            {
                TestAll<TestClass>(() => new TestClass(rand), (x) => x.Clone());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void ComplexWithTestClass2()
        {
            Random rand = new Random(51235);

            try
            {
                TestAll<TestClass2>(() => new TestClass2(rand), (x) => x.Clone());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidOperationException));
            }
        }

        internal void TestAll<T>(Func<T> create, Func<T, T> copy)
        {
            ConcurrentDictionary<T, T> map = new ConcurrentDictionary<T, T>();
            ConcurrentHashSet<T> set = new ConcurrentHashSet<T>();

            HashSet<T> items = new HashSet<T>();
            for (int i = 0; i < MAX; i++)
            {
                T obj = create();
                items.Add(obj);
            }

            foreach (T item in items)
            {
                // IsEmpty
                bool v1 = set.IsEmpty;
                bool v2 = map.IsEmpty;
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                // Remove
                v1 = set.TryRemove(item, out T item1);
                v2 = map.TryRemove(item, out T item2);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                if (!Equals(item2, item2))
                {
                    throw new InvalidOperationException();
                }

                if (set.Count != map.Count)
                {
                    throw new InvalidOperationException();
                }

                if (set.Remove(item))
                {
                    throw new InvalidOperationException();
                }

                if (set.Count != map.Count)
                {
                    throw new InvalidOperationException();
                }

                // Add
                v1 = set.Add(item);
                v2 = map.TryAdd(item, item);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                if (set.Count != map.Count)
                {
                    throw new InvalidOperationException();
                }

                if (set.Add(item))
                {
                    throw new InvalidOperationException();
                }

                if (set.Count != map.Count)
                {
                    throw new InvalidOperationException();
                }

                // Contains
                T clone = copy(item);
                v1 = set.Contains(clone);
                v2 = map.ContainsKey(clone);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                // TryGetValue
                v1 = set.TryGetValue(clone, out item1);
                v2 = map.TryGetValue(clone, out item2);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                if (!item1.Equals(item2))
                {
                    throw new InvalidOperationException();
                }

                if (!clone.Equals(item1))
                {
                    throw new InvalidOperationException();
                }

                if (typeof(T).IsClass && !ReferenceEquals(item1, item2))
                {
                    throw new InvalidOperationException();
                }

                // TryUpdate
                v1 = set.TryUpdate(clone);
                v2 = map.TryUpdate(clone, clone, clone);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                set.TryGetValue(clone, out item1);
                map.TryGetValue(clone, out item2);
                if (!item1.Equals(item2))
                {
                    throw new InvalidOperationException();
                }

                if (!clone.Equals(item1))
                {
                    throw new InvalidOperationException();
                }

                if (typeof(T).IsClass && !ReferenceEquals(item1, item2))
                {
                    throw new InvalidOperationException();
                }

                v1 = set.TryUpdate(item);
                v2 = map.TryUpdate(item, item, item);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                if (v1 != true)
                {
                    throw new InvalidOperationException();
                }
            }

            // Remove
            foreach (T item in items)
            {
                bool v1 = set.TryRemove(item, out T item1);
                bool v2 = map.TryRemove(item, out T item2);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                if (set.Count != map.Count)
                {
                    throw new InvalidOperationException();
                }

                // Contains
                v1 = set.Contains(item);
                v2 = map.ContainsKey(item);
                if (v1 != v2)
                {
                    throw new InvalidOperationException();
                }

                // AddOrUpdate
                T clone = copy(item);
                set.Add(item);
                if (!set.TryGetValue(item, out T v3))
                {
                    throw new InvalidOperationException();
                }

                set.AddOrUpdate(clone);
                if (!set.TryGetValue(item, out v3))
                {
                    throw new InvalidOperationException();
                }

                var v4 = map.AddOrUpdate(item, item, (x, y) => x);
                T v5 = map.AddOrUpdate(clone, clone, (x, y) => x);
                if (!item.Equals(clone))
                {
                    throw new InvalidOperationException();
                }

                if (!item.Equals(v3))
                {
                    throw new InvalidOperationException();
                }

                if (!item.Equals(v4))
                {
                    throw new InvalidOperationException();
                }

                if (!item.Equals(v4))
                {
                    throw new InvalidOperationException();
                }

                if (typeof(T).IsClass)
                {
                    if (!ReferenceEquals(v3, v5))
                    {
                        throw new InvalidOperationException();
                    }

                    if (ReferenceEquals(item, clone))
                    {
                        throw new InvalidOperationException();
                    }

                    if (ReferenceEquals(v3, v4))
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            // Clear
            map.Clear();
            set.Clear();
            if (map.Count != set.Count)
            {
                throw new InvalidOperationException();
            }

            // Set specific
            if (!set.Add(default))
            {
                throw new InvalidOperationException();
            }

            if (set.Add(default))
            {
                throw new InvalidOperationException();
            }

            if (!set.Contains(default))
            {
                throw new InvalidOperationException();
            }
        }

        private class TestClass : IEquatable<TestClass>
        {
            public TestClass() { }
            public TestClass(Random rand)
            {
                Int = rand.Next();
                Str = rand.Next().ToString();
            }

            public int Int { get; set; }
            protected string Str { get; set; }

            public TestClass Clone()
            {
                return new TestClass() { Int = Int, Str = Str };
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TestClass);
            }

            public bool Equals(TestClass other)
            {
                return other != null &&
                       Int == other.Int &&
                       Str == other.Str;
            }

            public override int GetHashCode()
            {
                int hashCode = 2115060958;
                hashCode = hashCode * -1521134295 + Int.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Str);
                return hashCode;
            }
        }

        private class TestClass2 : IEquatable<TestClass2>
        {
            public TestClass2() { }
            public TestClass2(Random rand)
            {
                Int = rand.Next();
                Str = rand.Next().ToString();
            }

            public int Int { get; set; }
            protected string Str { get; set; }

            public TestClass2 Clone()
            {
                return new TestClass2() { Int = Int };
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TestClass2);
            }

            public bool Equals(TestClass2 other)
            {
                return other != null &&
                       Int == other.Int;
            }

            public override int GetHashCode()
            {
                return 1497029550 + Int.GetHashCode();
            }
        }
    }
}
