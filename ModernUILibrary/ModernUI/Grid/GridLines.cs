//-----------------------------------------------------------------------
// <copyright file="GridEx.cs" company="Lifeprojects.de">
//     Class: GridEx
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>22.11.2018</date>
//
// <summary>Class for UI Control GridLines</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class GridEx : Grid
    {
        public static readonly DependencyProperty GridLinesShowProperty =
            DependencyProperty.Register("GridLinesShow", typeof(bool), typeof(GridEx), new UIPropertyMetadata(true));

        public static readonly DependencyProperty GridLinesVisibilityProperty =
            DependencyProperty.Register("GridLinesVisibility", typeof(GridLinesVisibility), typeof(GridEx), new UIPropertyMetadata(GridLinesVisibility.Both));

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(GridEx), new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register("GridLineThickness", typeof(double), typeof(GridEx), new UIPropertyMetadata(1.0));

        static GridEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridEx), new FrameworkPropertyMetadata(typeof(GridEx)));
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
        }    }
}