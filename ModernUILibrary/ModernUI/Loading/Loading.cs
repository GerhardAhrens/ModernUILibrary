﻿namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class Loading : Control
    {
        private FrameworkElement PART_Root;

        public static readonly DependencyProperty IsActivedProperty =
            DependencyProperty.Register("IsActived", typeof(bool), typeof(Loading), new PropertyMetadata(true, OnIsActivedChangedCallback));

        private static void OnIsActivedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Loading loading = d as Loading;
            if(loading.PART_Root == null)
            {
                return;
            }
            VisualStateManager.GoToElementState(loading.PART_Root, (bool)e.NewValue ? "Active" : "Inactive", true);
        }

        // Using a DependencyProperty as the backing store for SpeedRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeedRatioProperty =
            DependencyProperty.Register("SpeedRatio", typeof(double), typeof(Loading), new PropertyMetadata(1d, OnSpeedRatioChangedCallback));

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumLoadingType), typeof(Loading), new PropertyMetadata(EnumLoadingType.DoubleArc));

        static Loading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading)));
        }

        public EnumLoadingType Type
        {
            get { return (EnumLoadingType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public bool IsActived
        {
            get { return (bool)GetValue(IsActivedProperty); }
            set { SetValue(IsActivedProperty, value); }
        }

        public double SpeedRatio
        {
            get { return (double)GetValue(SpeedRatioProperty); }
            set { SetValue(SpeedRatioProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_Root = this.GetTemplateChild("PART_Root") as FrameworkElement;
            if(this.PART_Root != null)
            {
                VisualStateManager.GoToElementState(this.PART_Root, this.IsActived ? "Active" : "Inactive", true);
                this.SetSpeedRatio(this.PART_Root, this.SpeedRatio);
            }
        }

        private void SetSpeedRatio(FrameworkElement element, double speedRatio)
        {
            foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(element))
            {
                if (group.Name == "ActiveStates")
                {
                    foreach (VisualState state in group.States)
                    {
                        if (state.Name == "Active")
                        {
                            state.Storyboard.SetSpeedRatio(element, speedRatio);
                        }
                    }
                }
            }
        }

        private static void OnSpeedRatioChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Loading loading = d as Loading;
            if (loading.PART_Root == null || !loading.IsActived)
            {
                return;
            }

            loading.SetSpeedRatio(loading.PART_Root, loading.SpeedRatio);
        }
    }
}
