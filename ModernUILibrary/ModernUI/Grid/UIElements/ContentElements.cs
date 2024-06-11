namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ContentElements : ItemsControl
    {
        #region Member
        public static readonly DependencyProperty ContentFrameColorsProperty =
            DependencyProperty.Register("ContentFrameColors", typeof(Brush), typeof(ContentElements), new UIPropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty VerticalTitleAlignmentsProperty =
            DependencyProperty.Register("VerticalTitleAlignments", typeof(VerticalAlignment), typeof(ContentElements), new UIPropertyMetadata(VerticalAlignment.Center));

        public static readonly DependencyProperty HorizontalTitleAlignmentsProperty =
            DependencyProperty.Register("HorizontalTitleAlignments", typeof(HorizontalAlignment), typeof(ContentElements), new UIPropertyMetadata(HorizontalAlignment.Left));

        public static readonly DependencyProperty TitleMarginsProperty =
            DependencyProperty.Register("TitleMargins", typeof(Thickness), typeof(ContentElements), new UIPropertyMetadata(new Thickness(5, 0, 5, 0)));

        public static readonly DependencyProperty HorizontalContentAlignmentsProperty =
            DependencyProperty.Register("HorizontalContentAlignments", typeof(HorizontalAlignment), typeof(ContentElements), new UIPropertyMetadata(HorizontalAlignment.Stretch));

        public static readonly DependencyProperty VerticalContentAlignmentsProperty =
            DependencyProperty.Register("VerticalContentAlignments", typeof(VerticalAlignment), typeof(ContentElements), new UIPropertyMetadata(VerticalAlignment.Center));

        public static readonly DependencyProperty ContentMarginsProperty =
            DependencyProperty.Register("ContentMargins", typeof(Thickness), typeof(ContentElements), new UIPropertyMetadata(new Thickness(0, 2, 0, 2)));
        #endregion Member

        #region Constructor
        static ContentElements()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentElements), new FrameworkPropertyMetadata(typeof(ContentElements)));
        }
        #endregion Constructor

        #region Properties
        public Brush ContentFrameColors
        {
            get { return (Brush)GetValue(ContentFrameColorsProperty); }
            set { this.SetValue(ContentFrameColorsProperty, value); }
        }

        [DefaultValue(VerticalAlignment.Center)]
        public VerticalAlignment VerticalTitleAlignments
        {
            get { return (VerticalAlignment)GetValue(VerticalTitleAlignmentsProperty); }
            set { this.SetValue(VerticalTitleAlignmentsProperty, value); }
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment HorizontalTitleAlignments
        {
            get { return (HorizontalAlignment)GetValue(HorizontalTitleAlignmentsProperty); }
            set { this.SetValue(HorizontalTitleAlignmentsProperty, value); }
        }

        public Thickness TitleMargins
        {
            get { return (Thickness)GetValue(TitleMarginsProperty); }
            set { this.SetValue(TitleMarginsProperty, value); }
        }

        [DefaultValue(HorizontalAlignment.Stretch)]
        public HorizontalAlignment HorizontalContentAlignments
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentsProperty); }
            set { this.SetValue(HorizontalContentAlignmentsProperty, value); }
        }

        [DefaultValue(VerticalAlignment.Center)]
        public VerticalAlignment VerticalContentAlignments
        {
            get { return (VerticalAlignment)GetValue(VerticalContentAlignmentsProperty); }
            set { this.SetValue(VerticalContentAlignmentsProperty, value); }
        }

        public Thickness ContentMargins
        {
            get { return (Thickness)GetValue(ContentMarginsProperty); }
            set { this.SetValue(ContentMarginsProperty, value); }
        }
        #endregion Properties

        #region protected override Methodes
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentFrame();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ContentFrame;
        }
        #endregion protected override Methodes
    }
}
