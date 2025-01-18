﻿namespace ModernTest.ModernBaseLibrary.Text
{
    using System.Diagnostics;

    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringTokenizer_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
        }

        [TestMethod]
        public void StringTokenizer_Token()
        {
            string input = "Das ist ein Test für 1 (einen) Token <Hurra/>";

            StringTokenizer tok = new StringTokenizer(input);
            Token token = null;
            do
            {
                token = tok.Next();
                Debug.WriteLine($"{token.Kind.ToString()} at {token.Line}, {token.Column}: {token.Value}");

            } while (token.Kind != TokenKind.EOF);

        }
    }
}