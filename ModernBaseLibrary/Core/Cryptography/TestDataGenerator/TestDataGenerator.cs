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


namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Globalization;

    public static class TestDataGenerator<Tin>
    {
        static TestDataGenerator()
        {
        }

        public static Func<Tin,Tin> ConfigObject { get; private set; }


        public static List<Tin> CreateTestData<Tín>(Func<Tin,Tin> method, int count = 1000)
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
                    object obj = (Tin)Activator.CreateInstance(typeof(Tin));
                    result = ConfigObject((Tin)obj);
                    testDataSource.Add((Tin)result);
                }
            }

            return testDataSource;
        }
    }

    public static class TestDataGenerator
    {
        private static readonly string[] Consonants =
{
            "b","c","d","f","g","h","j","k","l","m","n","p","q","r","s","ß","t","v","w","x","z"
        };
        private static readonly string[] vokale = { "a", "e", "i", "o", "u" };
        private static readonly string[] firstNames =
        {
            "Aiden","Jackson","Mason","Liam","Jacob","Jayden","Ethan","Noah","Lucas","Logan","Caleb","Caden","Jack","Ryan","Connor","Michael","Elijah","Brayden","Benjamin","Nicholas","Alexander",
            "William","Matthew","James","Landon","Nathan","Dylan","Evan","Luke","Andrew","Gabriel","Gavin","Joshua","Owen","Daniel","Carter","Tyler","Cameron","Christian","Wyatt","Henry","Eli",
            "Joseph","Max","Isaac","Samuel","Anthony","Grayson","Zachary","David","Christopher","John","Isaiah","Levi","Jonathan","Oliver","Chase","Cooper","Tristan","Colton","Austin","Colin",
            "Charlie","Dominic","Parker","Hunter","Thomas","Alex","Ian","Jordan","Cole","Julian","Aaron","Carson","Miles","Blake","Brody","Adam","Sebastian","Adrian","Nolan","Sean","Riley",
            "Bentley","Xavier","Hayden","Jeremiah","Jason","Jake","Asher","Micah","Jace","Brandon","Josiah","Hudson","Nathaniel","Bryson","Ryder","Justin","Bryce",  null
        };

        private static readonly string[] lastNames =
                {
            "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia",
            "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King", "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker",
            "Gonzalez", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers",
            "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks",
            "Kelly", "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman", "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler",
            "Simmons", "Foster", "Gonzales", "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes", null
        };

        private static readonly string[] countries = { "Mannheim","Ludwigshafen", "Neuhofen","Hamburg","Hannover","Berlin","Bremen","Frankfurt","Dresden","Erfurt","Schwerin" };

        private static readonly Random rnd;

        static TestDataGenerator()
        {
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

        public static TResult Numbers<TResult>(int length)
        {
            const string chars = "0123456789";

            if (length > chars.Length)
            {
                return (TResult)Convert.ChangeType(0, typeof(TResult), CultureInfo.InvariantCulture);
            }

            string nums = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());

            return (TResult)Convert.ChangeType(nums, typeof(TResult), CultureInfo.InvariantCulture);
        }

        public static DateTime Dates(DateTime from, DateTime to)
        {
            TimeSpan range = to - from;

            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));

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

        public static string Country()
        {
            return countries[rnd.Next(countries.Length)];
        }
    }
}
