namespace EasyPrototypingNET.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Collection;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class ObservableList_test
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
        }

        [TestMethod]
        public void Add()
        {
            const string expectedItem = "a string to add";
            string actualItem = "";
            int actualIndex = -1;

            ObservableList<string> observableList = new ObservableList<string>();
            observableList.ListChanged += (s, e) =>
            {
                actualItem = e.Item;
                actualIndex = e.Index;
            };
            observableList.Add(expectedItem);

            Assert.AreEqual(expectedItem, actualItem);
            Assert.AreEqual(0, actualIndex);

            string value = observableList.ToArray().Single();

            Assert.IsTrue(value == expectedItem);
        }

        [TestMethod]
        public void CompareItemContent()
        {
            var observableList = new ObservableList<decimal> { 1m };
            decimal[] expected = new decimal[] { 1m };
            Assert.That.AreEqualValue(observableList.ToArray<decimal>(), expected.ToArray<decimal>());
            Assert.AreEqual(expected[0], observableList[0]);
        }

        [TestMethod]
        public void ClearFiresListClearedEvent()
        {
            var didFire = false;
            var observableList = new ObservableList<string> { "Ein neuer Eintrag" };

            observableList.ListCleared += (s, e) => { didFire = true; };
            observableList.Clear();

            Assert.IsTrue(didFire);
        }

        [TestMethod]
        public void IndexOfGivenContainedValueReturnsCorrectIndex()
        {
            const string containedValue = "Charlie";
            const int expectedIndex = 2;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", containedValue, "Delta", "Echo" }.AsEnumerable());

            Assert.AreEqual(expectedIndex, observableList.IndexOf(containedValue));
        }

        [TestMethod]
        public void IndexOfGivenUncontainedValueReturnsMinusOne()
        {
            const string uncontainedValue = "Charly";

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            Assert.AreEqual(-1, observableList.IndexOf(uncontainedValue));
        }

        [TestMethod]
        public void IndexOfReturnsCorrectIndex()
        {
            var list = new List<short> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
            var observableList = new ObservableList<short>(list);
            foreach (var value in list)
            {
                Assert.AreEqual(list.IndexOf(value), observableList.IndexOf(value));
            }
        }

        [TestMethod]
        public void IndexerSetToDiferentObjectDoesFireListChangedEvent()
        {
            var didFire = false;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.ListChanged += (s, e) => { didFire = true; };
            observableList[2] = "Cedilla";

            Assert.IsTrue(didFire);
        }

        [TestMethod]
        public void IndexerSetToSameObjectDoesNotFireListChangedEvent()
        {
            var didFire = false;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.ListChanged += (s, e) => { didFire = true; };
            observableList[2] = "Charlie";

            Assert.IsFalse(didFire);
        }

        [TestMethod]
        public void InsertFiresListChangedEvent()
        {
            const int expectedItem = 68030;
            var actualItem = -1;
            var actualIndex = -1;

            var observableList = new ObservableList<int> { 1, 2, 4, 8 };
            observableList.ListChanged += (s, e) =>
            {
                actualItem = e.Item;
                actualIndex = e.Index;
            };

            observableList.Insert(3, expectedItem);

            Assert.AreEqual(expectedItem, actualItem);
            Assert.AreEqual(3, actualIndex);
        }

        [TestMethod]
        public void RemoveAtFiresListChangedEvent()
        {
            const string expectItem = "another string to remove";
            const int expectIndex = 1;
            var actualItem = "";
            var actualIndex = -1;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.Insert(expectIndex, expectItem);
            observableList.ListChanged += (s, e) =>
            {
                actualItem = e.Item;
                actualIndex = e.Index;
            };

            observableList.RemoveAt(expectIndex);

            Assert.AreEqual(expectItem, actualItem);
            Assert.AreEqual(expectIndex, actualIndex);
        }

        [TestMethod]
        public void RemoveFiresListChangedEvent()
        {
            const string expectItem = "a string to remove";
            const int expectIndex = 4;
            var actualItem = "";
            var actualIndex = -1;

            var observableList = new ObservableList<string>(new[] { "A", "B", "C", "D", "E" }.AsEnumerable());
            observableList.Insert(expectIndex, expectItem);
            observableList.ListChanged += (s, e) =>
            {
                actualItem = e.Item;
                actualIndex = e.Index;
            };

            observableList.Remove(expectItem);

            Assert.AreEqual(expectItem, actualItem);
            Assert.AreEqual(expectIndex, actualIndex);
        }
    }
}