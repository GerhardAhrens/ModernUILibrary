//-----------------------------------------------------------------------
// <copyright file="OSCommand.cs" company="Lifeprojects.de">
//     Class: OSCommand
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.12.2020</date>
//
// <summary>
// Die Klasse stellt Methoden bereit um einen Kommandozeilenaufruf erstellen zu können.
// var aa = new OSCommand().ForProgram("netsh.exe").WithArguments("wlan show interfaces").Output(CmdOutput.ToString).Run'string'();
// </summary>
// <Link>
// https://learn.microsoft.com/de-de/dotnet/api/system.text.codepagesencodingprovider?view=net-7.0
// </Link>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Windows;

    using ModernBaseLibrary.Core.IO;
    using ModernBaseLibrary.Extension;

    public enum OSCommandOutput : int
    {
        None = 0,
        ToString = 1,
        ToClipboard = 2,
        RunProgram = 3,
        SelectFile = 4,
        GetVerbs = 5,
        ToFile = 6
    }

    /// <summary>
    /// Die Klasse stellt Mehoden bereit um einen Kommandozeilenaufruf erstellen zu können.
    /// </summary>
    /// <example>
    /// var aa = new OSCommand().ForProgram("netsh.exe").WithArguments("wlan show interfaces").Output(CmdOutput.ToString).Run'string'();
    /// </example>
    [SupportedOSPlatform("windows")]
    public sealed class OSCommand
    {
        private OSCommandOutput outputTyp = OSCommandOutput.None;

        public OSCommand()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            this.outputTyp = OSCommandOutput.None;
            this.FileName = string.Empty;
            this.Arguments = string.Empty;
            this.InternalCmd = string.Empty;
            this.Verb = string.Empty;
            this.Domain = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.StandardOutput = true;
            this.OutputEncoding = Encoding.GetEncoding(850);
            this.UseShellExecute = false;
            this.CreateNoWindow = true;
            this.WindowStyle = ProcessWindowStyle.Hidden;
        }

        private string FileName { get; set; }

        private string Arguments { get; set; }

        private string InternalCmd { get; set; }

        private string Verb { get; set; }

        private string Username { get; set; }

        private string Password { get; set; }

        private string Domain { get; set; }

        private string Directory { get; set; }

        private bool StandardOutput { get; set; }

        private Encoding OutputEncoding { get; set; }

        private bool UseShellExecute { get; set; }

        private bool CreateNoWindow { get; set; }

        private ProcessWindowStyle WindowStyle { get; set; }

        private string ResultFilename { get; set; }

        public OSCommand ForProgram(string programName = null)
        {
            if (programName == null)
            {
                this.FileName = "cmd.exe";
            }
            else
            {
                this.FileName = programName;
            }

            return this;
        }

        public OSCommand WithArguments(string arguments)
        {
            this.Arguments = arguments;

            return this;
        }

        public OSCommand InternalCommand(string internalCmd)
        {
            this.InternalCmd = $"/c {internalCmd}";

            return this;
        }

        public OSCommand ResultFile(string filename = null)
        {
            if (string.IsNullOrEmpty(filename) == true)
            {
                this.ResultFilename = "Result.txt";
            }
            else
            {
                this.ResultFilename = filename;
            }

            return this;
        }

        public OSCommand WithVerb(string verb)
        {
            this.Verb = verb;

            return this;
        }

        public OSCommand CodePage(int codePage = 850)
        {
            this.OutputEncoding = Encoding.GetEncoding(codePage);

            return this;
        }

        public OSCommand WorkingDirectory(string directory)
        {
            this.Directory = directory;

            return this;
        }

        public OSCommand Authorization(string domain, string username, string password)
        {
            this.Domain = domain;
            this.Username = username;
            this.Password = password;

            return this;
        }

        public OSCommand Authorization(string username, string password)
        {
            string[] userdomain = username.Split("\\");
            if (userdomain.Length == 1)
            {
                this.Domain = Environment.UserDomainName;
                this.Username = userdomain[1];
            }
            else if (userdomain.Length == 2)
            {
                this.Domain = userdomain[0];
                this.Username = userdomain[1];
            }

            this.Password = password;

            return this;
        }

        /// <summary>
        /// Die Methode legt eine mögliche Ausgabe des Ergbenis fest
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public OSCommand Output(OSCommandOutput output)
        {
            this.outputTyp = output;

            return this;
        }

        /// <summary>
        /// Die Methode führt die Anweisung aus
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>Text von der Console</returns>
        public TResult Run<TResult>()
        {
            object result = null;

            if (this.outputTyp == OSCommandOutput.None)
            {
                string cmdOutpout = "No output parameter 'OSCommandOutput' was set";
                result = cmdOutpout == null ? default(TResult) : (TResult)Convert.ChangeType(cmdOutpout, typeof(TResult), CultureInfo.InvariantCulture);
                return (TResult)result;
            }

            if (this.outputTyp == OSCommandOutput.GetVerbs)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = this.FileName;
                string[] verbs = psi.Verbs;
                string cmdOutpout = string.Join(";", verbs);
                result = cmdOutpout == null ? default(TResult) : (TResult)Convert.ChangeType(cmdOutpout, typeof(TResult), CultureInfo.InvariantCulture);
                return (TResult)result;
            }

            if (string.IsNullOrEmpty(this.InternalCmd) == false)
            {
                this.UseShellExecute = false;
                this.Arguments = $"{this.InternalCmd} {this.Arguments}";
            }

                using (Process p = new Process())
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = this.FileName;
                psi.Arguments = this.Arguments;
                psi.Verb = this.Verb;

                if (this.outputTyp.In(OSCommandOutput.ToString,OSCommandOutput.ToClipboard, OSCommandOutput.ToFile))
                {
                    psi.StandardOutputEncoding = this.OutputEncoding;
                    psi.RedirectStandardOutput = this.StandardOutput;
                    psi.UseShellExecute = this.UseShellExecute;
                    psi.CreateNoWindow = this.CreateNoWindow;
                    psi.WindowStyle = this.WindowStyle;
                }

                p.StartInfo = psi;

                bool startOK = false;
                try
                {
                    startOK = p.Start();
                }
                catch (Exception ex)
                {
                    string errorText = ex.Message;
                    throw;
                }

                if (startOK == true && this.outputTyp == OSCommandOutput.ToString)
                {
                    string cmdOutpout = p.StandardOutput.ReadToEnd().Trim();
                    p.WaitForExit();
                    result = cmdOutpout == null ? default(TResult) : (TResult)Convert.ChangeType(cmdOutpout, typeof(TResult), CultureInfo.InvariantCulture);
                }
                else if (startOK == true && this.outputTyp == OSCommandOutput.ToClipboard)
                {
                    string cmdOutpout = p.StandardOutput.ReadToEnd().Trim();
                    p.WaitForExit();
                    Clipboard.SetText(cmdOutpout);
                    result = (TResult)Convert.ChangeType(string.IsNullOrEmpty(cmdOutpout) == false, typeof(TResult), CultureInfo.InvariantCulture);
                }
                else if (startOK == true && this.outputTyp == OSCommandOutput.ToFile)
                {
                    string cmdOutpout = p.StandardOutput.ReadToEnd().Trim();
                    p.WaitForExit();

                    string path = Path.Combine(SpecialFolder.GetPath(SpecialFolderTyp.Downloads),this.ResultFilename);
                    FileInfo fi = path.ToFileInfo();
                    using (StreamWriter str = fi.CreateText())
                    {
                        str.WriteLine(cmdOutpout);
                        str.Close();
                    }

                    result = (TResult)Convert.ChangeType(string.IsNullOrEmpty(cmdOutpout) == false, typeof(TResult), CultureInfo.InvariantCulture);
                }
                else if (startOK == true && this.outputTyp == OSCommandOutput.RunProgram)
                {
                }
                else if (startOK == true && this.outputTyp == OSCommandOutput.SelectFile)
                {
                }
            }

            return (TResult)result;
        }
    }
}
