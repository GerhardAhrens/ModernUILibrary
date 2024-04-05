namespace ModernIU.Controls
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    public class ButtonGroup : ItemsControl
    {
        public static readonly DependencyProperty CornerRadiusProperty =
    DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ButtonGroup));

        public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(ButtonGroup));

        static ButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonGroup), new FrameworkPropertyMetadata(typeof(ButtonGroup)));
        }

        public event RoutedPropertyChangedEventHandler<object> ItemClick
        {
            add
            {
                this.AddHandler(ItemClickEvent, value);
            }
            remove
            {
                this.RemoveHandler(ItemClickEvent, value);
            }
        }

        public virtual void OnItemClick(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, ItemClickEvent);
            this.RaiseEvent(arg);
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            int index = this.ItemContainerGenerator.IndexFromContainer(element);
            ButtonGroupItem buttonGroupItem = element as ButtonGroupItem;
            if (buttonGroupItem == null)
            {
                return;
            }

            if (index == 0)
            {
                buttonGroupItem.IsFirstItem = true;
                buttonGroupItem.CornerRadius = new CornerRadius(this.CornerRadius.TopLeft, 0, 0, this.CornerRadius.BottomLeft);
            }

            if (index == this.Items.Count - 1)
            {
                buttonGroupItem.IsLastItem = true;
                buttonGroupItem.CornerRadius = new CornerRadius(0, this.CornerRadius.TopRight, this.CornerRadius.BottomRight, 0);
            }

            base.PrepareContainerForItemOverride(buttonGroupItem, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ButtonGroupItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            /* Mit dem folgenden Code wird das Erscheinungsbild der einzelnen Elemente korrekt eingestellt, 
             * wenn Elemente hinzugefügt oder entfernt werden */
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    /* Wenn das neu hinzugefügte Element an der ersten Position platziert wird, 
                     * wird der Wert der ursprünglichen Eigenschaft erste Position geändert. */
                    if (e.NewStartingIndex == 0)
                    {
                        this.SetButtonGroupItem(e.NewStartingIndex + e.NewItems.Count);
                    }

                    /* Wenn das neu hinzugefügte Element an der letzten Position platziert wird, 
                     * ändern Sie den Wert der Eigenschaft an der letzten Position. */
                    if (e.NewStartingIndex == this.Items.Count - e.NewItems.Count)
                    {
                        this.SetButtonGroupItem(e.NewStartingIndex - 1);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    /* Wenn die Entfernung die erste ist, ändern Sie den Eigenschaftswert des aktualisierten ersten Elements */
                    if (e.OldStartingIndex == 0)
                    {
                        this.SetButtonGroupItem(0);
                    }
                    else
                    {
                        this.SetButtonGroupItem(e.OldStartingIndex - 1);
                    }
                    break;
            }
        }

        /// <summary>
        /// Einstellung der Eigenschaft Position eines SegmentItems
        /// </summary>
        /// <param name="index"></param>
        private void SetButtonGroupItem(int index)
        {
            if (index > this.Items.Count || index < 0)
            {
                return;
            }

            ButtonGroupItem buttonGroupItem = this.ItemContainerGenerator.ContainerFromIndex(index) as ButtonGroupItem;
            if (buttonGroupItem == null)
            {
                return;
            }
            buttonGroupItem.IsFirstItem = index == 0;
            buttonGroupItem.IsLastItem = index == this.Items.Count - 1;
            buttonGroupItem.IsMiddleItem = index > 0 && index < this.Items.Count - 1;
        }
    }
}
