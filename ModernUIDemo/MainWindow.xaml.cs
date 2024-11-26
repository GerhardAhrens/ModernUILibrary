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
            this.tabItemSource.Add(new TabControlItem("TextBox (Numeric) Controls", new TextBoxNumericControlsUC()) { Stichworte = "TextBox;Zahl;Numeric" });
            this.tabItemSource.Add(new TabControlItem("NumericUpDown Controls", new NumericUpDownControlsUC()) { Stichworte = "TextBox;Zahl;Numeric" });
            this.tabItemSource.Add(new TabControlItem("TextBox Multiline Controls", new TextBoxMultilineControlsUC()) { Stichworte = "TextBox;String;Text" });
            this.tabItemSource.Add(new TabControlItem("TextBox RTF Controls", new TextBoxRtfControlsUC()) { Stichworte = "TextBox;String;Text;rtf" });
            this.tabItemSource.Add(new TabControlItem("CheckBox Controls", new CheckBoxUC()){ Stichworte = "CheckBox;Flat"} );
            this.tabItemSource.Add(new TabControlItem("DateTime Picker Controls", new DateTimeControlsUC()) { Stichworte = "DateTime;Datum;Flat;Time;Picker" });

            this.tabItemSource.Add(new TabControlItem("Button", true));
            this.tabItemSource.Add(new TabControlItem("Button Controls", new ButtonControlsUC()));
            this.tabItemSource.Add(new TabControlItem("DropDownButton Controls", new DropDownButtonControlsUC()));
            this.tabItemSource.Add(new TabControlItem("RadioButton Controls", new RadioButtonControlsUC()));
            this.tabItemSource.Add(new TabControlItem("FloatingAction Controls", new FloatingActionControlsUC()));

            this.tabItemSource.Add(new TabControlItem("Ausgabe/Anzeige", true));
            this.tabItemSource.Add(new TabControlItem("TextBlock Controls", new TextBlockControlsUC()) { Stichworte = "TextBlock;Anzeigen;Animation;Search" });
            this.tabItemSource.Add(new TabControlItem("TextBox RTF HTML Controls", new TextBoxRtfHTMLControlsUC()));
            this.tabItemSource.Add(new TabControlItem("ListBox/ComboBox Controls", new ListBoxControlsUC()) { Stichworte = "ListBox;ComboBox;Flat" });
            this.tabItemSource.Add(new TabControlItem("ListTextBox Controls", new ListTextBoxControlsUC()) { Stichworte = "ListBox;Flat;ListView" });
            this.tabItemSource.Add(new TabControlItem("ComboTree Controls", new ComboTreeControlsUC()) { Stichworte = "ComboBox;Tree;Flat" });
            this.tabItemSource.Add(new TabControlItem("ComboBox Controls", new ComboBoxControlsUC()) { Stichworte = "ComboBox;Flat" });
            this.tabItemSource.Add(new TabControlItem("CascaderBox Controls", new CascaderBoxControlsUC()) { Stichworte = "ComboBox;Flat;Tree" });
            this.tabItemSource.Add(new TabControlItem("LED Controls", new LedControlsUC()) { Stichworte = "Anzeige; LED" });
            this.tabItemSource.Add(new TabControlItem("Dashboard Controls", new DashboardControlsUC()) {Stichworte="Anzeige" });
            this.tabItemSource.Add(new TabControlItem("Accordion Controls", new AccordionControlsUC()) { Stichworte = "Anzeige;Accordion" });

            this.tabItemSource.Add(new TabControlItem("Layout Grid, Panel, Separator", true));
            this.tabItemSource.Add(new TabControlItem("LayoutPanel Controls", new LayoutPanelControlsUC()) { Stichworte = "Anzeige;Layout" });
            this.tabItemSource.Add(new TabControlItem("Separator Controls", new SeparatorControlsUC()) { Stichworte = "Anzeige;Layout;Separator" });
            this.tabItemSource.Add(new TabControlItem("Expander Control", new ExpanderControlsUC()) { Stichworte = "Anzeige;Layout;Expander" });
            this.tabItemSource.Add(new TabControlItem("TabControl", new TabControlControlsUC()) { Stichworte = "Anzeige;TabControl;Register;Layout" });
            this.tabItemSource.Add(new TabControlItem("CarouselControl", new CarouselControlsUC()) { Stichworte = "Anzeige;SlideSwitch;Carousel;Karusell;Layout" });
            this.tabItemSource.Add(new TabControlItem("TagControl", new TagControlsUC()) { Stichworte = "Anzeige;Tag;Layout" });
            this.tabItemSource.Add(new TabControlItem("Poptip", new PoptipControlsUC()) { Stichworte = "Anzeige;Poptip;Layout" });

            this.tabItemSource.Add(new TabControlItem("Listen, Collection Darstellung", true));
            this.tabItemSource.Add(new TabControlItem("FlatListView Control", new FlatListViewControlsUC()) { Stichworte = "ListView;Flat" });

            this.tabItemSource.Add(new TabControlItem("View, Loading", true));
            this.tabItemSource.Add(new TabControlItem("Badges Controls", new BadgesControlsUC()));
            this.tabItemSource.Add(new TabControlItem("BusyIndicator Controls", new BusyIndicatorControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Loading Animation Controls", new LoadingControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Slider Controls", new SliderControlsUC()) { Stichworte = "Slider;Flat" });
            this.tabItemSource.Add(new TabControlItem("ProgressBar Controls", new ProgressBarControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Tooltip Controls", new TooltipControlsUC()));
            this.tabItemSource.Add(new TabControlItem("GroupBox Controls", new GroupBoxControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Rating Controls", new RatingControlsUC()));

            this.tabItemSource.Add(new TabControlItem($"MessageBox, NoticeMessage\nWindow", true));
            this.tabItemSource.Add(new TabControlItem("NoticeMessage Controls", new NoticeMessageControlsUC()));
            this.tabItemSource.Add(new TabControlItem("MessageBox Window", new MessageBoxControlsUC()));
            this.tabItemSource.Add(new TabControlItem("NotificationBox Window", new NotifiactionBoxControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Notification Service Dialog", new NotificationServiceControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Messaging mit zwei Controls", new MessagingAControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Messaging B", new MessagingBControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Flyout Window", new FlyoutControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Window", new WindowControlsUC()));
            this.tabItemSource.Add(new TabControlItem("PopUp Window", new PopUpControlsUC()));

            this.tabItemSource.Add(new TabControlItem($"Dateien, IO", true));
            this.tabItemSource.Add(new TabControlItem("Datei/Verzeichnis Controls", new ChooseBoxControlsUC()));
            this.tabItemSource.Add(new TabControlItem("Upload Controls", new UploadControlsUC()));

            this.tabItemSource.Add(new TabControlItem($"Behavior Control\nErweiterungen", true));
            this.tabItemSource.Add(new TabControlItem("TextBlock Controls", new BehaviorsControlsUC()) { Stichworte="TextBlock;" });
            this.tabItemSource.Add(new TabControlItem("TextBox Controls", new BehaviorTxTControlsUC()) { Stichworte="TextBox;Eingabe;Input;Masken;Pattern"});
            this.tabItemSource.Add(new TabControlItem("TextBox Watermarket", new BehaviorWaterMControlsUC()) { Stichworte = "TextBox;Wasserzeichen;Watermarket;Behavior" });
            this.tabItemSource.Add(new TabControlItem("Excel Cell Behavior für Controls", new BehaviorExcelCellControlsUC()) { Stichworte = "Excel;Cell;Behavior" });
            this.tabItemSource.Add(new TabControlItem("CheckBox Behavior", new BehaviorCheckBoxUC()) {Stichworte = "CheckBox;Behavior" });

            this.CountSamples = this.tabItemSource.Count(c => c.IsGroupItem == false);

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
                if (value != null)
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

        private int countSamples;

        public int CountSamples
        {
            get { return countSamples; }
            set
            {
                this.countSamples = value;
                this.OnPropertyChanged();
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

            this.ListBoxSource.MoveCurrentTo(2);
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