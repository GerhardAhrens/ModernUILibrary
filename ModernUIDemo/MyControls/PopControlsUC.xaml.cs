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

            NotifiactionPopup.PopupClick += this.NotifiactionPopup_PopupClick;


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
            NotifiactionPopup.PlacementTarget = this.BtnShowErrors;
            NotifiactionPopup.Delay = 5;

            Popup po = NotifiactionPopup.CreatePopup("Vorname", "Eingabefehler A, Das ist ein langer Text zur Bescreibung des Fehlertextes in der Eingabe.");
            this.listPop.Add(po);
            po = NotifiactionPopup.CreatePopup("Beruf", "Eingabefehler B");
            this.listPop.Add(po);
            po = NotifiactionPopup.CreatePopup("Geburtstag", "Eingabefehler C");
            this.listPop.Add(po);
            po = NotifiactionPopup.CreatePopup("Rolle", "Eingabefehler D");
            this.listPop.Add(po);


            MessageBox.Show("Einträge zum NotifiactionPopup erstellt!", "NotifiactionPopup");
        }

        private void NotifiactionPopup_PopupClick(object sender, PopupResultArgs e)
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


    /// <summary>
    /// Die Klasse erstellt ein Popup um einen Einghabefehler darzustellen
    /// </summary>
    /// <remarks>
    /// Das Control wird direkt im C# Source erstellt
    /// </remarks>
    public class NotifiactionPopup 
    {
        /// <summary>
        /// Event das beim Klick auf ein Popup ausgelöst wird
        /// </summary>
        public static event EventHandler<PopupResultArgs> PopupClick;

        private static int popNumber = 0;

        /// <summary>
        /// Element/Control an dem das erste Popup plaziert ist. Alle weiteren werden direkt darunter gezeigt.
        /// </summary>
        public static UIElement PlacementTarget { get; set; }

        /// <summary>
        /// Zeit in Sekunden, bis sich das Popup automatisch schließt. Bei -1 bleibt das Popup solang stehen, bis es angeklickt wird.
        /// </summary>
        public static int Delay { get; set; } = 5;

        public static PlacementMode PlacementMode { get; set; } = PlacementMode.Right;

        public static PopupAnimation PopupAnimation { get; set; } = PopupAnimation.Slide;

        public static double PopupWidth { get; set; } = 200;

        public static double PopupHeight { get; set; } = 100;


        public static double HorizontalOffset { get; set; }

        public static double VerticalOffset { get; set; }

        /// <summary>
        /// Erstelle Popup
        /// </summary>
        /// <param name="field">Eingabefeld</param>
        /// <param name="msgText">Fehlerbeschreibung</param>
        /// <returns>Popup Object</returns>
        public static Popup CreatePopup(string field, string msgText)
        {
            popNumber++;
            Popup popup = new Popup();
            popup.Tag = $"{popNumber}";
            popup.AllowsTransparency = true;
            popup.Width = NotifiactionPopup.PopupWidth;
            popup.Height = NotifiactionPopup.PopupHeight;
            popup.Placement = NotifiactionPopup.PlacementMode;
            popup.PlacementTarget = NotifiactionPopup.PlacementTarget;
            popup.PopupAnimation = NotifiactionPopup.PopupAnimation;
            popup.HorizontalOffset = -110;
            popup.VerticalOffset = 30;
            if (popNumber > 1)
            {
                popup.VerticalOffset = -70;
                popup.VerticalOffset = (popup.Height * popNumber) + popup.VerticalOffset;
            }

            Border border = new Border();
            border.Background = (Brush)(new BrushConverter().ConvertFrom("#FF6600"));
            border.BorderBrush = Brushes.Blue;
            border.BorderThickness = new Thickness(1);
            border.CornerRadius = new CornerRadius(5);
            StackPanel grid = new StackPanel();
            grid.Orientation = Orientation.Vertical;
            grid.Margin = new Thickness(5, 5, 0, 0);

            TextBlock textBlockTitle = new TextBlock();
            textBlockTitle.Foreground= Brushes.White;
            textBlockTitle.Height = 18;
            textBlockTitle.FontWeight = FontWeights.Bold;
            textBlockTitle.FontSize = 14;
            textBlockTitle.Inlines.Add(new Underline(new Run("Eingabefehler")));

            TextBlock textBlockMsgA = new TextBlock();
            textBlockMsgA.Foreground = Brushes.White;
            textBlockMsgA.Height = 18;
            textBlockMsgA.Margin = new Thickness(0, 4, 0, 0);
            textBlockMsgA.FontWeight = FontWeights.Medium;
            textBlockMsgA.Inlines.Add(new Run("Feld:"));
            textBlockMsgA.Inlines.Add(new Run(field));

            TextBlock textBlockMsgB = new TextBlock();
            textBlockMsgB.Foreground = Brushes.White;
            textBlockMsgB.Width = popup.Width - 10;
            textBlockMsgB.Height = popup.Height - (textBlockTitle.Height + textBlockMsgA.Height);
            textBlockMsgB.TextWrapping = TextWrapping.Wrap;
            textBlockMsgB.Margin = new Thickness(0, 4, 0, 0);
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
                    RaisePopupResult(internalPopup.Tag);
                }
            };

            if (Delay > 0)
            {
                popup.Opened += (s, e) =>
                {
                    Popup internalPopup = (Popup)s;
                    if (internalPopup != null)
                    {
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(Delay + (0.25 * popNumber));
                        timer.Start();
                        timer.Tick += (s, e) =>
                        {
                            internalPopup.IsOpen = false;
                            timer.Stop();
                            timer = null;
                        };
                    }
                };
            }

            popup.UpdateLayout();

            return popup;
        }

        private static void RaisePopupResult(object tag)
        {
            var handler = PopupClick;
            if (handler != null)
            {
                var args = new PopupResultArgs();
                args.Tag = tag;
                handler(typeof(NotifiactionPopup), args);
            }
        }
    }

    public class PopupResultArgs : EventArgs
    {
        public object Tag { get; set; }
    }
}
