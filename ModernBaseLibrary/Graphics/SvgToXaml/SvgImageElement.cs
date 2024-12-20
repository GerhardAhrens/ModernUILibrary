
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents an &lt;image&gt; element.
    /// </summary>
    internal class SvgImageElement : SvgDrawableBaseElement
    {
        public readonly SvgCoordinate Y = new SvgCoordinate(0.0);
        public readonly SvgCoordinate X = new SvgCoordinate(0.0);
        public readonly SvgLength Width = new SvgLength(0.0);
        public readonly SvgLength Height = new SvgLength(0.0);

        public readonly string DataType = null;
        public readonly byte[] Data = null;

        public SvgImageElement(SvgDocument document, SvgBaseElement parent, XElement imageElement)
          : base(document, parent, imageElement)
        {
            XAttribute x_attribute = imageElement.Attribute("x");
            if (x_attribute != null)
                X = SvgCoordinate.Parse(x_attribute.Value);

            XAttribute y_attribute = imageElement.Attribute("y");
            if (y_attribute != null)
                Y = SvgCoordinate.Parse(y_attribute.Value);

            XAttribute width_attribute = imageElement.Attribute("width");
            if (width_attribute != null)
                Width = SvgLength.Parse(width_attribute.Value);

            XAttribute height_attribute = imageElement.Attribute("height");
            if (height_attribute != null)
                Height = SvgLength.Parse(height_attribute.Value);

            XAttribute href_attribute = imageElement.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"));
            if (href_attribute != null)
            {
                string reference = href_attribute.Value.TrimStart();
                if (reference.StartsWith("data:"))
                {
                    reference = reference.Substring(5).TrimStart();
                    int index = reference.IndexOf(";");
                    if (index > -1)
                    {
                        string type = reference.Substring(0, index).Trim();
                        reference = reference.Substring(index + 1);

                        index = reference.IndexOf(",");
                        string encoding = reference.Substring(0, index).Trim();
                        reference = reference.Substring(index + 1).TrimStart();

                        switch (encoding)
                        {
                            case "base64":
                                Data = Convert.FromBase64String(reference);
                                break;

                            default:
                                throw new NotSupportedException(String.Format("Unsupported encoding: {0}", encoding));
                        }

                        string[] type_tokens = type.Split('/');
                        if (type_tokens.Length != 2)
                            throw new NotSupportedException(String.Format("Unsupported type: {0}", type));

                        type_tokens[0] = type_tokens[0].Trim();
                        if (type_tokens[0] != "image")
                            throw new NotSupportedException(String.Format("Unsupported type: {0}", type));

                        switch (type_tokens[1].Trim())
                        {
                            case "jpeg":
                                DataType = "jpeg";
                                break;

                            case "png":
                                DataType = "png";
                                break;

                            default:
                                throw new NotSupportedException(String.Format("Unsupported type: {0}", type));
                        }
                    }
                }
            }
        }

        public override Drawing GetBaseDrawing()
        {
            if (Data == null)
                return null;

            string temp_file = Path.GetTempFileName();
            using (FileStream file_stream = new FileStream(temp_file, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(file_stream))
                writer.Write(Data);

            return new ImageDrawing(new BitmapImage(new Uri(temp_file)), new Rect(
              new Point(X.ToDouble(), Y.ToDouble()),
              new Size(Width.ToDouble(), Height.ToDouble())
              ));
        }

        public override Geometry GetBaseGeometry()
        {
            return new RectangleGeometry(new Rect(
              new Point(X.ToDouble(), Y.ToDouble()),
              new Size(Width.ToDouble(), Height.ToDouble())
              ));
        }
    }
}
