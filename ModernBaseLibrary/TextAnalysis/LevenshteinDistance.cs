/*
 * <copyright file="LevenshteinDistance.cs" company="Lifeprojects.de">
 *     Class: LevenshteinDistance
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>04.04.2020</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse zum Näherungsvergleich von Worten/Token auf Basis der LevenshteinDistance
 * </summary>
 *
 * <WebSite>
 * https://de.wikipedia.org/wiki/Levenshtein-Distanz#:~:text=Die%20Levenshtein%2DDistanz%20(auch%20Editierdistanz,Zeichenkette%20in%20die%20zweite%20umzuwandeln.
 * </WebSite>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Text
{
    using System;

    public class LevenshteinDistance
    {
        /// <summary>
        /// Levenshteins the distance.
        /// </summary>
        /// <param name="pText">The pText.</param>
        /// <param name="pCompareText">The pCompareText.</param>
        /// <returns></returns>
        public static int Calculate(string sourceString, string compareString)
        {
            var source1Length = sourceString.Length;
            var source2Length = compareString.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
            {
                return source2Length;
            }

            if (source2Length == 0)
            {
                return source1Length;
            }

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (compareString[j - 1] == sourceString[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[source1Length, source2Length];
        }

        /// <summary>
        /// Checks the femal or male.
        /// </summary>
        /// <param name="pText">The pText.</param>
        /// <returns></returns>
        public static string IsFemalMale(string pText)
        {
            //additive Mädels
            string[] f1 = { "a", "e", "i", "n", "y" };
            string[] f2 = { "ah", "al", "bs", "dl", "el", "et", "id", "il", "it", "ll", "th", "ud", "uk" };
            string[] f3 = {"ary", "aut", "des", "een", "eig", "ett",
                              "fer", "got", "ies", "ild", "ind", "itt", "jam", "joy", "kim",
                              "lar", "len", "lis", "men", "mor", "oan", "ren", "res", "rix",
                              "san", "tas", "udy", "urg", "vig"};
            string[] f4 = {"ardi", "atie", "borg", "cole", "endy",
                              "gard", "gart", "gnes", "gund",
                              "iede", "indy", "ines", "iris", "ison", "istl",
                              "ldie", "lilo", "loni", "lott", "lynn", "mber", "moni", "nken",
                              "oldy", "riam", "rien", "sann", "smin", "ster", "uste", "vien"};
            string[] f5 = {"achel", "agmar", "almut", "Candy", "Doris",
                              "echen", "edwig", "irene", "mandy", "rauke", "sandy", "silja",
                              "sther", "uriel", "velin", "vroni", "ybill"};
            string[] f6 = { "irsten", "almuth" };

            //subtraktive Jungs
            string[] v1 = {"ai", "an", "ay", "dy", "en", "ey", "fa", "gi",
                              "hn", "iy", "ki", "nn", "oy", "pe", "ri", "ry", "ua", "uy", "ve", "we", "zy"};
            string[] v2 = {"ael", "ali", "aid", "ain", "are", "bal", "bby",
                              "bin", "cal", "cca", "cel", "cil", "cin", "die", "don", "dre",
                              "ede", "eil", "eit", "emy", "eon", "ffer", "gon", "gun",
                              "hal", "hel", "hil", "hka", "iel", "ill", "ini", "kie",
                              "lge", "lon", "lte", "lja", "mal", "met", "mil", "min", "mon", "mre",
                              "mud", "muk", "nid", "nsi", "oah", "obi", "oel", "örn", "ole", "oni",
                              "oly", "phe", "pit", "rcy", "rdi", "rel", "rge", "rka", "ron", "rne",
                              "rre", "rti", "sil", "son", "sse", "ste", "tie", "ton", "uce", "udi",
                              "uel", "uli", "uke", "vel", "vid", "vin", "wel", "win", "xei", "xel"};

            //subtraktive Jungs zwei
            string[] p1 = {"abel", "akim", "atti", "dres", "eith", "elin",
                           "ence", "erin", "ffer", "frid", "gary", "gene", "hane", "hein",
                           "idel", "iete", "irin", "jona", "kind", "kita", "kola", "lion",
                           "levi", "mike", "muth", "naud", "neth", "nnie", "ntin", "nuth",
                           "olli", "ommy", "önke", "ören", "pete", "rene", "ries", "rlin",
                           "rome", "rtin", "stas", "tell", "tila", "tony", "tore", "uele"};
            string[] p2 = {"astel", "benny", "billy", "billi", "elice",
                              "ianni", "laude", "danny", "dolin", "ormen", "ronny", "ustel",
                              "ustin", "willi", "willy"};
            string[] p3 = { "jascha", "tienne", "vester" };

            // Zwitter Vornamen ???
            string[] x1 = {"Alexis", "Auguste", "Carol", "Chris", "Conny",
                              "Dominique", "Eike", "Folke", "Gabriele", "Gerrit", "Heilwig",
                              "Jean", "Kay", "Kersten", "Kim", "Leslie", "Maris", "Maxime",
                              "Nicola", "Nikola", "Patrice", "Sandy", "Sascha", "Toni", "Winnie"};

            string result = "";
            bool w1 = false;
            bool w2 = false;
            bool w3 = false;
            bool w4 = false;
            int av = -1;
            string vSub = "";

            for (int i = pText.Length - 1; i >= 0; i--)
            {
                vSub = pText.Substring(pText.Length - i, i);
                switch (i)
                {
                    case 4:
                        av = Array.IndexOf(p1, vSub);
                        w3 = w3 ^ (av >= 0);
                        break;
                    case 5:
                        av = Array.IndexOf(p2, vSub);
                        w3 = w3 ^ (av >= 0);
                        break;
                    case 6:
                        av = Array.IndexOf(p3, vSub);
                        w3 = w3 ^ (av >= 0);
                        break;
                }
            }

            for (int i = pText.Length - 1; i >= 0; i--)
            {
                vSub = pText.Substring(pText.Length - i, i);
                switch (i)
                {
                    case 2:
                        av = Array.IndexOf(v1, vSub);
                        w2 = w2 ^ (av >= 0);
                        break;
                    case 3:
                        av = Array.IndexOf(v2, vSub);
                        w2 = w2 ^ (av >= 0);
                        break;
                }
            }

            for (int i = pText.Length - 1; i >= 0; i--)
            {
                vSub = pText.Substring(pText.Length - i, i);
                switch (i)
                {
                    case 1:
                        av = Array.IndexOf(f1, vSub);
                        w1 = w1 ^ (av >= 0);
                        break;
                    case 2:
                        av = Array.IndexOf(f2, vSub);
                        w1 = w1 ^ (av >= 0);
                        break;
                    case 3:
                        av = Array.IndexOf(f3, vSub);
                        w1 = w1 ^ (av >= 0);
                        break;
                    case 4:
                        av = Array.IndexOf(f4, vSub);
                        w1 = w1 ^ (av >= 0);
                        break;
                    case 5:
                        av = Array.IndexOf(f5, vSub);
                        w1 = w1 ^ (av >= 0);
                        break;
                    case 6:
                        av = Array.IndexOf(f6, vSub);
                        w1 = w1 ^ (av >= 0);
                        break;
                }
            }
            w1 = w1 ^ w2 ^ w3;
            av = Array.IndexOf(x1, pText);
            w4 = (av >= 0);
            if (w4 == true)
                result = "???";
            else
                if (w1 == true)
                result = "femal";

            else
                result = "male";
            return (result);
        }
    }
}
