//-----------------------------------------------------------------------
// <copyright file="SimpleGrid.cs" company="Lifeprojects.de">
//     Class: SimpleGrid
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>22.08.2019</date>
//
// <summary>
// Class for UI Control SimpleGrid
// </summary>
// < Website >
// https://thomaslevesque.com/tag/wpf/page/2/
// </ Website >
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class SimpleGrid : Grid
    {
        private GridLengthCollection _rows;
        private GridLengthCollection _columns;

        public static readonly DependencyProperty GridLinesShowProperty = DependencyProperty.Register("GridLinesShow", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(false));

        public static readonly DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(GridLinesVisibility), typeof(SimpleGrid), new UIPropertyMetadata(GridLinesVisibility.Both));

        public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(SimpleGrid), new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register("GridLineThickness", typeof(double), typeof(SimpleGrid), new UIPropertyMetadata(1.0));

        static SimpleGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleGrid), new FrameworkPropertyMetadata(typeof(SimpleGrid)));
        }


        public GridLengthCollection Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                RowDefinitions.Clear();
                if (_rows == null)
                    return;
                foreach (var length in _rows)
                {
                    RowDefinitions.Add(new RowDefinition { Height = length });
                }
            }
        }

        public GridLengthCollection Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                if (_columns == null)
                    return;
                ColumnDefinitions.Clear();
                foreach (var length in _columns)
                {
                    ColumnDefinitions.Add(new ColumnDefinition { Width = length });
                }
            }
        }

        public bool GridLinesShow
        {
            get { return (bool)GetValue(GridLinesShowProperty); }
            set { SetValue(GridLinesShowProperty, value); }
        }

        public GridLinesVisibility GridLinesVisibility
        {
            get { return (GridLinesVisibility)GetValue(GridLinesVisibilityProperty); }
            set { SetValue(GridLinesVisibilityProperty, value); }
        }

        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (GridLinesShow == true)
            {
                if (GridLinesVisibility == GridLinesVisibility.Both)
                {
                    foreach (var rowDefinition in RowDefinitions)
                    {
                        dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(0, rowDefinition.Offset), new Point(ActualWidth, rowDefinition.Offset));
                    }

                    foreach (var columnDefinition in ColumnDefinitions)
                    {
                        dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(columnDefinition.Offset, 0), new Point(columnDefinition.Offset, ActualHeight));
                    }

                    //dc.DrawRectangle(Brushes.Transparent, new Pen(GridLineBrush, GridLineThickness), new Rect(0, 0, ActualWidth, ActualHeight));
                }
                else if (GridLinesVisibility == GridLinesVisibility.Vertical)
                {
                    foreach (var columnDefinition in ColumnDefinitions)
                    {
                        dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(columnDefinition.Offset, 0), new Point(columnDefinition.Offset, ActualHeight));
                    }

                    //dc.DrawRectangle(Brushes.Transparent, new Pen(GridLineBrush, GridLineThickness), new Rect(0, 0, ActualWidth, ActualHeight));
                }
                else if (GridLinesVisibility == GridLinesVisibility.Horizontal)
                {
                    foreach (var rowDefinition in RowDefinitions)
                    {
                        dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(0, rowDefinition.Offset), new Point(ActualWidth, rowDefinition.Offset));
                    }

                    //dc.DrawRectangle(Brushes.Transparent, new Pen(GridLineBrush, GridLineThickness), new Rect(0, 0, ActualWidth, ActualHeight));
                }
            }

            base.OnRender(dc);
        }
    }

    [TypeConverter(typeof(GridLengthCollectionConverter))]
    public class GridLengthCollection : ReadOnlyCollection<GridLength>
    {
        public GridLengthCollection(IList<GridLength> lengths) : base(lengths)
        {
        }
    }

    public class GridLengthCollectionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string s = value as string;
            if (s != null)
                return ParseString(s, culture);
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is GridLengthCollection)
                return ToString((GridLengthCollection)value, culture);
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private string ToString(GridLengthCollection value, CultureInfo culture)
        {
            var converter = new GridLengthConverter();
            return string.Join(",", value.Select(v => converter.ConvertToString(v)));
        }

        private GridLengthCollection ParseString(string s, CultureInfo culture)
        {
            var converter = new GridLengthConverter();
            var lengths = s.Split(',').Select(p => (GridLength)converter.ConvertFromString(p.Trim()));
            return new GridLengthCollection(lengths.ToArray());
        }
    }
}