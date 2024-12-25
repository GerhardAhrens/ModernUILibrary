namespace ModernUIDemo.MyControls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
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
            this.CreateLinearBarcode39();
            this.CreateLinearBarcodeEAN13();
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

        public void CreateLinearBarcode39()
        {
            string fullPath = Path.Combine(CurrentAssemblyPath(), "Barcode", "Code39.png");
            LinearBarcode newBarcode = new LinearBarcode("Gerhard-Ahrens", Symbology.Code39)
            {
                Encoder = { Dpi = 300, BarcodeHeight = 200 }
            };

            string barcodeContent = "Ein kleiner Text";
            newBarcode.Encoder.HumanReadableValue = barcodeContent;
            newBarcode.Encoder.SetHumanReadablePosition("Above");
            newBarcode.Encoder.SetHumanReadableFont("Arial", 10);
            newBarcode.Encoder.ShowEncoding = false;
            string zplString = newBarcode.ZplEncode;

            byte[] barcodeImage = newBarcode.SaveImage("PNG");
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(barcodeImage)))
            {
                image.Save(fullPath, ImageFormat.Png);
            }

            if (File.Exists(fullPath))
            {
                this.tbLinearBarcode39.Text = $"Inhalt: {barcodeContent}";
                this.LinearBarcode39.Source = new BitmapImage(new Uri(fullPath));
            }
        }

        public void CreateLinearBarcodeEAN13()
        {
            string fullPath = Path.Combine(CurrentAssemblyPath(), "Barcode", "EAN13.png");

            LinearBarcode newBarcode = new LinearBarcode("4042448073150", Symbology.Ean13)
            {
                Encoder = { Dpi = 300, BarcodeHeight = 200 }
            };

            string barcodeContent = "4042448073150";
            newBarcode.Encoder.HumanReadableValue = barcodeContent;
            newBarcode.Encoder.SetHumanReadablePosition("Above");
            newBarcode.Encoder.SetHumanReadableFont("Arial", 9);
            newBarcode.Encoder.ShowEncoding = false;
            string zplString = newBarcode.ZplEncode;

            byte[] barcodeImage = newBarcode.SaveImage("PNG");
            using (MemoryStream stream = new MemoryStream(barcodeImage))
            {
                this.LinearBarcodeEAN13.Source = BitmapFrame.Create(stream,
                                                  BitmapCreateOptions.None,
                                                  BitmapCacheOption.OnLoad);
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
