namespace ModernIU.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;

    [TemplatePart(Name = "PART_SlideSwitchPanel", Type = typeof(SlideSwitchPanel))]
    [TemplatePart(Name = "PART_IndexPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PART_LastButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NextButton", Type = typeof(Button))]
    public class Carousel : Control
    {
        private SlideSwitchPanel PART_SlideSwitchPanel;
        private StackPanel PART_IndexPanel;
        private Button PART_LastButton;
        private Button PART_NextButton;
        private int ChildCount;
        private Timer autoPlayTimer;
        private string GroupName;

        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemTemplateProperty;
        public static readonly DependencyProperty AutoPlayProperty;
        public static readonly DependencyProperty AutoPlaySpeedProperty;

        #region Constructors
        static Carousel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Carousel), new FrameworkPropertyMetadata(typeof(Carousel)));
            Carousel.ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(Carousel));
            Carousel.ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(Carousel));
            Carousel.AutoPlayProperty = DependencyProperty.Register(nameof(AutoPlay), typeof(bool), typeof(Carousel), new PropertyMetadata(false, OnAutoPlayChangedCallback));
            Carousel.AutoPlaySpeedProperty = DependencyProperty.Register(nameof(AutoPlaySpeed), typeof(double), typeof(Carousel), new PropertyMetadata(2d, OnAutoPlaySpeedChangedCallback));
        }

        public Carousel()
        {
            this.ItemsSource = new List<object>();
            if (this.autoPlayTimer == null)
            {
                this.autoPlayTimer = new Timer();
                this.autoPlayTimer.Interval = this.AutoPlaySpeed;
                this.autoPlayTimer.Elapsed += AutoPlayTimer_Elapsed;
            }

            this.autoPlayTimer.Enabled = this.AutoPlay;
        }

        ~Carousel() { }
        #endregion

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }
        public double AutoPlaySpeed
        {
            get { return (double)GetValue(AutoPlaySpeedProperty); }
            set { SetValue(AutoPlaySpeedProperty, value); }
        }

        private static void OnAutoPlaySpeedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;

            carousel.autoPlayTimer.Enabled = false;
            carousel.autoPlayTimer.Interval = (double)e.NewValue;
            carousel.autoPlayTimer.Enabled = carousel.AutoPlay;
        }

        private static void OnAutoPlayChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;
            carousel.autoPlayTimer.Enabled = (bool)e.NewValue;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_SlideSwitchPanel = this.GetTemplateChild("PART_SlideSwitchPanel") as SlideSwitchPanel;
            this.PART_IndexPanel = this.GetTemplateChild("PART_IndexPanel") as StackPanel;
            this.PART_LastButton = this.GetTemplateChild("PART_LastButton") as Button;
            this.PART_NextButton = this.GetTemplateChild("PART_NextButton") as Button;

            this.GroupName = Guid.NewGuid().ToString("N");

            this.AddChildToPanel();
            this.AddIndexControlToPanel();

            if (this.PART_SlideSwitchPanel != null)
            {
                this.PART_SlideSwitchPanel.IndexChanged += PART_SlideSwitchPanel_IndexChanged;
                
            }

            this.MouseEnter += PART_SlideSwitchPanel_MouseEnter;
            this.MouseLeave += PART_SlideSwitchPanel_MouseLeave;

            if (this.PART_IndexPanel != null && this.PART_IndexPanel.Children.Count > 0)
            {
                ((RadioButton)this.PART_IndexPanel.Children[0]).IsChecked = true;
            }

            if(this.PART_LastButton != null)
            {
                this.PART_LastButton.Click += PART_LastButton_Click;
            }
            if (this.PART_NextButton != null)
            {
                this.PART_NextButton.Click += PART_NextButton_Click;
            }

            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void AddChildToPanel()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            foreach (var item in this.ItemsSource)
            {
                ContentControl control = new ContentControl();
                control.Content = item;
                control.HorizontalAlignment = HorizontalAlignment.Stretch;
                control.HorizontalContentAlignment = HorizontalAlignment.Center;
                control.VerticalContentAlignment = VerticalAlignment.Center;
                control.ContentTemplate = this.ItemTemplate;
                this.PART_SlideSwitchPanel.Children.Add(control);
            }
        }

        private void AddIndexControlToPanel()
        {
            if (this.PART_SlideSwitchPanel == null)
            {
                return;
            }
            if (this.PART_IndexPanel == null)
            {
                return;
            }

            int count = this.PART_SlideSwitchPanel.Children.Count;
            for (int i = 0; i < count; i++)
            {
                MRadionButton radioButton = new MRadionButton();
                radioButton.GroupName = "Index" + this.GroupName;
                radioButton.Checked += RadioButton_Checked;
                this.PART_IndexPanel.Children.Add(radioButton);
            }
            this.ChildCount = count;
        }

        private void HandleButtonMouse(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //VisualStateManager.GoToState(this, "ButtonMouseOver", true);
        }

        private void PART_NextButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToNext();
        }

        private void PART_LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.PART_SlideSwitchPanel == null)
            {
                return;
            }

            int index = this.PART_SlideSwitchPanel.Index;
            index--;

            if (index <= 0)
            {
                index = this.ChildCount;
            }
            this.PART_SlideSwitchPanel.Index = index;
        }

        private void PART_SlideSwitchPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void PART_SlideSwitchPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.PART_SlideSwitchPanel == null)
            {
                return;
            }
            if (this.PART_IndexPanel == null)
            {
                return;
            }

            RadioButton btn = (RadioButton)e.OriginalSource;
            for (int i = 0; i < this.PART_IndexPanel.Children.Count; i++)
            {
                if (btn == this.PART_IndexPanel.Children[i] && i + 1 != this.PART_SlideSwitchPanel.Index)
                {
                    this.PART_SlideSwitchPanel.Index = i + 1;
                }
            }
        }

        private void PART_SlideSwitchPanel_IndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            SlideSwitchPanel panel = sender as SlideSwitchPanel;
            this.SetIndexPanelChecked(panel.Index);
        }

        private void SetIndexPanelChecked(int index)
        {
            if (this.PART_IndexPanel != null && this.PART_IndexPanel.Children[index - 1] is RadioButton)
            {
                RadioButton radioButton = this.PART_IndexPanel.Children[index - 1] as RadioButton;
                radioButton.IsChecked = true;
            }
        }

        private void AutoPlayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.SwitchToNext();
            }));
        }

        private void SwitchToNext()
        {
            if (this.PART_SlideSwitchPanel == null)
            {
                return;
            }

            int index = this.PART_SlideSwitchPanel.Index;
            index++;

            if (index > this.ChildCount)
            {
                index = 1;
            }

            this.PART_SlideSwitchPanel.Index = index;
        }
    }
}
