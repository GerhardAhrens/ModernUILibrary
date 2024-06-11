namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ContentFrame : ContentControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty ContentFrameColorProperty =
            DependencyProperty.Register("ContentFrameColor", typeof(Brush), typeof(ContentFrame), new UIPropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty VerticalTitleAlignmentProperty =
            DependencyProperty.Register("VerticalTitleAlignment", typeof(VerticalAlignment), typeof(ContentFrame), new UIPropertyMetadata(VerticalAlignment.Center));

        public static readonly DependencyProperty HorizontalTitleAlignmentProperty =
            DependencyProperty.Register("HorizontalTitleAlignment", typeof(HorizontalAlignment), typeof(ContentFrame), new UIPropertyMetadata(HorizontalAlignment.Right));

        public static readonly DependencyProperty TitleProperty = 
            DependencyProperty.Register("Title", typeof(object), typeof(ContentFrame), new UIPropertyMetadata("ToDo"));

        public static readonly DependencyProperty TitleMarginProperty =
            DependencyProperty.Register("TitleMargin", typeof(Thickness), typeof(ContentFrame), new UIPropertyMetadata(new Thickness(5, 0, 5, 0)));

        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ContentFrame), new UIPropertyMetadata(new Thickness(0, 2, 0, 2)));

        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register("TitleFontWeight", typeof(FontWeight), typeof(ContentFrame), new UIPropertyMetadata(FontWeights.Medium, OnTitleFontWeightsChanged));

        public static readonly DependencyProperty RequiredFieldProperty =
            DependencyProperty.Register("RequiredField", typeof(bool), typeof(ContentFrame), new UIPropertyMetadata(false, OnRequiredFieldChanged));
        #endregion Dependency Properties

        #region Constructor
        static ContentFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentFrame), new FrameworkPropertyMetadata(typeof(ContentFrame)));
        }
        #endregion Constructor

        #region Properties
        /// <summary>
        /// Gets or sets the title of the item shown on the left.
        /// </summary>
        [DefaultValue(null)]
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set {this.SetValue(TitleProperty, value); }
        }

        public Brush ContentFrameColor
        {
            get { return (Brush)GetValue(ContentFrameColorProperty); }
            set { this.SetValue(ContentFrameColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the title.
        /// </summary>
        [DefaultValue(VerticalAlignment.Center)]
        public VerticalAlignment VerticalTitleAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalTitleAlignmentProperty); }
            set { this.SetValue(VerticalTitleAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the title.
        /// </summary>
        [DefaultValue(HorizontalAlignment.Right)]
        public HorizontalAlignment HorizontalTitleAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalTitleAlignmentProperty); }
            set { this.SetValue(HorizontalTitleAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin of the title.
        /// </summary>
        public Thickness TitleMargin
        {
            get { return (Thickness)GetValue(TitleMarginProperty); }
            set { this.SetValue(TitleMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin of the content.
        /// </summary>
        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { this.SetValue(ContentMarginProperty, value); }
        }

        public FontWeight TitleFontWeight
        {
            get { return (FontWeight)GetValue(TitleFontWeightProperty); }
            set { this.SetValue(TitleFontWeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the RequiredField.
        /// </summary>
        public bool RequiredField
        {
            get { return (bool)GetValue(RequiredFieldProperty); }
            set { this.SetValue(RequiredFieldProperty, value); }
        }
        #endregion Properties

        #region Private Methodes
        private static void OnTitleFontWeightsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (ContentFrame)d;

                if (e.NewValue is FontWeight)
                {
                    control.FontWeight = (FontWeight)e.NewValue;
                }
            }
        }

        private static void OnRequiredFieldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (ContentFrame)d;

                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.Title = $"{control.Title}*";
                        control.FontStyle = FontStyles.Italic;
                    }
                    else
                    {
                        control.Title = $"{control.Title} ";
                        control.FontStyle = FontStyles.Normal;
                    }
                }
                else
                {
                    control.Title = $"{control.Title} ";
                    control.FontStyle = FontStyles.Normal;
                }
            }
        }
        #endregion Private Methodes
    }
}