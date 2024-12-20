//-----------------------------------------------------------------------
// <copyright file="SecureStringExtension.cs" company="Lifeprojects.de">
//     Class: SecureStringExtension
//     Copyright © company="Lifeprojects.de" 2023
// </copyright>
//
// <author>Gerhard Ahrens - company="Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>31.5.2023</date>
//
// <summary>Class with SecureString Definition</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public static class SecureStringExtension
    {
        /// <summary>
        /// Die Methode konvertiert einen String in einen SecureString
        /// </summary>
        /// <param name="this">String</param>
        /// <returns>Typ of SecureString</returns>
        public static SecureString ToSecureString(this string @this)
        {
            return new NetworkCredential(string.Empty, @this).SecurePassword;
        }

        /// <summary>
        /// Die Methode konvertiert einen SecureString in einen String.
        /// </summary>
        /// <param name="this">SecureString</param>
        /// <returns>Typ of String</returns>
        public static string SecureStringToString(this SecureString @this)
        {
            return new NetworkCredential(string.Empty, @this).Password;
        }

        public static bool IsEqualTo(this SecureString ss1, SecureString ss2)
        {
            Argument.NotNull(ss1, nameof(ss1));
            Argument.NotNull(ss2, nameof(ss2));

            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }
    }
}