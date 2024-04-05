namespace ModernIU.Controls
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Segmenttastensteuerung, ähnlich wie IOS SegmentControl.
    /// </summary>
    public class SegmentControl : ListBox
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SegmentControl));

        public static readonly DependencyProperty IsAllRoundProperty =
            DependencyProperty.Register("IsAllRound", typeof(bool), typeof(SegmentControl), new PropertyMetadata(false));

        public static readonly RoutedEvent ItemClickEvent = 
            EventManager.RegisterRoutedEvent("ItemClick", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(SegmentControl));

        static SegmentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SegmentControl), new FrameworkPropertyMetadata(typeof(SegmentControl)));
        }

        [Bindable(true), Description("Abrufen oder Einstellen der Randabrundung")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
        [Bindable(true), Description("Liest oder setzt, ob die abgerundeten Ecken um das SegmentItem mit dem SegmentControl konsistent sind.")]
        public bool IsAllRound
        {
            get { return (bool)GetValue(IsAllRoundProperty); }
            set { SetValue(IsAllRoundProperty, value); }
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

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            int index = this.ItemContainerGenerator.IndexFromContainer(element);
            SegmentItem segmentItem = element as SegmentItem;
            if (segmentItem == null)
            {
                return;
            }

            if (index == 0)
            {
                segmentItem.IsFirstItem = true;
                segmentItem.CornerRadius = new CornerRadius(this.CornerRadius.TopLeft, 0, 0, this.CornerRadius.BottomLeft);
            }

            if (index == this.Items.Count - 1)
            {
                segmentItem.IsLastItem = true;
                segmentItem.CornerRadius = new CornerRadius(0, this.CornerRadius.TopRight, this.CornerRadius.BottomRight, 0);
            }

            if(this.IsAllRound)
            {
                segmentItem.CornerRadius = this.CornerRadius;
                segmentItem.BorderThickness = new Thickness(0);
                segmentItem.Padding = new Thickness(20, 5, 20, 5);
            }

            base.PrepareContainerForItemOverride(segmentItem, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SegmentItem();
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
                        this.SetSegmentItem(e.NewStartingIndex + e.NewItems.Count);
                    }

                    /* Wenn das neu hinzugefügte Element an der letzten Position platziert wird, 
                     * ändern Sie den Wert der Eigenschaft an der letzten Position. */
                    if (e.NewStartingIndex == this.Items.Count - e.NewItems.Count)
                    {
                        this.SetSegmentItem(e.NewStartingIndex - 1);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    /* Wenn die Entfernung die erste ist, ändern Sie den Eigenschaftswert des aktualisierten ersten Elements */
                    if (e.OldStartingIndex == 0) 
                    {
                        this.SetSegmentItem(0);
                    }
                    else
                    {
                        this.SetSegmentItem(e.OldStartingIndex - 1);
                    }
                    break;
            }
        }

        /// <summary>
        /// Einstellung der Eigenschaft Position eines SegmentItems
        /// </summary>
        /// <param name="index"></param>
        private void SetSegmentItem(int index)
        {
            if (index > this.Items.Count || index < 0)
            {
                return;
            }

            SegmentItem segmentItem = this.ItemContainerGenerator.ContainerFromIndex(index) as SegmentItem;
            if (segmentItem == null)
            {
                return;
            }
            segmentItem.IsFirstItem = index == 0;
            segmentItem.IsLastItem = index == this.Items.Count - 1;
            segmentItem.IsMiddleItem = index > 0 && index < this.Items.Count - 1;
        }
    }
}
