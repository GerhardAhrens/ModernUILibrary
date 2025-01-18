namespace ModernTest.ModernBaseLibrary.Core
{
    using EasyPrototypingNET.Core;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Range_Test
    {
        [TestMethod]
        public void RangeInt()
        {
            Range<int> range = new Range<int>(37, 42);
            bool isIn = range.ContainsValue(40);

            Range<int> innerRange1 = new Range<int>(40, 41);
            bool isInside = range.ContainsRange(innerRange1);

            Range<int> outerRange2 = new Range<int>(10, 51);
            bool isInside1 = range.IsInsideRange(outerRange2);
        }
    }
}
