//-----------------------------------------------------------------------
// <copyright file="IntToColorHexConverter.cs" company="Lifeprojects.de">
//     Class: ImageFromAwesomeFont
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.06.2017</date>
//
// <summary>
// IntToColorHexConverter Converter Class
// </summary>
//
// <example>
// IntToColorHexConverter x:Key="intToColorHexConverter" 
// Value="{Binding Path=Flags, Converter={StaticResource intColorConverter}}"
// </example>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System.Globalization;
    using System.Windows.Data;

    public sealed class IntToColorHexConverter : ConverterBase<int, string>
    {
        protected override string Convert(int value, CultureInfo culture)
        {
            string hexColor = string.Empty;

            int numColor = 0;
            if (int.TryParse(value.ToString(), out numColor) == true)
            {
                if (numColor == 0)
                {
                    hexColor = "#00FFFFFF";
                }
                else
                {
                    string result = System.Drawing.Color.FromArgb(numColor).Name;
                    hexColor = string.Format($"#{result}");
                }
            }

            return hexColor.ToUpper();
        }
    }
}