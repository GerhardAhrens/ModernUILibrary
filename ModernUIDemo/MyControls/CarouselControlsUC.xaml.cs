namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Core;

    /// <summary>
    /// Interaktionslogik für CarouselControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class CarouselControlsUC : UserControl, INotifyPropertyChanged
    {
        public CarouselControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CarouseASource = new ObservableCollection<string>();
            for (int i = 0; i < 10; i++)
            {
                CarouseASource.Add(i.ToString());
            }

            this.CarouseBSource = new ObservableCollection<CarouselModel>();
            string[] resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string file in resourceName.Where(f => f.ToLower().EndsWith("png") == true))
            {
                var picture = XAMLResourceManager.GetResourceContent<ImageSource>(file, AssemblyLocation.EntryAssembly);

                this.CarouseBSource.Add(new CarouselModel()
                {
                    Title = file,
                    ImageUrl = picture
                });
            }

            this.DataContext = this;
        }

        private ObservableCollection<string> _CarouseASource;

        public ObservableCollection<string> CarouseASource
        {
            get { return _CarouseASource; }
            set 
            {
                _CarouseASource = value;
                this.OnPropertyChanged();
            }
        }

        private ObservableCollection<CarouselModel> _CarouseBSource;

        public ObservableCollection<CarouselModel> CarouseBSource
        {
            get { return _CarouseBSource; }
            set
            {
                _CarouseBSource = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

        public static System.Windows.Controls.Image ConvertImageToWpfImage(System.Drawing.Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image", "Image darf nicht null sein.");
            }

            using (System.Drawing.Bitmap dImg = new System.Drawing.Bitmap(image))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    dImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(ms.ToArray());
                    bImg.EndInit();

                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = bImg;

                    return img;
                }
            }
        }

        public static System.Drawing.Image ConvertWpfImageToImage(System.Windows.Controls.Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image", "Image darf nicht null sein.");
            }

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            MemoryStream ms = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
            encoder.Save(ms);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
    }

    public class CarouselModel
    {
        public string Title { get; set; }
        public ImageSource ImageUrl { get; set; }
    }
}
