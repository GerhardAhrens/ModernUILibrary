namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für TabViewControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class TabViewControlsUC : UserControl, INotifyPropertyChanged
    {
        public TabViewControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.AddTab, "Click", this.OnAddTabClick);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnAddTabClick(object sender, RoutedEventArgs e)
        {
            this.CustomTabView.TabViewItems.Add(new TabItem()
            {
                Header = $"Tab vom Button {DateTime.Now.ToShortDateString()}"
            });
        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }
}
