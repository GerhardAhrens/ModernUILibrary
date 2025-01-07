//-----------------------------------------------------------------------
// <copyright file="DirectoryHelper.cs" company="Lifeprojects.de">
//     Class: DirectoryHelper
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>19.12.2022</date>
//
// <summary>
// Helper Klasse für Datei Operationen
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Comparer;

    /// <summary>
    /// File facilities.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class FileHelper
    {
        /// <summary>
        /// Determines whether the specified file is readable.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>
        /// <c>true</c> if the specified file is readable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReadable(string filename)
        {
            WindowsIdentity principal = WindowsIdentity.GetCurrent();
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                AuthorizationRuleCollection acl = fi.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
                for (int i = 0; i < acl.Count; i++)
                {
                    FileSystemAccessRule rule = (FileSystemAccessRule)acl[i];
                    if (principal.User.Equals(rule.IdentityReference))
                    {
                        if (AccessControlType.Deny.Equals(rule.AccessControlType) == true)
                        {
                            if ((((int)FileSystemRights.Read) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Read))
                            {
                                return false;
                            }
                        }
                        else if (AccessControlType.Allow.Equals(rule.AccessControlType) == true)
                        {
                            if ((((int)FileSystemRights.Read) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Read))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified file is writeable.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>
        /// 	<c>true</c> if the specified file is writeable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWriteable(string filename)
        {
            WindowsIdentity principal = WindowsIdentity.GetCurrent();
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                if (fi.IsReadOnly == true)
                {
                    return false;
                }

                AuthorizationRuleCollection acl = fi.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
                for (int i = 0; i < acl.Count; i++)
                {
                    FileSystemAccessRule rule = (FileSystemAccessRule)acl[i];
                    if (principal.User.Equals(rule.IdentityReference))
                    {
                        if (AccessControlType.Deny.Equals(rule.AccessControlType))
                        {
                            if ((((int)FileSystemRights.Write) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Write))
                            {
                                return false;
                            }
                        }
                        else if (AccessControlType.Allow.Equals(rule.AccessControlType))
                        {
                            if ((((int)FileSystemRights.Write) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Write))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public static int LastFileNumber(string rootPath = "", string filePattern = "*.*")
        {
            int result = 0;

            if (string.IsNullOrEmpty(rootPath) == true)
            {
                rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            DirectoryInfo di = new DirectoryInfo(rootPath);
            IEnumerable<FileInfo> files = di.EnumerateFiles(filePattern, SearchOption.TopDirectoryOnly);

            FileInfo lastFile = files.OrderBy(x => x.FullName, new OrdinalStringComparer()).LastOrDefault();
            if (lastFile != null)
            {
                string resultString = Regex.Match(lastFile.FullName, @"\d+").Value;

                if (int.TryParse(resultString, out result) == false)
                {
                    result = -1;
                }
            }

            return result;
        }

        /// <summary>
        /// Compares the specified file1.
        /// This method accepts two strings the represent two files to 
        /// compare.
        /// </summary>
        /// <param name="file1">The file1.</param>
        /// <param name="file2">The file2.</param>
        /// <returns>True, if files are equal, otherwise false.</returns>
        private static bool Compare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times. 
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same. 
                return true;
            }

            // Open the two files. 
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same. 
            if (fs1.Length != fs2.Length)
            {
                // Close the file 
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different 
                return false;
            }

            // Read and compare a byte from each file until either a 
            // non-matching set of bytes is found or until the end of 
            // file1 is reached. 
            do
            {
                // Read one byte from each file. 
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files. 
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same. 
            return ((file1byte - file2byte) == 0);
        }
    }
}