namespace ModernIU.Controls
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für ComboBoxColor.xaml
    /// </summary>
    public partial class ComboBoxColor : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Brush), typeof(ComboBoxColor), new UIPropertyMetadata(null, OnSelectedColorChangedCallback));

        public static readonly DependencyProperty SelectedColorCodeProperty = DependencyProperty.Register(nameof(SelectedColorCode), typeof(string), typeof(ComboBoxColor), new UIPropertyMetadata(string.Empty, OnSelectedColorChangedCallback));

        public ComboBoxColor()
        {
            this.InitializeComponent();
            this.BorderBrush = Brushes.Green;
            this.BorderThickness = new Thickness(1);
            this.superCombo.SelectionChanged += OnSelectionChanged;
        }

        ~ComboBoxColor()
        {
            this.superCombo.SelectionChanged -= OnSelectionChanged;
        }

        public Brush SelectedColor
        {
            get { return (Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public string SelectedColorCode
        {
            get { return (string)GetValue(SelectedColorCodeProperty); }
            set { SetValue(SelectedColorCodeProperty, value); }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            var colorName = ((Selector)e.Source).SelectedValue;
            BrushConverter convBrush = new BrushConverter();
            SolidColorBrush brush = convBrush.ConvertFromString(colorName.ToString()) as SolidColorBrush;
            this.SelectedColor = brush;
            this.SelectedColorCode = brush.Color.ToString();
        }

        private static void OnSelectedColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxColor colorComboBox = d as ComboBoxColor;

            if (d != null)
            {
                if (e.NewValue != e.OldValue && e.NewValue is Brush)
                {
                    colorComboBox.SelectedColor = (Brush)e.NewValue;

                    BrushConverter convBrush = new BrushConverter();
                    SolidColorBrush brush = convBrush.ConvertFromString(e.NewValue.ToString()) as SolidColorBrush;

                    if (brush is SolidColorBrush colorBrush)
                    {
                        Color color = colorBrush.Color;
                        string colorName = GetColorName(color);
                        colorComboBox.superCombo.SelectedValue = colorName;
                    }
                }
                else
                {
                    colorComboBox.SelectedColorCode = (string)e.NewValue;
                }
            }
        }

        private static string GetColorName(Color col)
        {
            PropertyInfo colorProperty = typeof(Colors).GetProperties().FirstOrDefault(p => Color.AreClose((Color)p.GetValue(null), col));
            return colorProperty != null ? colorProperty.Name : "unnamed color";
        }
    }
}
