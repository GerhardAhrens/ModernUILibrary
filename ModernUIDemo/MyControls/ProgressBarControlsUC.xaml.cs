namespace ModernUIDemo.MyControls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für ProgressBarControlsUC.xaml
    /// </summary>
    public partial class ProgressBarControlsUC : UserControl, INotifyPropertyChanged
    {
        public ProgressBarControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CmdAgg.AddOrSetCommand("ProgressBarButtonCommand", new RelayCommand(p1 => this.OnProgressBarButtonClick(p1), p2 => true));

            this.DataContext = this;
        }

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        private double flatProgressBarValue;

        public double FlatProgressBarValue
        {
            get { return this.flatProgressBarValue; }
            set
            { 
                this.flatProgressBarValue = value;
                this.OnPropertyChanged();
            }
        }

        private string flatProgressBarText;

        public string FlatProgressBarText
        {
            get { return this.flatProgressBarText; }
            set
            {
                this.flatProgressBarText = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private async void OnProgressBarButtonClick(object p1)
        {
            this.FlatProgressBarValue = 0;

            _ = await GenerateItems();

            this.FlatProgressBarValue = 0;
        }

        private Task<List<String>> GenerateItems()
        {
            IProgress<double> progress = new Progress<double>(this.UpdateProgressText);

            return Task.Run(() =>
            {
                int maxRecords = 10_000_000;

                List<String> listOfStrings = new List<string>();
                for (int i = 0; i < maxRecords; i++)
                {
                    //we don't want to flood the UI message loop so do periodic updates
                    if (i % 1500 == 0)
                    {
                        double percentage = (double)i / maxRecords;
                        progress.Report(percentage);
                    }

                    listOfStrings.Add(String.Format($"Item: {i}"));
                }

                return listOfStrings;
            });
        }

        private void UpdateProgressText(double percentage)
        {
            this.FlatProgressBarValue = percentage * 100;
            this.FlatProgressBarText = (percentage).ToString("0%");

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
