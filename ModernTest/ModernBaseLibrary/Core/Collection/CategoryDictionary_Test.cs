namespace ModernTest.ModernBaseLibrary.Collection
{
    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class CategoryDictionary_Test
    {
        private CategoryDictionary categories = new CategoryDictionary();

        [TestInitialize]
        public void OnTestInitialize()
        {
            categories = new CategoryDictionary();

        }

        [TestMethod]
        public void Add()
        {
            if (categories == null)
            {
                categories = new CategoryDictionary();
            }
            categories.Add("key1", "caregory1", "Testvalue1", typeof(string));
            categories.Add("key2", "caregory2", false, typeof(bool));
            categories.Add("key3", "caregory3", new DateTime(2017, 8, 3), typeof(DateTime));

            foreach (KeyValuePair<string, CategoryItem> item in categories)
            {
                CategoryItem citem = item.Value;
                System.Console.WriteLine(string.Format("Key={0}, Category={1}, Value1={2}, Value2={3}", item.Key, citem.Value, citem.ValueX, citem.ValueXType.Name));
            }

            Assert.IsTrue(this.categories.Count == 3);

        }

        [TestMethod]
        public void ListKeys()
        {

            this.categories.Clear();
            this.Add();

            int count = this.categories.Keys.Count;
            Assert.IsTrue(this.categories.Count == 3);
        }

        [TestMethod]
        public void ListNames()
        {
            this.categories.Clear();
            this.Add();

            int count = this.categories.Values.Count;
            Assert.IsTrue(this.categories.Count == 3);
        }

        [TestMethod]
        public void ThisKey()
        {
            this.categories.Clear();
            this.Add();

            CategoryItem value = this.categories["key1"];
        }

        [TestMethod]
        public void Contains()
        {
            this.categories.Clear();
            this.Add();

            KeyValuePair<string, CategoryItem> value = this.categories.First();

            bool isContains = this.categories.Contains(value);
            Assert.IsTrue(isContains == true);

        }

        [TestMethod]
        public void ContainsKey()
        {
            this.categories.Clear();
            this.Add();
        }

    }
}
