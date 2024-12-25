//-----------------------------------------------------------------------
// <copyright file="CultureInfoExtension.cs" company="Lifeprojects.de">
//     Class: CultureInfoExtension
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.07.2017</date>
//
// <summary>Extensions Class for CultureInfo Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Globalization;

    public static class CultureInfoExtension
    {
        public static CultureInfo GetRootCulture(this CultureInfo cultureInfo)
        {
            while (cultureInfo.Parent.LCID != CultureInfo.InvariantCulture.LCID)
            {
                cultureInfo = cultureInfo.Parent;
            }

            return cultureInfo;
        }

        public static string CurrencySymbol(this CultureInfo cultureInfo)
        {
            return cultureInfo.NumberFormat.CurrencySymbol;
        }

        public static string ShortDatePattern(this CultureInfo cultureInfo)
        {
            return cultureInfo.DateTimeFormat.ShortDatePattern;
        }
    }
}