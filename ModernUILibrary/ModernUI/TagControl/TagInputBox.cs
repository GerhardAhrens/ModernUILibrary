namespace ModernIU.Controls
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;

    public class TagInputBox : Control
    {

        #region ItemsSource

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(TagInputBox), new PropertyMetadata(null));

        #endregion

        #region ItemsSourceInternal

        public IEnumerable ItemsSourceInternal
        {
            get { return (IEnumerable)GetValue(ItemsSourceInternalProperty); }
            set { SetValue(ItemsSourceInternalProperty, value); }
        }
        
        public static readonly DependencyProperty ItemsSourceInternalProperty =
            DependencyProperty.Register(nameof(ItemsSourceInternal), typeof(IEnumerable), typeof(TagInputBox), new PropertyMetadata(null));

        #endregion

        #region Constructors

        static TagInputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TagInputBox), new FrameworkPropertyMetadata(typeof(TagInputBox)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
