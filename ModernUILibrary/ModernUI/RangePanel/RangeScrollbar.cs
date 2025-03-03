﻿namespace ModernIU.Controls
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;


    [TemplatePart(Name = "PART_RangeOverlay", Type = typeof(RangeItemsControl))]
    [ContentProperty("Items")]
    public class RangeScrollbar : ScrollBar, INotifyPropertyChanged
    {

        static RangeScrollbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeScrollbar), new FrameworkPropertyMetadata(typeof(RangeScrollbar)));
        }

        public RangeScrollbar()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _RangeControl = base.GetTemplateChild("PART_RangeOverlay") as RangeItemsControl;


            if (_RangeControl == null)
                return;

            if (_iItems != null && _iItems.Count > 0)
            {
                foreach (var item in _iItems)
                    _RangeControl.Items.Add(item);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Items"));

                _iItems = null;

            }
        }


        RangeItemsControl _RangeControl;
        [Bindable(false), Category("Content")]
        public RangeItemsControl RangeControl
        {
            get { return _RangeControl; }
        }


        ObservableCollection<UIElement> _iItems = new ObservableCollection<UIElement>();

        public event PropertyChangedEventHandler PropertyChanged;

        [Bindable(true), Category("Content")]
        public IList Items
        {
            get
            {
                if (_RangeControl == null)
                    ApplyTemplate();
                if (_RangeControl != null)
                    return _RangeControl.Items;

                return _iItems;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(RangeScrollbar));
        [Bindable(true), Category("Content")]
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public static readonly DependencyProperty ItemTemplateProperty = ItemsControl.ItemTemplateProperty.AddOwner(typeof(RangeScrollbar));
        [Bindable(true), Category("Content")]
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty AlternationCountProperty = ItemsControl.AlternationCountProperty.AddOwner(typeof(RangeScrollbar));
        [Bindable(true), Category("Content")]
        public int AlternationCount
        {
            get { return (int)GetValue(AlternationCountProperty); }
            set { SetValue(AlternationCountProperty, value); }
        }


    }
}
