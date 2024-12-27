//-----------------------------------------------------------------------
// <copyright file="ArrayCollection_Test.cs" company="Lifeprojects.de">
//     Class: ArrayCollection_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>24.02.2022 06:33:17</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrayCollection_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayCollection_Test"/> class.
        /// </summary>
        public ArrayCollection_Test()
        {
        }

        [TestMethod]
        public void CreateArrayCollectionWithInt()
        {
            ArrayCollection<int> array = new ArrayCollection<int>();
            for (int i = 0; i < 10; i++)
            {
                array.Add(i);
            }

            WriteArray("add", array);

            Assert.IsTrue(array.Count == 10);
        }

        [TestMethod]
        public void CreateArrayCollectionWithDirectInt()
        {
            ArrayCollection<int> array = new ArrayCollection<int>() {1,2,3,4,5,6,7,8,9,0 };

            WriteArray("add", array);

            Assert.IsTrue(array.Count == 10);
        }

        [TestMethod]
        public void CreateArrayCollectionWithString()
        {
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            ArrayCollection<string> array = new ArrayCollection<string>();
            for (int i = 0; i < alphabet.Count(); i++)
            {
                array.Add(alphabet[i].ToString());
            }

            WriteArray("add", array);

            Assert.IsTrue(array.Count == 26);
        }

        [TestMethod]
        public void ReadWithIndexer()
        {
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            ArrayCollection<string> array = new ArrayCollection<string>();
            for (int i = 0; i < alphabet.Count(); i++)
            {
                array.Add(alphabet[i].ToString());
            }

            Debug.Write("Indexer(get)    : ");
            for (int i = 0; i < array.Count; i++)
            {
                Debug.Write($"{array[i]}");
            }

            Debug.WriteLine("");
        }

        [TestMethod]
        public void RemoveAndInsert()
        {
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            ArrayCollection<string> array = new ArrayCollection<string>();
            for (int i = 0; i < alphabet.Count(); i++)
            {
                array.Add(alphabet[i].ToString());
            }

            array.RemoveAt(5);
            WriteArray("RemoveAt", array);

            array.Remove("Z");
            WriteArray("Remove", array);

            array.Insert(3, "z");
            array.Insert(5, "z");
            WriteArray("Insert", array);

            array[3] = "y";
            array[5] = "y";
            WriteArray("indexer(set)", array);
            Debug.WriteLine("");
        }

        [TestMethod]
        public void ContainsAndIndexOfAndClear()
        {
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            ArrayCollection<string> array = new ArrayCollection<string>();
            for (int i = 0; i < alphabet.Count(); i++)
            {
                array.Add(alphabet[i].ToString());
            }

            bool isA = array.Contains("A");
            Write("Contains", isA);
            Assert.IsTrue(isA);

            int indexB = array.IndexOf("B");
            Write("IndexOf", indexB);
            Assert.IsTrue(indexB == 1);

            array.Clear();
            WriteArray("Clear", array);
            Assert.IsTrue(array.Count == 0);

            Debug.WriteLine(string.Empty);
        }

        [TestMethod]
        public void CopyArray()
        {
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            ArrayCollection<string> array = new ArrayCollection<string>();
            for (int i = 0; i < alphabet.Count(); i++)
            {
                array.Add(alphabet[i].ToString());
            }

            int offSet = 2;
            string[] array2 = new string[alphabet.Count()+ offSet];
            array.CopyTo(array2, offSet);
            WriteArray("CopyFrom", array);
            WriteArray("CopyTo", array2);

            Assert.IsTrue(array.Count + offSet == array2.Count());

            Debug.WriteLine(string.Empty);
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        private static void WriteArray<T>(string text, T array) where T : IEnumerable
        {
            Debug.Write($"{text,-15} : ");
            foreach (var item in array)
            {
                Debug.Write($"{item}");
            }

            Debug.WriteLine("");
        }

        private static void Write<T>(string text, T item)
        {
            Debug.WriteLine($"{text,-15} : {item.ToString()}");
        }
    }
}
