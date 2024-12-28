//-----------------------------------------------------------------------
// <copyright file="DynamicXml.cs" company="Lifeprojects.de">
//     Class: DynamicXml
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.06.2017</date>
//
// <summary>
// Class for read and write to XML Files
// </summary>
// <Link>
// https://unclassified.software/de/source/easyxml
// </Link>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.XML
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    public class DynamicXml : IDisposable
    {
        private bool classIsDisposed = false;
        private XmlDocument xd;

        /// <summary>
        /// Initialises a new instance of the DynamicXml class and creates a new empty XML document.
        /// </summary>
        public DynamicXml()
        {
            this.xd = new XmlDocument();
        }

        /// <summary>
        /// Initialises a new instance of the DynamicXml class and creates a new XML document from the specified content.
        /// </summary>
        /// <param name="xml">XML document string.</param>
        public DynamicXml(string xml)
        {
            this.xd = new XmlDocument();
            if (string.IsNullOrWhiteSpace(xml) == false)
            {
                this.xd.LoadXml(xml);
            }
        }

        /// <summary>
        /// Initialises a new instance of the DynamicXml class with the specified XML document.
        /// </summary>
        /// <param name="xmlDocument">XML document.</param>
        public DynamicXml(XmlDocument xmlDocument)
        {
            this.xd = xmlDocument;
        }

        /// <summary>
        /// Gets or sets the current XML document as a formatted string.
        /// </summary>
        public string Xml
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Encoding = Encoding.UTF8;
                    xws.Indent = true;
                    xws.IndentChars = "  ";
                    XmlWriter xw = XmlWriter.Create(ms, xws);
                    this.xd.Save(xw);

                    /* If the BOM is in the stream, skip it for the string output */
                    int start = 0;
                    int count = (int)ms.Length;
                    byte[] bom = new byte[3];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(bom, 0, 3);
                    if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
                    {
                        /* Skip UTF-8 Byte Order Mark (BOM) */
                        start = 3;
                        count -= start;
                    }

                    return Encoding.UTF8.GetString(ms.ToArray(), start, count);
                }
            }

            set
            {
                this.xd.LoadXml(value);
            }
        }

        /// <summary>
        /// Gets the current XML document as a formatted string without any declarative markup.
        /// </summary>
        public string PlainXml
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Encoding = Encoding.GetEncoding(28605);   /* ISO-8859-15 */
                    xws.Indent = true;
                    xws.IndentChars = "  ";
                    xws.OmitXmlDeclaration = true;
                    XmlWriter xw = XmlWriter.Create(ms, xws);
                    this.xd.Save(xw);
                    return Encoding.GetEncoding(28605).GetString(ms.ToArray());   /* ISO-8859-15 */
                }
            }
        }

        /// <summary>
        /// Gets the internal XML document for direct access.
        /// </summary>
        public XmlDocument XmlDocument
        {
            get
            {
                return this.xd;
            }
        }

        public static T ReadAs<T>(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value) == true)
                {
                    return default(T);
                }

                return string.IsNullOrEmpty(value) == true ? default(T) : (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }


        /// <summary>
        /// Creates a new document instance and clears all contents.
        /// </summary>
        public void Clear()
        {
            this.xd = new XmlDocument();
        }

        /// <summary>
        /// Loads an XML file into this document.
        /// </summary>
        /// <param name="fileName">XML file name.</param>
        public void Load(string fileName)
        {
            this.xd.Load(fileName);
        }

        /// <summary>
        /// Determines whether an element or attribute specified by the XPath selector exists in the document.
        /// </summary>
        /// <param name="xPath">XPath selector to check.</param>
        /// <returns>true if an element or attribute exists, false otherwise.</returns>
        public bool Exists(string xPath)
        {
            return this.Exists(this.xd, xPath);
        }

        /// <summary>
        /// Determines whether an element or attribute specified by the XPath selector exists in the document.
        /// </summary>
        /// <param name="baseNode">XML node to start searching at.</param>
        /// <param name="xPath">XPath selector to check.</param>
        /// <returns>true if an element or attribute exists, false otherwise.</returns>
        public bool Exists(XmlNode baseNode, string xPath)
        {
            return baseNode.SelectSingleNode(xPath) != null;
        }

        /// <summary>
        /// Reads a string value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Text contents or attribute value of the selected node.</returns>
        public string ReadString(string xPath)
        {
            return this.ReadStringInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads a string value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Text contents or attribute value of the selected node.</returns>
        public string ReadString(string xPath, string defaultValue)
        {
            return this.ReadStringInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a string value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Text contents or attribute value of the selected node.</returns>
        public string ReadString(XmlNode baseNode, string xPath)
        {
            return this.ReadStringInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads a string value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Text contents or attribute value of the selected node.</returns>
        public string ReadString(XmlNode baseNode, string xPath, string defaultValue)
        {
            return this.ReadStringInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads an int value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Int value of the selected node.</returns>
        public int ReadInt(string xPath)
        {
            return (int)this.ReadIntInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads an int value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Int value of the selected node.</returns>
        public int ReadInt(string xPath, int defaultValue)
        {
            return (int)this.ReadIntInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads an int value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Int value of the selected node.</returns>
        public int? ReadInt(string xPath, int? defaultValue)
        {
            return this.ReadIntInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads an int value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Int value of the selected node.</returns>
        public int ReadInt(XmlNode baseNode, string xPath)
        {
            return (int)this.ReadIntInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads an int value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Int value of the selected node.</returns>
        public int ReadInt(XmlNode baseNode, string xPath, int defaultValue)
        {
            return (int)this.ReadIntInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads an int value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Int value of the selected node.</returns>
        public int? ReadInt(XmlNode baseNode, string xPath, int? defaultValue)
        {
            return this.ReadIntInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a decimal value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Decimal value of the selected node.</returns>
        public decimal ReadDecimal(string xPath)
        {
            return (decimal)this.ReadDecimalInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads a decimal value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Decimal value of the selected node.</returns>
        public decimal ReadDecimal(string xPath, decimal defaultValue)
        {
            return (decimal)this.ReadDecimalInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a decimal value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Decimal value of the selected node.</returns>
        public decimal? ReadDecimal(string xPath, decimal? defaultValue)
        {
            return this.ReadDecimalInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a decimal value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Decimal value of the selected node.</returns>
        public decimal ReadDecimal(XmlNode baseNode, string xPath)
        {
            return (decimal)this.ReadDecimalInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads a decimal value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Decimal value of the selected node.</returns>
        public decimal ReadDecimal(XmlNode baseNode, string xPath, decimal defaultValue)
        {
            return (decimal)this.ReadDecimalInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a decimal value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Decimal value of the selected node.</returns>
        public decimal? ReadDecimal(XmlNode baseNode, string xPath, decimal? defaultValue)
        {
            return this.ReadDecimalInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a bool value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Bool value of the selected node.</returns>
        public bool ReadBool(string xPath)
        {
            return (bool)this.ReadBoolInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads a bool value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Bool value of the selected node.</returns>
        public bool ReadBool(string xPath, bool defaultValue)
        {
            return (bool)this.ReadBoolInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a bool value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Bool value of the selected node.</returns>
        public bool? ReadBool(string xPath, bool? defaultValue)
        {
            return this.ReadBoolInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a bool value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Bool value of the selected node.</returns>
        public bool ReadBool(XmlNode baseNode, string xPath)
        {
            return (bool)this.ReadBoolInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads a bool value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Bool value of the selected node.</returns>
        public bool ReadBool(XmlNode baseNode, string xPath, bool defaultValue)
        {
            return (bool)this.ReadBoolInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a bool value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Bool value of the selected node.</returns>
        public bool? ReadBool(XmlNode baseNode, string xPath, bool? defaultValue)
        {
            return this.ReadBoolInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a DateTime value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>DateTime value of the selected node.</returns>
        public DateTime ReadDateTime(string xPath)
        {
            return (DateTime)this.ReadDateTimeInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads a DateTime value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>DateTime value of the selected node.</returns>
        public DateTime ReadDateTime(string xPath, DateTime defaultValue)
        {
            return (DateTime)this.ReadDateTimeInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a DateTime value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>DateTime value of the selected node.</returns>
        public DateTime? ReadDateTime(string xPath, DateTime? defaultValue)
        {
            return this.ReadDateTimeInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a DateTime value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>DateTime value of the selected node.</returns>
        public DateTime ReadDateTime(XmlNode baseNode, string xPath)
        {
            return (DateTime)this.ReadDateTimeInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads a DateTime value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>DateTime value of the selected node.</returns>
        public DateTime ReadDateTime(XmlNode baseNode, string xPath, DateTime defaultValue)
        {
            return (DateTime)this.ReadDateTimeInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a DateTime value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>DateTime value of the selected node.</returns>
        public DateTime? ReadDateTime(XmlNode baseNode, string xPath, DateTime? defaultValue)
        {
            return this.ReadDateTimeInternal(baseNode, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a TimeSpan value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>TimeSpan value of the selected node.</returns>
        public TimeSpan ReadTimeSpan(string xPath)
        {
            return (TimeSpan)this.ReadTimeSpanInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads a TimeSpan value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>TimeSpan value of the selected node.</returns>
        public TimeSpan ReadTimeSpan(string xPath, TimeSpan defaultValue)
        {
            return (TimeSpan)this.ReadTimeSpanInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a TimeSpan value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>TimeSpan value of the selected node.</returns>
        public TimeSpan? ReadTimeSpan(string xPath, TimeSpan? defaultValue)
        {
            return this.ReadTimeSpanInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a TimeSpan value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>TimeSpan value of the selected node.</returns>
        public TimeSpan ReadTimeSpan(XmlNode baseNode, string xPath)
        {
            return (TimeSpan)this.ReadTimeSpanInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads a TimeSpan value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>TimeSpan value of the selected node.</returns>
        public TimeSpan ReadTimeSpan(XmlNode baseNode, string xPath, TimeSpan defaultValue)
        {
            return (TimeSpan)this.ReadTimeSpanInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a TimeSpan value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>TimeSpan value of the selected node.</returns>
        public TimeSpan? ReadTimeSpan(XmlNode baseNode, string xPath, TimeSpan? defaultValue)
        {
            return this.ReadTimeSpanInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a Color value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Color value of the selected node.</returns>
        public Color ReadColor(string xPath)
        {
            return (Color)this.ReadColorInternal(this.xd, xPath, true, null);
        }

        /// <summary>
        /// Reads a Color value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Color value of the selected node.</returns>
        public Color ReadColor(string xPath, Color defaultValue)
        {
            return (Color)this.ReadColorInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a Color value from a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Color value of the selected node.</returns>
        public Color? ReadColor(string xPath, Color? defaultValue)
        {
            return this.ReadColorInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a Color value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>Color value of the selected node.</returns>
        public Color ReadColor(XmlNode baseNode, string xPath)
        {
            return (Color)this.ReadColorInternal(baseNode, xPath, true, null);
        }

        /// <summary>
        /// Reads a Color value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Color value of the selected node.</returns>
        public Color ReadColor(XmlNode baseNode, string xPath, Color defaultValue)
        {
            return (Color)this.ReadColorInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Reads a Color value from a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="defaultValue">Value to be returned if the node does not exist.</param>
        /// <returns>Color value of the selected node.</returns>
        public Color? ReadColor(XmlNode baseNode, string xPath, Color? defaultValue)
        {
            return this.ReadColorInternal(this.xd, xPath, false, defaultValue);
        }

        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>The created or existing node.</returns>
        public XmlNode Create(string xPath)
        {
            return this.Create(this.xd, xPath, false);
        }

        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>The created or existing node.</returns>
        public XmlNode Create(XmlNode baseNode, string xPath)
        {
            return this.Create(baseNode, xPath, false);
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>The created node.</returns>
        public XmlNode CreateNew(string xPath)
        {
            return this.Create(this.xd, xPath, true);
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <returns>The created node.</returns>
        public XmlNode CreateNew(XmlNode baseNode, string xPath)
        {
            return this.Create(baseNode, xPath, true);
        }

        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="newNode">true to always create a new node, false to return an existing node.</param>
        /// <returns>The created or existing node.</returns>
        public XmlNode Create(XmlNode baseNode, string xPath, bool newNode)
        {
            if (Regex.IsMatch(xPath, "^(/?[-._a-z0-9]+)(/[-._a-z0-9]+)*(/@[-._a-z0-9]+)?$", RegexOptions.IgnoreCase) == false)
            {
                if (!Regex.IsMatch(xPath, "^@[-._a-z0-9]+$", RegexOptions.IgnoreCase))
                {
                    throw new NotSupportedException("Unsupported XPath expression for creating an XML node.");
                }
            }

            string[] elements = xPath.Split('/');

            if (newNode && elements[elements.Length - 1][0] == '@')
            {
                throw new NotSupportedException("You cannot create a new node with an XPath expression for an XML attribute.");
            }

            XmlNode node = baseNode;
            for (int i = 0; i < elements.Length; i++)
            {
                string element = elements[i];
                if (element.Length == 0)
                {
                    continue;
                }

                XmlNode subNode = node.SelectSingleNode(element);
                bool skip = newNode && i == elements.Length - 1;   /* Skip existing to create a new element */
                if (subNode != null && !skip)
                {
                    node = subNode;
                }
                else
                {
                    if (element[0] == '@')
                    {
                        string attribute = element.Substring(1);
                        subNode = this.xd.CreateAttribute(attribute);
                        node.Attributes.Append((XmlAttribute)subNode);
                        node = subNode;
                    }
                    else
                    {
                        subNode = this.xd.CreateElement(element);
                        node.AppendChild(subNode);
                        node = subNode;
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Writes an int value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The int value to write.</param>
        public void Write(string xPath, int? value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes an int value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The int value to write.</param>
        public void Write(XmlNode baseNode, string xPath, int? value)
        {
            if (value != null)
            {
                this.Write(baseNode, xPath, ((int)value).ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                this.Write(baseNode, xPath, string.Empty);
            }
        }

        /// <summary>
        /// Writes a decimal value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The decimal value to write.</param>
        public void Write(string xPath, decimal? value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes a decimal value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The decimal value to write.</param>
        public void Write(XmlNode baseNode, string xPath, decimal? value)
        {
            if (value != null)
            {
                this.Write(baseNode, xPath, ((decimal)value).ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                this.Write(baseNode, xPath, string.Empty);
            }
        }

        /// <summary>
        /// Writes a bool value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The bool value to write.</param>
        public void Write(string xPath, bool? value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes a bool value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The bool value to write.</param>
        public void Write(XmlNode baseNode, string xPath, bool? value)
        {
            if (value != null)
            {
                this.Write(baseNode, xPath, value == true ? "true" : "false");
            }
            else
            {
                this.Write(baseNode, xPath, string.Empty);
            }
        }

        /// <summary>
        /// Writes a DateTime value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The DateTime value to write.</param>
        public void Write(string xPath, DateTime? value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes a DateTime value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The DateTime value to write.</param>
        public void Write(XmlNode baseNode, string xPath, DateTime? value)
        {
            if (value != null)
            {
                this.Write(baseNode, xPath, ((DateTime)value).ToString("s"));
            }
            else
            {
                this.Write(baseNode, xPath, string.Empty);
            }
        }

        /// <summary>
        /// Writes a TimeSpan value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The TimeSpan value to write.</param>
        public void Write(string xPath, TimeSpan? value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes a TimeSpan value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The TimeSpan value to write.</param>
        public void Write(XmlNode baseNode, string xPath, TimeSpan? value)
        {
            if (value != null)
            {
                TimeSpan t = (TimeSpan)value;
                string s = "P";
                if (t.Days > 0)
                {
                    s += t.Days + "D";
                }

                if (t.Hours > 0 || t.Minutes > 0 || t.Seconds > 0)
                {
                    s += "T";
                }

                if (t.Hours > 0)
                {
                    s += t.Hours + "H";
                }

                if (t.Minutes > 0)
                {
                    s += t.Minutes + "M";
                }

                if (t.Seconds > 0)
                {
                    s += t.Seconds + "S";
                }

                if (s == "P")
                {
                    s = "PT0S";
                }

                this.Write(baseNode, xPath, s);
            }
            else
            {
                this.Write(baseNode, xPath, string.Empty);
            }
        }

        /// <summary>
        /// Writes a Color value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The Color value to write.</param>
        public void Write(string xPath, Color? value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes a Color value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The Color value to write.</param>
        public void Write(XmlNode baseNode, string xPath, Color? value)
        {
            if (value != null)
            {
                Color c = (Color)value;
                this.Write(baseNode, xPath, "#" + c.R.ToString("x2") + c.G.ToString("x2") + c.B.ToString("x2"));
            }
            else
            {
                this.Write(baseNode, xPath, string.Empty);
            }
        }

        /// <summary>
        /// Writes a string value to a node.
        /// </summary>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The string value to write.</param>
        public void Write(string xPath, string value)
        {
            this.Write(this.xd, xPath, value);
        }

        /// <summary>
        /// Writes a string value to a node.
        /// </summary>
        /// <param name="baseNode">Base node for the XPath selector.</param>
        /// <param name="xPath">XPath selector for the node.</param>
        /// <param name="value">The string value to write.</param>
        public void Write(XmlNode baseNode, string xPath, string value)
        {
            XmlNode node = baseNode.SelectSingleNode(xPath);
            if (node == null)
            {
                node = this.Create(baseNode, xPath);
            }

            XmlElement element = node as XmlElement;
            if (element != null)
            {
                element.InnerText = value;
                return;
            }

            XmlAttribute attribute = node as XmlAttribute;
            if (attribute != null)
            {
                attribute.Value = value;
                return;
            }

            throw new NotSupportedException("Unsupported node type selected.");
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                    /*
                    Managed Objekte freigeben. Wenn diese Obbjekte selbst
                    IDisposable implementieren, dann deren Dispose()
                    aufrufen
                    */
                }

                /*
                 * Hier unmanaged Objekte freigeben (z.B. IntPtr)
                */
            }

            this.classIsDisposed = true;
        }

        #region Private Methodes

        private string ReadStringInternal(XmlNode baseNode, string xPath, bool isRequired, string defaultValue)
        {
            XmlNode node = baseNode.SelectSingleNode(xPath);
            XmlElement element = node as XmlElement;
            if (element != null)
            {
                return element.InnerText;
            }

            XmlAttribute attribute = node as XmlAttribute;
            if (attribute != null)
            {
                return attribute.Value;
            }

            if (isRequired == false)
            {
                return defaultValue;
            }

            if (node != null)
            {
                throw new NotSupportedException("Unsupported node type selected.");
            }
            else
            {
                throw new DynamicXmlNodeNotFoundException(xPath);
            }
        }

        private int? ReadIntInternal(XmlNode baseNode, string xPath, bool isRequired, int? defaultValue)
        {
            string s = this.ReadStringInternal(baseNode, xPath, isRequired, null);
            if (string.IsNullOrWhiteSpace(s) && !isRequired)
            {
                return defaultValue;
            }

            return int.Parse(s, CultureInfo.InvariantCulture);
        }

        private decimal? ReadDecimalInternal(XmlNode baseNode, string xPath, bool isRequired, decimal? defaultValue)
        {
            string s = this.ReadStringInternal(baseNode, xPath, isRequired, null);
            if (string.IsNullOrWhiteSpace(s) && !isRequired)
            {
                return defaultValue;
            }

            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }

        private bool? ReadBoolInternal(XmlNode baseNode, string xPath, bool isRequired, bool? defaultValue)
        {
            string s = this.ReadStringInternal(baseNode, xPath, isRequired, null);
            if (string.IsNullOrWhiteSpace(s) && !isRequired)
            {
                return defaultValue;
            }

            if (!string.IsNullOrWhiteSpace(s))
            {
                string[] trueWords = new string[] {
                    "1",
                    "true", "yes", "on",
                    "wahr", "ja",
                    "verdadero", "si",
                    "tosi", "ei",
                    "vrai", "oui",
                    "igaz",
                    "rétt",
                    "vero", "si",
                    "waar", "ja",
                    "sant", "ja",
                    "prawda", "tak",
                    "verdadeiro", "sim",
                    "истина", "да",
                    "áno",
                    "sant", "ja",
                    "đúng", "có",
                    "是",
                };

                string[] falseWords = new string[] {
                    "0",
                    "false", "no", "off",
                    "falsch", "nein",
                    "falso", "no",
                    "epätosi", "kyllä",
                    "faux", "non",
                    "hamis",
                    "rangt",
                    "falso", "no",
                    "onwaar", "nee",
                    "usant", "nei",
                    "fałsz", "nie",
                    "falso", "nao", "não",
                    "ложь", "нет",
                    "nie",
                    "falskt", "nej",
                    "sai", "không",
                    "否",
                };

                s = s.Trim();
                foreach (var word in trueWords)
                {
                    if (s.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }

                foreach (var word in falseWords)
                {
                    if (s.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            throw new FormatException();
        }

        private DateTime? ReadDateTimeInternal(XmlNode baseNode, string xPath, bool isRequired, DateTime? defaultValue)
        {
            string s = this.ReadStringInternal(baseNode, xPath, isRequired, null);
            if (string.IsNullOrWhiteSpace(s) && !isRequired)
            {
                return defaultValue;
            }

            return DateTime.Parse(s, CultureInfo.InvariantCulture);
        }

        private TimeSpan? ReadTimeSpanInternal(XmlNode baseNode, string xPath, bool isRequired, TimeSpan? defaultValue)
        {
            string s = this.ReadStringInternal(baseNode, xPath, isRequired, null);
            if (string.IsNullOrWhiteSpace(s) && !isRequired)
            {
                return defaultValue;
            }

            Match m = Regex.Match(s, "^P((?<D>[0-9]+)D)?(T((?<H>[0-9]+)H)?((?<M>[0-9]+)M)?((?<S>[0-9]+)S)?)?$");
            if (m.Success)
            {
                int days = string.IsNullOrEmpty(m.Groups["D"].Value) ? 0 : int.Parse(m.Groups["D"].Value);
                int hours = string.IsNullOrEmpty(m.Groups["H"].Value) ? 0 : int.Parse(m.Groups["H"].Value);
                int mins = string.IsNullOrEmpty(m.Groups["M"].Value) ? 0 : int.Parse(m.Groups["M"].Value);
                int secs = string.IsNullOrEmpty(m.Groups["S"].Value) ? 0 : int.Parse(m.Groups["S"].Value);

                return TimeSpan.FromSeconds((days * 86400) + (hours * 3600) + (mins * 60) + secs);
            }

            throw new FormatException();
        }

        private Color? ReadColorInternal(XmlNode baseNode, string xPath, bool isRequired, Color? defaultValue)
        {
            string s = this.ReadStringInternal(baseNode, xPath, isRequired, null);
            if (string.IsNullOrWhiteSpace(s) && !isRequired)
            {
                return defaultValue;
            }

            Match m = Regex.Match(s, "^#([0-9a-f]{2})([0-9a-f]{2})([0-9a-f]{2})$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (m.Success)
            {
                int r = int.Parse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                int g = int.Parse(m.Groups[2].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                int b = int.Parse(m.Groups[3].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                return Color.FromArgb(r, g, b);
            }

            throw new FormatException();
        }

        #endregion Private Methodes
    }
}