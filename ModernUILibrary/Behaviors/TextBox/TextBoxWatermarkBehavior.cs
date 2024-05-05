//-----------------------------------------------------------------------
// <copyright file="TextBoxInputMaskBehavior.cs" company="Lifeprojects.de">
//     Class: TextBoxInputMaskBehavior
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.12.2022 08:50</date>
//
// <summary>
// Klasse für ein TextBoxWatermark Behavior
// </summary>
// <Website>
// https://blindmeis.wordpress.com/2010/07/16/wpf-watermark-textbox-behavior/
// </Website>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Xaml.Behaviors;

    using ModernIU.BehaviorsBase;

    [SupportedOSPlatform("windows")]
    public class TextBoxWatermarkBehavior : Behavior<TextBox>
    {
        private TextBlockAdorner adorner;
        private WeakPropertyChangeNotifier notifier;

        #region DependencyProperty's

        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(TextBoxWatermarkBehavior));

        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        public static readonly DependencyProperty WatermarkStyleProperty =
            DependencyProperty.RegisterAttached("WatermarkStyle", typeof(Style), typeof(TextBoxWatermarkBehavior));

        public Style WatermarkStyle
        {
            get { return (Style)GetValue(WatermarkStyleProperty); }
            set { SetValue(WatermarkStyleProperty, value); }
        }

        public static readonly DependencyProperty MinCharsProperty =
            DependencyProperty.RegisterAttached("MinChars", typeof(int), typeof(TextBoxWatermarkBehavior), new PropertyMetadata(1));

        public int MinChars
        {
            get { return (int)GetValue(MinCharsProperty); }
            set { SetValue(MinCharsProperty, value); }
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectLoaded;
            AssociatedObject.TextChanged += AssociatedObjectTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            AssociatedObject.TextChanged -= AssociatedObjectTextChanged;

            notifier = null;
        }

        private void AssociatedObjectTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAdorner();
        }

        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            adorner = new TextBlockAdorner(AssociatedObject, WatermarkText, WatermarkStyle);

            UpdateAdorner();

            //AddValueChanged for IsFocused in a weak manner
            notifier = new WeakPropertyChangeNotifier(AssociatedObject, UIElement.IsFocusedProperty);
            notifier.ValueChanged += new EventHandler(UpdateAdorner);
        }

        private void UpdateAdorner(object sender, EventArgs e)
        {
            UpdateAdorner();
        }


        private void UpdateAdorner()
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text) == false || AssociatedObject.IsFocused)
            {
                if (AssociatedObject.Text.Trim().Length > MinChars)
                {
                    // Hide the Watermark Label if the adorner layer is visible
                    AssociatedObject.TryRemoveAdorners<TextBlockAdorner>();
                }
                else
                {
                    if (adorner != null)
                    {
                        AssociatedObject.TryAddAdorner<TextBlockAdorner>(adorner);
                    }
                }
            }
            else
            {
                // Show the Watermark Label if the adorner layer is visible
                if (adorner != null)
                {
                    AssociatedObject.TryAddAdorner<TextBlockAdorner>(adorner);
                }
            }
        }
    }
}
