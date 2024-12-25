//-----------------------------------------------------------------------
// <copyright file="AssertMyExtensions.cs" company="Lifeprojects.de">
//     Class: AssertMyExtensions
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.09.2022 14:52:20</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace EasyPrototypingTest
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static partial class AssertExtension
    {
        public static void AreEquals(this Assert @this, byte[] expected, byte[] actual)
        {
            string asserName = "AssertExtension.AreEqual";
            bool isEquals = expected.SequenceEqual(actual);
            if (isEquals == false)
            {
                string messageFailed = $"{asserName} failed. Expected: <{expected.Length}> Actual: <{actual.Length}>";
                throw new AssertFailedException(messageFailed);
            }
        }

        public static void AreEquals(this Assert @this, Bitmap expected, Bitmap actual, string message)
        {
            string asserName = "AssertExtension.AreEqual";

            //Test to see if we have the same size of image
            if (expected.Size != actual.Size)
            {
                string messageFailed = $"{asserName} failed. Expected:<Height {expected.Size.Height} \", Width {expected.Size.Width}>. Actual:<Height {actual.Size.Height},Width {actual.Size.Width}>.{message}";
                throw new AssertFailedException(messageFailed);

            }

            //Convert each image to a byte array
            ImageConverter ic = new ImageConverter();
            byte[] btImageExpected = new byte[1];
            btImageExpected = (byte[])ic.ConvertTo(expected, btImageExpected.GetType());
            byte[] btImageActual = new byte[1];
            btImageActual = (byte[])ic.ConvertTo(actual, btImageActual.GetType());

            //Compute a hash for each image
            byte[] hash1 = GetSHA256Hash(btImageExpected);
            byte[] hash2 = GetSHA256Hash(btImageActual);

            //Compare the hash values
            for (int i = 0; i < hash1.Length && i < hash2.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    string messageFailed = $"{asserName} failed. Expected:<hash value {hash1[i]} \">. Actual:<hash value {hash2[i]}>. {message}";
                    throw new AssertFailedException(messageFailed);
                }
            }
        }

        public static void AreEquals(this Assert @this, ImageSource expected, ImageSource actual, string message)
        {
            string asserName = "AssertExtension.AreEqual";

            //Test to see if we have the same size of image
            if (expected.Height != actual.Height)
            {
                string messageFailed = $"{asserName} failed. Expected:<Height {expected.Height} \", Width {expected.Width}>. Actual:<Height {actual.Height},Width {actual.Width}>.{message}";
                throw new AssertFailedException(messageFailed);

            }

            //Convert each image to a byte array
            byte[] expectedImage = BitmapSorceToByteArray(expected as BitmapSource);
            byte[] actualImage = BitmapSorceToByteArray(actual as BitmapSource);
            bool isEquals = expectedImage.SequenceEqual(actualImage);
            if (isEquals == false)
            {
                string messageFailed = $"{asserName} failed. Expected: <{expectedImage.Length}> Actual: <{actualImage.Length}>";
                throw new AssertFailedException(messageFailed);
            }
        }

        private static byte[] GetSHA256Hash(byte[] input)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var byteHash = hashAlgorithm.ComputeHash(input);
                return byteHash;
            }
        }

        private static byte[] BitmapSorceToByteArray(BitmapSource pBitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
            encoder.QualityLevel = 100;
            byte[] bit = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();
            }

            return bit;
        }

    }
}
