namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class FloatingActionButton : ContentControl
    {
        #region Constructors

        static FloatingActionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatingActionButton), new FrameworkPropertyMetadata(typeof(FloatingActionButton)));
        }

        #endregion

        private FloatingActionMenu ParentItemsControl
        {
            get { return this.ParentSelector as FloatingActionMenu; }
        }

        internal ItemsControl ParentSelector
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as ItemsControl; }
        }

        public string TipContent
        {
            get { return (string)GetValue(TipContentProperty); }
            set { SetValue(TipContentProperty, value); }
        }

        public static readonly DependencyProperty TipContentProperty =
            DependencyProperty.Register(nameof(TipContent), typeof(string), typeof(FloatingActionButton), new PropertyMetadata(string.Empty));


        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseLeftButtonDown += FloatingActionButton_MouseLeftButtonDown;
        }

        private void FloatingActionButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(this.ParentItemsControl != null)
            {
                this.ParentItemsControl.OnItemClick(this.Content, this.Content);
                this.ParentItemsControl.IsDropDownOpen = false;
            }
        }

        #endregion
    }
}
