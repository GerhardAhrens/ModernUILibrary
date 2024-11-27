//-----------------------------------------------------------------------
// <copyright file="GeometryButton.cs" company="Lifeprojects.de">
//     Class: GeometryButton
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>29.04.2023</date>
//
// <summary>Class for UI Control Button</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public sealed class GeometryButton : Button
    {
        static GeometryButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeometryButton), new FrameworkPropertyMetadata(typeof(GeometryButton)));
        }

        public GeometryButton()
        {
        }

        #region PathData

        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register(nameof(PathData), typeof(Geometry), typeof(GeometryButton), new PropertyMetadata(Geometry.Empty));

        public Geometry PathData
        {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        #endregion

        #region TextProperty
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(GeometryButton),
                new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region Orientation

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(GeometryButton),
                new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        #region TextVisibility

        public static readonly DependencyProperty TextVisibilityProperty =
            DependencyProperty.Register(nameof(TextVisibility), typeof(Visibility), typeof(GeometryButton),
                new PropertyMetadata(default(Visibility)));

        public Visibility TextVisibility
        {
            get { return (Visibility)GetValue(TextVisibilityProperty); }
            set { SetValue(TextVisibilityProperty, value); }
        }

        #endregion

        #region IconVisibility

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(GeometryButton),
                new PropertyMetadata(default(Visibility)));

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        #endregion
    }
}
