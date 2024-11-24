namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class TagBox : ListBox
    {
        #region Constructors

        static TagBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TagBox), new FrameworkPropertyMetadata(typeof(TagBox)));
        }

        #endregion

        #region DependencyProperty

        #region IsLineFeed

        public bool IsLineFeed
        {
            get { return (bool)GetValue(IsLineFeedProperty); }
            set { SetValue(IsLineFeedProperty, value); }
        }
        
        public static readonly DependencyProperty IsLineFeedProperty =
            DependencyProperty.Register(nameof(IsLineFeed), typeof(bool), typeof(TagBox), new PropertyMetadata(true));

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TagBox));

        #endregion

        #endregion

        #region Override

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Tag();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
