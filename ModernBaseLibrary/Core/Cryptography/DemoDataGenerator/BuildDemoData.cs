/*
 * <copyright file="TestDataGenerator.cs" company="Lifeprojects.de">
 *     Class: TestDataGenerator
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.01.2025 12:03:44</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace DemoDataGeneratorLib.Base
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.Json;
    using System.Windows.Controls;

    using ModernBaseLibrary.Graphics;

    public static class BuildDemoData<Tin>
    {
        static BuildDemoData()
        {
        }

        public static Func<Tin,int,Tin> ConfigObject { get; private set; }
        public static Func<Tin,object, Tin> ConfigObjectDict { get; private set; }

        public static List<Tin> CreateForList<Tín>(Func<Tin,int,Tin> method, int count = 1000)
        {
            List<Tin> testDataSource = null;
            Type type = typeof(Tin);
            object result = null;
            if (method != null)
            {
                testDataSource = new List<Tin>();
                ConfigObject = method;
                for (int i = 0; i < count; i++)
                {
                    if (typeof(Tin) ==  typeof(string))
                    {
                        object obj = null;
                        result = ConfigObject((Tin)obj,i);
                        testDataSource.Add((Tin)result);
                    }
                    else
                    {
                        object obj = (Tin)Activator.CreateInstance(typeof(Tin));
                        result = ConfigObject((Tin)obj,i);
                        testDataSource.Add((Tin)result);
                    }
                }
            }

            return testDataSource;
        }

        public static ObservableCollection<Tin> CreateForObservableCollection<Tín>(Func<Tin,int, Tin> method, int count = 1000)
        {
            ObservableCollection<Tin> testDataSource = null;
            Type type = typeof(Tin);
            object result = null;
            if (method != null)
            {
                testDataSource = new ObservableCollection<Tin>();
                ConfigObject = method;
                for (int i = 0; i < count; i++)
                {
                    if (typeof(Tin) == typeof(string))
                    {
                        object obj = null;
                        result = ConfigObject((Tin)obj, i);
                        testDataSource.Add((Tin)result);
                    }
                    else
                    {
                        object obj = (Tin)Activator.CreateInstance(typeof(Tin));
                        result = ConfigObject((Tin)obj,i);
                        testDataSource.Add((Tin)result);
                    }
                }
            }

            return testDataSource;
        }

        public static IEnumerable<Tin> CreateForIEnumerable<Tín>(Func<Tin, int, Tin> method, int count = 1000)
        {
            List<Tin> testDataSource = null;
            Type type = typeof(Tin);
            object result = null;
            if (method != null)
            {
                testDataSource = new List<Tin>();
                ConfigObject = method;
                for (int i = 0; i < count; i++)
                {
                    if (typeof(Tin) == typeof(string))
                    {
                        object obj = null;
                        result = ConfigObject((Tin)obj, i);
                        testDataSource.Add((Tin)result);
                    }
                    else
                    {
                        object obj = (Tin)Activator.CreateInstance(typeof(Tin));
                        result = ConfigObject((Tin)obj, i);
                        testDataSource.Add((Tin)result);
                    }
                }
            }

            return testDataSource.AsEnumerable<Tin>();
        }

        public static DataTable CreateForDataTable<Tín>(Func<Tin, int,Tin> method, int count = 1000)
        {
            DataTable testDataSource = null;
            Type type = typeof(Tin);
            object result = null;
            if (method != null)
            {
                testDataSource = new DataTable();
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    testDataSource.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                }

                testDataSource.AcceptChanges();

                ConfigObject = method;
                for (int i = 0; i < count; i++)
                {
                    object obj = (Tin)Activator.CreateInstance(typeof(Tin));
                    result = ConfigObject((Tin)obj, i);
                    if (result != null)
                    {
                        object[] values = new object[properties.Length];
                        for (int ii = 0; ii < properties.Length; ii++)
                        {
                            values[ii] = properties[ii].GetValue(result);
                        }

                        testDataSource.Rows.Add(values);
                        testDataSource.AcceptChanges();
                    }
                }
            }

            return testDataSource;
        }

        public static Dictionary<object,object> CreateForDictionary<Tín>(Func<Tin,object, Tin> method, int count = 1000)
        {
            Dictionary<object, object> testDataSource = null;
            Type type = typeof(Tin);
            dynamic result = null;
            if (method != null)
            {
                Type[] argTypes = type.GetGenericArguments();
                Type typeKey = argTypes[0];
                Type typeValue = argTypes[1];

                testDataSource = new Dictionary<object, object>();
                ConfigObjectDict = method;
                for (int i = 0; i < count; i++)
                {
                    Tin obj = (Tin)Activator.CreateInstance(typeof(Tin));
                    result = ConfigObjectDict((Tin)obj,i+1);
                    testDataSource.Add(Convert.ChangeType(result.Key, typeKey), Convert.ChangeType(result.Value, typeValue));
                }
            }

            return testDataSource;
        }
    }

    [DebuggerDisplay("Name={this.name};{this.district}")]
    internal class CityModel
    {
        public string area { get; set; }
        public Coords coords { get; set; }
        public string district { get; set; }
        public string name { get; set; }
        public string population { get; set; }
        public string state { get; set; }
    }

    [DebuggerDisplay("Koordinaten={this.lat}:{this.lon}")]
    internal class Coords
    {
        public string lat { get; set; }
        public string lon { get; set; }
    }

    internal class Currency
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string symbol_native { get; set; }
        public int decimal_digits { get; set; }
        public int rounding { get; set; }
        public string code { get; set; }
        public string name_plural { get; set; }
    }

    public static class BuildDemoData
    {
        private static readonly string[] firstNames = { "" };
        private static readonly string[] lastNames = { "" };
        private static readonly string[] cities = { "" };
        private static readonly string[] programmingLanguage = { "" };
        private static readonly string[] currencyInfo = { "" };
        private static readonly string[] symbols = 
        { 
            "M17,21L14.25,18L15.41,16.84L17,18.43L20.59,14.84L21.75,16.25M12.8,21H5C3.89,21 3,20.11 3,19V5C3,3.89 3.89,3 5,3H19C20.11,3 21,3.89 21,5V12.8C20.39,12.45 19.72,12.2 19,12.08V5H5V19H12.08C12.2,19.72 12.45,20.39 12.8,21M12,17H7V15H12M14.68,13H7V11H17V12.08C16.15,12.22 15.37,12.54 14.68,13M17,9H7V7H17",
            "M14.4,6L14,4H5V21H7V14H12.6L13,16H20V6H14.4Z",
            "M12,10A2,2 0 0,0 10,12C10,13.11 10.9,14 12,14C13.11,14 14,13.11 14,12A2,2 0 0,0 12,10Z",
            "M5,3C3.89,3 3,3.89 3,5V19C3,20.11 3.89,21 5,21H19C20.11,21 21,20.11 21,19V5C21,3.89 20.11,3 19,3H5M5,5H19V19H5V5M7,7V9H17V7H7M7,11V13H17V11H7M7,15V17H14V15H7Z",
            "M8,12H16V14H8V12M10,20H6V4H13V9H18V12.1L20,10.1V8L14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H10V20M8,18H12.1L13,17.1V16H8V18M20.2,13C20.3,13 20.5,13.1 20.6,13.2L21.9,14.5C22.1,14.7 22.1,15.1 21.9,15.3L20.9,16.3L18.8,14.2L19.8,13.2C19.9,13.1 20,13 20.2,13M20.2,16.9L14.1,23H12V20.9L18.1,14.8L20.2,16.9Z",
            "M13.81 22H6C4.89 22 4 21.11 4 20V4C4 2.9 4.89 2 6 2H14L20 8V13.09C19.67 13.04 19.34 13 19 13S18.33 13.04 18 13.09V9H13V4H6V20H13.09C13.21 20.72 13.46 21.39 13.81 22M22.54 21.12L20.41 19L22.54 16.88L21.12 15.47L19 17.59L16.88 15.47L15.47 16.88L17.59 19L15.47 21.12L16.88 22.54L19 20.41L21.12 22.54L22.54 21.12Z",
            "M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2H6M6,4H13V9H18V20H6V4M8,12V14H16V12H8M8,16V18H13V16H8Z",
            "M17,21L14.25,18L15.41,16.84L17,18.43L20.59,14.84L21.75,16.25M12.8,21H5C3.89,21 3,20.11 3,19V5C3,3.89 3.89,3 5,3H19C20.11,3 21,3.89 21,5V12.8C20.39,12.45 19.72,12.2 19,12.08V5H5V19H12.08C12.2,19.72 12.45,20.39 12.8,21M12,17H7V15H12M14.68,13H7V11H17V12.08C16.15,12.22 15.37,12.54 14.68,13M17,9H7V7H17",
            "M23 3V2.5C23 1.12 21.88 0 20.5 0S18 1.12 18 2.5V3C17.45 3 17 3.45 17 4V8C17 8.55 17.45 9 18 9H23C23.55 9 24 8.55 24 8V4C24 3.45 23.55 3 23 3M22 3H19V2.5C19 1.67 19.67 1 20.5 1S22 1.67 22 2.5V3M6 11H15V13H6V11M6 7H15V9H6V7M22 11V16C22 17.11 21.11 18 20 18H6L2 22V4C2 2.89 2.9 2 4 2H15V4H4V17.17L5.17 16H20V11H22Z",
            "M20 17H22V15H20V17M20 7V13H22V7M6 16H11V18H6M6 12H14V14H6M4 2C2.89 2 2 2.89 2 4V20C2 21.11 2.89 22 4 22H16C17.11 22 18 21.11 18 20V8L12 2M4 4H11V9H16V20H4Z",
            /* Abacus */
            "M5 5H7V11H5V5M10 5H8V11H10V5M5 19H7V13H5V19M10 13H8V19H10V17H15V15H10V13M2 21H4V3H2V21M20 3V7H13V5H11V11H13V9H20V15H18V13H16V19H18V17H20V21H22V3H20Z",
            /* Account */
            "M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z",
            /* Account-Group */
            "M12,5.5A3.5,3.5 0 0,1 15.5,9A3.5,3.5 0 0,1 12,12.5A3.5,3.5 0 0,1 8.5,9A3.5,3.5 0 0,1 12,5.5M5,8C5.56,8 6.08,8.15 6.53,8.42C6.38,9.85 6.8,11.27 7.66,12.38C7.16,13.34 6.16,14 5,14A3,3 0 0,1 2,11A3,3 0 0,1 5,8M19,8A3,3 0 0,1 22,11A3,3 0 0,1 19,14C17.84,14 16.84,13.34 16.34,12.38C17.2,11.27 17.62,9.85 17.47,8.42C17.92,8.15 18.44,8 19,8M5.5,18.25C5.5,16.18 8.41,14.5 12,14.5C15.59,14.5 18.5,16.18 18.5,18.25V20H5.5V18.25M0,20V18.5C0,17.11 1.89,15.94 4.45,15.6C3.86,16.28 3.5,17.22 3.5,18.25V20H0M24,20H20.5V18.25C20.5,17.22 20.14,16.28 19.55,15.6C22.11,15.94 24,17.11 24,18.5V20Z",
            /* Alert */
            "M13 14H11V9H13M13 18H11V16H13M1 21H23L12 2L1 21Z",
            /* alert-circle */
            "M13,13H11V7H13M13,17H11V15H13M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z",
            /* Database */
            "M12,3C7.58,3 4,4.79 4,7C4,9.21 7.58,11 12,11C16.42,11 20,9.21 20,7C20,4.79 16.42,3 12,3M4,9V12C4,14.21 7.58,16 12,16C16.42,16 20,14.21 20,12V9C20,11.21 16.42,13 12,13C7.58,13 4,11.21 4,9M4,14V17C4,19.21 7.58,21 12,21C16.42,21 20,19.21 20,17V14C20,16.21 16.42,18 12,18C7.58,18 4,16.21 4,14Z",
            /* Calendar */
            "M19,19H5V8H19M16,1V3H8V1H6V3H5C3.89,3 3,3.89 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5C21,3.89 20.1,3 19,3H18V1"
        };

        private static readonly string[] isbn13Number =
 {
            "978-5-1781-5937-8",
            "978-0-4380-0455-9",
            "978-6-6125-0040-4",
            "978-4-8427-6833-5",
            "978-5-0449-4343-8",
            "978-0-3503-8226-1",
            "978-5-2726-4417-6",
            "978-1-4923-5117-7",
            "978-1-7999-2467-8",
            "978-0-7402-1301-4",
            "978-2-3910-5992-8",
            "978-3-4659-2938-3",
            "978-9-3124-2684-5",
            "978-8-3155-3191-4",
            "978-1-6083-0841-5",
            "978-7-3142-0055-5",
            "978-4-6346-2792-5",
            "978-0-5034-2790-0",
            "978-3-7718-0454-1",
            "978-8-6667-8218-8",
            "978-1-8317-4415-8",
            "978-9-9877-0412-5",
            "978-4-4115-1452-3",
            "978-2-1244-9853-6"
        };

        private static readonly Random rnd;

        static BuildDemoData()
        {
            var resourceCity = "ModernBaseLibrary.Resources.DemoDataGenerator.GermanyCities.json";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceCity))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonFileContent = reader.ReadToEnd();
                        cities = JsonSerializer.Deserialize<List<CityModel>>(jsonFileContent).Select(s => s.name).ToArray();
                    }
                }
            }

            var resourceCurrency = "ModernBaseLibrary.Resources.DemoDataGenerator.Currency.json";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceCurrency))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonFileContent = reader.ReadToEnd();
                        currencyInfo = JsonSerializer.Deserialize<List<Currency>>(jsonFileContent).Select(s => s.code).ToArray();
                    }
                }
            }

            var resourceProgs = "ModernBaseLibrary.Resources.DemoDataGenerator.ProgrammingLanguage.json";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceProgs))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonFileContent = reader.ReadToEnd();
                        programmingLanguage = JsonSerializer.Deserialize<List<string>>(jsonFileContent).ToArray();
                    }
                }
            }

            var resourceFirstNames = "ModernBaseLibrary.Resources.DemoDataGenerator.Vornamen.json";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFirstNames))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonFileContent = reader.ReadToEnd();
                        firstNames = JsonSerializer.Deserialize<List<string>>(jsonFileContent).Where(w =>w != string.Empty).ToArray();
                    }
                }
            }

            var resourceLastNames = "ModernBaseLibrary.Resources.DemoDataGenerator.Nachnamen.json";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLastNames))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonFileContent = reader.ReadToEnd();
                        lastNames = JsonSerializer.Deserialize<List<string>>(jsonFileContent).Where(w => w != string.Empty).ToArray();
                    }
                }
            }

            rnd = new Random();
        }

        public static string Letters(int length, CharacterCasing casing = CharacterCasing.Normal)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            if (length > chars.Length)
            {
                return string.Empty;
            }

            string result = string.Empty;

            if (casing == CharacterCasing.Lower)
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray()).ToLower();
            }
            else if (casing == CharacterCasing.Upper)
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray()).ToUpper();
            }
            else
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
            }

            return result;
        }

        public static string AlphabetAndNumeric(int length, CharacterCasing casing = CharacterCasing.Normal)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            if (length > chars.Length)
            {
                return string.Empty;
            }

            string result = string.Empty;

            if (casing == CharacterCasing.Lower)
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray()).ToLower();
            }
            else if (casing == CharacterCasing.Upper)
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray()).ToUpper();
            }
            else
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
            }

            return result;
        }

        public static string Username(int lengthLetter = 4, int lengthNumber = 4)
        {
            const string charsLetter = "abcdefghijklmnopqrstuvwxyz";
            const string charsNumber = "0123456789";


            if (lengthLetter > charsLetter.Length && lengthNumber > charsNumber.Length)
            {
                return string.Empty;
            }

            string resultLetter = new string(Enumerable.Repeat(charsLetter, lengthLetter).Select(s => s[rnd.Next(s.Length)]).ToArray()).ToLower();
            string resultNumber = new string(Enumerable.Repeat(charsNumber, lengthNumber).Select(s => s[rnd.Next(s.Length)]).ToArray());

            return $"{resultLetter.ToUpper()}{resultNumber}";
        }

        public static double Double(double min, double max, int countDigits = 2)
        {
            var value = rnd.NextDouble() * (max - min) + min;

            if (value == 0)
            {
                value = rnd.NextDouble() * (max - min) + min;
            }

            return Math.Round(value, countDigits, MidpointRounding.AwayFromZero);
        }

        public static decimal Decimal(decimal min, decimal max, int countDigits = 2)
        {
            decimal value = (decimal)rnd.NextDouble() * (max - min) + min;

            if (value == 0)
            {
                value = (decimal)rnd.NextDouble() * (max - min) + min;
            }

            return Math.Round(value, countDigits, MidpointRounding.AwayFromZero);
        }

        public static decimal CurrencyValue(decimal min, decimal max)
        {
            decimal value = (decimal)rnd.NextDouble() * (max - min) + min;

            if (value == 0)
            {
                value = (decimal)rnd.NextDouble() * (max - min) + min;
            }

            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public static int Integer(int min, int max)
        {
            return rnd.Next(min, max);
        }
        public static DateTime Dates(DateTime from, DateTime to)
        {
            TimeSpan range = to - from;

            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        public static DateTime? Dates(DateTime? from, DateTime? to)
        {
            TimeSpan? range = to - from;

            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * ((TimeSpan)range).Ticks));

            return from + randTimeSpan;
        }

        public static bool Boolean()
        {
            return rnd.Next(0, 2) == 0;
        }

        public static string Word(List<string> words)
        {
            int index = rnd.Next(words.Count);
            return words[index];
        }

        public static string FirstName()
        {
            return firstNames[rnd.Next(firstNames.Length)];
        }

        public static string LastName()
        {
            return lastNames[rnd.Next(lastNames.Length)];
        }

        public static string City()
        {
            return cities[rnd.Next(cities.Length)];
        }

        public static string ColorName()
        {
            List<string> colorNames = ColorInfo.ListOfColorNames().ToList();

            return colorNames[rnd.Next(colorNames.Count())];
        }

        public static string Symbols()
        {
            return symbols[rnd.Next(symbols.Length)];
        }

        public static string ProgrammingLanguage()
        {
            return programmingLanguage[rnd.Next(programmingLanguage.Length)];
        }

        public static string CurrencyText()
        {
            return currencyInfo[rnd.Next(currencyInfo.Length)];
        }

        public static string ISBN13()
        {
            return isbn13Number[rnd.Next(isbn13Number.Length)];
        }

        public static string IpAddress()
        {
            return $"{rnd.Next(1, 255)}.{rnd.Next(0, 255)}.{rnd.Next(0, 255)}.{rnd.Next(0, 255)}";
        }

        public static string MacAddress()
        {
            var buffer = new byte[6];
            rnd.NextBytes(buffer);
            return string.Join(':', buffer.Select(x => x.ToString("X2")));
        }

        public static (DateTime CreateOn, string CreateBy, DateTime ModifiedOn, string ModifiedBy) SetTimeStamp()
        {
            DateTime modifiedOn = DateTime.Now;
            DateTime createOn = DateTime.Now.AddMinutes(-rnd.Next(1,20_000));
            string modifiedBy = $"{Environment.UserDomainName}\\{Environment.UserName}";
            string createBy = $"{Environment.UserDomainName}\\{Environment.UserName}";
            return (createOn, createBy, modifiedOn, modifiedBy);
        }
    }
}
