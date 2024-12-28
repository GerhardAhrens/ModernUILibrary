//-----------------------------------------------------------------------
// <copyright file="SecureDelete.cs" company="Lifeprojects.de">
//     Class: SecureDelete
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.04.2023</date>
//
// <summary>
//  Mit der Klasse ist ein sichers Löshen von dateien möglich.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public sealed class SecureDelete
    {
        /// <summary>
        /// Deletes a file in a secure way by overwriting it with
        /// random garbage data n times.
        /// </summary>
        /// <param name="pFilename">Full path of the file to be deleted</param>
        /// <param name="pTimesToWrite">Specifies the number of times the file should be overwritten</param>
        public void SecureDeleteFile(string pFilename, int pTimesToWrite)
        {
            try
            {
                if (File.Exists(pFilename))
                {
                    // Set the files attributes to normal in case it's read-only.
                    File.SetAttributes(pFilename, FileAttributes.Normal);

                    // Calculate the total number of sectors in the file.
                    double sectors = Math.Ceiling(new FileInfo(pFilename).Length / 512.0);

                    // Create a dummy-buffer the size of a sector.
                    byte[] dummyBuffer = new byte[512];

                    // Create a cryptographic Random Number Generator.
                    // This is what I use to create the garbage data.
#pragma warning disable SYSLIB0023 // Typ oder Element ist veraltet
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
#pragma warning restore SYSLIB0023 // Typ oder Element ist veraltet

                    // Open a FileStream to the file.
                    FileStream inputStream = new FileStream(pFilename, FileMode.Open);
                    for (int currentPass = 0; currentPass < pTimesToWrite; currentPass++)
                    {
                        UpdatePassInfo(pFilename, currentPass + 1, pTimesToWrite);

                        // Go to the beginning of the stream
                        inputStream.Position = 0;

                        // Loop all sectors
                        for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                        {
                            UpdateSectorInfo(sectorsWritten + 1, (int)sectors);

                            // Fill the dummy-buffer with random data
                            rng.GetBytes(dummyBuffer);
                            // Write it to the stream
                            inputStream.Write(dummyBuffer, 0, dummyBuffer.Length);
                        }
                    }
                    // Truncate the file to 0 bytes.
                    // This will hide the original file-length if you try to recover the file.
                    inputStream.SetLength(0);
                    // Close the stream.
                    inputStream.Close();

                    // As an extra precaution I change the dates of the file so the
                    // original dates are hidden if you try to recover the file.
                    DateTime dt = new DateTime(2037, 1, 1, 0, 0, 0);
                    File.SetCreationTime(pFilename, dt);
                    File.SetLastAccessTime(pFilename, dt);
                    File.SetLastWriteTime(pFilename, dt);

                    File.SetCreationTimeUtc(pFilename, dt);
                    File.SetLastAccessTimeUtc(pFilename, dt);
                    File.SetLastWriteTimeUtc(pFilename, dt);

                    // Finally, delete the file
                    File.Delete(pFilename);

                    SecureDeleteDone();
                }
            }
            catch (Exception e)
            {
                WipeError(e);
            }
        }

        # region Events
        public event PassInfoEventHandler PassInfoEvent;
        private void UpdatePassInfo(string pFilename, int currentPass, int totalPasses)
        {
            if (PassInfoEvent != null)
            {
                PassInfoEvent(new PassInfoEventArgs(pFilename, currentPass, totalPasses));
            }
        }

        public event SectorInfoEventHandler SectorInfoEvent;
        private void UpdateSectorInfo(int currentSector, int totalSectors)
        {
            if (SectorInfoEvent != null)
            {
                SectorInfoEvent(new SectorInfoEventArgs(currentSector, totalSectors));
            }
        }

        public event SecureDeleteDoneEventHandler SecureDeleteDoneEvent;
        private void SecureDeleteDone()
        {
            if (SecureDeleteDoneEvent != null)
            {
                SecureDeleteDoneEvent(new SecureDeleteDoneEventArgs());
            }
        }

        public event WipeErrorEventHandler WipeErrorEvent;
        private void WipeError(Exception e)
        {
            WipeErrorEvent(new WipeErrorEventArgs(e));
        }
        # endregion

        public void DeleteFile(string pFilename)
        {
            try
            {
                if (File.Exists(pFilename))
                {
                    File.Delete(pFilename);
                }
            }
            catch (Exception ex)
            {
                WipeError(ex);
            }
        }

    }

    # region PassInfo
    public delegate void PassInfoEventHandler(PassInfoEventArgs e);
    public class PassInfoEventArgs : EventArgs
    {
        private readonly string cFilename;
        private readonly int cPass;
        private readonly int tPass;

        public PassInfoEventArgs(string pFilename, int currentPass, int totalPasses)
        {

            cFilename = pFilename;
            cPass = currentPass;
            tPass = totalPasses;
        }

        public string CurrentFilename { get { return cFilename; } }

        /// <summary> Get the current pass </summary>
        public int CurrentPass { get { return cPass; } }
        /// <summary> Get the total number of passes to be run </summary> 
        public int TotalPasses { get { return tPass; } }
    }
    # endregion

    # region SectorInfo
    public delegate void SectorInfoEventHandler(SectorInfoEventArgs e);
    public class SectorInfoEventArgs : EventArgs
    {
        private readonly int cSector;
        private readonly int tSectors;

        public SectorInfoEventArgs(int currentSector, int totalSectors)
        {
            cSector = currentSector;
            tSectors = totalSectors;
        }

        /// <summary> Get the current sector </summary> 
        public int CurrentSector { get { return cSector; } }
        /// <summary> Get the total number of sectors to be run </summary> 
        public int TotalSectors { get { return tSectors; } }
    }
    # endregion

    # region WipeDone
    public delegate void SecureDeleteDoneEventHandler(SecureDeleteDoneEventArgs e);
    public class SecureDeleteDoneEventArgs : EventArgs
    {
    }
    # endregion

    # region WipeError
    public delegate void WipeErrorEventHandler(WipeErrorEventArgs e);
    public class WipeErrorEventArgs : EventArgs
    {
        private readonly Exception e;

        public WipeErrorEventArgs(Exception error)
        {
            e = error;
        }

        public Exception WipeError { get { return e; } }
    }
    # endregion

}