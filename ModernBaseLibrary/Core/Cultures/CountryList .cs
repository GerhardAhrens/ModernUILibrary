//-----------------------------------------------------------------------
// <copyright file="CountryList.cs" company="Lifeprojects.de">
//     Class: CountryList
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.05.2023 15:30:31</date>
//
// <summary>
// Die Klasse gibt eine Liste von CountryInfo zurück
// </summary>
// <Website>
// https://stackoverflow.com/questions/49310719/get-iso-country-code-from-country-name
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CountryList
    {
        private CultureTypes cultureType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryList"/> class.
        /// </summary>
        public CountryList(bool AllCultures)
        {
            cultureType = AllCultures ? CultureTypes.AllCultures : CultureTypes.SpecificCultures;
            this.Countries = GetAllCountries(cultureType);
        }

        public List<CountryInfo> Countries { get; set; }

        public List<CountryInfo> GetCountryInfoByName(string CountryName, bool NativeName)
        {
            return NativeName ? Countries.Where(info => info.Region?.NativeName == CountryName).ToList()
                              : Countries.Where(info => info.Region?.EnglishName == CountryName).ToList();
        }

        public List<CountryInfo> GetCountryInfoByName(string CountryName, bool NativeName, bool IsNeutral)
        {
            return NativeName ? Countries.Where(info => info.Region?.NativeName == CountryName &&
                                                        info.Culture?.IsNeutralCulture == IsNeutral).ToList()
                              : Countries.Where(info => info.Region?.EnglishName == CountryName &&
                                                        info.Culture?.IsNeutralCulture == IsNeutral).ToList();
        }

        public string GetTwoLettersName(string CountryName, bool NativeName)
        {
            CountryInfo country = NativeName ? Countries.Where(info => info.Region?.NativeName == CountryName).FirstOrDefault()
                                              : Countries.Where(info => info.Region?.EnglishName == CountryName).FirstOrDefault();

            return country?.Region?.TwoLetterISORegionName;
        }

        public string GetThreeLettersName(string CountryName, bool NativeName)
        {
            CountryInfo country = NativeName ? Countries.Where(info => info.Region?.NativeName == CountryName).FirstOrDefault()
                                              : Countries.Where(info => info.Region?.EnglishName == CountryName).FirstOrDefault();

            return country?.Region?.ThreeLetterISORegionName;
        }

        public List<string> GetIetfLanguageTag(string CountryName, bool UseNativeName)
        {
            return UseNativeName ? Countries.Where(info => info.Region?.NativeName == CountryName)
                                            .Select(info => info.Culture?.IetfLanguageTag).ToList()
                                 : Countries.Where(info => info.Region?.EnglishName == CountryName)
                                            .Select(info => info.Culture?.IetfLanguageTag).ToList();
        }

        public List<int?> GetRegionGeoId(string CountryName, bool UseNativeName)
        {
            return UseNativeName ? Countries.Where(info => info.Region?.NativeName == CountryName)
                                            .Select(info => info.Region?.GeoId).ToList()
                                 : Countries.Where(info => info.Region?.EnglishName == CountryName)
                                            .Select(info => info.Region?.GeoId).ToList();
        }

        private static List<CountryInfo> GetAllCountries(CultureTypes cultureTypes)
        {
            List<CountryInfo> countries = new List<CountryInfo>();

            foreach (CultureInfo culture in CultureInfo.GetCultures(cultureTypes))
            {
                if (culture.LCID != 127)
                {
                    countries.Add(new CountryInfo()
                    {
                        Culture = culture,
                        Region = new RegionInfo(culture.TextInfo.CultureName)
                    });
                }
            }

            return countries;
        }
    }

    public class CountryInfo
    {
        public CultureInfo Culture { get; set; }

        public RegionInfo Region { get; set; }
    }
}
