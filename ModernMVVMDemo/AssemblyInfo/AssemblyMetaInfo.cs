//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaInfo.cs" company="www.lifeprojects.de">
//     Class: AssemblyMetaInfo
//     Copyright © www.lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>03.12.2024 07:49:42</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;
    using System.Globalization;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public class AssemblyMetaInfo : IAssemblyInfo
    {
        public string PacketName => "ModernUI";
        public Version PacketVersion => new Version(1, 0, 2025, 1);

        public string AssemblyName => $"ModernMVVMDemo";

        public Version AssemblyVersion => new Version(1,0,2025,10);

        public string Description => "Demoprogramm für MVVM Funktionen";

        private DateTime BuildDateTime()
        {
            const string BUILDDATENAME = "BuildDate.txt";
            string[] dateFormats = new string[] { "d.M.yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy  H:mm:ss" };
            DateTime result = new DateTime(1900,1,1);

            try
            {
                bool isBuildFile = XAMLResourceManager.HasResource(BUILDDATENAME, AssemblyLocation.CallingAssembly);
                if (isBuildFile == true)
                {
                    string content = XAMLResourceManager.GetResourceContent<string>(BUILDDATENAME, AssemblyLocation.CallingAssembly).Trim();
                    if (content.Contains(',') == true)
                    {
                        content = content.Split(',')[0].Trim();

                        DateTime outDT;
                        bool dtOK = DateTime.TryParseExact(content, dateFormats, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None,out outDT);
                        if (dtOK == true)
                        {
                            result = outDT;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
