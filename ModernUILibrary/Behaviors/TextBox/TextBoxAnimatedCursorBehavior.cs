//-----------------------------------------------------------------------
// <copyright file="AnimatedTextBoxBehavior.cs" company="Lifeprojects.de">
//     Class: AnimatedTextBoxBehavior
//     Copyright © Gerhard Ahrens, 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.10.2022</date>
//
// <summary>
// Änderung der Cursor Darstellung in einer Textbox
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    using Microsoft.Xaml.Behaviors;

    using ModernIU.BehaviorsBase;

    [SupportedOSPlatform("windows")]
    public sealed class TextBoxAnimatedCursorBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            if (!AssociatedObject.IsLoaded)
            {
                AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            }
            else
            {
                SetupAdorner();
            }
        }

        protected override void OnDetaching()
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(TextBoxBase.CaretBrushProperty, typeof(TextBox));
            descriptor.RemoveValueChanged(AssociatedObject, OnCaretBrushChanged);

            AssociatedObject.CaretBrush = originalCaretBrush;
            layer.Remove(caretAdorner);
            caretAdorner.Dispose();
            caretAdorner = null;
        }

        private Brush originalCaretBrush;
        private AdornerLayer layer;
        private CaretAdorner caretAdorner;

        private void OnCaretBrushChanged(object sender, EventArgs eventArgs)
        {
            Brush brush = AssociatedObject.CaretBrush;
            caretAdorner.CaretBrush = brush;
            originalCaretBrush = brush;
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            SetupAdorner();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }

        private void SetupAdorner()
        {
            originalCaretBrush = AssociatedObject.CaretBrush;
            AssociatedObject.CaretBrush = Brushes.Transparent;

            var descriptor = DependencyPropertyDescriptor.FromProperty(TextBoxBase.CaretBrushProperty, typeof(TextBox));
            descriptor.AddValueChanged(AssociatedObject, OnCaretBrushChanged);

            layer = AdornerLayer.GetAdornerLayer(AssociatedObject);

            caretAdorner = new CaretAdorner(AssociatedObject)
            {
                CaretBrush = originalCaretBrush,
                DataContext = AssociatedObject
            };
            caretAdorner.SetBinding(UIElement.VisibilityProperty, new Binding(nameof(TextBox.Visibility)));
            layer.Add(caretAdorner);
        }
    }
}
