/*
 * <copyright file="KeyDetection.cs" company="Lifeprojects.de">
 *     Class: KeyDetection
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * 
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

namespace ModernBaseLibrary.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class CmdLineKeyDetection
    {
        public const string HELP_KEY_REGEX = @"^(--help)|(-h)|(-\?)|(\\\?)$";
        public const string SHORT_KEY_REGEX = @"^-([a-zA-Z])(\w*)(=(\"")?([a-zA-Z0-9\.\/\\])+(\"")?)?$";
        public const string LONG_KEY_REGEX = @"^--[a-z][a-z]*(-?([a-z]+))(=(\"")?([a-zA-Z0-9\.\/\\])+(\"")?)?$";
        private static ShortKeyDetection _shortDetector;
        private static LongKeyDetection _longDetector;

        public static ShortKeyDetection GetShortKeyDetector()
        {
            if (_shortDetector == null) _shortDetector = new ShortKeyDetection();
            return _shortDetector;
        }

        public static LongKeyDetection GetLongKeyDetector()
        {
            if (_longDetector == null) _longDetector = new LongKeyDetection();
            return _longDetector;
        }

        public class ShortKeyDetection
        {
            private const string KEY_JOINS_VALUE_REGEX = @"(=(\"")?([a-zA-Z0-9\.\/\\])+)(\"")?$"; //-u=name
            public bool IsKey(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, CmdLineKeyDetection.SHORT_KEY_REGEX);
            }

            public bool IsJoinedToValue(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, KEY_JOINS_VALUE_REGEX);
            }

            public bool IsFollowedByValue(string potentialKey)
            {
                return potentialKey.Length > 2;
            }

            public Tuple<bool, KeyValuePair<string[], string>> IsFollowedByValue(string potentialKey, char[] requiredKeyList)
            {
                Func<string, char[], KeyValuePair<string[], string>> getValueFn = (string pKey, char[] rList) =>
                 {
                     int indexOfNotKey = -1;
                     List<string> keys = new List<string>();
                     for (int i = 1; i < pKey.Length; i++)
                     {
                         if (!requiredKeyList.Contains(pKey[i]))
                         {
                             indexOfNotKey = i;
                             break;
                         }
                         else
                         {
                             keys.Add("-" + pKey[i]);
                         }
                     }
                     return new KeyValuePair<string[], string>(keys.ToArray(), string.Join(string.Empty, pKey.Skip(indexOfNotKey)));
                 };
                return new Tuple<bool, KeyValuePair<string[], string>>(IsFollowedByValue(potentialKey), getValueFn(potentialKey, requiredKeyList));
            }

            public bool IsAggregated(string potentialKey, char[] requiredKeyList)
            {
                var keys = potentialKey.AsEnumerable().Skip(1);
                return keys.Count() > 1 && keys.All(key => requiredKeyList.Contains(key));
            }

            public string[] GetAggregatedKeys(string potentialKey, char[] requiredKeyList)
            {
                var keys = potentialKey.AsEnumerable().Skip(1);
                return keys.Where(key => requiredKeyList.Contains(key)).Select(key => "-" + key).ToArray();
            }
        }

        public class LongKeyDetection
        {
            private const string KEY_JOINS_VALUE_REGEX = @"=(\"")?([a-zA-Z0-9\.\/\\])+(\"")?$"; //--user=name
            public bool IsKey(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, CmdLineKeyDetection.LONG_KEY_REGEX);
            }

            public bool IsJoinedToValue(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, KEY_JOINS_VALUE_REGEX);
            }
        }
    }
}
