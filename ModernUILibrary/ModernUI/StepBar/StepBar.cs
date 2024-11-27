namespace ModernIU.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    public class StepBar : ItemsControl
    {
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register(nameof(Progress), typeof(int), typeof(StepBar), new PropertyMetadata(0, OnProgressChangedCallback, OnProgressCoerceValueCallback));

        #region Constructors
        static StepBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepBar), new FrameworkPropertyMetadata(typeof(StepBar)));
        }
        #endregion

        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        private static object OnProgressCoerceValueCallback(DependencyObject d, object baseValue)
        {
            StepBar stepBar = d as StepBar;
            int newValue = Convert.ToInt32(baseValue);
            if (newValue < 0)
            {
                return 0;
            }
            else if (newValue >= stepBar.Items.Count)
            {
                return stepBar.Items.Count - 1;
            }
            return newValue;
        }

        private static void OnProgressChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(StepBar), new PropertyMetadata(50d));

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StepBarItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            StepBarItem stepBarItem = element as StepBarItem;
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(stepBarItem);
            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(stepBarItem);
            stepBarItem.Number = Convert.ToString(++index);
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            for (int i = 0; i < this.Items.Count; i++)
            {
                StepBarItem stepBarItem = this.ItemContainerGenerator.ContainerFromIndex(i) as StepBarItem;
                if(stepBarItem != null)
                {
                    int temp = i;
                    stepBarItem.Number = Convert.ToString(++temp);
                }
            }

            this.Progress = 0;
        }
    }
}
