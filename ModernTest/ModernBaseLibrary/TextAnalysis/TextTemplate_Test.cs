namespace ModernTest.ModernBaseLibrary.Text
{
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TextTemplate_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
        }

        [TestMethod]
        public void TextTemplate_V1()
        {
            var tplStr1 = @"Hello {Name},\nNice to meet you!";
            var tplStr2 = @"This {Type} \{contains} \\ some things \n that shouldn't be rendered";

            var variableValues = new Dictionary<string, object>
            {
                ["Name"] = "Bob",
                ["Type"] = "string",
            };

            var result1 = TextTemplate.Render(tplStr1, variableValues);

            var template = new TextTemplate(tplStr2);
            var result2 = template.Render(variableValues);
        }
    }
}