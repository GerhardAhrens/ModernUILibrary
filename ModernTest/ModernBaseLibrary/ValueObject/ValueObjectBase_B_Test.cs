namespace ModernTest.ModernBaseLibrary.CoreBase
{
    using System;

    using global::ModernBaseLibrary.CoreBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueObjectBase_B_Test
    {
        [TestMethod]
        public void Nesting()
        {
            var value = new Recursive();
            var value2 = new Recursive();
            var nestedValue = new Recursive() { Terminal = "test" };
            var nestedValue2 = new Recursive() { Terminal = "test" };

            value.Recurse = nestedValue;
            value2.Recurse = nestedValue2;

            Assert.IsTrue(value.Equals(value2));
            Assert.AreEqual(value.GetHashCode(), value2.GetHashCode());
        }

        private class Recursive : ValueObjectBase
        {
            public Recursive Recurse { get; set; }
            public string Terminal;
        }

    }
}