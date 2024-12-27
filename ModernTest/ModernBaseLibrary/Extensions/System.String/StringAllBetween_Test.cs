namespace ModernTest.ModernBaseLibrary
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringAllBetween_Test
    {
        [TestMethod]
        [DataRow("Look * i * am surrounded just like * me * ", '*')]
        public void AllBetween(string input, char enclosureCharacter)
        {
            /*
            IEnumerable<string> result = StringExtension.AllBetween(input, enclosureCharacter);

            Assert.IsNotNull(result);
            */
        }

        [TestMethod]
        [DataRow("Look [i] am surrounded just like [me]", '[',']')]
        public void AllBetween(string input, char firstEnclosureCharacter, char secondEnclosureCharacter)
        {
            /*
            IEnumerable<string> result = StringExtension.AllBetween(input, firstEnclosureCharacter, secondEnclosureCharacter);

            Assert.IsNotNull(result);
            */
        }
    }
}
