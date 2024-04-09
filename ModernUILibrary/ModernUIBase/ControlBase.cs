/*
 * <copyright file="ControlBase.cs" company="Lifeprojects.de">
 *     Class: ControlBase
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>19.02.2024 17:18:52</date>
 * <Project>CurrentProject</Project>
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

namespace ModernIU.Base
{
    using System.Windows;
    using System.Windows.Media;

    public static class ControlBase
    {
        static ControlBase()
        {
            FontFamily = new FontFamily("Segoe UI");
            FontSize = 12.0d;
            MinHeight = 18.0;
            DefaultHeight = 23.0;
            DefaultMargin = new Thickness(2);
            BorderThickness = new Thickness(1);
            BorderBrush = Brushes.Green;
            ReadOnlyColor = Brushes.LightYellow;
        }

        public static FontFamily FontFamily { get; set; }

        public static double FontSize { get; set; }

        public static double MinHeight { get; set; }

        public static double DefaultHeight { get; set; }

        public static Thickness DefaultMargin { get; set; }

        public static Thickness BorderThickness { get; set; }

        public static Brush BorderBrush { get; set; }

        public static Brush ReadOnlyColor { get; set; }
    }
}
