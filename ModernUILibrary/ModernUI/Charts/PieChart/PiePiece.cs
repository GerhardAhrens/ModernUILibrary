namespace ModernIU.Controls.Chart
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;


    public class PiePiece : Shape
    {
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(PiePiece), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty ClientHeightProperty =
            DependencyProperty.Register("ClientHeight", typeof(double), typeof(PiePiece), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ClientWidthProperty =
        DependencyProperty.Register("ClientWidth", typeof(double), typeof(PiePiece), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(Guid), typeof(PiePiece),  new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ItemsProperty =
          DependencyProperty.Register("Items", typeof(ItemCollection), typeof(PiePiece), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PiePiece),  new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(PiePiece), new FrameworkPropertyMetadata(0.0));

        private double m_percent;

        /// <summary>
        /// Initializes a new instance of the <see cref="PiePiece"/> class.
        /// </summary>
        public PiePiece()
        {
        }

        #region Dependency Property
        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the client.
        /// </summary>
        /// <value>The height of the client.</value>
        public double ClientHeight
        {
            get { return (double)GetValue(ClientHeightProperty); }
            set { SetValue(ClientHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the client.
        /// </summary>
        /// <value>The width of the client.</value>
        public double ClientWidth
        {
            get { return (double)GetValue(ClientWidthProperty); }
            set { SetValue(ClientWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id
        {
            get { return (Guid)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [BindableAttribute(true)]
        public ItemCollection Items
        {
            get { return (ItemCollection)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion Dependency Property

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        /// <value>The percent.</value>
        public double Percent
        {
            get { return m_percent; }
            set
            {
                m_percent = value;
            }
        }

        /// <summary>
        /// Gets a value that represents the <see cref="T:System.Windows.Media.Geometry"/> of the <see cref="T:System.Windows.Shapes.Shape"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Windows.Media.Geometry"/> of the <see cref="T:System.Windows.Shapes.Shape"/>.
        /// </returns>
        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        /// <summary>
        /// Computes the cartesian coordinate.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="radius">The radius.</param>
        /// <returns></returns>
        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        /// <summary>
        /// Draws the geometry.
        /// </summary>
        /// <param name="context">The context.</param>
        private void DrawGeometry(StreamGeometryContext context)
        {
            if (Items != null)
            {
                m_percent = GetPercent(Value);

                Point center = GetCenter();

                double startAngle = GetAngle(false);
                double endAngle = GetAngle(true);

                double radius = GetRadius();

                Point outerStartPoint = ComputeCartesianCoordinate(startAngle, radius);
                outerStartPoint.Offset(center.X, center.Y);

                Point outerEndPoint = ComputeCartesianCoordinate(endAngle, radius);
                outerEndPoint.Offset(center.X, center.Y);

                Size outerSize = new Size(radius, radius);

                bool isLarge = (endAngle - startAngle) > 180.0;

                context.BeginFigure(center, true, true);
                context.LineTo(outerStartPoint, true, true);
                context.ArcTo(outerEndPoint, outerSize, 0, isLarge, SweepDirection.Clockwise, true, true);

                this.ToolTip = this.Caption;

            }
        }

        /// <summary>
        /// Gets the angle.
        /// </summary>
        /// <param name="include">if set to <c>true</c> [include].</param>
        /// <returns></returns>
        private double GetAngle(bool include)
        {
            double result = 0;

            double sum = 0;
            foreach (var item in Items)
            {
                if (item is ChartRow)
                {
                    ChartRow row = item as ChartRow;
                    if (row.Id == Id)
                    {
                        if (include)
                        {
                            sum += (item as ChartRow).Value;
                        }
                        break;
                    }
                    sum += (item as ChartRow).Value;
                }
            }

            double percent = GetPercent(sum);

            result = (360 / 100.0) * percent;

            return result;
        }

        /// <summary>
        /// Gets the center.
        /// </summary>
        /// <returns></returns>
        private Point GetCenter()
        {
            return new Point(ClientWidth / 2, ClientHeight / 2);
            ;
        }

        /// <summary>
        /// Gets the chart row.
        /// </summary>
        /// <returns></returns>
        private object GetChartRow()
        {
            object result = null;

            foreach (var item in Items)
            {
                if (item is ChartRow)
                {
                    ChartRow row = item as ChartRow;
                    if (row.Id == Id) { result = row; }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the percent.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private double GetPercent(double value)
        {
            double result = 0;

            double sum = 0;
            foreach (var item in Items)
            {
                if (item is ChartRow)
                {
                    sum += (item as ChartRow).Value;
                }
            }

            result = value / (sum / 100);
            return result;
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <returns></returns>
        private double GetRadius()
        {
            double result;
            if (ClientHeight < ClientWidth)
            {
                result = ClientHeight / 2;
            }
            else
            {
                result = ClientWidth / 2;
            }

            return result - 5;
        }
    }
}