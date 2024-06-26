/*
 * <copyright file="FormPanel.cs" company="Lifeprojects.de">
 *     Class: FormPanel
 *     Copyright � Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>05.05.2024 19:32:56</date>
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
 * Inspiration von
 * https://github.com/nirdobovizki/MvvmControls
*/

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public class LayoutPanel : Panel
    {
        public static readonly DependencyProperty ColumnsProperty =
                    DependencyProperty.Register("Columns", typeof(int), typeof(LayoutPanel), new FrameworkPropertyMetadata(2, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ColumnSpacingProperty =
            DependencyProperty.Register("ColumnSpacing", typeof(double), typeof(LayoutPanel), new FrameworkPropertyMetadata(15.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RowSpacingProperty =
            DependencyProperty.Register("RowSpacing", typeof(double), typeof(LayoutPanel), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty LabelControlSpacingProperty =
            DependencyProperty.Register("LabelControlSpacing", typeof(double), typeof(LayoutPanel), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty LabelSizeProperty = DependencyProperty.Register("LabelSize", typeof(Size), typeof(LayoutPanel));

        public static readonly DependencyProperty LabelControlPairSizeProperty = DependencyProperty.Register("LabelControlPairSize", typeof(Size), typeof(LayoutPanel));

        public static readonly DependencyProperty IsStandaloneProperty = 
            DependencyProperty.RegisterAttached("IsStandalone", typeof(bool), typeof(LayoutPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ControlSizeProperty = DependencyProperty.Register("ControlSize", typeof(Size), typeof(LayoutPanel));

        public static readonly DependencyProperty IsGroupHeaderProperty =
            DependencyProperty.RegisterAttached("IsGroupHeader", typeof(bool), typeof(LayoutPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        public double LabelControlSpacing
        {
            get { return (double)GetValue(LabelControlSpacingProperty); }
            set { SetValue(LabelControlSpacingProperty, value); }
        }

        public Size LabelSize
        {
            get { return (Size)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        public Size ControlSize
        {
            get { return (Size)GetValue(ControlSizeProperty); }
            set { SetValue(ControlSizeProperty, value); }
        }

        public Size LabelControlPairSize
        {
            get { return (Size)GetValue(LabelControlPairSizeProperty); }
            set { SetValue(LabelControlPairSizeProperty, value); }
        }

        public static bool GetIsStandalone(UIElement elem)
        {
            return (bool)elem.GetValue(IsStandaloneProperty);
        }
        public static void SetIsStandalone(UIElement elem, bool value)
        {
            elem.SetValue(IsStandaloneProperty, value);
        }

        public static bool GetIsGroupHeader(UIElement elem)
        {
            return (bool)elem.GetValue(IsGroupHeaderProperty);
        }
        public static void SetIsGroupHeader(UIElement elem, bool value)
        {
            elem.SetValue(IsGroupHeaderProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var actualColumns = Math.Max(1, Columns);

            int currentType = 0; // 0-label, 1-control, 2-standalone, 3-header
            double labelMaxWidth = 0;
            double rowMaxHeight = 0;
            double headMaxWidth = 0;
            double headTotalHeight = 0;
            bool inItem = false;
            int currentColumn = 0;
            int currentRow = 0;
            var controls = new List<UIElement>();
            var standalones = new List<UIElement>();
            for (int i = 0; i < Children.Count; ++i)
            {
                if (GetIsGroupHeader(Children[i])) currentType = 3;
                else if (Children.Count == 1 || (i == Children.Count - 1 && !inItem)) currentType = 2;
                else if (GetIsStandalone(Children[i])) currentType = 2;

                switch (currentType)
                {
                    case 0:
                        Children[i].Measure(availableSize);
                        labelMaxWidth = Math.Max(labelMaxWidth, Children[i].DesiredSize.Width);
                        rowMaxHeight = Math.Max(rowMaxHeight, Children[i].DesiredSize.Height);
                        currentType = 1;
                        inItem = true;
                        break;
                    case 1:
                        controls.Add(Children[i]);
                        currentType = 0;
                        inItem = false;
                        ++currentColumn;
                        break;
                    case 2:
                        standalones.Add(Children[i]);
                        currentType = 0;
                        ++currentColumn;
                        break;
                    case 3:
                        Children[i].Measure(availableSize);
                        if (currentColumn != 0)
                        {
                            currentColumn = 0;
                            currentRow++;
                        }
                        headMaxWidth = Math.Max(headMaxWidth, Children[i].DesiredSize.Width);
                        headTotalHeight += Children[i].DesiredSize.Height;
                        currentType = 0;
                        break;
                }

                if (currentColumn == actualColumns)
                {
                    currentColumn = 0;
                    ++currentRow;
                }
            }

            double columnContentWidth = Math.Max(0, (availableSize.Width - (actualColumns - 1) * ColumnSpacing) / actualColumns);
            double maxDesiredStandaloneWidth = 0;
            foreach (var current in standalones)
            {
                current.Measure(new Size(columnContentWidth, availableSize.Height));
                rowMaxHeight = Math.Max(rowMaxHeight, current.DesiredSize.Height);
                maxDesiredStandaloneWidth = Math.Max(maxDesiredStandaloneWidth, current.DesiredSize.Width);
            }

            double controlWidth = Math.Max(0, columnContentWidth - labelMaxWidth - LabelControlSpacing);
            double maxDesiredControlWidth = 0;
            foreach (var current in controls)
            {
                current.Measure(new Size(controlWidth, availableSize.Height));
                rowMaxHeight = Math.Max(rowMaxHeight, current.DesiredSize.Height);
                maxDesiredControlWidth = Math.Max(maxDesiredControlWidth, current.DesiredSize.Width);
            }

            // this prevents infinite sizes in a horizontal stack panel
            if (double.IsInfinity(columnContentWidth))
            {
                columnContentWidth = Math.Max(0, Math.Max(maxDesiredStandaloneWidth, labelMaxWidth + LabelControlSpacing + maxDesiredControlWidth));
            }

            if (currentColumn != 0)
            {
                ++currentRow;
            }

            switch (Children.Count)
            {
                case 0:
                    return Size.Empty;
                case 1:
                    return new Size(availableSize.Width, rowMaxHeight);
            }

            LabelSize = new Size(labelMaxWidth, rowMaxHeight);
            ControlSize = new Size(controlWidth, rowMaxHeight);

            LabelControlPairSize = new Size(
                columnContentWidth,
                rowMaxHeight);

            return new Size(
                Math.Min(availableSize.Width, Math.Max(actualColumns * LabelControlPairSize.Width + (actualColumns - 1) * ColumnSpacing, headMaxWidth)),
                currentRow * LabelControlPairSize.Height + (currentRow - 1) * RowSpacing + headTotalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var actualColumns = Math.Max(1, Columns);

            switch (Children.Count)
            {
                case 0:
                    return finalSize;
                case 1:
                    Children[0].Arrange(new Rect(finalSize));
                    return finalSize;
            }
            double rowFullHeight = LabelControlPairSize.Height + RowSpacing;
            double columnContentWidth = Math.Max(0, (finalSize.Width - (actualColumns - 1) * ColumnSpacing) / actualColumns);
            double controlWidth = Math.Max(0, columnContentWidth - LabelSize.Width - LabelControlSpacing);
            double columnFullWidth = columnContentWidth + ColumnSpacing;

            int currentType = 0; // 0-label, 1-control, 2-standalone;
            bool inPair = false;
            int currentColumn = 0;
            double yPos = 0;
            for (int i = 0; i < Children.Count; ++i)
            {
                if (GetIsGroupHeader(Children[i]))
                {
                    currentType = 3;
                }
                else if (Children.Count == 1 || (i == Children.Count - 1 && !inPair)) currentType = 2;
                else if (GetIsStandalone(Children[i]))
                {
                    currentType = 2;
                    inPair = false;
                }
                switch (currentType)
                {
                    case 0:
                        {
                            var labelRect = new Rect(
                                columnFullWidth * currentColumn, yPos,
                                LabelSize.Width, rowFullHeight - RowSpacing);
                            Children[i].Arrange(
                                new Rect(
                                    labelRect.Left,
                                    labelRect.Top + (labelRect.Height - Children[i].DesiredSize.Height) / 2,
                                    Children[i].DesiredSize.Width, Children[i].DesiredSize.Height));
                            currentType = 1;
                            inPair = true;
                        }
                        break;
                    case 1:
                        {
                            var ctrlRect = new Rect(
                                Math.Max(0, columnFullWidth * currentColumn + LabelSize.Width + LabelControlSpacing),
                                yPos,
                                controlWidth,
                                rowFullHeight - RowSpacing);
                            Children[i].Arrange(ctrlRect);
                            currentType = 0;
                            inPair = false;
                            ++currentColumn;
                        }
                        break;
                    case 2:
                        {
                            var pairRect = new Rect(
                                columnFullWidth * currentColumn,
                                yPos,
                                columnContentWidth,
                                rowFullHeight - RowSpacing);
                            Children[i].Arrange(pairRect);
                            currentType = 0;
                            ++currentColumn;
                        }
                        break;
                    case 3:
                        {
                            if (currentColumn != 0)
                            {
                                currentColumn = 0;
                                yPos += rowFullHeight;
                            }
                            Children[i].Arrange(new Rect(0, yPos, finalSize.Width, Children[i].DesiredSize.Height));
                            yPos += Children[i].DesiredSize.Height;
                            currentType = 0;
                        }
                        break;
                }

                if (currentColumn == actualColumns)
                {
                    currentColumn = 0;
                    yPos += rowFullHeight;
                }

            }
            return new Size(finalSize.Width, yPos);
        }
    }
}
