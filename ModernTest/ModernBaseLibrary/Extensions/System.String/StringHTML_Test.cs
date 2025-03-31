//-----------------------------------------------------------------------
// <copyright file="StringHTML_Test.cs" company="Lifeprojects.de">
//     Class: StringHTML_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>31.03.2025 09:46:52</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::System.Globalization;
    using global::System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringHTML_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringHTML_Test"/> class.
        /// </summary>
        public StringHTML_Test()
        {
        }

        [TestMethod]
        public void IsHtml_String_Found()
        {
            string htmlText = "<b>Test</b>";
            bool result = htmlText.IsHtmlTag("b");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsHtml_String_NotFound()
        {
            string htmlText = "<b>Test</b>";
            bool result = htmlText.IsHtmlTag("x");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsHtml_TagsInString_Found()
        {
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<html><body scroll=\"no\">");
            htmlContent.Append("<h2 style=\"color:black;\">Soll das Programm beendet werden?</h2>");
            htmlContent.Append($"<h3 style=\"color:blue;\">Datum/Zeit: {DateTime.Now}</h3>");
            htmlContent.Append("</body></html>");

            bool result = htmlContent.ToString().IsHtmlTags();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Replace_Diacriticals_String()
        {
            string htmlText = "Prüfen für alle";
            string result = htmlText.ReplaceHtmlDiacriticals();
            Assert.AreEqual(result, "Pr&uuml;fen f&uuml;r alle");
        }

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
    }
}
