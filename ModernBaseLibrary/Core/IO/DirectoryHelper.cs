namespace ModernBaseLibrary.Core.IO
{
    using System.IO;

    /// <summary>
    /// Enthält Methoden zur Verwaltung von Verzeichnissen.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Löscht rekursiv ein Verzeichnis sowie alle Unterverzeichnisse und Dateien. Wenn die Dateien schreibgeschützt sind, werden sie als normal gekennzeichnet und dann gelöscht.
        /// </summary>
        /// <param name="directory">Der Name des zu entfernenden Verzeichnisses.</param>
        public static void DeleteReadOnlyDirectory(string directory)
        {
            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                DeleteReadOnlyDirectory(subdirectory);
            }

            foreach (var fileName in Directory.EnumerateFiles(directory))
            {
                var fileInfo = new FileInfo(fileName);
                fileInfo.Attributes = FileAttributes.Normal;
                fileInfo.Delete();
            }

            Directory.Delete(directory);
        }
    }
}
