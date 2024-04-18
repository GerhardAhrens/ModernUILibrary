namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class MToolTip : ToolTip
    {
        private EnumPlacement mPlacement;

        public static readonly DependencyProperty PlacementExProperty = 
            DependencyProperty.Register("PlacementEx" , typeof(EnumPlacement), typeof(MToolTip), new PropertyMetadata(EnumPlacement.TopLeft));
        public static readonly DependencyProperty IsShowShadowProperty = 
            DependencyProperty.Register("IsShowShadow" , typeof(bool), typeof(MToolTip), new PropertyMetadata(true));

        #region Constructors
        static MToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MToolTip), new FrameworkPropertyMetadata(typeof(MToolTip)));
        }
        #endregion

        public EnumPlacement PlacementEx
        {
            get { return (EnumPlacement)GetValue(PlacementExProperty); }
            set { SetValue(PlacementExProperty, value); }
        }

        public bool IsShowShadow
        {
            get { return (bool)GetValue(IsShowShadowProperty); }
            set { SetValue(IsShowShadowProperty, value); }
        }

        public MToolTip()
        {
            this.Initialized += (o, e) =>
            {
                this.mPlacement = this.PlacementEx;
            };
        }

        protected override void OnOpened(RoutedEventArgs e)
        {
            if(this.PlacementTarget != null)
            {
                double workAreaX = SystemParameters.WorkArea.Width;
                double workAreaY = SystemParameters.WorkArea.Height;

                FrameworkElement control = this.PlacementTarget as FrameworkElement;
                double controlWidth = control.ActualWidth;
                double controlHeight = control.ActualHeight;

                Point p = this.PlacementTarget.PointFromScreen(new Point(0, 0));
                if(p != default)
                {
                    double pointX = Math.Abs(p.X); 
                    double pointY = Math.Abs(p.Y);

                    switch (this.mPlacement)
                    {
                        case EnumPlacement.LeftTop:
                            this.SetLeftPosition(pointX, EnumPlacement.RightTop);
                            break;
                        case EnumPlacement.LeftBottom:
                            this.SetLeftPosition(pointX, EnumPlacement.RightBottom);
                            break;
                        case EnumPlacement.LeftCenter:
                            this.SetLeftPosition(pointX, EnumPlacement.RightCenter);
                            break;
                        case EnumPlacement.RightTop:
                            SetRightPosition(workAreaX, controlWidth, pointX, EnumPlacement.LeftTop);
                            break;
                        case EnumPlacement.RightBottom:
                            SetRightPosition(workAreaX, controlWidth, pointX, EnumPlacement.LeftBottom);
                            break;
                        case EnumPlacement.RightCenter:
                            SetRightPosition(workAreaX, controlWidth, pointX, EnumPlacement.LeftCenter);
                            break;
                        case EnumPlacement.TopLeft:
                            this.SetTopPosition(pointY, EnumPlacement.BottomLeft);
                            break;
                        case EnumPlacement.TopCenter:
                            this.SetTopPosition(pointY, EnumPlacement.BottomCenter);
                            break;
                        case EnumPlacement.TopRight:
                            this.SetTopPosition(pointY, EnumPlacement.BottomRight);
                            break;
                        case EnumPlacement.BottomLeft:
                            SetBottomPosition(workAreaY, controlHeight, pointY, EnumPlacement.TopLeft);
                            break;
                        case EnumPlacement.BottomCenter:
                            SetBottomPosition(workAreaY, controlHeight, pointY, EnumPlacement.TopCenter);
                            break;
                        case EnumPlacement.BottomRight:
                            SetBottomPosition(workAreaY, controlHeight, pointY, EnumPlacement.TopRight);
                            break;
                        default:
                            break;
                    }
                }
            }
            base.OnOpened(e);
        }

        private void SetBottomPosition(double workAreaY, double controlHeight, double pointY, EnumPlacement placement)
        {
            if (workAreaY - (pointY + controlHeight) < this.ActualHeight)
            {
                this.PlacementEx = placement;
            }
            else
            {
                this.PlacementEx = this.mPlacement;
            }
        }

        private void SetTopPosition(double pointY, EnumPlacement placement)
        {
            if (pointY < this.ActualHeight)
            {
                this.PlacementEx = placement;
            }
            else
            {
                this.PlacementEx = this.mPlacement;
            }
        }

        private void SetRightPosition(double workAreaX, double controlWidth, double pointX, EnumPlacement placement)
        {
            if (workAreaX - (pointX + controlWidth) < this.ActualWidth)
            {
                this.PlacementEx = placement;
            }
            else
            {
                this.PlacementEx = this.mPlacement;
            }
        }

        private void SetLeftPosition(double pointX, EnumPlacement placement)
        {
            if (pointX < this.ActualWidth)
            {
                this.PlacementEx = placement;
            }
            else
            {
                this.PlacementEx = this.mPlacement;
            }
        }
    }
}