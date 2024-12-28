
namespace EasyPrototypingNET.Test
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using EasyPrototypingNET.Core.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class DropOutStack_Test
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
            var stack = new DropOutStack<string>(5);
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            stack.Push("D");
            stack.Push("E");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "E", "D", "C", "B", "A" }, stack));
        }

        [TestMethod]
        public void TestB()
        {
            var stack = new DropOutStack<string>(5);
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            stack.Push("D");
            stack.Push("E");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "E", "D", "C", "B", "A" }, stack));

            stack.Push("F");
            stack.Push("G");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "G", "F", "E", "D", "C" }, stack));
        }

        [TestMethod]
        public void TestC()
        {
            var stack = new DropOutStack<string>(5);
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            stack.Push("D");
            stack.Push("E");

            stack.Clear();
            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { }, stack));

            stack.Push("A");
            stack.Push("B");
            stack.Push("C");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "C", "B", "A" }, stack));

            Assert.AreEqual("C", stack.Peek());
        }

        [TestMethod]
        public void TestD()
        {
            var stack = new DropOutStack<string>(5);
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            stack.Push("D");
            stack.Push("E");

            stack.Clear();
            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { }, stack));

            stack.Push("A");
            stack.Push("B");
            stack.Push("C");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "C", "B", "A" }, stack));

            Assert.AreEqual("C", stack.Peek());

            stack.Pop();

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "B", "A" }, stack));

            stack.Push("X");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "X", "B", "A" }, stack));

            stack.Push("1");
            stack.Push("2");
            stack.Push("3");
            stack.Push("4");
            stack.Push("5");

            System.Console.WriteLine(string.Join(", ", stack));
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "5", "4", "3", "2", "1" }, stack));
        }
    }
}
