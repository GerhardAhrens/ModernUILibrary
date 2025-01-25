namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ModernIU.Controls.Chart;

    /// <summary>
    /// Interaktionslogik für ChartControlsUC.xaml
    /// </summary>
    public partial class ChartControlsUC : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<ChartLegendItem> _barLegend = new ObservableCollection<ChartLegendItem>();
        private ObservableCollection<ChartRow> _barRows = new ObservableCollection<ChartRow>();
        private ObservableCollection<ChartLegendItem> _pieLegend = new ObservableCollection<ChartLegendItem>();
        private ObservableCollection<ChartRow> _pieRows = new ObservableCollection<ChartRow>();
        private ObservableCollection<ChartLegendItem> _stackedLegend = new ObservableCollection<ChartLegendItem>();
        private ObservableCollection<StackedChartRow> _stackedRows = new ObservableCollection<StackedChartRow>();

        public ChartControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        public ObservableCollection<ChartLegendItem> BarLegend
        {
            get { return this._barLegend; }
            set 
            { 
                this._barLegend = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<ChartRow> BarRows
        {
            get { return this._barRows; }
            set {
                this._barRows = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<ChartLegendItem> PieLegend
        {
            get { return this._pieLegend; }
            set
            {
                this._pieLegend = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<ChartRow> PieRows
        {
            get { return this._pieRows; }
            set
            {
                this._pieRows = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<ChartLegendItem> StackedLegend
        {
            get { return this._stackedLegend; }
            set
            {
                this._stackedLegend = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<StackedChartRow> StackedRows
        {
            get { return this._stackedRows; }
            set
            {
                this._stackedRows = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.PrepareBarChart();
            this.PreparePieChart();
            this.PrepareStackedChart();
        }

        private void PrepareBarChart()
        {
            Random gen = new Random(DateTime.Now.Millisecond);

            this.BarRows.Clear();
            this.BarRows.Add(new ChartRow("Bar-Value 1", 20, Colors.Green));
            this.BarRows.Add(new ChartRow("Bar-Value 2", 10, Colors.Violet));
            this.BarRows.Add(new ChartRow("Bar-Value 3", 30, Colors.Red));
            this.BarRows.Add(new ChartRow("Bar-Value 4", 5, Colors.Yellow));
            this.BarRows.Add(new ChartRow("Bar-Value 5", 30, Colors.DarkOrange));
            this.BarRows.Add(new ChartRow("Bar-Value 6", 15, Colors.LawnGreen));

            this.BarLegend.Clear();
            this.BarLegend.Add(new ChartLegendItem("Bar-Value 1", Colors.Green));
            this.BarLegend.Add(new ChartLegendItem("Bar-Value 2", Colors.Violet));
            this.BarLegend.Add(new ChartLegendItem("Bar-Value 3", Colors.Red));
            this.BarLegend.Add(new ChartLegendItem("Bar-Value 4", Colors.Yellow));
            this.BarLegend.Add(new ChartLegendItem("Bar-Value 5", Colors.DarkOrange));
            this.BarLegend.Add(new ChartLegendItem("Bar-Value 6", Colors.LawnGreen));
        }

        private void PreparePieChart()
        {
            this.PieRows.Add(new ChartRow("Pie-Value 1", 20, Colors.Green));
            this.PieRows.Add(new ChartRow("Pie-Value 2", 10, Colors.Violet));
            this.PieRows.Add(new ChartRow("Pie-Value 3", 30, Colors.Red));
            this.PieRows.Add(new ChartRow("Pie-Value 4", 5, Colors.Yellow));
            this.PieRows.Add(new ChartRow("Pie-Value 5", 30, Colors.DarkOrange));
            this.PieRows.Add(new ChartRow("Pie-Value 6", 15, Colors.LawnGreen));

            this.PieLegend.Clear();
            this.PieLegend.Add(new ChartLegendItem("Pie-Value 1", Colors.Green));
            this.PieLegend.Add(new ChartLegendItem("Pie-Value 2", Colors.Violet));
            this.PieLegend.Add(new ChartLegendItem("Pie-Value 3", Colors.Red));
            this.PieLegend.Add(new ChartLegendItem("Pie-Value 4", Colors.Yellow));
            this.PieLegend.Add(new ChartLegendItem("Pie-Value 5", Colors.DarkOrange));
            this.PieLegend.Add(new ChartLegendItem("Pie-Value 6", Colors.LawnGreen));
        }

        private void PrepareStackedChart()
        {
            AddStackedChartRow("Stacked-Value 1", 2, 4, 6, 8, 10);
            AddStackedChartRow("Stacked-Value 2", 12, 4, 2, 3, 1);
            AddStackedChartRow("Stacked-Value 3", 1, 8, 1, 1, 1);
            AddStackedChartRow("Stacked-Value 4", 1, 2, 3, 4, 5);
            AddStackedChartRow("Stacked-Value 5", 5, 4, 3, 2, 1);


            this.StackedLegend.Clear();
            this.StackedLegend.Add(new ChartLegendItem("Stacked-Value 1", Colors.Green));
            this.StackedLegend.Add(new ChartLegendItem("Stacked-Value 2", Colors.Violet));
            this.StackedLegend.Add(new ChartLegendItem("Stacked-Value 3", Colors.Red));
            this.StackedLegend.Add(new ChartLegendItem("Stacked-Value 4", Colors.Yellow));
            this.StackedLegend.Add(new ChartLegendItem("Stacked-Value 5", Colors.DarkOrange));
        }

        private void AddStackedChartRow(string caption, double value1, double value2, double value3, double value4, double value5)
        {
            StackedChartRow row;

            row = new StackedChartRow(caption);
            row.Values.Add(new ChartRow("Stacked-Value 1", value1, Colors.Green));
            row.Values.Add(new ChartRow("Stacked-Value 2", value2, Colors.Violet));
            row.Values.Add(new ChartRow("Stacked-Value 3", value3, Colors.Red));
            row.Values.Add(new ChartRow("Stacked-Value 4", value4, Colors.Yellow));
            row.Values.Add(new ChartRow("Stacked-Value 5", value5, Colors.DarkOrange));

            _stackedRows.Add(row);
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string pathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DateTime.Now.Ticks.ToString());
            SaveToPng(this.gridBarChart, pathFile);
            SaveToBmp(this.gridBarChart, pathFile);
            SaveToJpg(this.gridBarChart, pathFile);
        }

        private void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, Path.ChangeExtension(fileName, ".bmp"), encoder);
        }

        private void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, Path.ChangeExtension(fileName, ".png"), encoder);
        }

        private void SaveToJpg(FrameworkElement visual, string fileName)
        {
            var encoder = new JpegBitmapEncoder();
            SaveUsingEncoder(visual, Path.ChangeExtension(fileName, ".jpg"), encoder);
        }

        private void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
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
