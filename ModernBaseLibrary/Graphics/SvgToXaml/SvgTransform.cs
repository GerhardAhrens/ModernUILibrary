
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    internal abstract class SvgTransform
    {
        public abstract Transform ToTransform();

        public static SvgTransform Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            value = value.Trim();
            if (value == "")
                throw new ArgumentException("value must not be empty", "value");

            List<SvgTransform> transforms = new List<SvgTransform>();

            string transform = value;
            while (transform.Length > 0)
            {

                if (transform.StartsWith("translate"))
                {
                    transform = transform.Substring(9).TrimStart();
                    if (transform.StartsWith("("))
                    {
                        transform = transform.Substring(1);
                        int index = transform.IndexOf(")");
                        if (index >= 0)
                        {
                            transforms.Add(SvgTranslateTransform.Parse(transform.Substring(0, index).Trim()));
                            transform = transform.Substring(index + 1).TrimStart();
                            continue;
                        }
                    }
                }

                if (transform.StartsWith("matrix"))
                {
                    transform = transform.Substring(6).TrimStart();
                    if (transform.StartsWith("("))
                    {
                        transform = transform.Substring(1);
                        int index = transform.IndexOf(")");
                        if (index >= 0)
                        {
                            transforms.Add(SvgMatrixTransform.Parse(transform.Substring(0, index).Trim()));
                            transform = transform.Substring(index + 1).TrimStart();
                            continue;
                        }
                    }
                }

                if (transform.StartsWith("scale"))
                {
                    transform = transform.Substring(5).TrimStart();
                    if (transform.StartsWith("("))
                    {
                        transform = transform.Substring(1);
                        int index = transform.IndexOf(")");
                        if (index >= 0)
                        {
                            transforms.Add(SvgScaleTransform.Parse(transform.Substring(0, index).Trim()));
                            transform = transform.Substring(index + 1).TrimStart();
                            continue;
                        }
                    }
                }

                if (transform.StartsWith("skew"))
                {
                    transform = transform.Substring(5).TrimStart();
                    if (transform.StartsWith("("))
                    {
                        transform = transform.Substring(1);
                        int index = transform.IndexOf(")");
                        if (index >= 0)
                        {
                            transforms.Add(SvgSkewTransform.Parse(transform.Substring(0, index).Trim()));
                            transform = transform.Substring(index + 1).TrimStart();
                            continue;
                        }
                    }
                }

                if (transform.StartsWith("rotate"))
                {
                    transform = transform.Substring(6).TrimStart();
                    if (transform.StartsWith("("))
                    {
                        transform = transform.Substring(1);
                        int index = transform.IndexOf(")");
                        if (index >= 0)
                        {
                            transforms.Add(SvgScaleTransform.Parse(transform.Substring(0, index).Trim()));
                            transform = transform.Substring(index + 1).TrimStart();
                            continue;
                        }
                    }
                }

            }

            if (transforms.Count == 1)
                return transforms[0];
            else if (transforms.Count > 1)
                return new SvgTransformGroup(transforms.ToArray());

            throw new ArgumentException(String.Format("Unsupported transform value: {0}", value));
        }
    }
}
