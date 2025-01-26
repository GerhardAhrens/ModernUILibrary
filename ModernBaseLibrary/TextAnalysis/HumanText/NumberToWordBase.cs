//-----------------------------------------------------------------------
// <copyright file="NumberToWordBase.cs" company="Lifeprojects.de">
//     Class: NumberToWordBase
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>
// Abstrakte Klasse als basis zur Darstellung von Zahlen in Worte
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;

    public abstract class NumberToWordBase
    {
        private static readonly string[] EinerArray = { "", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun", "zehn", "elf", "zwölf" };
        private static readonly string[] ZehnerArray = { "", "zehn", "zwanzig", "dreisig", "vierzig", "fünfzig", "sechzig", "siebzig", "achtzig", "neunzig" };

        private long tausender = 0;
        private int hundert = 0;
        private int zehner = 0;
        private int einer = 0;

        public string Bezeichner { get; set; }

        protected NumberToWordBase()
        {
            Bezeichner = this.GetType().Name;
        }

        /// <summary>
        /// Berechnet die Tausender, Hunderter, Zehner und Einer Stellen aus der Zahl
        /// </summary>
        /// <param name="number">Zahl</param>
        /// <param name="numberlist">Liste mit den Zahlen Objekten</param>
        internal void Calculate(long number, List<NumberToWordBase> numberlist)
        {
            long num = number;

            tausender = RestValue(ref num, 1000);
            hundert = (int)RestValue(ref num, 100);
            zehner = (int)RestValue(ref num, 10);
            einer = (int)RestValue(ref num, 1);

            if (tausender < 1) return;
            NumberToWordBase numbase = (NumberToWordBase)Activator.CreateInstance(this.GetType().BaseType); // Typ der Eltern Klasse erstellen
            numberlist.Add(numbase);
            numbase.Calculate(tausender, numberlist);
        }

        /// <summary>
        /// Dividiert die Zahl und ermittelt den Restwert
        /// </summary>
        /// <param name="number"></param>
        /// <param name="teiler"></param>
        /// <returns></returns>
        private long RestValue(ref long number, int teiler)
        {
            long temp = number;
            long rest = temp % teiler;
            temp -= rest;
            temp /= teiler;
            number = rest;

            return temp;
        }

        /// <summary>
        /// Wandelt die Ziffern in Wörter um
        /// </summary>
        /// <returns></returns>
        private string ConvertToString()
        {
            string output = string.Empty;

            if (hundert == 0 && zehner == 0 && einer == 0) return "";

            if (hundert > 0)
                output += EinerArray[hundert] + typeof(Hundert).Name; // Die Hunderter Ziffer

            if (zehner > 0)
            {
                if (zehner == 1)                            // Die Zehner Ziffer mit ihren Ausnahmen für 11, 12, 17
                {
                    string zehnerstring = string.Empty;

                    if (einer == 1 || einer == 2)                                          // Bei 11 und 12
                        zehnerstring += EinerArray[Convert.ToInt32("" + zehner + einer)];
                    else
                        zehnerstring += EinerArray[einer] + ZehnerArray[zehner];

                    if (einer == 7)
                        zehnerstring = zehnerstring.Replace("en", ""); // Bei Sieben das 'en' für Siebzehn löschen
                    output += zehnerstring;
                }
                else if (einer == 0) output += ZehnerArray[zehner];     // Wenn der Einer 0 ist dann nur der Zehner z.B. 20 = zwansig
                else
                    output += EinerArray[einer] + "und" + ZehnerArray[zehner]; // Alles über 19 und nicht 0 als Einer
            }
            else
            {
                if (this.GetType() == typeof(Hundert) && hundert == 0 && tausender > 0) output += "und";

                // Die Eins ist im Deutsch doch echt zum Kotzen Eine Millionen Ein Tausend Eins
                output += EinerArray[einer];                                     // Wenn der Zehner 0 ist muss der Einer alleine stehen

                if (einer == 1)// Wenn der Einer eine Eins ist :D
                {
                    if (hundert > 0 && zehner == 0 || this.GetType() == typeof(Hundert))
                        output += "s"; // TK

                    else if (this.GetType() != typeof(Tausend) || this.GetType() == typeof(Hundert)) output += "e";
                }
            }

            if (this.GetType() != typeof(Hundert)) output += Bezeichner;  // Den Bezeichner noch dazu, aber nur wenn es nicht Hundert ist

            return output.Trim();
        }

        public override string ToString()
        {
            return ConvertToString();
        }
    }

    #region NumberClasses

    public class Hundert : Tausend
    {
    }

    public class Tausend : Millionen
    {
    }

    public class Millionen : Milliarden
    {
    }

    public class Milliarden : Billionen
    {
    }

    public class Billionen : Billiarden
    {
    }

    public class Billiarden : Trillionen
    {
    }

    public class Trillionen : Trilliarden
    {
    }

    public class Trilliarden : NumberToWordBase
    {
    }
    #endregion
}
