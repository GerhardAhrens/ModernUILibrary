namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Xml;

    using global::ModernBaseLibrary.XML;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicXml_Test : BaseTest
    {
        private static string app = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        private static string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Directory.CreateDirectory(TempDirPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(TempDirPath))
            {
                Directory.Delete(TempDirPath, true);
            }
        }

        [TestMethod]
        public void SimpleExample()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DynamicXml\\DemoData\\Demo_01.xml");

            string xmlResult = string.Empty;
            using (DynamicXml xmlWrite = new DynamicXml())
            {
                xmlWrite.Write("/configuration/time/@start", DateTime.Now);
                xmlWrite.Write("/configuration/time/@end", DateTime.Now.AddHours(10));
                xmlWrite.Write("/configuration/size/@width", 800);
                xmlWrite.Write("/configuration/size/@height", 600);

                XmlNode user = xmlWrite.CreateNew("/configuration/user");
                xmlWrite.Write(user, "LastAccess", DateTime.Now);
                xmlWrite.Write(user, "DomainName", Environment.UserDomainName);
                xmlWrite.Write("/configuration/user/@Username", Environment.UserName);


                XmlNode application = xmlWrite.CreateNew("/configuration/Application");
                xmlWrite.Write(application, "Name", app);
                xmlWrite.Write(application, "Path", appPath);

                xmlResult = xmlWrite.Xml;
            }

            File.WriteAllText(pathFileName, xmlResult);
            Assert.IsTrue(File.Exists(pathFileName) == true);

            using (DynamicXml xmlRead = new DynamicXml(xmlResult))
            {
                XmlNodeList times = xmlRead.XmlDocument.SelectNodes("/configuration/time");
                XmlNode timesNode = times[0];
                DateTime? start = xmlRead.ReadDateTime(timesNode, "@start", null);
                DateTime? end = xmlRead.ReadDateTime(timesNode, "@end", null);

                XmlNodeList sizes = xmlRead.XmlDocument.SelectNodes("/configuration/size");
                XmlNode sizesNode = sizes[0];
                int? width = xmlRead.ReadInt(sizesNode, "@width", null);
                int? height = xmlRead.ReadInt(sizesNode, "@height", null);

                XmlNodeList users = xmlRead.XmlDocument.SelectNodes("/configuration/user");
                XmlNode userNode = users[0];
                string lastAccess = xmlRead.ReadString(userNode, "LastAccess", null);
                string domainName = xmlRead.ReadString(userNode, "DomainName", null);

                Assert.AreEqual(lastAccess, "2025-03-01T18:46:10");
                Assert.AreEqual(domainName, "GERHARD-G6");
            }
        }

        [TestMethod]
        public void CreateVSSnippet()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DynamicXml\\DemoData\\test.snippet");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = Environment.NewLine;
            settings.NewLineOnAttributes = false;
            using (XmlWriter writer = XmlWriter.Create(pathFileName, settings))
            {
                writer.WriteComment("XML Snippet Definition");

                writer.WriteStartElement("CodeSnippets", "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet");
                writer.WriteStartElement("CodeSnippet");
                writer.WriteAttributeString("Format", "1.0.0");
                writer.WriteStartElement("Header");
                writer.WriteStartElement("SnippetTypes");
                writer.WriteStartElement("SnippetType");
                writer.WriteString("Expansion");
                writer.WriteEndElement();
                writer.WriteStartElement("SnippetType");
                writer.WriteString("SurroundsWith");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("Title");
                writer.WriteString("Test Snippet");
                writer.WriteEndElement();
                writer.WriteStartElement("Author");
                writer.WriteString("Gerhard Ahrens");
                writer.WriteEndElement();
                writer.WriteStartElement("Description");
                writer.WriteString("Beschreibung zum Test Snippet");
                writer.WriteEndElement();
                writer.WriteStartElement("HelpUrl");
                writer.WriteString("www.lifeprojects.de");
                writer.WriteEndElement();
                writer.WriteStartElement("Shortcut");
                writer.WriteString("FSTestSnippet");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("Snippet");
                writer.WriteStartElement("Declarations");
                writer.WriteStartElement("Literal");
                writer.WriteAttributeString("Editable", "true");
                writer.WriteStartElement("ID");
                writer.WriteString("SnippetText");
                writer.WriteEndElement();
                writer.WriteStartElement("ToolTip");
                writer.WriteString("ToolTip, SnippetText");
                writer.WriteEndElement();
                writer.WriteStartElement("Default");
                writer.WriteString("Default, SnippetText");
                writer.WriteEndElement();
                writer.WriteStartElement("Function");
                writer.WriteString("Function, SnippetText");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("Code");
                writer.WriteAttributeString("Language", "csharp");
                writer.WriteCData("// Kommentar");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
        }
    }
}
