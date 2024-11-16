namespace ModernIU.Base
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class NumberBox : ComboBox
    {
        static NumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
        }

        public static readonly DependencyProperty StartNumberProperty = DependencyProperty.Register("StartNumber" , typeof(int), typeof(NumberBox));

        public int StartNumber
        {
            get { return (int)GetValue(StartNumberProperty); }
            set { SetValue(StartNumberProperty, value); }
        }

        public static readonly DependencyProperty EndNumberProperty = DependencyProperty.Register("EndNumber" , typeof(int), typeof(NumberBox));

        public int EndNumber
        {
            get { return (int)GetValue(EndNumberProperty); }
            set { SetValue(EndNumberProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title" , typeof(string), typeof(NumberBox));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty MaxDropDownWidthProperty = DependencyProperty.Register("MaxDropDownWidth" , typeof(double), typeof(NumberBox));

        public double MaxDropDownWidth
        {
            get { return (double)GetValue(MaxDropDownWidthProperty); }
            set { SetValue(MaxDropDownWidthProperty, value); }
        }

        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.Register("ShowShadow" , typeof(bool), typeof(NumberBox));

        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        public static readonly DependencyProperty ShadowBlurProperty = DependencyProperty.Register("ShadowBlur" , typeof(Thickness), typeof(NumberBox));

        public Thickness ShadowBlur
        {
            get { return (Thickness)GetValue(ShadowBlurProperty); }
            set { SetValue(ShadowBlurProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            List<int> list = new List<int>();
            for (int i = StartNumber; i <= EndNumber; i++)
            {
                list.Add(i);
            }
            this.ItemsSource = list;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new NumberBoxItem();
            item.OnItemSingleClickHandler += Item_OnClickHandler;
            return item;
        }

        private void Item_OnClickHandler(object sender, ItemMouseSingleClickEventArgs<object> e)
        {
            NumberBoxItem item = sender as NumberBoxItem;
            this.SelectedItem = item.Content;
        }
    }

    public class NumberBoxItem : System.Windows.Controls.ComboBoxItem
    {
        public event EventHandler<ItemMouseSingleClickEventArgs<object>> OnItemSingleClickHandler;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var selectedItem = ((System.Windows.FrameworkElement)e.OriginalSource).DataContext;
            this.OnItemSingleClickHandler(this, ItemMouseSingleClickEventArgs<object>.ItemSingleClick(selectedItem));
            base.OnMouseLeftButtonDown(e);
        }
    }
}
