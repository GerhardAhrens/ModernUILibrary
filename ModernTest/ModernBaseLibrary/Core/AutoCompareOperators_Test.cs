namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AutoCompareOperators_Test
    {

        [TestMethod]
        public void CompareReturnsNegativeOneWhenFirstArgIsNullAndSecondIsNot()
        {
            var second = new DemoClass<string>("Amiga");

            var comparison = AutoCompareOperators.Compare(null, second);

            Assert.AreEqual(-1, comparison);
        }

        [TestMethod]
        public void CompareReturnsPositiveOneWhenSecondArgIsNullAndFirstIsNot()
        {
            var first = new DemoClass<string>("Amiga");

            var comparison = AutoCompareOperators.Compare(first, null);

            Assert.AreEqual(1, comparison);
        }

        [TestMethod]
        public void CompareReturnsZeroWhenBothArgsNull()
        {
            var comparison = AutoCompareOperators.Compare(null, null);

            Assert.AreEqual(0, comparison);
        }

        [TestMethod]
        public void CompareReturnsZeroWhenEqualValue()
        {
            var equal1 = new DemoClass<string>("Amiga");
            var equal2 = new DemoClass<string>("Amiga");

            var comparison = AutoCompareOperators.Compare(equal1, equal2);

            Assert.AreEqual(0, comparison);
        }

        [TestMethod]
        public void CompareReturnsZeroWhenSameReference()
        {
            var same = new DemoClass<string>("Amiga");

            var comparison = AutoCompareOperators.Compare(same, same);

            Assert.AreEqual(0, comparison);
        }

        [TestMethod]
        public void EqualsReturnsFalseWhenNotEqual()
        {
            var notEqual1 = new DemoClass<int>(2521);
            var notEqual2 = new DemoClass<int>(2);

            Assert.IsFalse(notEqual1 == notEqual2);
        }

        [TestMethod]
        public void EqualsReturnsTrueWhenEqual()
        {
            var equal1 = new DemoClass<int>(2521);
            var equal2 = new DemoClass<int>(2521);

            Assert.IsTrue(equal1 == equal2);
        }

        [TestMethod]
        public void GreaterThanOrEqualReturnsFalseWhenLessThan()
        {
            var lower = new DemoClass<int>(500);
            var higher = new DemoClass<int>(2000);

            Assert.IsFalse(lower >= higher);
        }

        [TestMethod]
        public void GreaterThanOrEqualReturnsTrueWhenEqual()
        {
            var equal1 = new DemoClass<int>(520);
            var equal2 = new DemoClass<int>(520);

            Assert.IsTrue(equal1 >= equal2);
        }

        [TestMethod]
        public void GreaterThanOrEqualReturnsTrueWhenGreaterThan()
        {
            var lower = new DemoClass<int>(720);
            var higher = new DemoClass<int>(880);

            Assert.IsTrue(higher >= lower);
        }

        [TestMethod]
        public void GreaterThanReturnsFalseWhenLessThan()
        {
            var lower = new DemoClass<int>(252);
            var higher = new DemoClass<int>(50809);

            Assert.IsFalse(lower > higher);
        }

        [TestMethod]
        public void GreaterThanReturnsTrueWhenGreaterThan()
        {
            var lower = new DemoClass<int>(252);
            var higher = new DemoClass<int>(50809);

            Assert.IsTrue(higher > lower);
        }

        [TestMethod]
        public void GreaterThanReturnsFalseWhenEqual()
        {
            var lower = new DemoClass<int>(6508);
            var higher = new DemoClass<int>(6508);

            Assert.IsFalse(lower > higher);
        }

        [TestMethod]
        public void LessThanOrEqualReturnsFalseWhenGreaterThan()
        {
            var lower = new DemoClass<int>(25);
            var higher = new DemoClass<int>(5009);

            Assert.IsFalse(higher < lower);
        }

        [TestMethod]
        public void LessThanOrEqualReturnsTrueWhenEqual()
        {
            var lower = new DemoClass<int>(984);
            var higher = new DemoClass<int>(984);

            Assert.IsTrue(lower <= higher);
        }

        [TestMethod]
        public void LessThanOrEqualReturnsTrueWhenLessThan()
        {
            var lower = new DemoClass<int>(123);
            var higher = new DemoClass<int>(789);

            Assert.IsTrue(lower <= higher);
        }

        [TestMethod]
        public void LessThanReturnsFalseWhenEqual()
        {
            var lower = new DemoClass<int>(68000);
            var higher = new DemoClass<int>(68000);

            Assert.IsFalse(higher < lower);
        }

        [TestMethod]
        public void LessThanReturnsFalseWhenGreaterThan()
        {
            var lower = new DemoClass<int>(16);
            var higher = new DemoClass<int>(4096);

            Assert.IsFalse(higher < lower);
        }

        [TestMethod]
        public void LessThanReturnsTrueWhenLessThan()
        {
            var lower = new DemoClass<int>(4);
            var higher = new DemoClass<int>(8);

            Assert.IsTrue(lower < higher);
        }

        [TestMethod]
        public void NotEqualsReturnsFalseWhenEqual()
        {
            var equal1 = new DemoClass<int>(2521);
            var equal2 = new DemoClass<int>(2521);

            Assert.IsFalse(equal1 != equal2);
        }
        [TestMethod]
        public void NotEqualsReturnsTrueWhenNotEqual()
        {
            var notEqual1 = new DemoClass<int>(2521);
            var notEqual2 = new DemoClass<int>(2);

            Assert.IsTrue(notEqual1 != notEqual2);
        }

        private class DemoClass<T> : AutoCompareOperators where T : IComparable
        {
            readonly T primitive;

            public DemoClass(T primitive)
            {
                this.primitive = primitive;
            }

            public override int CompareTo(object obj)
            {
                var comparison = obj as DemoClass<T>;
                if (comparison == null)
                    return -1;

                return primitive.CompareTo(comparison.primitive);
            }

            public override int GetHashCode()
            {
                return primitive.GetHashCode();
            }
        }
    }
}