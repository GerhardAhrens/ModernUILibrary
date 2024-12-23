namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Barcode;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für BarcodeControlsUC.xaml
    /// </summary>
    public partial class BarcodeControlsUC : UserControl, INotifyPropertyChanged
    {
        public BarcodeControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.CreateSmallNumeric();
            this.CreateSmallText();
        }

        public void CreateSmallNumeric()
        {
            string fullPath = Path.Combine(CurrentAssemblyPath(), "Barcode", "QRSmallNumeric.png");
            if (Directory.Exists(Path.GetDirectoryName(fullPath)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            string qrCodeContent = "0123456789";

            using (CreateQRCode code = new CreateQRCode(qrCodeContent))
            {
                code.Save(fullPath, 4);
            }

            if (File.Exists(fullPath))
            {
                this.tbQRCodeNum.Text = $"Inhalt: {qrCodeContent}";
                this.QRCodeImageA.Source = new BitmapImage(new Uri(fullPath));
            }
        }

        public void CreateSmallText()
        {
            string fullPath = Path.Combine(CurrentAssemblyPath(), "Barcode", "QRSmallText.png");
            if (Directory.Exists(Path.GetDirectoryName(fullPath)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            string qrCodeContent = "Ein kleiner Text";

            using (CreateQRCode code = new CreateQRCode(qrCodeContent))
            {
                code.Save(fullPath, 4);
            }

            if (File.Exists(fullPath))
            {
                this.tbQRCodeText.Text = $"Inhalt: {qrCodeContent}";
                this.QRCodeImageB.Source = new BitmapImage(new Uri(fullPath));
            }
        }

        private string CurrentAssemblyPath()
        {
            string result = string.Empty;

            result = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            return result;
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
