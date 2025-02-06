namespace ModernIU.Controls
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class PathButton : Button
    {
        public static readonly DependencyProperty PathWidthProperty = 
            DependencyProperty.Register("PathWidth", typeof(double), typeof(PathButton), new FrameworkPropertyMetadata(13d));

        public static readonly DependencyProperty PathDataProperty = 
            DependencyProperty.Register("PathData", typeof(PathGeometry), typeof(PathButton));

        public static readonly DependencyProperty MouseOverBackgroundProperty = 
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 193, 233))));

        public static readonly DependencyProperty PressedBackgroundProperty = 
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(74, 137, 220))));

        public static readonly DependencyProperty MouseOverForegroundProperty = 
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 193, 233))));

        public static readonly DependencyProperty PressedForegroundProperty = 
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(74, 137, 220))));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PathButton));

        public static readonly DependencyProperty GroupNameProperty = 
            DependencyProperty.Register("GroupName", typeof(string), typeof(PathButton), new PropertyMetadata(string.Empty, OnGroupNameChanged));

        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));
        }

        public double PathWidth
        {
            get { return (double)GetValue(PathWidthProperty); }
            set { SetValue(PathWidthProperty, value); }
        }

        public PathGeometry PathData
        {
            get { return (PathGeometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PathButton button = d as PathButton;
            if (button != null)
            {
                if ((string)e.NewValue != string.Empty)
                {
                    var rootElement = ((System.Windows.FrameworkElement)d).Parent;
                    List<PathButton> uiElements = GetLogicalChildCollection<PathButton>(rootElement);
                    if (uiElements != null && uiElements.Count > 0)
                    {
                        foreach (PathButton uiElement in uiElements)
                        {
                            uiElement.IsEnabled = button.IsEnabled;
                        }
                    }
                }
            }
        }

        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (var child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                }
            }
        }
    }
}
