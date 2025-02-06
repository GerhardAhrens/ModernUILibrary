//-----------------------------------------------------------------------
// <copyright file="SerializeHelper.cs" company="Lifeprojects.de">
//     Class: SerializeHelper
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>Helper Class for Serialize/DeSerialize</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text.Json;
    using System.Xml.Serialization;

    public class SerializeHelper<T> where T : class
    {
        public static string Serialize(T obj, SerializeFormatter formatter = SerializeFormatter.Xml, string fileOrContent = "")
        {
            try
            {
                switch (formatter)
                {
                    case SerializeFormatter.Xml:
                        XmlSerializer serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                        using (TextWriter textWriter = new StreamWriter(fileOrContent))
                        {
                            serializer.Serialize(textWriter, obj);
                            textWriter.Close();
                        }

                        return null;

                    case SerializeFormatter.Text:
                        string result = string.Empty;
                        XmlSerializer serializerText = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                        using (StringWriter textWriterText = new StringWriter())
                        {
                            serializerText.Serialize(textWriterText, obj);
                            result = textWriterText.ToString();
                        }

                        return result;

                    case SerializeFormatter.Json:
                        string resultJson = JsonSerializer.Serialize<T>(obj);
                        using (TextWriter textWriter = new StreamWriter(fileOrContent))
                        {
                            textWriter.Write(resultJson);
                            textWriter.Flush();
                            textWriter.Close();
                        }

                        return resultJson;

                    default:
                        throw new Exception("Invalid Formatter option");
                }
            }
            catch (SerializationException ex)
            {
                var errMsg = string.Format("Unable to serialize {0} into file {1}", obj, fileOrContent);
                throw new Exception(errMsg, ex);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public static T DeSerialize(SerializeFormatter formatter = SerializeFormatter.Xml, string fileOrContent = "")
        {
            try
            {
                switch (formatter)
                {
                    case SerializeFormatter.Xml:
                        if (File.Exists(fileOrContent) == false)
                        {
                            return null;
                        }

                        XmlSerializer SerializerXml = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                        using (TextReader rdr = new StreamReader(fileOrContent))
                        {
                            object obj = (T)SerializerXml.Deserialize(rdr);
                            rdr.Close();
                            return (T)obj;
                        }

                    case SerializeFormatter.Text:
                        if (string.IsNullOrEmpty(fileOrContent) == false)
                        {
                            XmlSerializer SerializerText = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                            using (StringReader textReader = new StringReader(fileOrContent))
                            {
                                return (T)SerializerText.Deserialize(textReader);
                            }
                        }
                        else
                        {
                            return (T)null;
                        }

                    case SerializeFormatter.Json:
                        if (File.Exists(fileOrContent) == false)
                        {
                            return null;
                        }

                        string jsonString = File.ReadAllText(fileOrContent);
                        return (T)JsonSerializer.Deserialize<T>(jsonString);

                    default:
                        throw new Exception("Invalid Formatter option");
                }
            }
            catch (SerializationException ex)
            {
                var errMsg = string.Format("Unable to deserialize {0} from file {1}", typeof(T), fileOrContent);
                throw new Exception(errMsg, ex);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }
    }
}