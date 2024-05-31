//-----------------------------------------------------------------------
// <copyright file="CheckBoxBehavior.cs" company="Lifeprojects.de">
//     Class: CheckBoxBehavior
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.02.2023</date>
//
// <summary>
// Die Behavior Klasse stellt die Zusände einer CheckBox in zuweisbaren Farben dar
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Microsoft.Xaml.Behaviors;

    [SupportedOSPlatform("windows")]
    public class CheckBoxBehavior : Behavior<CheckBox>
    {
        #region Brushes as Dependency Properties

        public Brush CheckedBrush
        {
            get { return (Brush)GetValue(CheckedBrushProperty); }
            set { SetValue(CheckedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedBrushProperty =
            DependencyProperty.Register("CheckedBrush", typeof(Brush), typeof(CheckBoxBehavior), new PropertyMetadata(Brushes.Black));

        public Brush IntermediateBrush
        {
            get { return (Brush)GetValue(IntermediateBrushProperty); }
            set { SetValue(IntermediateBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntermediateBrushProperty =
            DependencyProperty.Register("IntermediateBrush", typeof(Brush), typeof(CheckBoxBehavior), new PropertyMetadata(Brushes.Black));

        public Brush UncheckedBrush
        {
            get { return (Brush)GetValue(UncheckedBrushProperty); }
            set { SetValue(UncheckedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UncheckedBrushProperty =
            DependencyProperty.Register("UncheckedBrush", typeof(Brush), typeof(CheckBoxBehavior), new PropertyMetadata(Brushes.Black));

        #endregion Brushes as Dependency Properties

        /// <summary>
        /// Wird nach dem Anfügen des Verhaltens an das "AssociatedObject" aufgerufen.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Checked += AssociatedObjectChecked;
            AssociatedObject.Unchecked += AssociatedObjectChecked;
            AssociatedObject.Indeterminate += AssociatedObjectChecked;

            base.OnAttached();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObjectChecked(sender, null);
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Verhalten vom "AssociatedObject" getrennt wird.
        /// Der Aufruf erfolgt vor dem eigentlichen Trennvorgang.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Checked -= AssociatedObjectChecked;
            AssociatedObject.Unchecked -= AssociatedObjectChecked;
            AssociatedObject.Indeterminate -= AssociatedObjectChecked;

            base.OnDetaching();
        }

        private void AssociatedObjectChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            bool? status = AssociatedObject.IsChecked;

            if (status == true)
            {
                AssociatedObject.Background = CheckedBrush;
                AssociatedObject.BorderBrush = CheckedBrush;
                AssociatedObject.Foreground = CheckedBrush;
            }

            if (status == null)
            {
                AssociatedObject.Background = IntermediateBrush;
                AssociatedObject.BorderBrush = IntermediateBrush;
                AssociatedObject.Foreground = IntermediateBrush;
            }

            if (status == false)
            {
                AssociatedObject.Background = UncheckedBrush;
                AssociatedObject.BorderBrush = UncheckedBrush;
                AssociatedObject.Foreground = UncheckedBrush;
            }
        }
    }
}