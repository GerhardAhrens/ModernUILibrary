//-----------------------------------------------------------------------
// <copyright file="GuidExtensions.cs" company="Lifeprojects.de">
//     Class: GuidExtensions
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.07.2017</date>
//
// <summary>Extensions Class for Guid Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    [SupportedOSPlatform("windows")]
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid @this)
        {
            return (@this == Guid.Empty);
        }

        public static bool IsNullOrEmpty(this Guid? @this)
        {
            return (@this == null || @this == Guid.Empty);
        }

        public static bool IsNotEmpty(this Guid @this)
        {
            return @this != Guid.Empty;
        }

        public static bool IsNotEmpty(this Guid? @this)
        {
            return @this != null && @this != Guid.Empty;
        }

        public static bool In(this Guid @this, params Guid[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool NotIn(this Guid @this, params Guid[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        public static Guid ToGuid(this object @this)
        {
            if (@this == null)
            {
                return Guid.Empty;
            }

            string pattern = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            Regex r = new Regex(pattern);
            if (r.IsMatch(@this.ToString()))
            {
                return new Guid(@this.ToString());
            }
            else
            {
                return Guid.Empty;
            }
        }

        public static Guid ToGuid(this string @this)
        {
            if (@this == null)
            {
                return Guid.Empty;
            }

            if (@this.Contains("\"") == true)
            {
                @this = @this.Replace("\"", string.Empty);
            }

            string pattern = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            Regex r = new Regex(pattern);
            if (r.IsMatch(@this.ToString()))
            {
                return new Guid(@this.ToString());
            }
            else
            {
                return Guid.Empty;
            }
        }

        public static bool IsGuidEmpty(this Guid @this)
        {
            string pattern = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            Regex r = new Regex(pattern);
            if (r.IsMatch(@this.ToString()))
            {
                if (new Guid(@this.ToString()) == Guid.Empty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsGuidEmpty(this string @this)
        {
            string pattern = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            Regex r = new Regex(pattern);
            if (r.IsMatch(@this))
            {
                if (new Guid(@this) == Guid.Empty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsGuid(this string @this)
        {
            string pattern = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            Regex r = new Regex(pattern);
            if (r.IsMatch(@this))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Guid EmptyGuid(this Guid @this)
        {
            return new Guid("00000000-0000-0000-0000-000000000000");
        }

        /// <summary>
        /// Konvertiert eine Guid zum Typ DateTime
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Guid guid)
        {
            byte[] bytes;
            DateTime result = DateTime.Now.DefaultDate();

            try
            {
                bytes = guid.ToByteArray();
                Array.Resize(ref bytes, 8);
                result = new DateTime(BitConverter.ToInt64(bytes, 0));
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException($"Die übergebene Guid '{guid.ToString()}' kann nicht zu einem gültigem DateTime konvertiert werden.",ex);
            }

            return result;
        }
    }
}