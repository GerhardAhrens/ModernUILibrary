
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Globalization;

    internal class SvgLength
    {
        public readonly double Value;
        public readonly string Unit;

        public SvgLength(double value)
        {
            Value = value;
            Unit = null;
        }

        public SvgLength(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        public static SvgLength Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            value = value.Trim();
            if (value == "")
                throw new ArgumentException("value must not be empty", "value");

            if (value == "inherit")
                return new SvgLength(Double.NaN, null);

            string unit = null;

            foreach (string unit_identifier in new string[] { "in", "cm", "mm", "pt", "pc", "px", "%" })
                if (value.EndsWith(unit_identifier))
                {
                    unit = unit_identifier;
                    value = value.Substring(0, value.Length - unit_identifier.Length).Trim();
                    break;
                }

            return new SvgLength(Double.Parse(value, CultureInfo.InvariantCulture.NumberFormat), unit);
        }

        public double ToDouble()
        {
            return Value;
        }
    }
}
