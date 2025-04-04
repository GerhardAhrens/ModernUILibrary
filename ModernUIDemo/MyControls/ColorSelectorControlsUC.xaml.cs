﻿namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für ColorSelectorControlsUC.xaml
    /// </summary>
    public partial class ColorSelectorControlsUC : UserControl
    {
        public ColorSelectorControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Color> list = new List<Color>
            {
                Color.FromRgb(0, 0, 0),
                Color.FromRgb(153, 51, 0),
                Color.FromRgb(51, 51, 0),
                Color.FromRgb(0, 51, 0),
                Color.FromRgb(0, 51, 102),
                Color.FromRgb(0, 0, 128),
                Color.FromRgb(51, 51, 153),
                Color.FromRgb(51, 51, 51),
                Color.FromRgb(128, 0, 0),
                Color.FromRgb(255, 102, 0),
                Color.FromRgb(128, 128, 0),
                Color.FromRgb(0, 128, 0),
                Color.FromRgb(0, 128, 128),
                Color.FromRgb(0, 0, 255),
                Color.FromRgb(102, 102, 153),
                Color.FromRgb(128, 128, 128),
                Color.FromRgb(255, 0, 0),
                Color.FromRgb(255, 153, 0),
                Color.FromRgb(153, 204, 0),
                Color.FromRgb(51, 153, 102),
                Color.FromRgb(51, 204, 204),
                Color.FromRgb(51, 102, 255),
                Color.FromRgb(128, 0, 128),
                Color.FromRgb(153, 153, 153),
                Color.FromRgb(255, 0, 255),
                Color.FromRgb(255, 204, 0),
                Color.FromRgb(255, 255, 0),
                Color.FromRgb(0, 255, 0),
                Color.FromRgb(0, 255, 255),
                Color.FromRgb(0, 204, 255),
                Color.FromRgb(153, 51, 102),
                Color.FromRgb(192, 192, 192),
                Color.FromRgb(255, 153, 204),
                Color.FromRgb(255, 204, 153),
                Color.FromRgb(255, 255, 153),
                Color.FromRgb(0, 255, 0),
                Color.FromRgb(204, 255, 204),
                Color.FromRgb(153, 204, 255),
                Color.FromRgb(204, 153, 255),
            };
            this.ColorSelectorA.ItemsSource = new ReadOnlyCollection<Color>(list);
        }
    }
}
