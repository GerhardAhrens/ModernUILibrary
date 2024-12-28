/*
 * <copyright file="RandomContentFile.cs" company="Lifeprojects.de">
 *     Class: RandomContentFile
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>08.06.2023 09:37:42</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Cryptography.File
{
    using System.IO;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Core.IO;

    [SupportedOSPlatform("windows")]
    public sealed class RandomContentFile : DisposableCoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomContentFile"/> class.
        /// </summary>
        public RandomContentFile()
        {
        }

        public void CreateRandomByteArrayFile(string filePath, int size, FileSizeUnit unit = FileSizeUnit.Kb)
        {
            int blockSize = (int)unit;
            byte[] data = new byte[blockSize];

            using (RandomDataContent crypto = new RandomDataContent())
            {
                using (FileStream stream = File.OpenWrite(filePath))
                {
                    for (int i = 0; i < size; i++)
                    {
                        data = crypto.GetByteArray(blockSize);
                        stream.Write(data, 0, data.Length);
                    }

                    stream.Flush();
                    stream.Close();
                }
            }
        }

        public void CreateRandomTextFile(string filePath, int size, FileSizeUnit unit = FileSizeUnit.Kb)
        {
            int blockSize = (int)unit;

            using (RandomDataContent crypto = new RandomDataContent())
            {
                using (TextWriter stream = new StreamWriter(filePath))
                {
                    for (int i = 0; i < size; i++)
                    {
                        string data = DemoTextBuilder.GenerateString(blockSize);
                        stream.Write(data);
                    }

                    stream.Flush();
                    stream.Close();
                }
            }
        }

        public void CreateRandomTextFile(Stream filePath, int size, FileSizeUnit unit = FileSizeUnit.Kb)
        {
            int blockSize = (int)unit;

            using (RandomDataContent crypto = new RandomDataContent())
            {
                using (TextWriter stream = new StreamWriter(filePath))
                {
                    for (int i = 0; i < size; i++)
                    {
                        string data = DemoTextBuilder.GenerateString(blockSize);
                        stream.Write(data);
                    }

                    stream.Flush();
                    stream.Close();
                }
            }
        }
    }
}
