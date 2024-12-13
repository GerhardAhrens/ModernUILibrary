//-----------------------------------------------------------------------
// <copyright file="ExceptionLogging.cs" company="Lifeprojects.de">
//     Class: ExceptionLogging
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.06.2018</date>
//
// <summary>Model ExceptionLogging</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    [DebuggerStepThrough]
    [Serializable]
    [DebuggerDisplay("ErrorLevel = {ErrorLevel}, ErrorText = {ErrorText}")]
    public class ExceptionToFile : ExceptionLogger<ExceptionToFile>
    {
        private string _errorText;

        public ExceptionToFile()
        {
            this.CreateErrorsPath();
        }

        public override string ErrorText
        {
            get { return this._errorText; }
            set
            {
                if (this._errorText != value)
                {
                    this._errorText = value;
                }
            }
        }

        public override void Save()
        {
            try
            {
                string errorFilename = CurrentErrorsFullname();
                File.WriteAllText(errorFilename, this.ErrorText, Encoding.UTF8);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string CurrentAssemblyPath()
        {
            string result = string.Empty;

            Assembly assm = Assembly.GetEntryAssembly();
            result = Path.GetDirectoryName(assm.Location);

            return result;
        }

        private string CurrentErrorsPath()
        {
            string result = string.Empty;

            result = $"{CurrentAssemblyPath()}\\Errors";

            return result;
        }

        private string CurrentErrorsFullname()
        {
            string result = string.Empty;

            string path = $"{this.CurrentAssemblyPath()}\\Errors";
            string name = $"{this.ApplicationName()}_{DateTime.Now.ToString("yyyyMMdd")}.txt";

            result = Path.Combine(path, name);

            return result;
        }

        private void CreateErrorsPath()
        {
            string errorsPath = CurrentErrorsPath();
            if (string.IsNullOrEmpty(errorsPath) == false)
            {
                try
                {
                    if (Directory.Exists(errorsPath) == false)
                    {
                        Directory.CreateDirectory(errorsPath);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private string ApplicationName()
        {
            string result = string.Empty;

            Assembly assm = Assembly.GetEntryAssembly();
            result = assm.GetName().Name;
            return result;
        }
    }
}