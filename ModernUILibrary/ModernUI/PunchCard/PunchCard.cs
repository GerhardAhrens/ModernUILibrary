namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class PunchCard : Canvas
    {
        private const int NumberOfHours = 24;

        private double punchCardRenderWidth;
        private double punchCardRenderHeight;
        private double hourWidth;
        private double categoryHeight;
        private double countDiameterMultiplier;
        private readonly Canvas toolTipLayer;
        private long numberOfCategories;

        public static readonly DependencyProperty DataProperty = 
            DependencyProperty.Register("Data", typeof(List<Tuple<string, List<int>>>), typeof(PunchCard));

        public static readonly DependencyProperty LabelMarginProperty = 
            DependencyProperty.Register("LabelMargin", typeof(double), typeof(PunchCard), new FrameworkPropertyMetadata(30.0) { AffectsRender = true });

        public static readonly DependencyProperty CategoryLinePenProperty =
            DependencyProperty.Register("CategoryLinePen", typeof(Pen), typeof(PunchCard), new FrameworkPropertyMetadata(new Pen(Brushes.DarkGray, 0.5)) { AffectsRender = true });

        public static readonly DependencyProperty HourMarkerPenProperty =
            DependencyProperty.Register("HourMarkerPen", typeof(Pen), typeof(PunchCard), new FrameworkPropertyMetadata(new Pen(Brushes.LightGray, 0.5)) { AffectsRender = true });

        public static readonly DependencyProperty ToolTipsProperty =
            DependencyProperty.Register("ToolTips", typeof(bool), typeof(PunchCard), new FrameworkPropertyMetadata(true) { AffectsRender = true });

        public static readonly DependencyProperty PunchColorProperty =
    DependencyProperty.Register("PunchColor", typeof(Brush), typeof(PunchCard), new FrameworkPropertyMetadata(Brushes.Green) { AffectsRender = true });

        public PunchCard()
        {
            this.toolTipLayer = new Canvas {Background = Brushes.Transparent};
            Binding widthBinding = new Binding("ActualWidth") {RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (PunchCard), 1)};
            this.toolTipLayer.SetBinding(WidthProperty, widthBinding);
            Binding heightBinding = new Binding("ActualWidth") { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(PunchCard), 1) };
            this.toolTipLayer.SetBinding(HeightProperty, heightBinding);

            this.Children.Add(this.toolTipLayer);
        }

        public List<Tuple<string, List<int>>> Data
        {
            get { return (List<Tuple<string, List<int>>>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public double LabelMargin
        {
            get { return (double)GetValue(LabelMarginProperty); }
            set { SetValue(LabelMarginProperty, value); }
        }

        public Pen CategoryLinePen
        {
            get { return (Pen)GetValue(CategoryLinePenProperty); }
            set { SetValue(CategoryLinePenProperty, value); }
        }

        public Pen HourMarkerPen
        {
            get { return (Pen)GetValue(HourMarkerPenProperty); }
            set { SetValue(HourMarkerPenProperty, value); }
        }

        public bool ToolTips
        {
            get { return (bool)GetValue(ToolTipsProperty); }
            set { SetValue(ToolTipsProperty, value); }
        }

        public Brush PunchColor
        {
            get { return (Brush)GetValue(PunchColorProperty); }
            set { SetValue(PunchColorProperty, value); }
        }

        protected override void OnRender(DrawingContext dc)
        {
            this.toolTipLayer.Children.Clear();

            if (this.Data == null || this.Data.Count == 0)
            {
                return;
            }

            this.punchCardRenderWidth = ActualWidth - LabelMargin;
            this.punchCardRenderHeight = ActualHeight - LabelMargin;
            this.numberOfCategories = Data.Count;
            this.hourWidth = this.punchCardRenderWidth / NumberOfHours;
            this.categoryHeight = this.punchCardRenderHeight / this.numberOfCategories;

            var maxCount = Data.Max((l => l.Item2.Max()));

            this.countDiameterMultiplier = Math.Min(this.hourWidth / maxCount, this.categoryHeight / maxCount);

            DrawCategories(dc);
            DrawLabels(dc);
            DrawPunches(dc);

            base.OnRender(dc);
        }

        private void DrawLabels(DrawingContext dc)
        {
            // Categories
            for (int i = 0; i < Data.Count; i++)
            {
                var yPos = this.categoryHeight *(i + 1) - this.categoryHeight / 2;
                dc.DrawText(new FormattedText(((Data[i].Item1)), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 11.0, Brushes.Black,0), new Point(LabelMargin / 2, yPos));
            }

            // Hours
            for (int i = 0; i < NumberOfHours; i++)
            {
                var xPos = this.hourWidth * (i + 1) - this.hourWidth / 2 + LabelMargin;
                dc.DrawText(new FormattedText(((i+1).ToString()), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 11.0, Brushes.Black, 0), new Point(xPos - 3, ActualHeight - (LabelMargin / 2)));
            }
        }

        private void DrawHourMarkers(DrawingContext dc, double yOffset)
        {
            for (int i = 0; i < NumberOfHours; i++)
            {
                var xPos = this.hourWidth * (i + 1) - this.hourWidth / 2 + LabelMargin;
                dc.DrawLine(HourMarkerPen, new Point(xPos, yOffset - 10 - LabelMargin), new Point(xPos, yOffset - LabelMargin));
            }
        }

        private void DrawCategories(DrawingContext dc)
        {
            for (int i = 0; i < this.numberOfCategories; i++)
            {
                dc.DrawLine(CategoryLinePen, new Point(LabelMargin, this.categoryHeight * (i + 1)), new Point(this.punchCardRenderWidth + LabelMargin, this.categoryHeight * (i + 1)));
                DrawHourMarkers(dc, this.categoryHeight * (i + 1) + LabelMargin);
            }
        }

        private void DrawPunches(DrawingContext dc)
        {
            for (int i = 0; i < this.numberOfCategories; i++)
            {
                double yOffset = this.categoryHeight * (i + 1);

                for (int j = 0; j < NumberOfHours; j++)
                {
                    var xPos = this.hourWidth * (j + 1) - this.hourWidth / 2 + LabelMargin;
                    var punchPosition = new Point(xPos, yOffset - (this.categoryHeight - 20.0)/2.0 - 10);
                    var punchDiameter = CalculatePunchDiameter(Data[i].Item2[j]);
                    
                    dc.DrawEllipse(this.PunchColor, HourMarkerPen, punchPosition, punchDiameter / 2, punchDiameter / 2);

                    if (ToolTips == true)
                    {
                        var toolTipArea = new Ellipse
                        {
                            Height = punchDiameter,
                            Width = punchDiameter,
                            Fill = Brushes.Transparent,
                            ToolTip = $"{Data[i].Item1} - {Data[i].Item2[j]}",
                            RenderTransform = new TranslateTransform(punchPosition.X - (punchDiameter/2), punchPosition.Y - (punchDiameter/2))
                        };

                        this.toolTipLayer.Children.Add(toolTipArea);
                    }
                }
            }
        }

        private double CalculatePunchDiameter(int count)
        {
            return count * this.countDiameterMultiplier;
        }
    }
}
