﻿namespace ModernIU.Controls
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Base;

    public class AngleBorder : Decorator
    {
        #region DependencyProperty
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(nameof(Placement), typeof(EnumPlacement), typeof(AngleBorder), new FrameworkPropertyMetadata(EnumPlacement.RightCenter, FrameworkPropertyMetadataOptions.AffectsRender, OnDirectionPropertyChangedCallback));

        public EnumPlacement Placement
        {
            get { return (EnumPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        public static readonly DependencyProperty TailWidthProperty =
            DependencyProperty.Register("TailWidth", typeof(double), typeof(AngleBorder), new PropertyMetadata(10d));

        public double TailWidth
        {
            get { return (double)GetValue(TailWidthProperty); }
            set { SetValue(TailWidthProperty, value); }
        }
        public static readonly DependencyProperty TailHeightProperty =
            DependencyProperty.Register("TailHeight", typeof(double), typeof(AngleBorder), new PropertyMetadata(10d));

        public double TailHeight
        {
            get { return (double)GetValue(TailHeightProperty); }
            set { SetValue(TailHeightProperty, value); }
        }

        public static readonly DependencyProperty TailVerticalOffsetProperty =
            DependencyProperty.Register("TailVerticalOffset", typeof(double), typeof(AngleBorder), new PropertyMetadata(13d));

        public double TailVerticalOffset
        {
            get { return (double)GetValue(TailVerticalOffsetProperty); }
            set { SetValue(TailVerticalOffsetProperty, value); }
        }
        public static readonly DependencyProperty TailHorizontalOffsetProperty =
            DependencyProperty.Register("TailHorizontalOffset", typeof(double), typeof(AngleBorder),
                new PropertyMetadata(12d));

        public double TailHorizontalOffset
        {
            get { return (double)GetValue(TailHorizontalOffsetProperty); }
            set { SetValue(TailHorizontalOffsetProperty, value); }
        }
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(AngleBorder)
                , new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(Thickness), typeof(AngleBorder)
                , new PropertyMetadata(new Thickness(10, 5, 10, 5)));

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(AngleBorder)
                , new PropertyMetadata(default(Brush)));

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(AngleBorder), new PropertyMetadata(new Thickness(1d)));

        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius)
                , typeof(AngleBorder), new PropertyMetadata(new CornerRadius(0)));

        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region protected override
        protected override Size MeasureOverride(Size constraint)
        {
            Thickness padding = this.Padding;

            Size result = new Size();
            if (Child != null)
            {
                Child.Measure(constraint);

                switch (Placement)
                {
                    case EnumPlacement.LeftTop:
                    case EnumPlacement.LeftBottom:
                    case EnumPlacement.LeftCenter:
                    case EnumPlacement.RightTop:
                    case EnumPlacement.RightBottom:
                    case EnumPlacement.RightCenter:
                        result.Width = Child.DesiredSize.Width + padding.Left + padding.Right + this.TailWidth;
                        result.Height = Child.DesiredSize.Height + padding.Top + padding.Bottom;
                        break;
                    case EnumPlacement.TopLeft:
                    case EnumPlacement.TopCenter:
                    case EnumPlacement.TopRight:
                    case EnumPlacement.BottomLeft:
                    case EnumPlacement.BottomCenter:
                    case EnumPlacement.BottomRight:
                        result.Width = Child.DesiredSize.Width + padding.Left + padding.Right;
                        result.Height = Child.DesiredSize.Height + padding.Top + padding.Bottom + this.TailHeight;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Thickness padding = this.Padding;
            if (Child != null)
            {
                switch (Placement)
                {
                    case EnumPlacement.LeftTop:
                    case EnumPlacement.LeftBottom:
                    case EnumPlacement.LeftCenter:
                        this.TailWidth = 6;
                        Child.Arrange(new Rect(new Point(padding.Left + this.TailWidth, padding.Top), Child.DesiredSize));
                        //ArrangeChildLeft();
                        break;
                    case EnumPlacement.RightTop:
                    case EnumPlacement.RightBottom:
                    case EnumPlacement.RightCenter:
                        ArrangeChildRight(padding);
                        break;
                    case EnumPlacement.TopLeft:
                    case EnumPlacement.TopRight:
                    case EnumPlacement.TopCenter:
                        Child.Arrange(new Rect(new Point(padding.Left, this.TailHeight + padding.Top), Child.DesiredSize));
                        break;
                    case EnumPlacement.BottomLeft:
                    case EnumPlacement.BottomRight:
                    case EnumPlacement.BottomCenter:
                        Child.Arrange(new Rect(new Point(padding.Left, padding.Top), Child.DesiredSize));
                        break;
                    default:
                        break;
                }
            }
            return arrangeSize;
        }

        private void ArrangeChildRight(Thickness padding)
        {
            double x = padding.Left;
            double y = padding.Top;

            if (!Double.IsNaN(this.Height) && this.Height != 0)
            {
                y = (this.Height - (Child.DesiredSize.Height)) / 2;
            }

            Child.Arrange(new Rect(new Point(x, y), Child.DesiredSize));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Child != null)
            {
                Geometry cg = null;
                Brush brush = null;
                //DpiScale dpi = base.getd();
                Pen pen = new Pen();

                pen.Brush = this.BorderBrush;
                //pen.Thickness = BorderThickness * 0.5;
                pen.Thickness = UIElementEx.RoundLayoutValue(BorderThickness.Left, DoubleUtil.DpiScaleX);

                switch (Placement)
                {
                    case EnumPlacement.LeftTop:
                    case EnumPlacement.LeftBottom:
                    case EnumPlacement.LeftCenter:
                        cg = CreateGeometryTailAtLeft();
                        brush = CreateFillBrush();
                        break;
                    case EnumPlacement.RightTop:
                    case EnumPlacement.RightCenter:
                    case EnumPlacement.RightBottom:
                        cg = CreateGeometryTailAtRight();
                        brush = CreateFillBrush();
                        break;
                    case EnumPlacement.TopLeft:
                    case EnumPlacement.TopCenter:
                    case EnumPlacement.TopRight:
                        cg = CreateGeometryTailAtTop();
                        brush = CreateFillBrush();
                        break;
                    case EnumPlacement.BottomLeft:
                    case EnumPlacement.BottomCenter:
                    case EnumPlacement.BottomRight:
                        cg = CreateGeometryTailAtBottom();
                        brush = CreateFillBrush();
                        break;
                    default:
                        break;
                }

                GuidelineSet guideLines = new GuidelineSet();
                drawingContext.PushGuidelineSet(guideLines);
                drawingContext.DrawGeometry(brush, pen, cg);
            }
        }
        #endregion

        #region private
        private Geometry CreateGeometryTailAtRight()
        {
            CombinedGeometry result = new CombinedGeometry();

            this.TailHeight = 12;
            this.TailWidth = 6;

            switch (this.Placement)
            {
                case EnumPlacement.RightTop:
                    break;
                case EnumPlacement.RightBottom:
                    this.TailVerticalOffset = this.ActualHeight - this.TailHeight - this.TailVerticalOffset;
                    break;
                case EnumPlacement.RightCenter:
                    this.TailVerticalOffset = (this.ActualHeight - this.TailHeight) / 2;
                    break;
            }

            Point arcPoint1 = new Point(this.ActualWidth - TailWidth, TailVerticalOffset);
            Point arcPoint2 = new Point(this.ActualWidth, TailVerticalOffset + TailHeight / 2);
            Point arcPoint3 = new Point(this.ActualWidth - TailWidth, TailVerticalOffset + TailHeight);

            LineSegment as1_2 = new LineSegment(arcPoint2, false);
            LineSegment as2_3 = new LineSegment(arcPoint3, false);

            PathFigure pf1 = new PathFigure();
            pf1.IsClosed = false;
            pf1.StartPoint = arcPoint1;
            pf1.Segments.Add(as1_2);
            pf1.Segments.Add(as2_3);

            PathGeometry pg1 = new PathGeometry();
            pg1.Figures.Add(pf1);

            RectangleGeometry rg2 = new RectangleGeometry(new Rect(0, 0, this.ActualWidth - TailWidth, this.ActualHeight)
                , CornerRadius.TopLeft, CornerRadius.BottomRight, new TranslateTransform(0.5, 0.5));

            result.Geometry1 = pg1;
            result.Geometry2 = rg2;
            result.GeometryCombineMode = GeometryCombineMode.Union;

            return result;
        }

        private Geometry CreateGeometryTailAtLeft()
        {
            CombinedGeometry result = new CombinedGeometry();

            this.TailHeight = 12;
            this.TailWidth = 6;

            switch (this.Placement)
            {
                case EnumPlacement.LeftTop:
                    break;
                case EnumPlacement.LeftBottom:
                    this.TailVerticalOffset = this.ActualHeight - this.TailHeight - this.TailVerticalOffset;
                    break;
                case EnumPlacement.LeftCenter:
                    this.TailVerticalOffset = (this.ActualHeight - this.TailHeight) / 2;
                    break;
            }

            Point arcPoint1 = new Point(TailWidth, TailVerticalOffset);
            Point arcPoint2 = new Point(0, TailVerticalOffset + TailHeight / 2);
            Point arcPoint3 = new Point(TailWidth, TailVerticalOffset + TailHeight);

            LineSegment as1_2 = new LineSegment(arcPoint2, false);
            LineSegment as2_3 = new LineSegment(arcPoint3, false);

            PathFigure pf = new PathFigure();
            pf.IsClosed = false;
            pf.StartPoint = arcPoint1;
            pf.Segments.Add(as1_2);
            pf.Segments.Add(as2_3);

            PathGeometry g1 = new PathGeometry();
            g1.Figures.Add(pf);
            RectangleGeometry g2 = new RectangleGeometry(new Rect(TailWidth, 0, this.ActualWidth - this.TailWidth, this.ActualHeight) , CornerRadius.TopLeft, CornerRadius.BottomRight);

            result.Geometry1 = g1;
            result.Geometry2 = g2;
            result.GeometryCombineMode = GeometryCombineMode.Union;

            return result;
        }

        private Geometry CreateGeometryTailAtTop()
        {
            CombinedGeometry result = new CombinedGeometry();

            switch (this.Placement)
            {
                case EnumPlacement.TopLeft:
                    break;
                case EnumPlacement.TopCenter:
                    this.TailHorizontalOffset = (this.ActualWidth - this.TailWidth) / 2;
                    break;
                case EnumPlacement.TopRight:
                    this.TailHorizontalOffset = this.ActualWidth - this.TailWidth - this.TailHorizontalOffset;
                    break;
            }

            Point anglePoint1 = new Point(this.TailHorizontalOffset, this.TailHeight);
            Point anglePoint2 = new Point(this.TailHorizontalOffset + (this.TailWidth / 2), 0);
            Point anglePoint3 = new Point(this.TailHorizontalOffset + this.TailWidth, this.TailHeight);

            LineSegment as1_2 = new LineSegment(anglePoint2, true);
            LineSegment as2_3 = new LineSegment(anglePoint3, true);

            PathFigure pf = new PathFigure();
            pf.IsClosed = false;
            pf.StartPoint = anglePoint1;
            pf.Segments.Add(as1_2);
            pf.Segments.Add(as2_3);

            PathGeometry g1 = new PathGeometry();
            g1.Figures.Add(pf);
            RectangleGeometry g2 = new RectangleGeometry(new Rect(0, this.TailHeight, this.ActualWidth, this.ActualHeight - this.TailHeight)
                , CornerRadius.TopLeft, CornerRadius.BottomRight);

            result.Geometry1 = g1;
            result.Geometry2 = g2;
            result.GeometryCombineMode = GeometryCombineMode.Union;

            return result;
        }

        private Geometry CreateGeometryTailAtBottom()
        {
            CombinedGeometry result = new CombinedGeometry();

            switch (this.Placement)
            {
                case EnumPlacement.BottomLeft:
                    break;
                case EnumPlacement.BottomCenter:
                    this.TailHorizontalOffset = (this.ActualWidth - this.TailWidth) / 2;
                    break;
                case EnumPlacement.BottomRight:
                    this.TailHorizontalOffset = this.ActualWidth - this.TailWidth - this.TailHorizontalOffset;
                    break;
            }


            Point anglePoint1 = new Point(this.TailHorizontalOffset, this.ActualHeight - this.TailHeight);
            Point anglePoint2 = new Point(this.TailHorizontalOffset + this.TailWidth / 2, this.ActualHeight);
            Point anglePoint3 = new Point(this.TailHorizontalOffset + this.TailWidth, this.ActualHeight - this.TailHeight);

            LineSegment as1_2 = new LineSegment(anglePoint2, true);
            LineSegment as2_3 = new LineSegment(anglePoint3, true);

            PathFigure pf = new PathFigure();
            pf.IsClosed = false;
            pf.StartPoint = anglePoint1;
            pf.Segments.Add(as1_2);
            pf.Segments.Add(as2_3);

            PathGeometry g1 = new PathGeometry();
            g1.Figures.Add(pf);

            RectangleGeometry g2 = new RectangleGeometry(new Rect(0, 0, this.ActualWidth, this.ActualHeight - this.TailHeight)
                , CornerRadius.TopLeft, CornerRadius.BottomRight);

            result.Geometry1 = g1;
            result.Geometry2 = g2;
            result.GeometryCombineMode = GeometryCombineMode.Union;

            return result;
        }

        private Brush CreateFillBrush()
        {
            Brush result = null;

            GradientStopCollection gsc = new GradientStopCollection();
            gsc.Add(new GradientStop(((SolidColorBrush)this.Background).Color, 0));
            LinearGradientBrush backGroundBrush = new LinearGradientBrush(gsc, new Point(0, 0), new Point(0, 1));
            result = backGroundBrush;

            return result;
        }

        public static void OnDirectionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AngleBorder angleBorder = d as AngleBorder;
            if(angleBorder != null)
            {
                switch ((EnumPlacement)e.NewValue)
                {
                    case EnumPlacement.LeftTop:
                    case EnumPlacement.LeftBottom:
                    case EnumPlacement.LeftCenter:
                    case EnumPlacement.RightTop:
                    case EnumPlacement.RightBottom:
                    case EnumPlacement.RightCenter:
                        angleBorder.TailWidth = 6;
                        angleBorder.TailHeight = 12;
                        break;
                    case EnumPlacement.TopLeft:
                    case EnumPlacement.TopCenter:
                    case EnumPlacement.TopRight:
                    case EnumPlacement.BottomLeft:
                    case EnumPlacement.BottomCenter:
                    case EnumPlacement.BottomRight:
                        angleBorder.TailWidth = 12;
                        angleBorder.TailHeight = 6;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }

    public class DoubleUtil
    {
        public static double DpiScaleX
        {
            get
            {
                int dx = 0;
                int dy = 0;
                GetDPI(out dx, out dy);
                if (dx != 96)
                {
                    return (double)dx / 96.0;
                }
                return 1.0;
            }
        }

        public static double DpiScaleY
        {
            get
            {
                int dx = 0;
                int dy = 0;
                GetDPI(out dx, out dy);
                if (dy != 96)
                {
                    return (double)dy / 96.0;
                }
                return 1.0;
            }
        }

        public static void GetDPI(out int dpix, out int dpiy)
        {
            dpix = 0;
            dpiy = 0;
            using (System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_DesktopMonitor"))
            {
                using (System.Management.ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (System.Management.ManagementObject each in moc)
                    {
                        dpix = int.Parse((each.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                        dpiy = int.Parse((each.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                    }
                }
            }
        }
        public static bool GreaterThan(double value1, double value2)
        {
            return value1 > value2 && !DoubleUtil.AreClose(value1, value2);
        }

        public static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            double num = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
            double num2 = value1 - value2;
            return -num < num2 && num > num2;
        }

        public static bool IsZero(double value)
        {
            return Math.Abs(value) < 2.2204460492503131E-15;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)]
            internal double DoubleValue;
            [FieldOffset(0)]
            internal ulong UintValue;
        }

        public static bool IsNaN(double value)
        {
            DoubleUtil.NanUnion nanUnion = default(DoubleUtil.NanUnion);
            nanUnion.DoubleValue = value;
            ulong num = nanUnion.UintValue & 18442240474082181120uL;
            ulong num2 = nanUnion.UintValue & 4503599627370495uL;
            return (num == 9218868437227405312uL || num == 18442240474082181120uL) && num2 != 0uL;
        }
    }

    public static class UlementEx
    {
        public static double RoundLayoutValue(double value, double dpiScale)
        {
            double num;
            if (!DoubleUtil.AreClose(dpiScale, 1.0))
            {
                num = Math.Round(value * dpiScale) / dpiScale;
                if (DoubleUtil.IsNaN(num) || double.IsInfinity(num) || DoubleUtil.AreClose(num, 1.7976931348623157E+308))
                {
                    num = value;
                }
            }
            else
            {
                num = Math.Round(value);
            }
            return num;
        }
    }
}
