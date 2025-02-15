/*
 * <copyright file="NameStaticClass1.cs" company="Lifeprojects.de">
 *     Class: NameStaticClass1
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>15.02.2025 19:14:59</date>
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


namespace ModernBaseLibrary.Core.Comparer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using System.IO;
    using System.Security.Cryptography;

    public static class ImagesComparer
    {
        public static bool ImagesHashCompare(string imageFirst, string imageSecond)
        {
            try
            {
                return GetImageHash(imageFirst) == GetImageHash(imageSecond);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public static string GetImageHash(string imagePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static bool ImageCompare(string imageFirst, string imageSecond)
        {
            using (Bitmap bmp1 = new Bitmap(imageFirst))
            using (Bitmap bmp2 = new Bitmap(imageSecond))
            {
                if (bmp1.Width != bmp2.Width || bmp1.Height != bmp2.Height)
                {
                    return false;
                }

                for (int y = 0; y < bmp1.Height; y++)
                {
                    for (int x = 0; x < bmp1.Width; x++)
                    {
                        if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
