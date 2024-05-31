namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für BehaviorCheckBoxUC.xaml
    /// </summary>
    public partial class BehaviorCheckBoxUC : UserControl
    {
        public BehaviorCheckBoxUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        public XamlProperty<bool> CheckBoxEntweder { get; set; } = XamlProperty.Set<bool>();
        public XamlProperty<bool> CheckBoxOder { get; set; } = XamlProperty.Set<bool>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.CheckBoxEntweder.Value = true;
        }
    }
}
