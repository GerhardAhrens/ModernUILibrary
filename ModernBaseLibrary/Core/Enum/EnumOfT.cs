//-----------------------------------------------------------------------
// <copyright file="EnumOfT.cs" company="Lifeprojects.de">
//     Class: EnumOfT
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>29.01.2025 13:54:54</date>
//
// <summary>
// Struct Class for ...
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public struct EnumOfT<TEnum> where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Anzahl Enum Einträge
        /// </summary>
        public static int Count
        {
            get
            {
                if (!typeof(TEnum).IsEnum)
                {
                    throw new ArgumentException("T must be an enumerated type");
                }

                return Enum.GetNames(typeof(TEnum)).Length;
            }
        }

        /// <summary>
        /// Enum Type als Name (String)
        /// </summary>
        public static string EnumTypeName
        {
            get
            {
                if (!typeof(TEnum).IsEnum)
                {
                    throw new ArgumentException("T must be an enumerated type");
                }

                return typeof(TEnum).Name;
            }
        }

        /// <summary>
        /// Enum Type als Type
        /// </summary>
        public static Type EnumType
        {
            get
            {
                if (!typeof(TEnum).IsEnum)
                {
                    throw new ArgumentException("T must be an enumerated type");
                }

                return typeof(TEnum);
            }
        }

        /// <summary>
        /// Prüft ob ein Enum in einer Liste von Enums vorhanden ist
        /// </summary>
        /// <param name="this"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool In(Enum @this, params Enum[] values)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Array.IndexOf(values, @this) != -1;
        }

        /// <summary>
        /// Prüft ob ein Enum in einer Liste von Enums nicht vorhanden ist
        /// </summary>
        /// <param name="this"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool NotIn(Enum @this, params Enum[] values)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Array.IndexOf(values, @this) == -1;
        }

        /// <summary>
        /// Gibt den Namen eines Enum-Eintrag als String zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetName(Enum @this)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetName(@this.GetType(), @this);
        }

        /// <summary>
        /// Gibt den Wert eines Enum-Eintrag als int zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetValue(Enum @this)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var result = new Dictionary<int, string>();
            var values = System.Enum.GetValues(typeof(TEnum));

            foreach (int item in values)
            {
                result.Add(item, System.Enum.GetName(typeof(TEnum), item));
            }

            return result.FirstOrDefault(f => f.Value == @this.ToString()).Key;
        }

        /// <summary>
        /// Prüft, ob zwei Enum gleich sind
        /// </summary>
        /// <param name="enumA"></param>
        /// <param name="enumB"></param>
        /// <returns></returns>
        public static bool Equals(Enum enumA, Enum enumB)
        {
            bool result = false;

            if (typeof(TEnum).Name == enumA.GetType().Name && typeof(TEnum).Name == enumB.GetType().Name)
            {
                List<Enum> listA = Enum.GetValues(enumA.GetType()).Cast<Enum>().ToList();
                string nameA = Enum.GetName(enumA.GetType(), enumA);

                List<Enum> listB = Enum.GetValues(enumB.GetType()).Cast<Enum>().ToList();
                string nameB = Enum.GetName(enumB.GetType(), enumB);

                result = (nameA == nameB) && listA.Any(a => a.ToString() == nameA) == listB.Any(a => a.ToString() == nameB);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static bool EqualEnum(Enum enumA, Enum enumB)
        {
            List<Enum> thisList = Enum.GetValues(enumA.GetType()).Cast<Enum>().ToList();
            string thisName = Enum.GetName(enumA.GetType(), enumA);

            List<Enum> equalList = Enum.GetValues(enumB.GetType()).Cast<Enum>().ToList();
            string equalName = Enum.GetName(enumB.GetType(), enumB);

            bool result = (thisName == equalName) && thisList.Any(a => a.ToString() == thisName) == equalList.Any(a => a.ToString() == equalName);
            return result;
        }

        /// <summary>
        /// Gib ein Enum als Dictionary zurück
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary()
        {
            var result = new Dictionary<int, string>();
            var values = System.Enum.GetValues(typeof(TEnum));

            foreach (int item in values)
            {
                result.Add(item, System.Enum.GetName(typeof(TEnum), item));
            }

            return result;
        }

        /// <summary>
        /// Gibt von einem Enum-Eintrag die Beschreibung als dem Description-Attribute zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ToDescription(Enum @this)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            FieldInfo fieldInfo = @this.GetType().GetField(@this.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return @this.ToString();
            }
        }
    }
}