//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaInfo.cs" company="www.pta.de">
//     Class: AssemblyMetaInfo
//     Copyright © www.pta.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
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

    using ModernUILibrary.Core;

    public class AssemblyMetaInfo : IAssemblyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMetaInfo"/> class.
        /// </summary>
        public AssemblyMetaInfo()
        {
            var aa = BuildDateTime();
        }

        public string PacketName => "ModernUI";
        public Version PacketVersion => new Version(1, 0, 2024, 0);

        public string AssemblyName => $"ModernUIDemo; {BuildDateTime()}";

        public Version AssemblyVersion => new Version(1,0,2024,8);

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
