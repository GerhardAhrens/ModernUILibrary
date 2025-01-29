//-----------------------------------------------------------------------
// <copyright file="ToolsGraphics.cs" company="Lifeprojects.de">
//     Class: ToolsGraphics
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.08.2017</date>
//
// <summary>
//      Klasse mit Methoden zur Bearbeitung und Konvertierung von Grafiken/Images
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Versioning;
    using System.Text;

    public static class ImageUrlHelper
    {
        public static string ConvertImageURLToBase64(string url)
        {
            bool result = Uri.IsWellFormedUriString(url, UriKind.Absolute);
            if (result == true)
            {
                StringBuilder _sb = new StringBuilder();

                Byte[] _byte = GetImage(url);

                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

                return _sb.ToString();
            }

            return string.Empty;
        }

        static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
#pragma warning disable SYSLIB0014 // Typ oder Element ist veraltet
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // Typ oder Element ist veraltet

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                string errorText = exp.Message;
                buf = null;
            }

            return (buf);
        }
    }
}

