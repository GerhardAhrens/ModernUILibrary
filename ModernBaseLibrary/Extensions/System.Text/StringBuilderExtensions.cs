//-----------------------------------------------------------------------
// <copyright file="StringBuilderExtensions.cs" company="Lifeprojects.de">
//     Class: StringBuilderExtensions
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Extension Class</summary>
// <WebLink>https://jonlabelle.com/snippets/tag/extensions/2</WebLink>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using System.Collections.Generic;
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static StringBuilder InserBeforeToken(this StringBuilder @this, string beforeToken, string insertValues)
        {
            int pos = @this.IndexOf(beforeToken, 1, true);
            if (pos > 0)
            {
                return @this.Insert(pos, insertValues);
            }

            return @this;
        }

        public static StringBuilder InserBeforeToken(this StringBuilder @this, Expression<Func<string, bool>> predicate, params string[] values)
        {
            var body = (MethodCallExpression)predicate.Body;
            if (body.Arguments.Count == 1)
            {
                Expression argument = body.Arguments[0];
                string value = Expression.Lambda(argument).Compile().DynamicInvoke().ToString();
                string methode = body.Method.Name;

                Type magicType = Type.GetType("String");
                ConstructorInfo magicConstructor = magicType.GetConstructor(Type.EmptyTypes);
                object magicClassObject = magicConstructor.Invoke(new object[] { });

                string method = $"{MethodBase.GetCurrentMethod().DeclaringType.FullName}.Contains";

                var aa = body.Method.Invoke(method, new string[] { value });

                /*
                if (predicate("") == true)
                {
                }
                */
            }

            return @this;
        }

        public static StringBuilder InserAfterToken(this StringBuilder @this)
        {
            return @this;
        }

        /// <summary>
        /// Returns the index of the start of the contents in a StringBuilder
        /// </summary>        
        /// <param name="value">The char to find</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="ignoreCase">if set to <c>true</c> it will ignore case</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder @this, char value, int startIndex = 1, bool ignoreCase = true)
        {
            List<char> list = null;

            if (ignoreCase == true)
            {
                list = @this.ToString().Select(x => char.ToLower(x)).ToList<char>();
            }
            else
            {
                list = @this.ToString().ToList<char>();
            }

            int pos = list.IndexOf(ignoreCase == true ? char.ToLower(value) : value, startIndex);

            return pos;
        }

        /// <summary>
        /// Returns the index of the start of the contents in a StringBuilder
        /// </summary>        
        /// <param name="value">The string to find</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="ignoreCase">if set to <c>true</c> it will ignore case</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder @this, string value, int startIndex = 1, bool ignoreCase = true)
        {
            string str = ignoreCase == true ? @this.ToString().ToLower() : @this.ToString();
            int pos = str.IndexOf(ignoreCase == true ? value.ToLower() : value, startIndex);

            return pos;
        }

        /// <summary>
        /// Die Methode gibt das erste Zeichen aus einer StringBuilder Liste zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns><see cref="System.Text.StringBuilder"/></returns>
        public static char FirstChar(this StringBuilder @this)
        {
            char result = Convert.ToChar(@this.ToString().Substring(0, 1).ToString());
            return result;
        }

        /// <summary>
        /// Die Methode gibt das letzte Zeichen aus einer StringBuilder Liste zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns><see cref="System.Text.StringBuilder"/></returns>
        public static char LastChar(this StringBuilder @this)
        {
            char result = Convert.ToChar(@this.ToString().Substring(@this.Length - 1, 1).ToString());
            return result;
        }


        /// <summary>
        /// Die Methode entfernt alle Leerzeichen am Ende einer StringBuilder Liste
        /// </summary>
        /// <param name="this">StringBuilder Objekt</param>
        /// <returns><see cref="System.Text.StringBuilder"/></returns>
        public static StringBuilder TrimEnd(this StringBuilder @this)
        {
            if (@this == null || @this.Length == 0)
            {
                return @this;
            }

            int i = @this.Length - 1;
            for (; i >= 0; i--)
            {
                if (char.IsWhiteSpace(@this[i]) == false)
                {
                    break;
                }
            }

            if (i < @this.Length - 1)
            {
                @this.Length = i + 1;
            }

            return @this;
        }

        /// <summary>
        /// Die Methode entfernt alle Zeichen 'lastChar' am Ende einer StringBuilder Liste
        /// </summary>
        /// <param name="this">StringBuilder Objekt</param>
        /// <param name="lastChar">Ein Zeich das am Ende entfernt werden soll</param>
        /// <returns><see cref="System.Text.StringBuilder"/></returns>
        public static StringBuilder TrimEnd(this StringBuilder @this, char lastChar)
        {
            if (@this == null || @this.Length == 0)
            {
                return @this;
            }

            int i = @this.Length - 1;
            for (; i >= 0; i--)
            {
                if (@this[i] != lastChar)
                {
                    break;
                }
            }

            if (i < @this.Length - 1)
            {
                @this.Length = i + 1;
            }

            return @this;
        }

        public static StringBuilder TrimEnd(this StringBuilder @this, string lastToken)
        {
            int pos = @this.IndexOf(lastToken,@this.Length - lastToken.Length);

            return @this.Remove(pos, lastToken.Length).TrimEnd();
        }

        public static bool StartsWith(this StringBuilder @this, string value, bool ignoreCase = false)
        {
            int length = value.Length;

            if (length > @this.Length)
            {
                return false;
            }

            if (ignoreCase == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (@this[i] != value[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = 0; j < length; j++)
                {
                    if (char.ToLower(@this[j]) != char.ToLower(value[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool EndsWith(this StringBuilder @this, string value, bool ignoreCase = false)
        {
            int length = value.Length;
            int maxSBIndex = @this.Length - 1;
            int maxValueIndex = length - 1;

            if (length > @this.Length)
            {
                return false;
            }

            if (ignoreCase == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (@this[maxSBIndex - i] != value[maxValueIndex - i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = length - 1; j >= 0; j--)
                {
                    if (char.ToLower(@this[maxSBIndex - j]) != char.ToLower(value[maxValueIndex - j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Die Methode entfert alle Zeichen (Char) 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="removeChars"></param>
        /// <returns></returns>
        public static StringBuilder Remove(this StringBuilder @this, params char[] removeChars)
        {
            for (int i = 0; i < @this.Length;)
            {
                if (removeChars.Any(ch => ch == @this[i]))
                {
                    @this.Remove(i, 1);
                }
                else
                {
                    i++;
                }
            }

            return @this;
        }

        public static StringBuilder RemoveLastChar(this StringBuilder @this, int removeCount = 1)
        {
            if (@this != null && @this.Length > 0)
            {
                @this.Remove(@this.ToString().Trim().Length - removeCount, removeCount);
            }

            return @this;
        }

        public static StringBuilder ToLower(this StringBuilder @this)
        {
            for (int i = 0; i < @this.Length; i++)
            {
                @this[i] = char.ToLower(@this[i]);
            }

            return @this;
        }

        public static StringBuilder ToUpper(this StringBuilder @this)
        {
            for (int i = 0; i < @this.Length; i++)
            {
                @this[i] = char.ToUpper(@this[i]);
            }

            return @this;
        }

        /// <summary>
        /// Die Methode fügt einen String hinzu, wenn die Bedingung true ist. 
        /// </summary>
        /// <param name="this">StringBuilder Object</param>
        /// <param name="predicate">Bedingung</param>
        /// <param name="str">String der hinzugefügt wird</param>
        /// <returns></returns>
        public static StringBuilder AppendIf(this StringBuilder @this, bool predicate, string str)
        {
            if (@this == null)
            {
                return @this;
            }

            if (predicate == true)
            {
                @this.Append(str);
                @this.Append(" ");
            }

            return @this;
        }

        /// <summary>
        /// Die Methode fügt einen String hinzu, wenn die Bedingung true ist. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">StringBuilder Object</param>
        /// <param name="predicate">Bedingung</param>
        /// <param name="values">String der hinzugefügt wird</param>
        /// <returns></returns>
        public static StringBuilder AppendIf<T>(this StringBuilder @this, Func<T, bool> predicate, params T[] values)
        {
            foreach (var value in values)
            {
                if (predicate(value) == true)
                {
                    @this.Append(value);
                }
            }

            return @this;
        }
        public static StringBuilder AppendWhen(this StringBuilder @this, bool condition, params string[] values)
        {
            if (condition == true)
            {
                foreach (var value in values)
                {
                    @this.Append(value);
                }
            }

            return @this;
        }

        public static StringBuilder AppendWhen(this StringBuilder @this, bool condition, params char[] values)
        {
            if (condition)
            {
                foreach (var value in values)
                {
                    @this.Append(value);
                }
            }

            return @this;
        }

        public static StringBuilder IsLastCharThan(this StringBuilder @this, char lastChar, string str)
        {
            if (@this == null)
            {
                return @this;
            }

            if (@this.ToString().Substring(@this.ToString().Length - 1, 1) != lastChar.ToString())
            {
                @this.Append(str);
            }

            return @this;
        }

        public static StringBuilder AppendLineFormat(this StringBuilder @this, string format, params object[] arguments)
        {
            string value = string.Format(format, arguments);

            @this.AppendLine(value);

            return @this;
        }

        public static void CopyToFile(this StringBuilder @this, string path)
        {
            File.WriteAllText(path, @this.ToString());
        }

        public static string Join(this StringBuilder @this, string seperator)
        {
            if (@this == null)
            {
                return string.Empty;
            }

            var lst = new List<string>();
            for (int i = 0; i < @this.Length; i++)
            {
                lst.Add(@this[i].ToString());
            }

            return string.Join(seperator, lst.ToArray());
        }

        public static void AppendCollection<T>(this StringBuilder @this, IEnumerable<T> collection, Func<T, string> method)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            if (method == null)
            {
                throw new ArgumentNullException();
            }

            List<T> l = new List<T>(collection);
            l.ForEach(item => @this.AppendLine(method(item)));
        }

        public static StringBuilder AddLine(this StringBuilder @this, int tab, string text, params object[] args)
        {
            StringBuilder tabs = new StringBuilder();
            for (int i = 0; i < tab; i++)
            {
                tabs.Append("\t");
            }

            if (args.Length == 0)
            {
                return @this.Append(tabs).AppendLine(text);
            }
            else
            {
                return @this.Append(tabs).AppendFormat(string.Format("{0}\n", text), args);
            }
        }

        public static StringBuilder AddLine(this StringBuilder @this, string text, params object[] args)
        {
            if (args.Length == 0)
            {
                return @this.AppendLine(text);
            }
            else
            {
                return @this.AppendFormat(string.Format("{0}\n", text), args);
            }
        }

        public static StringBuilder AddLine(this StringBuilder @this)
        {
            return @this.AppendLine();
        }

        /// <summary>A StringBuilder extension method that substrings.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>A string.</returns>
        public static string Substring(this StringBuilder @this, int startIndex)
        {
            return @this.ToString(startIndex, @this.Length - startIndex);
        }

        /// <summary>A StringBuilder extension method that substrings.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public static string Substring(this StringBuilder @this, int startIndex, int length)
        {
            return @this.ToString(startIndex, length);
        }

        public static bool IsEmpty(this StringBuilder stringBuilder)
        {
            return stringBuilder.Length == 0;
        }

        public static bool IsNullOrEmpty(this StringBuilder stringBuilder)
        {
            return stringBuilder == null || stringBuilder.IsEmpty();
        }

        public static bool IsNullOrWhiteSpace(this StringBuilder stringBuilder)
        {
            return stringBuilder.IsNullOrEmpty() || stringBuilder.ToString().Trim().Length == 0;
        }
    }
}