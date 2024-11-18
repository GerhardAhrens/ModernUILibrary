namespace ModernUIDemo
{
    using System.ComponentModel;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using ModernUIDemo.Model;
    using ModernUIDemo.MyControls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        List<TabControlItem> tabItemSource = null;

        public MainWindow()
        {
            this.InitializeComponent();

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, EventArgs>.AddHandler(this, "Closed", this.OnWindowClosed);


            this.tabItemSource = new List<TabControlItem>();
            this.tabItemSource.Add(new TabControlItem("Darstellung", true));
            this.tabItemSource.Add(new TabControlItem("Icon (PathGeometry)", new IconsControlsUC()) { Stichworte = "Icon;PathGeometry" });
            this.tabItemSource.Add(new TabControlItem("Farben", new ColorControlsUC()) { Stichworte = "Color;Farbe" });
            this.tabItemSource.Add(new TabControlItem("Farbenauswahl", new ColorSelectorControlsUC()) { Stichworte = "Color;Farbe" });

            this.tabItemSource.Add(new TabControlItem("Eingabe", true));
            this.tabItemSource.Add(new TabControlItem("TextBox (String) Controls", new TextBoxStringControlsUC()) { Stichworte = "TextBox;String;Text;Icon" });
            this.tabItemSource.Add(new TabControlItem("TextBox (Numeric) Controls", new TextBoxNumericControlsUC()));
            this.tabItemSource.Add(new TabControlItem("TextBox Multiline Controls", new TextBoxMultilineControlsUC()));
            this.tabItemSource.Add(new TabControlItem("TextBox RTF Controls", new TextBoxRtfControlsUC()));
            this.tabItemSource.Add(new TabControlItem("TextBox RTF HTML Controls", new TextBoxRtfHTMLControlsUC()));
            this.tabItemSource.Add(new TabControlItem("CheckBox Controls", new CheckBoxUC()){ Stichworte = "CheckBox;Flat"} );
            this.tabItemSource.Add(new TabControlItem("DateTime Picker Controls", new DateTimeControlsUC()) { Stichworte = "DateTime;Datum;Flat" });

            tabItemSource.Add(new TabControlItem("Button", true));
            tabItemSource.Add(new TabControlItem("Button Controls", new ButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("DropDownButton Controls", new DropDownButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("RadioButton Controls", new RadioButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("NumericUpDown Controls", new NumericUpDownControlsUC()));

            tabItemSource.Add(new TabControlItem("Ausgabe/Anzeige", true));
            tabItemSource.Add(new TabControlItem("TextBlock Controls", new TextBlockControlsUC()) { Stichworte = "TextBlock;Anzeigen;Animation;Search" });
            tabItemSource.Add(new TabControlItem("ListBox/ComboBox Controls", new ListBoxControlsUC()) { Stichworte = "ListBox;ComboBox;Flat" });
            tabItemSource.Add(new TabControlItem("ListTextBox Controls", new ListTextBoxControlsUC()) { Stichworte = "ListBox;Flat;ListView" });
            tabItemSource.Add(new TabControlItem("ComboTree Controls", new ComboTreeControlsUC()) { Stichworte = "ComboBox;Tree;Flat" });
            tabItemSource.Add(new TabControlItem("ComboBox Controls", new ComboBoxControlsUC()) { Stichworte = "ComboBox;Flat" });
            tabItemSource.Add(new TabControlItem("CascaderBox Controls", new CascaderBoxControlsUC()) { Stichworte = "ComboBox;Flat;Tree" });
            tabItemSource.Add(new TabControlItem("LED Controls", new LedControlsUC()) { Stichworte = "Anzeige; LED" });
            tabItemSource.Add(new TabControlItem("Dashboard Controls", new DashboardControlsUC()) {Stichworte="Anzeige" });
            tabItemSource.Add(new TabControlItem("Accordion Controls", new AccordionControlsUC()) { Stichworte = "Anzeige;Accordion" });

            tabItemSource.Add(new TabControlItem("Layout Grid, Panel, Separator", true));
            tabItemSource.Add(new TabControlItem("LayoutPanel Controls", new LayoutPanelControlsUC()));
            tabItemSource.Add(new TabControlItem("Separator Controls", new SeparatorControlsUC()));
            tabItemSource.Add(new TabControlItem("Grid ContentFrame", new ContentFrameControlsUC()));
            tabItemSource.Add(new TabControlItem("TabControl", new TabControlControlsUC()) { Stichworte = "Anzeige;TabControl;Register" });

            tabItemSource.Add(new TabControlItem("Listen, Collection Darstellung", true));
            tabItemSource.Add(new TabControlItem("FlatListView Control", new FlatListViewControlsUC()) { Stichworte = "ListView;Flat" });

            tabItemSource.Add(new TabControlItem("View, Loading", true));
            tabItemSource.Add(new TabControlItem("Badges Controls", new BadgesControlsUC()));
            tabItemSource.Add(new TabControlItem("BusyIndicator Controls", new BusyIndicatorControlsUC()));
            tabItemSource.Add(new TabControlItem("Loading Animation Controls", new LoadingControlsUC()));
            tabItemSource.Add(new TabControlItem("Slider Controls", new SliderControlsUC()) { Stichworte = "Slider;Flat" });
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
            tabItemSource.Add(new TabControlItem("TextBlock Controls", new BehaviorsControlsUC()) { Stichworte="TextBlock;" });
            tabItemSource.Add(new TabControlItem("TextBox Controls", new BehaviorTxTControlsUC()) { Stichworte="TextBox;Eingabe;Input;Masken;Pattern"});
            tabItemSource.Add(new TabControlItem("TextBox Watermarket", new BehaviorWaterMControlsUC()) { Stichworte = "TextBox;Wasserzeichen;Watermarket;Behavior" });
            tabItemSource.Add(new TabControlItem("Excel Cell Behavior für Controls", new BehaviorExcelCellControlsUC()) { Stichworte = "Excel;Cell;Behavior" });
            tabItemSource.Add(new TabControlItem("CheckBox Behavior", new BehaviorCheckBoxUC()) {Stichworte = "CheckBox;Behavior" });

            this.DataContext = this;
        }

        private ICollectionView listBoxSource;

        public ICollectionView ListBoxSource
        {
            get { return this.listBoxSource; }
            set
            {
                this.listBoxSource = value;
                this.OnPropertyChanged();
            }
        }

        private TabControlItem currentSelectedItem;

        public TabControlItem CurrentSelectedItem
        {
            get { return this.currentSelectedItem; }
            set
            {
                this.currentSelectedItem = value;
                this.OnPropertyChanged();
                if (value.ItemContent != null)
                {
                    UserControl uc = value.ItemContent as UserControl;
                    this.ContentItem = uc;
                }
            }
        }

        private string filterText;

        public string FilterText
        {
            get { return filterText; }
            set
            {
                this.filterText = value;
                this.OnPropertyChanged();
                this.RefreshDefaultFilter(value);
            }
        }

        private UserControl contentItem;

        public UserControl ContentItem
        {
            get { return this.contentItem; }
            set
            {
                this.contentItem = value;
                this.OnPropertyChanged();
            }
        }

        private int maxRowCount;

        public int MaxRowCount
        {
            get { return this.maxRowCount; }
            set
            {
                this.maxRowCount = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ListBoxSource = CollectionViewSource.GetDefaultView(this.tabItemSource);

            this.ListBoxSource.Filter = item =>
            {
                TabControlItem vitem = item as TabControlItem;
                if (vitem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.FilterText) == false && vitem.Stichworte != null)
                {
                    return vitem.Stichworte.ToLower().Contains(this.FilterText.ToLower());
                }
                else
                {
                    return true;
                }
            };
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RefreshDefaultFilter(string value)
        {
            if (this.ListBoxSource != null)
            {
                this.ListBoxSource.Refresh();
                this.MaxRowCount = this.ListBoxSource.Cast<TabControlItem>().Count();
                this.ListBoxSource.MoveCurrentToFirst();
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