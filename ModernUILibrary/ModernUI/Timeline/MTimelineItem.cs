namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    public class MTimelineItem : ContentControl
    {
        #region IsFirstItem

        [Bindable(true), Description("Liest oder setzt, ob der Eintrag der erste in der Liste ist.")]
        public bool IsFirstItem
        {
            get { return (bool)GetValue(IsFirstItemProperty); }
            set { SetValue(IsFirstItemProperty, value); }
        }
        
        public static readonly DependencyProperty IsFirstItemProperty =
            DependencyProperty.Register(nameof(IsFirstItem), typeof(bool), typeof(MTimelineItem), new PropertyMetadata(false));

        #endregion

        #region IsMiddleItem

        [Bindable(true), Description("Liest oder setzt, ob der Eintrag einer der mittleren in der Liste ist.")]
        public bool IsMiddleItem
        {
            get { return (bool)GetValue(IsMiddleItemProperty); }
            set { SetValue(IsMiddleItemProperty, value); }
        }

        public static readonly DependencyProperty IsMiddleItemProperty =
            DependencyProperty.Register(nameof(IsMiddleItem), typeof(bool), typeof(MTimelineItem), new PropertyMetadata(false));

        #endregion

        #region IsLastItem
        [Bindable(true), Description("Liest oder setzt, ob der Eintrag der letzte in der Liste ist.")]
        public bool IsLastItem
        {
            get { return (bool)GetValue(IsLastItemProperty); }
            set { SetValue(IsLastItemProperty, value); }
        }
        
        public static readonly DependencyProperty IsLastItemProperty =
            DependencyProperty.Register("IsLastItem", typeof(bool), typeof(MTimelineItem), new PropertyMetadata(false));

        #endregion

        #region Constructors

        static MTimelineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTimelineItem), new FrameworkPropertyMetadata(typeof(MTimelineItem)));
        }

        #endregion
    }
}
