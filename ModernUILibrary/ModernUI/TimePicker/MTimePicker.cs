namespace ModernIU.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class MTimePicker : Control
    {
        private TextBox PART_TextBox;
        private TimeSelector PART_TimeSelector;
        private Popup PART_Popup;

        #region TimeStringFormat
        public string TimeStringFormat
        {
            get { return (string)GetValue(TimeStringFormatProperty); }
            set { SetValue(TimeStringFormatProperty, value); }
        }

        public static readonly DependencyProperty TimeStringFormatProperty =
            DependencyProperty.Register("TimeStringFormat", typeof(string), typeof(MTimePicker), new PropertyMetadata("HH:mm:ss"));
        #endregion

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(MTimePicker), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MTimePicker timePicker = d as MTimePicker;
            DateTime dt = (DateTime)e.NewValue;

            timePicker.PART_TimeSelector.SelectedTime = dt;
        }
        #endregion

        #region DropDownHeight

        public double DropDownHeight
        {
            get { return (double)GetValue(DropDownHeightProperty); }
            set { SetValue(DropDownHeightProperty, value); }
        }
        
        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(MTimePicker), new PropertyMetadata(168d));

        #endregion

        #region Constructors
        static MTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTimePicker), new FrameworkPropertyMetadata(typeof(MTimePicker)));
        }
        #endregion

        #region Overrider
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_TextBox = this.GetTemplateChild("PART_TextBox") as TextBox;
            this.PART_TimeSelector = this.GetTemplateChild("PART_TimeSelector") as TimeSelector;
            this.PART_Popup = this.GetTemplateChild("PART_Popup") as Popup;
            if(this.PART_TimeSelector != null)
            {
                this.PART_TimeSelector.Owner = this;
                this.PART_TimeSelector.SelectedTimeChanged += PART_TimeSelector_SelectedTimeChanged;
            }
            if(this.PART_Popup != null)
            {
                this.PART_Popup.Opened += PART_Popup_Opened;
            }
        }

        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            this.PART_TimeSelector.SetButtonSelected();
        }
        #endregion

        private void PART_TimeSelector_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (this.PART_TextBox != null && e.NewValue != null)
            {
                this.PART_TextBox.Text = e.NewValue.Value.ToString(this.TimeStringFormat);
                this.SelectedTime = e.NewValue;
            }
        }
    }
}
