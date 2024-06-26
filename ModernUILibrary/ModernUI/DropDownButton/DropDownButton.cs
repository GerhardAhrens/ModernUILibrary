﻿namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class DropDownButton : ContentControl
    {
        public static readonly DependencyProperty DropDownContentProperty;

        #region Constructors
        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
            DropDownButton.DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(DropDownButton), new UIPropertyMetadata(null, new PropertyChangedCallback(DropDownButton.OnDropDownContentChanged)));
        }
        #endregion

        public object DropDownContent
        {
            get { return base.GetValue(DropDownButton.DropDownContentProperty); }
            set { base.SetValue(DropDownButton.DropDownContentProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownButton), new PropertyMetadata(false));

        public double DropDownHeight
        {
            get { return (double)GetValue(DropDownHeightProperty); }
            set { SetValue(DropDownHeightProperty, value); }
        }
        
        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(DropDownButton), new PropertyMetadata(200d));

        public EnumTrigger Trigger
        {
            get { return (EnumTrigger)GetValue(TriggerProperty); }
            set { SetValue(TriggerProperty, value); }
        }
        
        public static readonly DependencyProperty TriggerProperty =
            DependencyProperty.Register("Trigger", typeof(EnumTrigger), typeof(DropDownButton), new PropertyMetadata(EnumTrigger.Click));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseLeftButtonUp += DropDownButton_MouseLeftButtonUp;
            this.MouseEnter += DropDownButton_MouseEnter;
            this.MouseLeave += DropDownButton_MouseLeave;
        }

        private void DropDownButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.Trigger == EnumTrigger.Hover)
            {
                this.IsDropDownOpen = false;
            }
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void DropDownButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(this.Trigger == EnumTrigger.Hover)
            {
                this.IsDropDownOpen = true;
            }
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        private void DropDownButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(this.Trigger == EnumTrigger.Click || this.Trigger == EnumTrigger.Custom)
            {
                this.IsDropDownOpen = true;
            }
            VisualStateManager.GoToState(this, "Pressed", true);
        }
        private static void OnDropDownContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DropDownButton dropDownButton = o as DropDownButton;
            if (dropDownButton != null)
            {
                dropDownButton.OnDropDownContentChanged(e.OldValue, e.NewValue);
            }
        }
        protected virtual void OnDropDownContentChanged(object oldValue, object newValue)
        {
        }
    }
}
