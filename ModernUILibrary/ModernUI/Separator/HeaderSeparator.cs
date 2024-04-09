/*
 * <copyright file="TextSeparator.cs" company="Lifeprojects.de">
 *     Class: TextSeparator
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>09.04.2024</date>
 * <Project>ModernUILibrary</Project>
 * <FrameworkVersion>8.0</FrameworkVersion>
 *
 * <summary>
 * Das Control erstellt eine horizontale Linie mit einem Text
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

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class HeaderSeparator : ContentControl
    {
        public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(HeaderSeparator));

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderSeparator"/> class.
        /// </summary>
        static HeaderSeparator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderSeparator), new FrameworkPropertyMetadata(typeof(HeaderSeparator)));
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}
