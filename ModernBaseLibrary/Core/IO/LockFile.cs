//-----------------------------------------------------------------------
// <copyright file="LockFile.cs" company="Lifeprojects.de">
//     Class: LockFile
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.01.2021</date>
//
// <summary>
// Die Klasse erstellt eine Lock-Datei bzw. prüft, ob diese vorhanden ist.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Threading;

    /// <summary>
    /// Represents an exclusive resource backed by a file.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class LockFile : IDisposable
    {
        public LockFile()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="LockFile"/>.
        /// </summary>
        public LockFile(FileStream fileStream)
        {
            fileStream.IsArgumentNull(nameof(fileStream));
        }

        /// <summary>
        /// File stream that represents this lock file.
        /// </summary>
        public FileStream FileStream { get; }

        /// <inheritdoc />
        public void Dispose() => FileStream?.Dispose();
    }

    public partial class LockFile
    {
        private const int ERROR_SHARING_VIOLATION = 32;
        private const int ERROR_LOCK_VIOLATION = 33;

        /// <summary>
        /// Tries to acquire a lock file with given file path.
        /// Returns null if the file is already in use.
        /// </summary>
        public static LockFile TryAcquire(string filePath)
        {
            filePath.IsArgumentNullOrEmpty(nameof(filePath));

            try
            {
                var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                return new LockFile(fileStream);
            }
            // When access to file is denied, an IOException (not derived) is thrown
            catch (IOException ex) when (ex.GetType() == typeof(IOException))
            {
                return null;
            }
        }

        /// <summary>
        /// Repeatedly tries to acquire a lock file, until the operation succeeds or is canceled.
        /// </summary>
        public static LockFile WaitAcquire(string filePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            filePath.IsArgumentNullOrEmpty(nameof(filePath));

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var lockFile = TryAcquire(filePath);
                if (lockFile != null)
                {
                    return lockFile;
                }
            }
        }

        public bool CanReadFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath) == true)
                {
                    using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        if (fileStream != null)
                        {
                            fileStream.Close();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                if (LockFile.IsFileLocked(ex))
                {
                    return false;
                }
            }

            return true;
        }

        public static byte[] ReadFileBytes(string filePath)
        {
            byte[] buffer = null;
            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    int length = (int)fileStream.Length;  // get file length
                    buffer = new byte[length];            // create buffer
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;  // sum is a buffer offset for next reading
                    }

                    fileStream.Close(); //This is not needed, just me being paranoid and explicitly releasing resources ASAP
                }
            }
            catch (IOException ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (LockFile.IsFileLocked(ex))
                {
                    // do something? 
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
            }

            return buffer;
        }

        public static string ReadFileTextWithEncoding(string filePath)
        {
            string fileContents = string.Empty;
            byte[] buffer;
            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    int length = (int)fileStream.Length;  // get file length
                    buffer = new byte[length];            // create buffer
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;  // sum is a buffer offset for next reading
                    }

                    fileStream.Close(); //Again - this is not needed, just me being paranoid and explicitly releasing resources ASAP

                    //Depending on the encoding you wish to use - I'll leave that up to you
                    fileContents = System.Text.Encoding.Default.GetString(buffer);
                }
            }
            catch (IOException ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (LockFile.IsFileLocked(ex))
                {
                    // do something? 
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
            }

            return fileContents;
        }

        public static string ReadFileTextNoEncoding(string filePath)
        {
            string fileContents = string.Empty;
            byte[] buffer;
            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    int length = (int)fileStream.Length;  // get file length
                    buffer = new byte[length];            // create buffer
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;  // sum is a buffer offset for next reading
                    }

                    fileStream.Close(); //Again - this is not needed, just me being paranoid and explicitly releasing resources ASAP

                    char[] chars = new char[buffer.Length / sizeof(char) + 1];
                    System.Buffer.BlockCopy(buffer, 0, chars, 0, buffer.Length);
                    fileContents = new string(chars);
                }
            }
            catch (IOException ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (LockFile.IsFileLocked(ex))
                {
                    // do something? 
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
            }

            return fileContents;
        }

        public static bool FileLocked(string FileName)
        {
            FileStream fs = null;

            try
            {
                // NOTE: This doesn't handle situations where file is opened for writing by another process but put into write shared mode, it will not throw an exception and won't show it as write locked
                fs = File.Open(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None); // If we can't open file for reading and writing then it's locked by another process for writing
            }
            catch (UnauthorizedAccessException) // https://msdn.microsoft.com/en-us/library/y973b725(v=vs.110).aspx
            {
                // This is because the file is Read-Only and we tried to open in ReadWrite mode, now try to open in Read only mode
                try
                {
                    fs = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception)
                {
                    return true; // This file has been locked, we can't even open it to read
                }
            }
            catch (Exception)
            {
                return true; // This file has been locked
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return false;
        }

        private static bool IsFileLocked(Exception exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }
    }
}