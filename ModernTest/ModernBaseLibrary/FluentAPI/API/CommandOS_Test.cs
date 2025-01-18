namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System.Collections.Generic;
    using System.IO;

    using global::ModernBaseLibrary.Core.IO;
    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Zusammenfassungsbeschreibung für DataRepository_Test
    /// </summary>
    [TestClass]
    public class CommandOS_Test
    {
        public CommandOS_Test()
        {
            //
            // TODO: Konstruktorlogik hier hinzufügen
            //
        }

        [TestMethod]
        public void OSCommandToString1()
        {
            var outAsString = new OSCommand().ForProgram("net.exe")
                .WithArguments("localgroup")
                .Output(OSCommandOutput.ToString)
                .CodePage(850)
                .Run<string>();

            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }

        [TestMethod]
        public void OSCommandToString2()
        {
            var outAsString = new OSCommand().ForProgram("net.exe")
                .WithArguments("help")
                .Output(OSCommandOutput.ToString)
                .Run<string>();

            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }

        [TestMethod]
        public void OSCommandForNetUse()
        {
            var outAsString = new OSCommand().ForProgram("net.exe")
                .WithArguments("use")
                .Output(OSCommandOutput.ToString)
                .Run<string>();

            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }

        [TestMethod]
        public void OSCommandToDIR()
        {
            var outAsString = new OSCommand().ForProgram("cmd.exe")
                .WithArguments(@"/c dir c:\*.*")
                .Output(OSCommandOutput.ToString)
                .Run<string>();
            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }

        [TestMethod]
        public void OSCommandToVerbs()
        {
            var outAsString = new OSCommand().ForProgram("net.exe")
                .WithArguments("localgroup")
                .Output(OSCommandOutput.GetVerbs)
                .Run<string>();
            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            Assert.IsTrue(outAsString == "open;runas;runasuser");
        }

        [TestMethod]
        public void OSCommandForNetUseToFile()
        {
            var outAsString = new OSCommand().ForProgram("net.exe")
                .WithArguments("use")
                .Output(OSCommandOutput.ToFile)
                .Run<string>();

            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);

            string path = Path.Combine(SpecialFolder.GetPath(SpecialFolderTyp.Downloads), "Result.txt");
            FileInfo fi = path.ToFileInfo();
            Assert.IsTrue(fi.Exists);
        }

        [TestMethod]
        public void OSCommandToRun_ByArguments_MKLink()
        {
            var outAsString = new OSCommand().ForProgram() /* null = cmd.exe */
                .WithArguments(@"/c mklink /J C:\_ToolsA c:\Users\Public\Tools\")
                .Output(OSCommandOutput.ToString)
                .Run<string>();
            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }

        [TestMethod]
        public void OSCommandToRun_ByInternalCommand_MKLink()
        {
            var outAsString = new OSCommand().ForProgram() /* null = cmd.exe */
                .InternalCommand("mklink")
                .WithArguments(@"/J C:\_ToolsA c:\Users\Public\Tools\")
                .Output(OSCommandOutput.ToString)
                .Run<string>();
            Assert.IsFalse(string.IsNullOrEmpty(outAsString));
            List<string> result = outAsString.ToLines();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }
    }
}
