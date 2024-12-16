namespace ModernIU.Controls
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Timeline
    /// </summary>
    public class MTimeline : ItemsControl
    {
        #region FirstSlotTemplate

        [Bindable(true), Description("FirstSlot")]
        public DataTemplate FirstSlotTemplate
        {
            get { return (DataTemplate)GetValue(FirstSlotTemplateProperty); }
            set { SetValue(FirstSlotTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty FirstSlotTemplateProperty =
            DependencyProperty.Register(nameof(FirstSlotTemplate), typeof(DataTemplate), typeof(MTimeline));

        #endregion

        #region MiddleSlotTemplate

        [Bindable(true), Description("Abrufen oder Festlegen des Aussehens des mittleren Zeitachsenpunkts")]
        public DataTemplate MiddleSlotTemplate
        {
            get { return (DataTemplate)GetValue(MiddleSlotTemplateProperty); }
            set { SetValue(MiddleSlotTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty MiddleSlotTemplateProperty =
            DependencyProperty.Register(nameof(MiddleSlotTemplate), typeof(DataTemplate), typeof(MTimeline));

        #endregion

        #region LastItemTemplate

        [Bindable(true), Description("Abrufen oder Festlegen des Aussehens des letzten Zeitachsenpunkts")]
        public DataTemplate LastSlotTemplate
        {
            get { return (DataTemplate)GetValue(LastSlotTemplateProperty); }
            set { SetValue(LastSlotTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty LastSlotTemplateProperty =
            DependencyProperty.Register(nameof(LastSlotTemplate), typeof(DataTemplate), typeof(MTimeline));

        #endregion

        #region IsCustomEverySlot

        [Bindable(true), Description("Liest oder legt fest, ob das Aussehen der einzelnen Zeitleistenpunkte angepasst werden soll. Wenn der Wert der Eigenschaft True ist, sind die Eigenschaften FirstSlotTemplate, MiddleSlotTemplate und LastSlotTemplate deaktiviert und nur das SlotTemplate kann eingestellt werden, um den Stil jedes Zeitleistenpunkts zu definieren")]
        public bool IsCustomEverySlot
        {
            get { return (bool)GetValue(IsCustomEverySlotProperty); }
            set { SetValue(IsCustomEverySlotProperty, value); }
        }
        
        public static readonly DependencyProperty IsCustomEverySlotProperty =
            DependencyProperty.Register(nameof(IsCustomEverySlot), typeof(bool), typeof(MTimeline), new PropertyMetadata(false));

        #endregion

        #region SlotTemplate

        [Bindable(true), Description("Liest oder legt das Aussehen jedes Zeitleistenpunkts fest. Diese Eigenschaft wird nur wirksam, wenn die Eigenschaft IsCustomEverySlot True ist.")]
        public DataTemplate SlotTemplate
        {
            get { return (DataTemplate)GetValue(SlotTemplateProperty); }
            set { SetValue(SlotTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty SlotTemplateProperty =
            DependencyProperty.Register(nameof(SlotTemplate), typeof(DataTemplate), typeof(MTimeline));

        #endregion

        #region Constructors

        static MTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTimeline), new FrameworkPropertyMetadata(typeof(MTimeline)));
        }

        #endregion

        #region Override

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            int index = this.ItemContainerGenerator.IndexFromContainer(element);
            MTimelineItem timelineItem = element as MTimelineItem;
            if(timelineItem == null)
            {
                return;
            }

            if(index == 0)
            {
                timelineItem.IsFirstItem = true;
            }

            if(index == this.Items.Count - 1)
            {
                timelineItem.IsLastItem = true;
            }

            base.PrepareContainerForItemOverride(timelineItem, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MTimelineItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == 0)
                    {
                        this.SetTimelineItem(e.NewStartingIndex + e.NewItems.Count);
                    }

                    if (e.NewStartingIndex == this.Items.Count - e.NewItems.Count)
                    {
                        this.SetTimelineItem(e.NewStartingIndex - 1);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if(e.OldStartingIndex == 0) 
                    {
                        this.SetTimelineItem(0);
                    }
                    else
                    {
                        this.SetTimelineItem(e.OldStartingIndex - 1);
                    }
                    break;
            }
        }
        #endregion

        #region private function
        private void SetTimelineItem(int index)
        {
            if(index > this.Items.Count || index < 0)
            {
                return;
            }

            MTimelineItem timelineItem = this.ItemContainerGenerator.ContainerFromIndex(index) as MTimelineItem;
            if(timelineItem == null)
            {
                return;
            }
            timelineItem.IsFirstItem = index == 0;
            timelineItem.IsLastItem = index == this.Items.Count - 1;
            timelineItem.IsMiddleItem = index > 0 && index < this.Items.Count - 1;
        }
        #endregion
    }
}
