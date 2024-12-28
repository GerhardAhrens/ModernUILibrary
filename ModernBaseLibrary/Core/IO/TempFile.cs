//-----------------------------------------------------------------------
// <copyright file="SpecialFolder.cs" company="Lifeprojects.de">
//     Class: SpecialFolder
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.04.2021</date>
//
// <summary>
//  Mit der Klasse kann ein temporärer Dateiname erstellt werden
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;
    using System.Reflection;

    using PathEx = System.IO.Path;

    public partial class TempFile : IDisposable
    {
        public string Path { get; }

        public TempFile(string path) => Path = path;

        public void Dispose()
        {
            try
            {
                File.Delete(Path);
            }
            catch (FileNotFoundException)
            {
            }
        }
    }

    public partial class TempFile
    {
        public static TempFile Create()
        {
            string dirPath = PathEx.Combine(PathEx.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory(), "Temp" );

            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }

            var filePath = PathEx.Combine(dirPath, Guid.NewGuid() + ".tmp");

            return new TempFile(filePath);
        }
    }
}