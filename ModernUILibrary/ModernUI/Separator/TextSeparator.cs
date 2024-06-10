/*
 * <copyright file="TextSeparator.cs" company="Lifeprojects.de">
 *     Class: TextSeparator
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>03.10.2023 14:36:54</date>
 * <Project>Git-Projekt</Project>
 * <FrameworkVersion>7.0</FrameworkVersion>
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

    public class TextSeparator : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextSeparator"/> class.
        /// </summary>
        static TextSeparator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextSeparator), new FrameworkPropertyMetadata(typeof(TextSeparator)));
        }

        public static DependencyProperty SeparatorAlignmentProperty = DependencyProperty.Register(nameof(SeparatorAlignment), typeof(string), typeof(TextSeparator), new PropertyMetadata("C", OnTextAlignmentPropertyChange));

        public static DependencyProperty WidthLeftProperty = DependencyProperty.Register(nameof(WidthLeft), typeof(double?), typeof(TextSeparator), new PropertyMetadata(null, OnWidthLeftPropertyChange));

        public double? WidthLeft
        {
            get { return (double)GetValue(WidthLeftProperty); }
            set { SetValue(WidthLeftProperty, value); }
        }

        public string SeparatorAlignment
        {
            get { return (string)GetValue(SeparatorAlignmentProperty); }
            set { SetValue(SeparatorAlignmentProperty, value); }
        }

        private static void OnTextAlignmentPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextSeparator)d;
                string alignment  = (string)e.NewValue;
                if (alignment.ToLower() == "l")
                {
                    control.WidthLeft = 10;
                }
                else if (alignment.ToLower() == "r")
                {
                    control.WidthLeft = null;
                }
                else if (alignment.ToLower() == "c")
                {
                    control.WidthLeft = null;
                }
            }
        }

        private static void OnWidthLeftPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextSeparator)d;
                control.WidthLeft = Convert.ToDouble(e.NewValue);
            }
        }
    }
}
