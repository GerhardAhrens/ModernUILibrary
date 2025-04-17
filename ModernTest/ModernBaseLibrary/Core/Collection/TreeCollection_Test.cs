namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TreeCollection_Test
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
        }

        [TestMethod]
        public void Add()
        {
            ITreeCollection<int, string> target;
            target = new TreeCollection<int, string>();

            target.Add(1, "1");
            target.Add(5, "5");
            target.Add(2, "2");
            target.Add(12, "12");
            target.Add(4, "4");

            Assert.AreEqual(5, target.Count);
            Assert.AreEqual("5", target[5]);
            Assert.AreEqual("4", target[4]);
            Assert.IsTrue(target.ContainsKey(1));

            Assert.IsFalse(target.ContainsKey(3));
        }

        [TestMethod]
        public void Add_KeyExistTest()
        {
            ITreeCollection<int, string> target;
            target = new TreeCollection<int, string>();

            target.Add(1, "1");

            try
            {
                target.Add(1, "4");
                Assert.Fail("It must faild");
            }
            catch (ArgumentException)
            {
                //It's OK
            }
        }

        [TestMethod]
        public void AddPair_Test()
        {
            ITreeCollection<int, string> target;
            target = new TreeCollection<int, string>();

            target.Add(new KeyValuePair<int, string>(1, "1"));
            target.Add(new KeyValuePair<int, string>(5, "5"));
            target.Add(new KeyValuePair<int, string>(2, "2"));
            target.Add(new KeyValuePair<int, string>(113, "113"));

            Assert.AreEqual(4, target.Count);
            Assert.AreEqual("5", target[5]);
            Assert.AreEqual("113", target[113]);
            Assert.IsTrue(target.ContainsKey(1));

            Assert.IsFalse(target.ContainsKey(3));
        }

        [TestMethod]
        public void AddPair_KeyExistTest()
        {
            ITreeCollection<int, string> target;
            target = new TreeCollection<int, string>();

            target.Add(1, "1");

            try
            {
                target.Add(new KeyValuePair<int, string>(1, "4"));
                Assert.Fail("It must faild");
            }
            catch (ArgumentException)
            {
                //It's OK
            }
        }

        [TestMethod]
        public void Indexer_KeyNotFoundTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            try
            {
                var value = target[2];
                Assert.Fail("It must fail");
            }
            catch (KeyNotFoundException)
            {
                //It's OK
            }
        }

        [TestMethod]
        public void Indexer_GetValueTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();
            target.Add(23, "Hello");
            target.Add(33, "Good morning");

            Assert.AreEqual("Hello", target[23]);
        }

        [TestMethod]
        public void Indexer_SetValueTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();
            target.Add(23, "Hello");
            target.Add(33, "Good morning");

            target[23] = "New value";
            Assert.AreEqual("New value", target[23]);
        }

        [TestMethod]
        public void Count_DefaultTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void Count_Test()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            for (int i = 0; i < 5; i++)
            {
                target.Add(i, i.ToString());
            }
            for (int i = 10; i >= 5; i--)
            {
                target.Add(i, i.ToString());
            }

            Assert.AreEqual(11, target.Count);
        }

        [TestMethod]
        public void IsReadOnly_Test()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            Assert.IsFalse(target.IsReadOnly);
        }

        [TestMethod]
        public void Contains_Test()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            target.Add(1, "1");
            target.Add(5, "5");
            target.Add(2, "2");
            target.Add(12, "12");
            target.Add(4, "4");

            Assert.IsTrue(target.ContainsKey(1));
            Assert.IsTrue(target.ContainsKey(5));
            Assert.IsTrue(target.ContainsKey(12));

            Assert.IsFalse(target.ContainsKey(33));
            Assert.IsFalse(target.ContainsKey(55));
        }

        [TestMethod]
        public void Remove_TrueTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            target.Add(1, "1");
            target.Add(5, "5");
            target.Add(2, "2");
            target.Add(12, "12");
            target.Add(4, "4");

            Assert.IsTrue(target.Remove(1));
            Assert.IsFalse(target.ContainsKey(1));
            Assert.AreEqual(4, target.Count);

            Assert.IsTrue(target.Remove(12));
            Assert.IsFalse(target.ContainsKey(12));
            Assert.AreEqual(3, target.Count);

            Assert.IsTrue(target.Remove(4));
            Assert.IsFalse(target.ContainsKey(4));
            Assert.AreEqual(2, target.Count);

            Assert.IsTrue(target.Remove(2));
            Assert.IsFalse(target.ContainsKey(2));
            Assert.AreEqual(1, target.Count);

            Assert.IsTrue(target.Remove(5));
            Assert.IsFalse(target.ContainsKey(5));
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void Remove_FalseTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            Assert.IsFalse(target.Remove(1));

            target.Add(1, "1");
            target.Add(5, "5");
            target.Add(2, "2");
            target.Add(12, "12");
            target.Add(4, "4");

            target.Remove(1);
            Assert.IsFalse(target.Remove(1));

            Assert.IsFalse(target.Remove(55));
        }

        [TestMethod]
        public void Clear_TrueTest()
        {
            ITreeCollection<int, string> target = new TreeCollection<int, string>();

            target.Add(1, "1");
            target.Add(5, "5");
            target.Add(2, "2");
            target.Add(12, "12");
            target.Add(4, "4");

            target.Clear();

            Assert.AreEqual(0, target.Count);
            Assert.IsFalse(target.ContainsKey(1));

            target.Add(1, "1");
            Assert.AreEqual(1, target.Count);
            Assert.IsTrue(target.ContainsKey(1));
        }
    }
}
