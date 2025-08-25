namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernBaseLibrary.Core;

    using ModernIU.Behaviors;
    using ModernIU.WPF.Base;

    /// <summary>
    /// Interaktionslogik für PopUpControlsUC.xaml
    /// </summary>
    public partial class PopUpControlsUC : UserControl, INotifyPropertyChanged
    {
        List<Popup> listPop = new List<Popup>();

        public PopUpControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        public XamlProperty<bool> ShowPopup { get; set; } = XamlProperty.Set<bool>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnOpenMaskLayer_Click(object sender, RoutedEventArgs e)
        {
            this.popupMaskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.popupMaskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }

        private void btnOpenNotificationPopup_Click(object sender, RoutedEventArgs e)
        {
            this.ShowPopup.Value = true;
        }

        private void btnOpenGeneratePopup_Click(object sender, RoutedEventArgs e)
        {
            using (NotifiactionPopup np = new NotifiactionPopup(this.BtnShowErrors))
            {
                WeakEventManager<NotifiactionPopup, PopupResultArgs>.AddHandler(np, "PopupClick", this.PopupClickHandler);

                Popup po = np.CreatePopup("Vorname", "Eingabefehler A, Das ist ein langer Text zur Bescreibung des Fehlertextes in der Eingabe.");
                this.listPop.Add(po);
                po = np.CreatePopup("Beruf", "Eingabefehler B");
                this.listPop.Add(po);
                po = np.CreatePopup("Geburtstag", "Eingabefehler C");
                this.listPop.Add(po);
                po = np.CreatePopup("Rolle", "Eingabefehler D");
                this.listPop.Add(po);
            }
        }

        private void PopupClickHandler(object sender, PopupResultArgs e)
        {
            MessageBox.Show($"Popup: {e.Tag}", "Eingabefehler");
        }

        private void btnShowPopup_Click(object sender, RoutedEventArgs e)
        {
            foreach (Popup item in this.listPop)
            {
                item.IsOpen = true;
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

    public class NotifiactionPopupSettings
    {
        public NotifiactionPopupSettings(UIElement placementTarget, int delay = 5)
        {
            this.PlacementTarget = placementTarget;
            this.Delay = delay;
        }

        public UIElement PlacementTarget { get; private set; }

        public int Delay { get; private set; }

        public PlacementMode PlacementMode { get; set; }

        public double PopupWidth { get; set; }

        public double PopupHeight { get; set; }

        public PopupAnimation PopupAnimation { get; set; }

        public double HorizontalOffset { get; set; }

        public double VerticalOffset { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <Link>
    /// https://putridparrot.com/blog/automatically-update-a-wpf-popup-position/
    /// </Link>
    public class NotifiactionPopup : IDisposable
    {
        public event EventHandler<PopupResultArgs> PopupClick;
        private bool disposed;

        private static int popNumber = 0;
        public NotifiactionPopup(UIElement placementTarget, int delay = 5)
        {
            this.PlacementTarget = placementTarget;
            this.Delay = delay;
        }

        public Popup GetPopup { get; private set; }


        public UIElement PlacementTarget { get; private set; }

        public int Delay { get; private set; }

        public Popup CreatePopup(string field, string msgText)
        {
            popNumber++;
            Popup popup = new Popup();
            popup.Tag = $"{popNumber}";
            popup.AllowsTransparency = true;
            popup.Width = 200;
            popup.Height = 100;
            popup.Placement = PlacementMode.Right;
            popup.PlacementTarget = PlacementTarget;
            popup.PopupAnimation = PopupAnimation.Slide;
            popup.HorizontalOffset = -110;
            popup.VerticalOffset = 30;
            if (popNumber > 1)
            {
                popup.VerticalOffset = -70;
                popup.VerticalOffset = (popup.Height * popNumber) + popup.VerticalOffset;
            }

            Border border = new Border();
            border.Background = Brushes.Pink;
            border.BorderBrush = Brushes.Blue;
            border.BorderThickness = new Thickness(1);
            border.CornerRadius = new CornerRadius(5);
            StackPanel grid = new StackPanel();
            grid.Orientation = Orientation.Vertical;
            grid.Margin = new Thickness(5, 5, 0, 0);

            TextBlock textBlockTitle = new TextBlock();
            textBlockTitle.Height = 18;
            textBlockTitle.FontWeight = FontWeights.Bold;
            textBlockTitle.FontSize = 14;
            textBlockTitle.Inlines.Add(new Underline(new Run("Eingabefehler")));

            TextBlock textBlockMsgA = new TextBlock();
            textBlockMsgA.Height = 18;
            textBlockMsgA.Margin = new Thickness(0, 5, 0, 0);
            textBlockMsgA.FontWeight = FontWeights.Medium;
            textBlockMsgA.Inlines.Add(new Run("Feld:"));
            textBlockMsgA.Inlines.Add(new Run(field));

            TextBlock textBlockMsgB = new TextBlock();
            textBlockMsgB.Width = popup.Width - 10;
            textBlockMsgB.Height = popup.Height - (textBlockTitle.Height + textBlockMsgA.Height);
            textBlockMsgB.TextWrapping = TextWrapping.Wrap;
            textBlockMsgB.Margin = new Thickness(0, 5, 0, 0);
            textBlockMsgB.FontWeight = FontWeights.Medium;
            textBlockMsgB.Inlines.Add(new Run(msgText));

            grid.Children.Add(textBlockTitle);
            grid.Children.Add(textBlockMsgA);
            grid.Children.Add(textBlockMsgB);

            border.Child = grid;
            popup.Child = border;
            popup.StaysOpen = true;
            popup.IsOpen = false;
            popup.PreviewMouseLeftButtonDown += (s, e) =>
            {
                Popup internalPopup = (Popup)s;
                if (internalPopup != null)
                {
                    internalPopup.IsOpen = false;
                    e.Handled = true;
                    this.RaisePopupResult(internalPopup.Tag);
                }
            };

            popup.Opened += (s, e) =>
            {
                Popup internalPopup = (Popup)s;
                if (internalPopup != null)
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(this.Delay+(0.25* popNumber));
                    timer.Start();
                    timer.Tick += (s, e) =>
                    {
                        internalPopup.IsOpen = false;
                        timer.Stop();
                        timer = null;
                    };
                }
            };

            return popup;
        }

        private void RaisePopupResult(object tag)
        {
            var handler = this.PopupClick;
            if (handler != null)
            {
                var args = new PopupResultArgs();
                args.Tag = tag;
                handler(this, args);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
            }


            disposed = true;
        }
    }

    public class PopupResultArgs : EventArgs
    {
        public object Tag { get; set; }
    }
}
