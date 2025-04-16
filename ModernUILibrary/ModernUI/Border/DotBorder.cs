//-----------------------------------------------------------------------
// <copyright file="DotBorder.cs" company="Lifeprojects.de">
//     Class: DotBorder
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>16.04.2025 14:51:02</date>
//
// <summary>
// Die Klasse erstellt einen Rahmen aus Punkt UI Elemente
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    public class DotBorder : Border
    {
        private Rectangle _antsRectangle;
        private Rectangle _trackRectangle;
        private VisualBrush _antsVisualBrush;
        private VisualBrush _trackBrush;
        private VisualBrush _layerBrush;
        private Storyboard _storyboard;
        private DoubleAnimation _animation;
        private DoubleCollection _originalDashArray;

        private bool _isAdjustingDashArray;

        public DotBorder() : base()
        {
            Unloaded += OnUnloaded;

            // Set default properties
            BorderThickness = new Thickness(2);
            SnapsToDevicePixels = false;

            // Setup brushes
            this._trackRectangle = new Rectangle
            {
                Fill = TrackBrush,
                //Stroke = TrackBrush,
                //StrokeThickness = StrokeThickness,
                Width = ActualWidth,
                Height = ActualHeight,
            };

            this._trackRectangle.SetBinding(Rectangle.RadiusXProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath("CornerRadius.TopLeft"),
                Mode = BindingMode.OneWay
            });

            this._trackRectangle.SetBinding(Rectangle.RadiusYProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath("CornerRadius.TopLeft"),
                Mode = BindingMode.OneWay
            });

            this._trackRectangle.SetBinding(Rectangle.FillProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(TrackBrushProperty),
                Mode = BindingMode.OneWay
            });

            this._trackRectangle.SetBinding(Shape.StrokeThicknessProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(StrokeThicknessProperty),
                Mode = BindingMode.OneWay
            });

            this._trackRectangle.SetBinding(FrameworkElement.WidthProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(ActualWidthProperty),
                Mode = BindingMode.OneWay
            });

            this._trackRectangle.SetBinding(FrameworkElement.HeightProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(ActualHeightProperty),
                Mode = BindingMode.OneWay
            });

            // Initialize the animated visual
            this._antsRectangle = new Rectangle
            {
                Stroke = BorderBrush,
                StrokeThickness = StrokeThickness,
                StrokeDashArray = StrokeDashArray,
                StrokeDashOffset = StrokeDashOffset,
                Width = ActualWidth,
                Height = ActualHeight,
            };

            this._antsRectangle.SetBinding(Rectangle.RadiusXProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath("CornerRadius.TopLeft"),
                Mode = BindingMode.OneWay
            });

            this._antsRectangle.SetBinding(Rectangle.RadiusYProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath("CornerRadius.TopLeft"),
                Mode = BindingMode.OneWay
            });

            this._antsRectangle.SetBinding(Shape.StrokeDashArrayProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(StrokeDashArrayProperty),
                Mode = BindingMode.OneWay
            });

            this._antsRectangle.SetBinding(Shape.StrokeDashOffsetProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(StrokeDashOffsetProperty),
                Mode = BindingMode.OneWay
            });

            this._antsRectangle.SetBinding(Shape.StrokeThicknessProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(StrokeThicknessProperty),
                Mode = BindingMode.OneWay
            });

            this._antsRectangle.SetBinding(FrameworkElement.WidthProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(ActualWidthProperty),
                Mode = BindingMode.OneWay
            });

            this._antsRectangle.SetBinding(FrameworkElement.HeightProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(ActualHeightProperty),
                Mode = BindingMode.OneWay
            });

            this._antsVisualBrush = new VisualBrush(this._antsRectangle);
            this._trackBrush = new VisualBrush(_trackRectangle);

            Grid grid = new Grid();
            grid.Children.Add(_trackRectangle);
            grid.Children.Add(this._antsRectangle);

            this._layerBrush = new VisualBrush(grid);
            BorderBrush = this._layerBrush;

            // Setup animation
            this._animation = new DoubleAnimation
            {
                From = Speed,
                To = 0,
                Duration = TimeSpan.FromMinutes(1),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(this._animation, this._antsRectangle);
            Storyboard.SetTargetProperty(this._animation, new PropertyPath("StrokeDashOffset"));

            this._storyboard = new Storyboard();
            this._storyboard.Children.Add(this._animation);
            this._storyboard.Begin();
        }

        #region Dependency Properties

        public static readonly DependencyProperty AnimateProperty =
            DependencyProperty.Register(
                name: nameof(Animate),
                propertyType: typeof(bool),
                ownerType: typeof(DotBorder),
                typeMetadata: new PropertyMetadata(true));

        public bool Animate
        {
            get => (bool)GetValue(AnimateProperty);
            set => SetValue(AnimateProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                name: nameof(StrokeThickness),
                propertyType: typeof(double),
                ownerType: typeof(DotBorder),
                typeMetadata: new PropertyMetadata(2d));

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register(
                name: nameof(Speed),
                propertyType: typeof(double),
                ownerType: typeof(DotBorder),
                typeMetadata: new PropertyMetadata(1000d));

        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public static readonly DependencyProperty StrokeDashOffsetProperty =
            DependencyProperty.Register(
                name: nameof(StrokeDashOffset),
                propertyType: typeof(double),
                ownerType: typeof(DotBorder),
                typeMetadata: new PropertyMetadata(0d));

        public double StrokeDashOffset
        {
            get => (double)GetValue(StrokeDashOffsetProperty);
            set => SetValue(StrokeDashOffsetProperty, value);
        }

        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(
                name: nameof(StrokeDashArray),
                propertyType: typeof(DoubleCollection),
                ownerType: typeof(DotBorder),
                typeMetadata: new PropertyMetadata(new DoubleCollection(new double[] { 5d })));

        public DoubleCollection StrokeDashArray
        {
            get => (DoubleCollection)GetValue(StrokeDashArrayProperty);
            set => SetValue(StrokeDashArrayProperty, value);
        }

        public static readonly DependencyProperty TrackBrushProperty =
        DependencyProperty.Register(
            name: nameof(TrackBrush),
            propertyType: typeof(Brush),
            ownerType: typeof(DotBorder),
            typeMetadata: new PropertyMetadata(Brushes.Transparent));

        public Brush TrackBrush
        {
            get => (Brush)GetValue(TrackBrushProperty);
            set => SetValue(TrackBrushProperty, value);
        }

        /// <summary>
        /// Sets <see cref="BorderThickness"/> and <see cref="StrokeThickness"/> simultaneously.
        /// </summary>
        public double BorderStrokeThickness
        {
            set
            {
                BorderThickness = new Thickness(value);
                StrokeThickness = value;
            }
        }

        #endregion Dependency Properties

        #region Methods

        private void SetAnimationSpeed(double speed)
        {
            _animation.From = speed;

            if (Animate)
            {
                this._storyboard.Stop();
                this._storyboard.Begin();
            }
        }

        private void SetAnimationEnabled(bool enabled)
        {
            if (enabled)
            {
                this._storyboard.Begin();
            }
            else
            {
                this._storyboard.Stop();
            }
        }

        private double CalculateCircumference()
        {
            // Centerline width and height
            double width = ActualWidth - StrokeThickness;
            double height = ActualHeight - StrokeThickness;

            // Handle zero corner radius case
            if (CornerRadius.TopLeft == 0 && CornerRadius.TopRight == 0 &&
                CornerRadius.BottomLeft == 0 && CornerRadius.BottomRight == 0)
            {
                return 2 * (width + height);
            }

            // Centerline arc widths (radius is adjusted to centerline)
            double topLeftArcRadius = Math.Min(CornerRadius.TopLeft, Math.Min(width / 2, height / 2)) - StrokeThickness / 2;
            double topRightArcRadius = Math.Min(CornerRadius.TopRight, Math.Min(width / 2, height / 2)) - StrokeThickness / 2;
            double bottomLeftArcRadius = Math.Min(CornerRadius.BottomLeft, Math.Min(width / 2, height / 2)) - StrokeThickness / 2;
            double bottomRightArcRadius = Math.Min(CornerRadius.BottomRight, Math.Min(width / 2, height / 2)) - StrokeThickness / 2;

            // Calculate straight edge lengths (account for arc radii on each side)
            double topEdgeLength = width - topLeftArcRadius - topRightArcRadius;
            double rightEdgeLength = height - topRightArcRadius - bottomRightArcRadius;
            double bottomEdgeLength = width - bottomLeftArcRadius - bottomRightArcRadius;
            double leftEdgeLength = height - topLeftArcRadius - bottomLeftArcRadius;

            // Calculate arc lengths for each corner (quarter-circle approximation)
            double topLeftArc = Math.PI * topLeftArcRadius / 2;
            double topRightArc = Math.PI * topRightArcRadius / 2;
            double bottomLeftArc = Math.PI * bottomLeftArcRadius / 2;
            double bottomRightArc = Math.PI * bottomRightArcRadius / 2;

            // Calculate total circumference
            return topEdgeLength + rightEdgeLength + bottomEdgeLength + leftEdgeLength +
                   topLeftArc + topRightArc + bottomLeftArc + bottomRightArc;
        }

        // Method to adjust the StrokeDashArray based on the perimeter
        private void AdjustDashArray()
        {
            if (this._originalDashArray == null || ActualWidth == 0 || ActualHeight == 0)
            {
                return;
            }

            // Calculate the total length of one full dash-gap cycle
            double totalPatternLength = this._originalDashArray.Count == 1 ? this._originalDashArray[0] * 2 : this._originalDashArray.Sum(); // Sum all dash and gap values

            // Calculate the circumference of the rectangle
            double circumference = CalculateCircumference();

            if (circumference == 0)
            {
                return;
            }

            totalPatternLength *= StrokeThickness;

            int desiredAnts = Convert.ToInt32(circumference / totalPatternLength);
            if (desiredAnts % 2 != 0) desiredAnts++;


            double requiredSize = circumference / desiredAnts;
            double scaleFactor = requiredSize / totalPatternLength;

            // Check if the scale factor is too small (close to 1)
            if (scaleFactor == 1 )
            {
                return;
            }

            // Create a new DoubleCollection to store the adjusted values
            DoubleCollection adjustedDashArray = new DoubleCollection();

            // Adjust each dash and gap length by multiplying it with the scale factor
            foreach (var value in _originalDashArray)
            {
                adjustedDashArray.Add(value * scaleFactor);
            }

            // Update the StrokeDashArray with the adjusted values
            this._isAdjustingDashArray = true;
            StrokeDashArray = adjustedDashArray;
            this._isAdjustingDashArray = false;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this._storyboard.Stop();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (BorderBrush != this._layerBrush)
            {
                this._antsRectangle.Stroke = BorderBrush;
                BorderBrush = this._layerBrush;
            }

            base.OnRender(dc);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            // Adjust the dash array whenever the size changes
            AdjustDashArray();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == StrokeDashArrayProperty)
            {
                if (!_isAdjustingDashArray)
                {
                    _originalDashArray = new DoubleCollection(e.NewValue as DoubleCollection);
                    AdjustDashArray();
                }
            }
            else if (e.Property == Border.CornerRadiusProperty ||
                e.Property == StrokeThicknessProperty)
            {
                AdjustDashArray();
            }
            else if (e.Property == SpeedProperty)
            {
                SetAnimationSpeed((double)e.NewValue);
            }
            else if (e.Property == AnimateProperty)
            {
                SetAnimationEnabled((bool)e.NewValue);
            }
        }

        #endregion Methods
    }
}
