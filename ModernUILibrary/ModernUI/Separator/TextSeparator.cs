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

        public static DependencyProperty SeparatorAlignmentProperty = DependencyProperty.Register(nameof(SeparatorAlignment), typeof(HorizontalSeparatorAlignment), typeof(TextSeparator), new PropertyMetadata(HorizontalSeparatorAlignment.Center, OnTextAlignmentPropertyChange));

        public static DependencyProperty WidthLeftProperty = DependencyProperty.Register(nameof(WidthLeft), typeof(GridLength), typeof(TextSeparator), new PropertyMetadata(new GridLength(1, GridUnitType.Star), OnWidthLeftPropertyChange));

        public static DependencyProperty WidthRightProperty = DependencyProperty.Register(nameof(WidthRight), typeof(GridLength), typeof(TextSeparator), new PropertyMetadata(new GridLength(1,GridUnitType.Star), OnWidthRightPropertyChange));

        public GridLength WidthLeft
        {
            get { return (GridLength)GetValue(WidthLeftProperty); }
            set { SetValue(WidthLeftProperty, value); }
        }

        public GridLength WidthRight
        {
            get { return (GridLength)GetValue(WidthRightProperty); }
            set { SetValue(WidthRightProperty, value); }
        }

        public HorizontalSeparatorAlignment SeparatorAlignment
        {
            get { return (HorizontalSeparatorAlignment)GetValue(SeparatorAlignmentProperty); }
            set { SetValue(SeparatorAlignmentProperty, value); }
        }

        private static void OnTextAlignmentPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextSeparator)d;
                HorizontalSeparatorAlignment alignment  = (HorizontalSeparatorAlignment)e.NewValue;
                if (alignment == HorizontalSeparatorAlignment.Left)
                {
                    control.WidthRight = new GridLength(1, GridUnitType.Star);
                    control.WidthLeft = new GridLength(5);
                }
                else if (alignment == HorizontalSeparatorAlignment.Right)
                {
                    control.WidthRight = new GridLength(5);
                    control.WidthLeft = new GridLength(1,GridUnitType.Star);
                }
                else if (alignment == HorizontalSeparatorAlignment.Center)
                {
                    control.WidthLeft = new GridLength(1,GridUnitType.Star);
                    control.WidthRight = new GridLength(1,GridUnitType.Star);
                }
            }
        }

        private static void OnWidthLeftPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextSeparator)d;
                control.WidthLeft = (GridLength)e.NewValue;
            }
        }

        private static void OnWidthRightPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextSeparator)d;
                control.WidthRight = (GridLength)e.NewValue;
            }
        }
    }

    public enum HorizontalSeparatorAlignment : int
    {
        None = 0,
        Left = 1,
        Center = 2,
        Right = 3,
    }
}
