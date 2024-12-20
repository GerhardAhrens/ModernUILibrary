//-----------------------------------------------------------------------
// <copyright file="ColorNameGerman.cs" company="Lifeprojects.de">
//     Class: ColorNameGerman
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2024 11:20:17</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System.Collections.Generic;
    using System.Drawing;

    public static class ColorNameGerman
    {
        /// <summary>
        /// Get the user-readable German name.
        /// </summary>
        /// <param name="color">The color to get the German name for.</param>
        /// <returns>Returns the German name of the English name if none found.</returns>
        public static string GetGermanName(Color color)
        {
            var hc = ToHex(color).ToUpperInvariant();

            string r;
            if (Names.TryGetValue(hc, out r))
            {
                return r;
            }
            else if (color.IsNamedColor)
            {
                return color.Name;
            }
            else
            {
                return '#' + hc;
            }
        }

        public static string GetGermanName(System.Windows.Media.Color color)
        {
            var hc = ToHex(color).ToUpperInvariant();

            string r;
            if (Names.TryGetValue(hc, out r))
            {
                return r;
            }
            else
            {
                return '#' + hc;
            }
        }

        public static string GetGermanName(System.Windows.Media.Brush color)
        {
            var hc = ToHex(color).ToUpperInvariant();

            string r;
            if (Names.TryGetValue(hc, out r))
            {
                return r;
            }
            else
            {
                return '#' + hc;
            }
        }

        /// <summary>
        /// The hard-coded names, based on http://farbe-computer.de/kapitel33.html,
        /// removed all the duplicates from the original list.
        /// </summary>
        private static readonly Dictionary<string, string> Names =
            new Dictionary<string, string>
            {
                {@"FAEBD7", "Antikweiß"},
                {@"7FFFD4", "Aquamarin"},
                {@"F0FFFF", "Azurblau"},
                {@"F5F5DC", "Beige"},
                {@"FFE4C4", "Biskuit / Porzellanfarben"},
                {@"000000", "Schwarz"},
                {@"FFEBCD", "Mandelweiß"},
                {@"0000FF", "Blau"},
                {@"8A2BE2", "Blauviolet"},
                {@"A52A2A", "Braun"},
                {@"DEB887", "Stammholz"},
                {@"5F9EA0", "Kadettenblau"},
                {@"7FFF00", "Grüngelb"},
                {@"D2691E", "Schokolade"},
                {@"FF7F50", "Koralle"},
                {@"6495ED", "Kornblumenblau"},
                {@"FFF8DC", "Kornseide"},
                {@"DC143C", "Karmesinrot / Blutrot"},
                {@"00FFFF", "Cyan"},
                {@"00008B", "Dunkelblau"},
                {@"008B8B", "Dunkelcyan"},
                {@"B8860B", "Dunkelgoldrute"},
                {@"A9A9A9", "Dunkelgrau"},
                {@"006400", "Dunkelgrün"},
                {@"BDB76B", "Dunkelkhaki"},
                {@"8B008B", "Dunkelmagenta"},
                {@"556B2F", "Dunkelolivengrün"},
                {@"FF8C00", "Dunkelorange"},
                {@"9932CC", "Dunkelorchideefarben"},
                {@"8B0000", "Dunkelrot"},
                {@"E9967A", "Dunkellachsfarben"},
                {@"8FBC8F", "Dunkelmeergrün"},
                {@"483D8B", "Dunkelschieferblau"},
                {@"2F4F4F", "Dunkelschiefergrau"},
                {@"00CED1", "Dunkeltürkis"},
                {@"9400D3", "Dunkelviolet"},
                {@"FF1493", "Dunkelrosarot"},
                {@"00BFFF", "Dunkeles Himmelblau"},
                {@"696969", "Dunkelgrau"},
                {@"1E90FF", "Persenningblau"},
                {@"B22222", "Ziegelfarben"},
                {@"FFFAF0", "Blumenweiß"},
                {@"228B22", "Waldgrün"},
                {@"FF00FF", "Fuchsie"},
                {@"DCDCDC", "Gainsboro"},
                {@"F8F8F8", "Geisterweiß"},
                {@"FFD700", "Gold"},
                {@"DAA520", "Goldrute"},
                {@"808080", "Grau"},
                {@"008000", "Grün"},
                {@"ADFF2F", "Grüngelb"},
                {@"F0FFF0", "Honigmelone"},
                {@"FF69B4", "Knallrosa"},
                {@"CD5C5C", "Indischrot"},
                {@"4B0082", "Indigo"},
                {@"FFFFF0", "Elfenbein"},
                {@"F0E68C", "Khaki"},
                {@"E6E6FA", "Lavendelfarben"},
                {@"FFF0F5", "Lavendelrot"},
                {@"7CFC00", "Rasengrün"},
                {@"FFFACD", "Zitronenchiffon"},
                {@"ADD8E6", "Hellblau"},
                {@"F08080", "Hellkorall"},
                {@"E9FFFF", "Hellcyan"},
                {@"FAFAD2", "Helles Goldrutengelb"},
                {@"90EE90", "Hellgrün"},
                {@"D3D3D3", "Hellgrau"},
                {@"FFB6C1", "Hellpink"},
                {@"FFA07A", "Helllachsfarben"},
                {@"20B2AA", "Helles Meergrün"},
                {@"87CEFA", "Helles Himmelblau"},
                {@"778899", "Helles Schiefergrau"},
                {@"B0C4DE", "Helles Stahlblau"},
                {@"FFFFE0", "Hellgelb"},
                {@"00FF00", "Limone"},
                {@"32CD32", "Limonengrün"},
                {@"FAF0E6", "Leinen"},
                {@"800000", "Kastanienbraun"},
                {@"66CDAA", "Mittleres Aquamarin"},
                {@"0000CD", "Mittleres Blau"},
                {@"BA55D3", "Mittel Orchideenfarben"},
                {@"9370DB", "Mittleres Purpur"},
                {@"3CB371", "Mittleres Meergrün"},
                {@"7B68EE", "Mittleres Schieferblau"},
                {@"00FA9A", "Mittleres Frühlingsgrün"},
                {@"48D1CC", "Mittleres Türkis"},
                {@"C71585", "Mittleres Violetrot"},
                {@"191970", "Mitternachtsblau"},
                {@"F5FFFA", "Pfefferminzcreme"},
                {@"FFE4E1", "Blaß Rosenfarben"},
                {@"FFE4B5", "Mokassin"},
                {@"FFDEAD", "Navajoweiß"},
                {@"000080", "Marineblau"},
                {@"FDF5E6", "Alte Spitze"},
                {@"808000", "Olive"},
                {@"6B8E23", "Olivengraubraun"},
                {@"FFA500", "Orange"},
                {@"FF4500", "Orangenrot"},
                {@"DA70D6", "Orchideenfarben"},
                {@"EEE8AA", "Blasse Goldrute"},
                {@"98FB98", "Blassgrün"},
                {@"AFEEEE", "Blasstürkis"},
                {@"DB7093", "Blassvioletrot"},
                {@"FFEFD5", "Papayacreme"},
                {@"FFDAB9", "Pfirsich"},
                {@"CD853F", "Peru"},
                {@"FFC0CB", "Pink"},
                {@"DDA0DD", "Pflaumenfarben"},
                {@"B0E0E6", "Pulverblau"},
                {@"800080", "Purpur"},
                {@"FF0000", "Rot"},
                {@"BC8F8F", "Rosiges Braun"},
                {@"4169E1", "Königsblau"},
                {@"8B4513", "Sattelbraun"},
                {@"FA8072", "Lachsfarben"},
                {@"F4A460", "Sandbraun"},
                {@"2E8B57", "Meergrün"},
                {@"FFF5EE", "Seemuschel"},
                {@"A0522D", "Sienaerde"},
                {@"C0C0C0", "Silber"},
                {@"87CEEB", "Himmelblau"},
                {@"6A5ACD", "Schieferblau"},
                {@"708090", "Schiefergrau"},
                {@"FFFAFA", "Schneefarben"},
                {@"00FF7F", "Frühlingsgrün"},
                {@"4682B4", "Stahlblau"},
                {@"D2B48C", "Gerbbraun"},
                {@"008080", "Blaugrün"},
                {@"D8BFD8", "Distelfarben"},
                {@"FF6347", "Tomatenfarben"},
                {@"40E0D0", "Türkisfarben"},
                {@"EE82EE", "Violett"},
                {@"F5DEB3", "Weizenfarben"},
                {@"FFFFFF", "Weiß"},
                {@"F5F5F5", "Rauchfarben"},
                {@"FFFF00", "Gelb"},
                {@"9ACD32", "Gelbgrün"},
            };

        private static string ToHex(Color c)
        {
            return string.Format(@"{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
        }

        private static string ToHex(System.Windows.Media.Color c)
        {
            return string.Format(@"{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
        }

        private static string ToHex(System.Windows.Media.Brush colorBrush)
        {
            System.Windows.Media.BrushConverter convBrush = new System.Windows.Media.BrushConverter();
            System.Windows.Media.SolidColorBrush brush = convBrush.ConvertFromString(colorBrush.ToString()) as System.Windows.Media.SolidColorBrush;

            if (brush is System.Windows.Media.SolidColorBrush solidBrush)
            {
                System.Windows.Media.Color co = solidBrush.Color;
                return string.Format(@"{0:X2}{1:X2}{2:X2}", co.R, co.G, co.B);
            }

            return string.Empty;
        }
    }
}
