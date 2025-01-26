namespace ModernTest.ModernBaseLibrary.CoreBase
{
    using System;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.CoreBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueObjectBase_C_Test
    {
        [TestMethod]
        public void IgnoreMember_Property_DoesNotConsider()
        {
            var value1 = new Ignore { Ignored = 2, Considered = 4 };
            var value2 = new Ignore { Ignored = 3, Considered = 4 };

            Assert.IsTrue(value1.Equals(value2));
        }

        [TestMethod]
        public void IgnoreMember_Field_DoesNotConsider()
        {
            var value1 = new Ignore { IgnoredField = 3, Considered = 4 };
            var value2 = new Ignore { IgnoredField = 2, Considered = 4 };

            Assert.IsTrue(value1.Equals(value2));
        }

        private class Ignore : ValueObjectBase
        {
            [IgnoreMember]
            public int Ignored { get; set; }
            [IgnoreMember]
            public int IgnoredField;
            public int Considered { get; set; }
        }
    }
}