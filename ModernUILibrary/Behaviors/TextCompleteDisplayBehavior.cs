namespace ModernIU.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    using Microsoft.Xaml.Behaviors;

    public class TextCompleteDisplayBehavior : Behavior<TextBlock>
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextCompleteDisplayBehavior), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ShowWidthProperty =
            DependencyProperty.Register("ShowWidth", typeof(double), typeof(TextCompleteDisplayBehavior), new PropertyMetadata(0d));

        private Popup popup;
        private TextBox textBox;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public double ShowWidth
        {
            get { return (double)GetValue(ShowWidthProperty); }
            set { SetValue(ShowWidthProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.OpenTextShowHost();
            }
        }

        private void OpenTextShowHost()
        {
            try
            {
                if (popup == null)
                {
                    popup = new Popup
                    {
                        PlacementTarget = AssociatedObject,
                        Placement = PlacementMode.Bottom,
                        AllowsTransparency = true,
                        StaysOpen = false,
                        VerticalOffset = -AssociatedObject.ActualHeight,
                    };

                    Grid root = new Grid();
                    root.Margin = new Thickness(5);
                    Border shadow = new Border()
                    {
                        Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Effect = new DropShadowEffect()
                        {
                            BlurRadius = 5,
                            Opacity = 0.2,
                            ShadowDepth = 0,
                            Color = Color.FromRgb(64, 64, 64),
                        },
                    };
                    root.Children.Add(shadow);

                    Border border = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(204, 206, 219)),
                        Padding = new Thickness(3),
                        Width = (this.ShowWidth == 0 || this.ShowWidth == double.NaN) ? AssociatedObject.ActualWidth : this.ShowWidth,
                        SnapsToDevicePixels = true,
                        UseLayoutRounding = true,
                    };

                    textBox = new TextBox
                    {
                        TextWrapping = TextWrapping.Wrap,
                        BorderThickness = new Thickness(0),
                        IsReadOnly = true
                    };

                    border.Child = textBox;

                    root.Children.Add(border);
                    popup.Child = root;
                }

                this.textBox.Text = this.Text;
                this.popup.IsOpen = true;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }
    }
}