/*
 * <copyright file="TryParseExtension.cs" company="Lifeprojects.de">
 *     Class: TryParseExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>23.05.2022 09:52:48</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Generic TryParse Extension for any Types
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

namespace System
{
    using System.Reflection;

    public static class TryParseExtension
    {
        private static MethodInfo enumTryParseMethod;

        private delegate bool TryParseDelegate<TTyp>(string value, out TTyp result);

        public static bool TryParse<TTyp>(this string value, out TTyp result)
        {
            return TryParser<TTyp>.TryParse(value.ToString(), out result);
        }

        private static bool TryParseEnum<TTyp>(this string value, out TTyp result)
        {
            try
            {
                object temp = Enum.Parse(typeof(TTyp), value, true);
                if (temp is TTyp)
                {
                    result = (TTyp)temp;
                    return true;
                }
            }
            catch
            {
            }

            result = default(TTyp);
            return false;
        }


        private static TryParseDelegate<TTyp> GetEnumTryParse<TTyp>()
        {
            Type type = typeof(TTyp);

            if (enumTryParseMethod == null)
            {
                var methods = typeof(Enum).GetMethods(BindingFlags.Public | BindingFlags.Static);

                foreach (var method in methods)
                {
                    if (method.Name == "TryParse" && method.IsGenericMethodDefinition && method.GetParameters().Length == 2 && method.GetParameters()[0].ParameterType == typeof(string))
                    {
                        enumTryParseMethod = method;
                        break;
                    }
                }
            }

            var result = Delegate.CreateDelegate(typeof(TryParseDelegate<TTyp>), enumTryParseMethod.MakeGenericMethod(type), false) as TryParseDelegate<TTyp>;
            if (result == null)
            {
                return TryParseEnum<TTyp>;
            }
            else
            {
            }

            return result;
        }

        private static bool TryParseNullable<TTyp>(string value, out TTyp? result) where TTyp : struct
        {
            TTyp temp;
            if (TryParser<TTyp>.TryParse(value, out temp))
            {
                result = temp;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        private static class TryParser<T>
        {
#pragma warning disable IDE0044 // Modifizierer "readonly" hinzufügen
            private static TryParseDelegate<T> parser;
#pragma warning restore IDE0044 // Modifizierer "readonly" hinzufügen

            static TryParser()
            {
                Type t = typeof(T);
                if (t.IsEnum)
                {
                    AssignClass<T>(GetEnumTryParse<T>());
                }
                else if (t == typeof(bool) || t == typeof(bool?))
                {
                    AssignStruct<bool>(bool.TryParse);
                }
                else if (t == typeof(byte) || t == typeof(byte?))
                {
                    AssignStruct<byte>(byte.TryParse);
                }
                else if (t == typeof(short) || t == typeof(short?))
                {
                    AssignStruct<short>(short.TryParse);
                }
                else if (t == typeof(char) || t == typeof(char?))
                {
                    AssignStruct<char>(char.TryParse);
                }
                else if (t == typeof(int) || t == typeof(int?))
                {
                    AssignStruct<int>(int.TryParse);
                }
                else if (t == typeof(long) || t == typeof(long?))
                {
                    AssignStruct<long>(long.TryParse);
                }
                else if (t == typeof(sbyte) || t == typeof(sbyte?))
                {
                    AssignStruct<sbyte>(sbyte.TryParse);
                }
                else if (t == typeof(ushort) || t == typeof(ushort?))
                {
                    AssignStruct<ushort>(ushort.TryParse);
                }
                else if (t == typeof(uint) || t == typeof(uint?))
                {
                    AssignStruct<uint>(uint.TryParse);
                }
                else if (t == typeof(ulong) || t == typeof(ulong?))
                {
                    AssignStruct<ulong>(ulong.TryParse);
                }
                else if (t == typeof(decimal) || t == typeof(decimal?))
                {
                    AssignStruct<decimal>(decimal.TryParse);
                }
                else if (t == typeof(float) || t == typeof(float?))
                {
                    AssignStruct<float>(float.TryParse);
                }
                else if (t == typeof(double) || t == typeof(double?))
                {
                    AssignStruct<double>(double.TryParse);
                }
                else if (t == typeof(DateTime) || t == typeof(DateTime?))
                {
                    AssignStruct<DateTime>(DateTime.TryParse);
                }
                else if (t == typeof(TimeSpan) || t == typeof(TimeSpan?))
                {
                    AssignStruct<TimeSpan>(TimeSpan.TryParse);
                }
                else if (t == typeof(Guid) || t == typeof(Guid?))
                {
                    AssignStruct<Guid>(Guid.TryParse);
                }
                else if (t == typeof(Version))
                {
                    AssignClass<Version>(Version.TryParse);
                }
            }

            public static bool TryParse(string value, out T result)
            {
                if (parser == null)
                {
                    result = default(T);
                    return false;
                }

                return parser(value, out result);
            }

            private static void AssignStruct<TTyp>(TryParseDelegate<TTyp> del) where TTyp : struct
            {
                TryParser<TTyp>.parser = del;
                if (typeof(TTyp).IsGenericType && typeof(TTyp).GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return;
                }

                AssignClass<TTyp?>(TryParseNullable<TTyp>);
            }

            private static void AssignClass<TTyp>(TryParseDelegate<TTyp> del)
            {
                TryParser<TTyp>.parser = del;
            }
        }
    }
}
