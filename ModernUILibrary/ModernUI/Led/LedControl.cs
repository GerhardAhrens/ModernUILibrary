//-----------------------------------------------------------------------
// <copyright file="LedControl.cs" company="Lifeprojects.de">
//     Class: LedControl
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.05.2022</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for LedControl
    /// </summary>
    /// <example>
    /// controls:LedControl ActiveLed="{Binding ActiveLedItem}" Height="60"
    /// controls:LedControl ActiveLed="{Binding ActiveLedItem}" Leds="{Binding Colors}" Height="60" 
    /// controls:LedControl ActiveLed="{Binding ActiveLedItem}" Leds="{Binding Colors}" LedOrientation="Vertical" Width="60" 
    /// </example>
    public class LedControl : ContentControl
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty LedOrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LedControl), new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty LedSizeProperty = 
            DependencyProperty.Register("LedSize", typeof(double), typeof(LedControl), new PropertyMetadata(30.0));

        /// <summary>
        /// Using a DependencyProperty as the backing store for Leds.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty LedsProperty =
            DependencyProperty.Register("Leds", typeof(List<Color>), typeof(LedControl),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    DefaultValue = new List<Color> { Colors.Red, Colors.Orange, Colors.Green },
                    PropertyChangedCallback = LedsChanged,
                });

        /// <summary>
        /// Using a DependencyProperty as the backing store for OffOpacity.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty OffOpacityProperty =
            DependencyProperty.Register("OffOpacity", typeof(double), typeof(LedControl), new PropertyMetadata(0.4));

        /// <summary>
        /// Using a DependencyProperty as the backing store for ActiveLed.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ActiveLedProperty =
            DependencyProperty.Register("ActiveLed", typeof(int), typeof(LedControl),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    DefaultValue = 0,
                    PropertyChangedCallback = ActiveLedChanged
                });

        /// <summary>
        /// Ellipses controls
        /// </summary>
        private List<Ellipse> ellipses = new List<Ellipse>();

        /// <summary>
        /// Constructor of the led control
        /// </summary>
        public LedControl()
        {
            WeakEventManager<ContentControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoadLeds);
            this.OffOpacity = 0.1;
        }


        /// <summary>
        /// Orientation of the leds
        /// </summary>
        public Orientation LedOrientation
        {
            get { return (Orientation)this.GetValue(LedOrientationProperty); }
            set { this.SetValue(LedOrientationProperty, value); }
        }

        /// <summary>
        /// Orientation of the leds
        /// </summary>
        public double LedSize
        {
            get { return (double)this.GetValue(LedSizeProperty); }
            set { this.SetValue(LedSizeProperty, value); }
        }

        /// <summary>
        /// Colors of the leds in on mode. Amount of colors equal the amount of leds displayed
        /// </summary>
        public List<Color> Leds
        {
            get { return (List<Color>)this.GetValue(LedsProperty); }
            set { this.SetValue(LedsProperty, value); }
        }


        /// <summary>
        /// Opacity of led in off mode
        /// </summary>
        public double OffOpacity
        {
            get { return (double)this.GetValue(OffOpacityProperty); }
            set { this.SetValue(OffOpacityProperty, value); }
        }

        /// <summary>
        /// Ative index of the leds
        /// Value 0 = nothing active
        /// Value 1 = first led active
        /// Value 2 = second led active etc.
        /// </summary>
        public int ActiveLed
        {
            get { return (int)this.GetValue(ActiveLedProperty); }
            set { this.SetValue(ActiveLedProperty, value); }
        }

        /// <summary>
        /// Property changed callback for LEDs
        /// </summary>
        /// <param name="d">The DependencyObject</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs</param>
        private static void LedsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LedControl c = d as LedControl;
            c.Leds = (List<Color>)e.NewValue;
            c.OnLoadLeds(d, null);
        }


        /// <summary>
        /// Property changed callback for the active LED index
        /// </summary>
        /// <param name="d">The DependencyObject</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs</param>
        private static void ActiveLedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LedControl c = d as LedControl;
            c.ActiveLed = (int)e.NewValue;
            c.LedOff();
            c.LedOn();
        }


        /// <summary>
        /// Load led into the panel
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Routed event atguments</param>
        private void OnLoadLeds(object sender, RoutedEventArgs e)
        {
            FrameworkElement parent = Parent as FrameworkElement;
            StackPanel panel = new StackPanel();
            this.Content = panel;
            panel.Orientation = this.LedOrientation;
            panel.Children.Clear();
            this.ellipses.Clear();
            double size;

            if (this.LedOrientation == Orientation.Horizontal)
            {
                size = this.Height;
            }
            else
            {
                size = this.Width;
            }

            if (size.Equals(double.NaN) || size == 0)
            {
                size = this.LedSize;
            }

            /* Give it some size if forgotten to define width or height in combination with orientation */
            if (size.Equals(double.NaN) && (parent != null) && (this.Leds.Count != 0))
            {
                if (parent.ActualWidth != double.NaN)
                {
                    size = parent.ActualWidth / this.Leds.Count;
                }
                else if (parent.ActualHeight != double.NaN)
                {
                    size = parent.ActualHeight / this.Leds.Count;
                }
            }

            foreach (Color color in this.Leds)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Height = size > 4 ? size - 4 : size;
                ellipse.Width = size > 4 ? size - 4 : size;
                ellipse.Margin = new Thickness(2);
                ellipse.Style = null;

                /* Border for led */
                RadialGradientBrush srgb = new RadialGradientBrush(new GradientStopCollection
                {
                    new GradientStop(Color.FromArgb(255, 211, 211, 211), 0.3d),
                    new GradientStop(Color.FromArgb(255, 169, 169, 169), 0.6d),
                    new GradientStop(Color.FromArgb(255, 150, 150, 150), 0.65d),
                });

                if (size <= 50)
                {
                    ellipse.StrokeThickness = 5;
                }
                else if (size <= 100)
                {
                    ellipse.StrokeThickness = 10;
                }
                else
                {
                    ellipse.StrokeThickness = 20;
                }

                srgb.GradientOrigin = new System.Windows.Point(0.5d, 0.5d);
                srgb.Center = new System.Windows.Point(0.5d, 0.5d);
                srgb.RadiusX = 0.5d;
                srgb.RadiusY = 0.5d;
                ellipse.Stroke = srgb;

                /* Color of led */
                RadialGradientBrush rgb = new RadialGradientBrush(new GradientStopCollection
                {
                    new GradientStop(Color.FromArgb(255, color.R, color.G, color.B), 1.0d),
                    new GradientStop(Color.FromArgb(200, color.R, color.G, color.B), 0.4d),
                    new GradientStop(Color.FromArgb(150, color.R, color.G, color.B), 0.1d),
                });

                rgb.GradientOrigin = new System.Windows.Point(0.5d, 0.5d);
                rgb.Center = new System.Windows.Point(0.5d, 0.5d);
                rgb.RadiusX = 0.5d;
                rgb.RadiusY = 0.5d;
                ellipse.Fill = rgb;
                ellipse.Fill.Opacity = this.OffOpacity;
                panel.Children.Add(ellipse);
                this.ellipses.Add(ellipse);
            }

            this.LedOn();
        }

        /// <summary>
        /// Switch on the active led
        /// </summary>
        private void LedOn()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = this.OffOpacity;
            animation.To = 1.0d;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            animation.AutoReverse = false;

            for (int i = 0; i < this.ellipses.Count; i++)
            {
                if ((this.ActiveLed - 1 == i) && (this.ellipses[i].Fill.Opacity < 1.0))
                {
                    this.ellipses[i].Fill.BeginAnimation(Brush.OpacityProperty, animation);
                }
            }
        }

        /// <summary>
        /// Switch off all but the active led
        /// </summary>
        private void LedOff()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1.0d;
            animation.To = this.OffOpacity;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            animation.AutoReverse = false;

            for (int i = 0; i < this.ellipses.Count; i++)
            {
                if ((this.ActiveLed - 1 != i) && (this.ellipses[i].Fill.Opacity > this.OffOpacity))
                {
                    this.ellipses[i].Fill.BeginAnimation(Brush.OpacityProperty, animation);
                }
            }
        }
    }
}