namespace ModernTest.ModernBaseLibrary.CoreBase
{
    using System;

    using global::ModernBaseLibrary.CoreBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueObjectBase_D_Test
    {
        [TestMethod]
        public void ObjectsOfDifferentTypeAreNotEqual_EvenIfSubclass()
        {
            var value1 = new MyValue();
            var value2 = new MyValue2();

            Assert.IsFalse(value1.Equals(value2));
        }

        private class MyValue : ValueObjectBase
        {
            public int Num = 0;

            public override string ToString()
            {
                return this.Num.ToString();
            }
        }

        private class MyValue2 : MyValue
        {
        }

    }
}