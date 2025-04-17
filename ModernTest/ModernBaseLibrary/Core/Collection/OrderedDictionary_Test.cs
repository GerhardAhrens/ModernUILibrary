
namespace ModernTest.ModernBaseLibrary.Collection
{
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Collection;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class OrderedDictionary_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            System.Console.SetOut(new DebugTextWriter());
        }

        [TestMethod]
        public void TestA()
        {
            var stack = new OrderedDictionary<int,string>();
            stack.Add(0,"A");
            stack.Add(7, "B");
            stack.Add(4, "C");
            stack.Add(2, "D");
            stack.Add(1, "E");

            System.Console.WriteLine(string.Join(", ", stack));
        }
    }
}
