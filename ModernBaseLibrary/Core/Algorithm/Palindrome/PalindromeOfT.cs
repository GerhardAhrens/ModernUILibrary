//-----------------------------------------------------------------------
// <copyright file="PalindromeOfT.cs" company="Lifeprojects.de">
//     Class: PalindromeOfT
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>10.12.2024 07:46:53</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Algorithm
{
    using System;

    public class Palindrome<T>
    {
        public static T Get(T value)
        {
            T result = default(T);

            if (typeof(T) == typeof(int))
            {
                result = (T)PalindromeOfNumber(value);
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T)PalindromeOfString(value);
            }
            else
            {
                throw new ArgumentException("Zum Auswerten eines Palindrome sind nur int oder string erlaubt.");
            }

            return result;
        }

        private static object PalindromeOfNumber(T value)
        {
            int number = Convert.ToInt32(value);
            int reminder = 0;
            int sum = 0;

            while (number > 0)
            {
                reminder = number % 10;
                sum = (sum * 10) + reminder;
                number = number / 10;
            }

            return sum;
        }

        private static object PalindromeOfString(T value)
        {
            string result = string.Empty;
            string text = Convert.ToString(value);
            char[] newArray = text.ToCharArray();
            Array.Reverse(newArray);
            result = new string(newArray);

            return result;
        }
    }
}
