//-----------------------------------------------------------------------
// <copyright file="StringMaskExtensions.cs" company="Lifeprojects.de">
//     Class: StringMaskExtensions
//     Copyright © PTA GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.07.2019</date>
//
// <summary>Extensions Class for String Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Runtime.Versioning;
    using System.Text;

    public enum MaskStyle
    {
        All,
        AlphaNumericOnly,
    }

    [SupportedOSPlatform("Windows")]
    public static class StringMaskExtensions
    {
        public static readonly char DefaultMaskCharacter = '*';

        public static string FormatMask(this string @this, string mask)
        {
            if (@this.IsNullOrEmpty() == true)
            {
                return @this;
            }

            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < @this.Length)
                    {
                        output += @this[index];
                        index++;
                    }
                }
                else
                {
                    output += m;
                }
            }

            return output;
        }

        public static bool IsLengthAtLeast(this string @this, int length)
        {
            length.IsArgumentOutOfRange(nameof(@this), length > 0, "The length must be a non - negative number.");

            return @this != null ? @this.Length >= length : false;
        }

        public static string Mask(this string @this, char maskChar, int numExposed, MaskStyle style)
        {
            var maskedString = @this;

            if (@this.IsLengthAtLeast(numExposed))
            {
                var builder = new StringBuilder(@this.Length);
                int index = maskedString.Length - numExposed;

                if (style == MaskStyle.AlphaNumericOnly)
                {
                    CreateAlphaNumMask(builder, @this, maskChar, index);
                }
                else
                {
                    builder.Append(maskChar, index);
                }

                builder.Append(@this.Substring(index));
                maskedString = builder.ToString();
            }

            return maskedString;
        }

        public static string Mask(this string sourceValue, char maskChar, int numExposed)
        {
            return Mask(sourceValue, maskChar, numExposed, MaskStyle.All);
        }

        public static string Mask(this string sourceValue, char maskChar)
        {
            return Mask(sourceValue, maskChar, 0, MaskStyle.All);
        }

        public static string Mask(this string sourceValue, int numExposed)
        {
            return Mask(sourceValue, DefaultMaskCharacter, numExposed, MaskStyle.All);
        }

        public static string Mask(this string sourceValue)
        {
            return Mask(sourceValue, DefaultMaskCharacter, 0, MaskStyle.All);
        }

        public static string Mask(this string sourceValue, char maskChar, MaskStyle style)
        {
            return Mask(sourceValue, maskChar, 0, style);
        }

        public static string Mask(this string sourceValue, int numExposed, MaskStyle style)
        {
            return Mask(sourceValue, DefaultMaskCharacter, numExposed, style);
        }

        public static string Mask(this string sourceValue, MaskStyle style)
        {
            return Mask(sourceValue, DefaultMaskCharacter, 0, style);
        }

        private static void CreateAlphaNumMask(StringBuilder buffer, string source, char mask, int length)
        {
            for (int i = 0; i < length; i++)
            {
                buffer.Append(char.IsLetterOrDigit(source[i]) ? mask : source[i]);
            }
        }
    }
}