namespace ModernUIDemo
{
    using System.ComponentModel;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Core.Algorithm;
    using ModernBaseLibrary.Core.Media;

    using ModernIU.Controls;

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
            WeakEventManager<PathButton, EventArgs>.AddHandler(this.BtnAbout, "Click", this.OnBtnAboutClick);
            this.DataContext = this;
        }

        #region Properties
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
                if (value != null && this.sourceWPF == true)
                {
                    UserControl uc = value.ItemContent as UserControl;
                    this.SourceName = uc.GetType().Name;
                    this.ContentItem = uc;
                }
                else
                {
                    if (value != null)
                    {
                        UserControl uc = value.ItemContent as UserControl;
                        uc.Tag = new Tuple<string,string>(this.CurrentSelectedItem.SourceFile, this.CurrentSelectedItem.Stichworte);
                        this.ContentItem = uc;
                        this.SourceName = uc.GetType().Name;
                        this.Focus();
                    }
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
            get { return this.countSamples; }
            set
            {
                this.countSamples = value;
                this.OnPropertyChanged();
            }
        }

        private string sourceName;

        public string SourceName
        {
            get { return this.sourceName; }
            set
            {
                this.sourceName = value;
                this.OnPropertyChanged();
            }
        }

        private bool sourceWPF;

        public bool SourceWPF
        {
            get { return this.sourceWPF; }
            set
            {
                this.sourceWPF = value;
                this.OnPropertyChanged();
                if (value == true)
                {
                    this.LoadUIControl();
                }
            }
        }

        private bool sourceCS;

        public bool SourceCS
        {
            get { return this.sourceCS; }
            set
            {
                this.sourceCS = value;
                this.OnPropertyChanged();
                if (value == true)
                {
                    this.LoadCSSource();
                }
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
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SourceWPF = true;
            this.SourceCS = false;

            NotificationService.RegisterDialog<SelectLB>();
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

        private void LoadUIControl()
        {
            this.ContentItem = null;
            this.tabItemSource = new List<TabControlItem>();
            this.tabItemSource.Add(new TabControlItem("Darstellung", true));
            this.tabItemSource.Add(new TabControlItem("Icon (PathGeometry)", new IconsControlsUC()) { Stichworte = "Icon;PathGeometry" });
            this.tabItemSource.Add(new TabControlItem("Farben", new ColorControlsUC()) { Stichworte = "Color;Farbe" });
            this.tabItemSource.Add(new TabControlItem("Farbenauswahl", new ColorSelectorControlsUC()) { Stichworte = "Color;Farbe" });
            this.tabItemSource.Add(new TabControlItem("Markdown", new MarkdownControlsUC()) { Stichworte = "Markdown;Text" });

            this.tabItemSource.Add(new TabControlItem("Eingabe", true));
            this.tabItemSource.Add(new TabControlItem("TextBox (String) Controls", new TextBoxStringControlsUC()) { Stichworte = "TextBox;String;Text;Icon" });
            this.tabItemSource.Add(new TabControlItem("TextBox (Design) Controls", new TextBoxDesignControlsUC()) { Stichworte = "TextBox;Text;Icon;Design" });
            this.tabItemSource.Add(new TabControlItem("TextBox (Numeric) Controls", new TextBoxNumericControlsUC()) { Stichworte = "TextBox;Zahl;Numeric" });
            this.tabItemSource.Add(new TabControlItem("NumericUpDown Controls", new NumericUpDownControlsUC()) { Stichworte = "TextBox;Zahl;Numeric" });
            this.tabItemSource.Add(new TabControlItem("TextBox Multiline Controls", new TextBoxMultilineControlsUC()) { Stichworte = "TextBox;String;Text" });
            this.tabItemSource.Add(new TabControlItem("TextBox RTF Controls", new TextBoxRtfControlsUC()) { Stichworte = "TextBox;String;Text;rtf" });
            this.tabItemSource.Add(new TabControlItem("CheckBox Controls", new CheckBoxUC()) { Stichworte = "CheckBox;Flat" });
            this.tabItemSource.Add(new TabControlItem("DateTime Picker Controls", new DateTimeControlsUC()) { Stichworte = "DateTime;Datum;Flat;Time;Picker" });
            this.tabItemSource.Add(new TabControlItem("Timeline Controls", new TimelineControlsUC()) { Stichworte = "DateTime;Datum;Timeline" });
            this.tabItemSource.Add(new TabControlItem("Syntax Box Control", new SyntaxBoxControlsUC()) { Stichworte = "TextBox;Syntax" });
            this.tabItemSource.Add(new TabControlItem("Custom Syntax Box Control", new CustomSyntaxBoxControlsUC()) { Stichworte = "TextBox;Syntax" });

            this.tabItemSource.Add(new TabControlItem("Button", true));
            this.tabItemSource.Add(new TabControlItem("Button Controls", new ButtonControlsUC()) { Stichworte = "Button" });
            this.tabItemSource.Add(new TabControlItem("DropDownButton Controls", new DropDownButtonControlsUC()) { Stichworte = "Button;DropDownButton" });
            this.tabItemSource.Add(new TabControlItem("RadioButton Controls", new RadioButtonControlsUC()) { Stichworte = "Button;RadioButton" });
            this.tabItemSource.Add(new TabControlItem("FloatingAction Controls", new FloatingActionControlsUC()));
            this.tabItemSource.Add(new TabControlItem("GeometryButton Controls", new GeometryButtonControlsUC()) { Stichworte = "Button;Icon;Flat" });

            this.tabItemSource.Add(new TabControlItem("Menü", true));
            this.tabItemSource.Add(new TabControlItem("SwitchMenu Controls", new SwitchMenuControlsUC()) { Stichworte = "SwitchMenu;Menü;Button" });
            this.tabItemSource.Add(new TabControlItem("Hotkey, CustomBinding", new HotKeyControlsUC()) { Stichworte = "HotKey;Menü" });

            this.tabItemSource.Add(new TabControlItem("Grafik", true));
            this.tabItemSource.Add(new TabControlItem("Lesen SVG", new GraphicsControlsUC()) { Stichworte = "Grafik;SVG" });
            this.tabItemSource.Add(new TabControlItem("Erstelle QRCode/Barcode", new BarcodeControlsUC()) { Stichworte = "Grafik;QRCode" });
            this.tabItemSource.Add(new TabControlItem("Image GIF", new ImageGIFControlsUC()) { Stichworte = "Image;Gif;Grafik" });
            this.tabItemSource.Add(new TabControlItem("Chart Controls", new ChartControlsUC()) { Stichworte = "Image;Grafik;Chart" });

            this.tabItemSource.Add(new TabControlItem("Ausgabe/Anzeige", true));
            this.tabItemSource.Add(new TabControlItem("TextBlock Controls", new TextBlockControlsUC()) { Stichworte = "TextBlock;Anzeigen;Animation;Search" });
            this.tabItemSource.Add(new TabControlItem("TextBox RTF HTML Controls", new TextBoxRtfHTMLControlsUC()));
            this.tabItemSource.Add(new TabControlItem("ListBox/ComboBox Controls", new ListBoxControlsUC()) { Stichworte = "ListBox;ComboBox;Flat" });
            this.tabItemSource.Add(new TabControlItem("ListTextBox Controls", new ListTextBoxControlsUC()) { Stichworte = "ListBox;Flat;ListView" });
            this.tabItemSource.Add(new TabControlItem("ComboTree Controls", new ComboTreeControlsUC()) { Stichworte = "ComboBox;Tree;Flat" });
            this.tabItemSource.Add(new TabControlItem("ComboBox Controls", new ComboBoxControlsUC()) { Stichworte = "ComboBox;Flat" });
            this.tabItemSource.Add(new TabControlItem("CascaderBox Controls", new CascaderBoxControlsUC()) { Stichworte = "ComboBox;Flat;Tree" });
            this.tabItemSource.Add(new TabControlItem("LED Controls", new LedControlsUC()) { Stichworte = "Anzeige; LED" });
            this.tabItemSource.Add(new TabControlItem("Dashboard Controls", new DashboardControlsUC()) { Stichworte = "Anzeige" });
            this.tabItemSource.Add(new TabControlItem("Accordion Controls", new AccordionControlsUC()) { Stichworte = "Anzeige;Accordion" });
            this.tabItemSource.Add(new TabControlItem("Lokalisierung Manager", new LocalizationUC()) { Stichworte = "Anzeige;Localization;Lokalisierung;Mehrsprachig" });
            this.tabItemSource.Add(new TabControlItem("PunchCard Control", new PunchCardControlsUC()) { Stichworte = "Anzeige;PunchCard;Lochkarte" });

            this.tabItemSource.Add(new TabControlItem("Layout Grid, Panel, Separator", true));
            this.tabItemSource.Add(new TabControlItem("LayoutPanel Controls", new LayoutPanelControlsUC()) { Stichworte = "Anzeige;Layout" });
            this.tabItemSource.Add(new TabControlItem("Layout Controls", new LayoutControlsUC()) { Stichworte = "Anzeige;Layout;Grid" });
            this.tabItemSource.Add(new TabControlItem("Layout ContentFrame", new ContentFrameControlsUC()) { Stichworte = "Anzeige;Layout;Grid;Content;Frame" });
            this.tabItemSource.Add(new TabControlItem("Separator Controls", new SeparatorControlsUC()) { Stichworte = "Anzeige;Layout;Separator" });
            this.tabItemSource.Add(new TabControlItem("Expander Control", new ExpanderControlsUC()) { Stichworte = "Anzeige;Layout;Expander" });
            this.tabItemSource.Add(new TabControlItem("TabControl", new TabControlControlsUC()) { Stichworte = "Anzeige;TabControl;Register;Layout" });
            this.tabItemSource.Add(new TabControlItem("TabView", new TabViewControlsUC()) { Stichworte = "Anzeige;TabControl;TabView;Register;Layout" });
            this.tabItemSource.Add(new TabControlItem("CarouselControl", new CarouselControlsUC()) { Stichworte = "Anzeige;SlideSwitch;Carousel;Karusell;Layout" });
            this.tabItemSource.Add(new TabControlItem("TagControl", new TagControlsUC()) { Stichworte = "Anzeige;Tag;Layout" });
            this.tabItemSource.Add(new TabControlItem("Poptip", new PoptipControlsUC()) { Stichworte = "Anzeige;Poptip;Layout" });
            this.tabItemSource.Add(new TabControlItem("StepBar Control", new StepBarControlsUC()) { Stichworte = "Anzeige;StepBar;Layout" });
            this.tabItemSource.Add(new TabControlItem("WaterfallPanel Control", new WaterfallPanelControlsUC()) { Stichworte = "Anzeige;Panel;Layout" });

            this.tabItemSource.Add(new TabControlItem("Listen, Collection, XML Darstellung", true));
            this.tabItemSource.Add(new TabControlItem("FlatListView Control", new FlatListViewControlsUC()) { Stichworte = "ListView;Flat" });
            this.tabItemSource.Add(new TabControlItem("FilterDataGrid Control", new FilterDataGridControlsUC()) { Stichworte = "DataGrid;Filter;Flat" });
            this.tabItemSource.Add(new TabControlItem("XML Viewer Control", new XMLViewerControlsUC()) { Stichworte = "xml;viewer" });

            this.tabItemSource.Add(new TabControlItem("Dialoge und Funktionen", true));
            this.tabItemSource.Add(new TabControlItem("Dialoge", new FunctionControlsUC()) { Stichworte = "Dialog;View;Function;Funktion" });
            this.tabItemSource.Add(new TabControlItem("Exception Dialoge", new ExceptionControlsUC()) { Stichworte = "Dialog;View;Function;Funktion;Exception" });

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
            this.tabItemSource.Add(new TabControlItem("Datei/Verzeichnis Controls", new FileFolderBoxControlsUC()) { Stichworte = "Dialog;Verzeichnis;Dateien;File;Folder" });

            this.tabItemSource.Add(new TabControlItem($"Behavior Control\nErweiterungen", true));
            this.tabItemSource.Add(new TabControlItem("TextBlock Controls", new BehaviorsControlsUC()) { Stichworte = "TextBlock;" });
            this.tabItemSource.Add(new TabControlItem("TextBox Controls", new BehaviorTxTControlsUC()) { Stichworte = "TextBox;Eingabe;Input;Masken;Pattern" });
            this.tabItemSource.Add(new TabControlItem("TextBox Watermarket", new BehaviorWaterMControlsUC()) { Stichworte = "TextBox;Wasserzeichen;Watermarket;Behavior" });
            this.tabItemSource.Add(new TabControlItem("Excel Cell Behavior für Controls", new BehaviorExcelCellControlsUC()) { Stichworte = "Excel;Cell;Behavior" });
            this.tabItemSource.Add(new TabControlItem("CheckBox Behavior", new BehaviorCheckBoxUC()) { Stichworte = "CheckBox;Behavior" });

            this.tabItemSource.Add(new TabControlItem($"Spiel", true));
            this.tabItemSource.Add(new TabControlItem("Tic Tac Toe", new GamesControlsUC()) { Stichworte = "Spiel;Tic Tac Toe" });

            this.tabItemSource.Add(new TabControlItem($"C#, Methoden/Funktionen", true));
            this.tabItemSource.Add(new TabControlItem("Factory/Handler", new FactoryControlsUC()) { Stichworte = "C#;Pattern" });

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

            this.LbSourceBox.Focus();
            this.ListBoxSource.Refresh();
            this.ListBoxSource.MoveCurrentTo(2);
            this.LbSourceBox.Focus();

            this.CountSamples = this.tabItemSource.Count(c => c.IsGroupItem == false);
        }

        private void LoadCSSource()
        {
            this.ContentItem = null;
            this.tabItemSource = new List<TabControlItem>();
            this.tabItemSource.Add(new TabControlItem("Pattern", true));
            this.tabItemSource.Add(new TabControlItem("Singelton", new SyntaxBoxControlsUC()) { Stichworte = "Pattern;Singelton", SourceFile= "Pattern.Pattern_Singelton.txt" });
            this.tabItemSource.Add(new TabControlItem("Dispose", new SyntaxBoxControlsUC()) { Stichworte = "Pattern;Dispose", SourceFile = "Pattern.Pattern_DisposableCoreBase.txt" });
            this.tabItemSource.Add(new TabControlItem("Result", new SyntaxBoxControlsUC()) { Stichworte = "Pattern;Result", SourceFile = "Pattern.Pattern_Result.txt" });
            this.tabItemSource.Add(new TabControlItem("OperationResult", new SyntaxBoxControlsUC()) { Stichworte = "Pattern;OperationResult", SourceFile = "Pattern.Pattern_OperationResult.txt" });

            this.tabItemSource.Add(new TabControlItem("Algorithm ", true));
            this.tabItemSource.Add(new TabControlItem("Palindrome", new SyntaxBoxControlsUC()) { Stichworte = "Algorithm;Palindrome", SourceFile = "Algorithm.Algorithm_Palindrome.txt" });
            this.tabItemSource.Add(new TabControlItem("Permutationen", new SyntaxBoxControlsUC()) { Stichworte = "Algorithm;Permutationen", SourceFile = "Algorithm.Algorithm_Heaps.txt" });

            this.tabItemSource.Add(new TabControlItem("Core ", true));
            this.tabItemSource.Add(new TabControlItem("ExpressionEvaluator", new SyntaxBoxControlsUC()) { Stichworte = "Expression;Evaluator", SourceFile = "ExpressionEvaluator.Core_ExpressionEvaluator.txt" });

            this.tabItemSource.Add(new TabControlItem("Media ", true));
            this.tabItemSource.Add(new TabControlItem("ResourcesSound", new SyntaxBoxControlsUC()) { Stichworte = "WAV;Sound;Resources", SourceFile = "Media.Media_PlayWAV_Sound.txt" });

            this.tabItemSource.Add(new TabControlItem("Console ", true));
            this.tabItemSource.Add(new TabControlItem("CommandLine", new SyntaxBoxControlsUC()) { Stichworte = "Console;CommandLine", SourceFile = "Console.Console_CommandLine.txt" });
            this.tabItemSource.Add(new TabControlItem("Spinner", new SyntaxBoxControlsUC()) { Stichworte = "Console;Spinner", SourceFile = "Console.Console_Spinner.txt" });
            this.tabItemSource.Add(new TabControlItem("Table", new SyntaxBoxControlsUC()) { Stichworte = "Console;Table", SourceFile = "Console.Console_Table.txt" });

            this.tabItemSource.Add(new TabControlItem("Application ", true));
            this.tabItemSource.Add(new TabControlItem("SmartSettings", new SyntaxBoxControlsUC()) { Stichworte = "Application;SmartSettings", SourceFile = "CoreBase.CoreBase_SmartSettings.txt" });

            this.tabItemSource.Add(new TabControlItem("Custom Data Type ", true));
            this.tabItemSource.Add(new TabControlItem("Custom Data Type Birthday", new SyntaxBoxControlsUC()) { Stichworte = "C#;Custom Data Type;cdt", SourceFile = "CSharp.CSharp_CDT_Birthday.txt" });
            this.tabItemSource.Add(new TabControlItem("Custom Data Type Currency", new SyntaxBoxControlsUC()) { Stichworte = "C#;Custom Data Type;cdt", SourceFile = "CSharp.CSharp_CDT_Currency.txt" });
            this.tabItemSource.Add(new TabControlItem("Custom Data Type Roman", new SyntaxBoxControlsUC()) { Stichworte = "C#;Custom Data Type;cdt", SourceFile = "CSharp.CSharp_CDT_Roman.txt" });

            this.tabItemSource.Add(new TabControlItem("C# Technik ", true));
            this.tabItemSource.Add(new TabControlItem("Instance Class", new SyntaxBoxControlsUC()) { Stichworte = "C#;Class;Generic;Instance;new;ctor", SourceFile = "CSharp.CSharp_ArtofGenerics.txt" });
            this.tabItemSource.Add(new TabControlItem("WeakEvent 1", new SyntaxBoxControlsUC()) { Stichworte = "C#;Class;Generic;WeakEvent", SourceFile = "CSharp.CSharp_WeakEvent.txt" });
            this.tabItemSource.Add(new TabControlItem("WeakEvent 2", new SyntaxBoxControlsUC()) { Stichworte = "C#;WeakEventManager;Event", SourceFile = "CSharp.WeakEventManager.txt" });
            this.tabItemSource.Add(new TabControlItem("DynamicObject2Json", new SyntaxBoxControlsUC()) { Stichworte = "C#;Class;ExpandoObject;json", SourceFile = "CSharp.CSharp_DynamicObject2Json.txt" });
            this.tabItemSource.Add(new TabControlItem("Override Methode", new SyntaxBoxControlsUC()) { Stichworte = "C#;Override Methode", SourceFile = "CSharp.CSharp_Override_Methode.txt" });
            this.tabItemSource.Add(new TabControlItem("Callback Methode", new SyntaxBoxControlsUC()) { Stichworte = "C#;Callback Methode", SourceFile = "CSharp.CSharp_Callback_Function.txt" });
            this.tabItemSource.Add(new TabControlItem("Compare Objects", new SyntaxBoxControlsUC()) { Stichworte = "C#;Compare", SourceFile = "Object.CSharp_CompareObjects.txt" });
            this.tabItemSource.Add(new TabControlItem("IsCollection", new SyntaxBoxControlsUC()) { Stichworte = "C#;IsCollection", SourceFile = "CSharp.CSharp_IsCollection.txt" });
            this.tabItemSource.Add(new TabControlItem("StringBasedEnums", new SyntaxBoxControlsUC()) { Stichworte = "C#;Enum", SourceFile = "CSharp.CSharp_StringBasedEnums.txt" });
            this.tabItemSource.Add(new TabControlItem("StaticClassWithExtension", new SyntaxBoxControlsUC()) { Stichworte = "C#;Class;Extension", SourceFile = "CSharp.CSharp_StaticClassWithExtension.txt" });
            this.tabItemSource.Add(new TabControlItem("EnvironmentVariable", new SyntaxBoxControlsUC()) { Stichworte = "C#;Environment;Variable", SourceFile = "Environment.EnvironmentVariable.txt" });

            this.tabItemSource.Add(new TabControlItem("Network ", true));
            this.tabItemSource.Add(new TabControlItem("Network-Ping", new SyntaxBoxControlsUC()) { Stichworte = "C#;Network;Ping", SourceFile = "Network.CSharp_Network_Ping.txt" });

            this.tabItemSource.Add(new TabControlItem("IO ", true));
            this.tabItemSource.Add(new TabControlItem("TarFile", new SyntaxBoxControlsUC()) { Stichworte = "C#;File;Compressed", SourceFile = "IO.File_TarFile.txt" });

            this.tabItemSource.Add(new TabControlItem("Data ", true));
            this.tabItemSource.Add(new TabControlItem("DataTable", new SyntaxBoxControlsUC()) { Stichworte = "C#;Data;DataTable", SourceFile = "Data.CSharp_DataTable.txt" });
            this.tabItemSource.Add(new TabControlItem("Custom DataTable", new SyntaxBoxControlsUC()) { Stichworte = "C#;Data;DataTable", SourceFile = "Data.CSharp_CustomDataTable.txt" });
            this.tabItemSource.Add(new TabControlItem("Custom DataTable Demo", new SyntaxBoxControlsUC()) { Stichworte = "C#;Data;DataTable", SourceFile = "Data.CSharp_CustomDataTableDemo.txt" });

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

            this.LbSourceBox.Focus();
            this.ListBoxSource.Refresh();
            this.ListBoxSource.MoveCurrentTo(2);
            this.LbSourceBox.Focus();

            this.CountSamples = this.tabItemSource.Count(c => c.IsGroupItem == false);
        }

        private void OnBtnAboutClick(object sender, EventArgs e)
        {
            IEnumerable<IAssemblyInfo> metaInfo = null;
            using (AssemblyMetaService ams = new AssemblyMetaService())
            {
                metaInfo = ams.GetMetaInfo();
            }

            List<string> assemblyList = new List<string>();
            foreach (IAssemblyInfo assembly in metaInfo)
            {
                assemblyList.Add($"{assembly.AssemblyName}; {assembly.AssemblyVersion}");
            }

            string headLineText = "Versionen zur Modern Library.";

            NotificationResult dlgResult = NotificationListBox.Show("Application", headLineText, assemblyList, MessageBoxButton.OK, NotificationIcon.Information, NotificationResult.No);
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