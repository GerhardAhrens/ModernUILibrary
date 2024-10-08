﻿namespace ModernUIDemo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Microsoft.VisualBasic;

    using ModernUIDemo.Core;
    using ModernUIDemo.Model;
    using ModernUIDemo.MyControls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            this.InitializeComponent();

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, EventArgs>.AddHandler(this, "Closed", this.OnWindowClosed);


            List<TabControlItem> tabItemSource = new List<TabControlItem>();
            tabItemSource.Add(new TabControlItem("Darstellung", true));
            tabItemSource.Add(new TabControlItem("Icon (PathGeometry)", new IconsControlsUC()));
            tabItemSource.Add(new TabControlItem("Farben", new ColorControlsUC()));

            tabItemSource.Add(new TabControlItem("Eingabe", true));
            tabItemSource.Add(new TabControlItem("TextBox (String) Controls", new TextBoxStringControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox (Numeric) Controls", new TextBoxNumericControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox Multiline Controls", new TextBoxMultilineControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox RTF Controls", new TextBoxRtfControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox RTF HTML Controls", new TextBoxRtfHTMLControlsUC()));

            tabItemSource.Add(new TabControlItem("Button", true));
            tabItemSource.Add(new TabControlItem("Button Controls", new ButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("DropDownButton Controls", new DropDownButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("RadioButton Controls", new RadioButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("NumericUpDown Controls", new NumericUpDownControlsUC()));

            tabItemSource.Add(new TabControlItem("Ausgabe/Anzeige", true));
            tabItemSource.Add(new TabControlItem("TextBlock Controls", new TextBlockControlsUC()));
            tabItemSource.Add(new TabControlItem("ListBox/ComboBox Controls", new ListBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("ListTextBox Controls", new ListTextBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("ComboTree Controls", new ComboTreeControlsUC()));
            tabItemSource.Add(new TabControlItem("LED Controls", new LedControlsUC()));

            tabItemSource.Add(new TabControlItem("Loyout Grid, Panel, Separator", true));
            tabItemSource.Add(new TabControlItem("LayoutPanel Controls", new LayoutPanelControlsUC()));
            tabItemSource.Add(new TabControlItem("Separator Controls", new SeparatorControlsUC()));
            tabItemSource.Add(new TabControlItem("Grid ContentFrame", new ContentFrameControlsUC()));

            tabItemSource.Add(new TabControlItem("View, Loading", true));
            tabItemSource.Add(new TabControlItem("Badges Controls", new BadgesControlsUC()));
            tabItemSource.Add(new TabControlItem("BusyIndicator Controls", new BusyIndicatorControlsUC()));
            tabItemSource.Add(new TabControlItem("Loading Animation Controls", new LoadingControlsUC()));
            tabItemSource.Add(new TabControlItem("Slider Controls", new SliderControlsUC()));
            tabItemSource.Add(new TabControlItem("ProgressBar Controls", new ProgressBarControlsUC()));
            tabItemSource.Add(new TabControlItem("PopUp Window", new PopUpControlsUC()));
            tabItemSource.Add(new TabControlItem("Tooltip Controls", new TooltipControlsUC()));
            tabItemSource.Add(new TabControlItem("GroupBox Controls", new GroupBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("Rating Controls", new RatingControlsUC()));

            tabItemSource.Add(new TabControlItem($"MessageBox, NoticeMessage\nWindow", true));
            tabItemSource.Add(new TabControlItem("NoticeMessage Controls", new NoticeMessageControlsUC()));
            tabItemSource.Add(new TabControlItem("MessageBox Window", new MessageBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("NotificationBox Window", new NotifiactionBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("Notification Service Dialog", new NotificationServiceControlsUC()));
            tabItemSource.Add(new TabControlItem("Messaging mit zwei Controls", new MessagingAControlsUC()));
            tabItemSource.Add(new TabControlItem("Messaging B", new MessagingBControlsUC()));
            tabItemSource.Add(new TabControlItem("Flyout Window", new FlyoutControlsUC()));
            tabItemSource.Add(new TabControlItem("Window", new WindowControlsUC()));

            tabItemSource.Add(new TabControlItem($"Dateien, IO", true));
            tabItemSource.Add(new TabControlItem("Datei/Verzeichnis Controls", new ChooseBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("Upload Controls", new UploadControlsUC()));

            tabItemSource.Add(new TabControlItem($"Behavior Control\nErweiterungen", true));
            tabItemSource.Add(new TabControlItem("TextBlock Controls", new BehaviorsControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox Controls", new BehaviorTxTControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox Watermarket", new BehaviorWaterMControlsUC()));
            tabItemSource.Add(new TabControlItem("Excel Cell Behavior für Controls", new BehaviorExcelCellControlsUC()));
            tabItemSource.Add(new TabControlItem("CheckBox Behavior", new BehaviorCheckBoxUC()));

            this.TabControlSource.Value = CollectionViewSource.GetDefaultView(tabItemSource);

            this.DataContext = this;
        }

        public XamlProperty<ICollectionView> TabControlSource { get; set; } = XamlProperty.Set<ICollectionView>();

        public XamlProperty<UserControl> ContentItem { get; set; } = XamlProperty.Set<UserControl>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                TabControlItem currrentItem = lb.SelectedItem as TabControlItem;
                UserControl uc = currrentItem.ItemContent as UserControl;
                this.ContentItem.Value = uc;
            }
        }
    }
}