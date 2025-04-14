//-----------------------------------------------------------------------
// <copyright file="ToHashSet_Test.cs" company="Lifeprojects.de">
//     Class: ToHashSet_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>14.04.2025 09:37:37</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary
{
    using global::ModernBaseLibrary.Cryptography;
    using global::ModernBaseLibrary.Extension;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Globalization;
    using global::System.Linq;
    using global::System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ToHashSet_Test
    {
        private const int ListSize = 1000;
        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private FastRandom _rnd = new FastRandom();

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToHashSet_Test"/> class.
        /// </summary>
        public ToHashSet_Test()
        {
        }

        #region List
        [TestMethod]
        public void ListToHashSetKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSetEx(k => k.Key);
            VerifyListsKey(singleList, dic);
        }

        [TestMethod]
        public void ListToHashSetKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSetEx(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }
        #endregion List

        #region Array
        [TestMethod]
        public void ArrayToHashSetKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToHashSetEx(k => k.Key);
            VerifyListsKey(singleList, dic);
        }

        [TestMethod]
        public void ArrayToHashSetKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToHashSetEx(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }
        #endregion Array

        #region IEnumerable (HashSet)
        [TestMethod]
        public void IEnumerableToHashSetKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToHashSetEx(k => k.Key);
            VerifyListsKey(singleList, dic);
        }


        [TestMethod]
        public void IEnumerableToHashSetKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToHashSetEx(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }
        #endregion IEnumerable (HashSet)

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

        private struct KV<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;

            #region Overrides of ValueType

            /// <summary>Indicates whether this instance and a specified object are equal.</summary>
            /// <param name="obj">The object to compare with the current instance.</param>
            /// <returns>
            /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
            public override bool Equals(object obj)
            {
                var ob = obj as KV<TKey, TValue>?;
                if (ob == null)
                {
                    return false;
                }

                var o = ob.Value;
                return Key.Equals(o.Key) && Value.Equals(o.Value);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            #endregion
        }

        private void SetUpLists(out List<KV<string, int>> singleList, out List<KV<string, int>> dupList)
        {
            singleList = new List<KV<string, int>>(ListSize);
            dupList = new List<KV<string, int>>(ListSize * 2);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = new KV<string, int>()
                {
                    Key = _rnd.NextString(Letters, 10),
                    Value = i
                };
                singleList.Add(kv);
                dupList.Add(kv);
            }
        }

        private void VerifyListsKey(List<KV<string, int>> singleList, HashSet<string> dic)
        {
            Assert.AreEqual(dic.Count, singleList.Count);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = singleList[i];
                Assert.IsTrue(dic.TryGetValue(kv.Key, out var val));
            }
        }
        private void VerifyListsKeyValue(List<KV<string, int>> singleList, HashSet<string> dic)
        {
            Assert.AreEqual(dic.Count, singleList.Count);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = singleList[i];
                Assert.IsTrue(dic.TryGetValue(kv.Key, out var val));
            }
        }
    }
}
