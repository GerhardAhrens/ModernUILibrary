namespace ModernIU.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    /// <summary>
    /// Interaktionslogik für TextBoxRtf.xaml
    /// </summary>
    /// <example>
    /// https://stackoverflow.com/questions/14955755/adding-custom-options-to-wpf-richtextbox-context-menu
    /// </example>
    [SupportedOSPlatform("windows")]
    public partial class TextBoxRtf : UserControl
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(string), typeof(TextBoxRtf), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(TextBoxRtf), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsReadOnlyChangedCallback));
        public static readonly DependencyProperty IsShowToolbarProperty = DependencyProperty.Register("IsShowToolbar", typeof(bool), typeof(TextBoxRtf), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsShowToolbarChangedCallback));
        public static readonly DependencyProperty IsShowIOButtonProperty = DependencyProperty.Register("IsShowIOButton", typeof(bool), typeof(TextBoxRtf), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsShowIOButtonChangedCallback));


        private bool _DataChanged = false;
        private bool isGetText = false;

        public TextBoxRtf()
        {
            this.InitializeComponent();
            this.BorderBrush = Brushes.Green;
            this.BorderThickness = new Thickness(1);
            this.RichTextControl.IsDocumentEnabled = true;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<System.Windows.Controls.RichTextBox, RoutedEventArgs>.AddHandler(this.RichTextControl, "SelectionChanged", this.OnSelectionChanged);
            WeakEventManager<System.Windows.Controls.RichTextBox, TextChangedEventArgs>.AddHandler(this.RichTextControl, "TextChanged", this.OnTextChanged);
            WeakEventManager<System.Windows.Controls.RichTextBox, KeyEventArgs>.AddHandler(this.RichTextControl, "KeyDown", this.OnKeyDown);

            WeakEventManager<System.Windows.Controls.ComboBox, EventArgs>.AddHandler(this.Fontheight, "DropDownClosed", this.OnFontheightDropDownClosed);
            WeakEventManager<System.Windows.Controls.ComboBox, EventArgs>.AddHandler(this.Fonttype, "DropDownClosed", this.OnFonttypeDropDownClosed);

            WeakEventManager<ToggleButton, RoutedEventArgs>.AddHandler(this.ToolStripButtonStrikeout, "Click", this.OnStrikethrough);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnOpenText, "Click", this.OnOpenText);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnSaveText, "Click", this.OnSaveText);

            /* Spezifisches Kontextmenü für Control übergeben */
            this.RichTextControl.ContextMenu = this.BuildContextMenu();
        }

        public string Document
        {
            get { return (string)this.GetValue(DocumentProperty); }

            set { this.SetValue(DocumentProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        public bool IsShowToolbar
        {
            get { return (bool)GetValue(IsShowToolbarProperty); }
            set { this.SetValue(IsShowToolbarProperty, value); }
        }

        public bool IsShowIOButton
        {
            get { return (bool)GetValue(IsShowIOButtonProperty); }
            set { this.SetValue(IsShowIOButtonProperty, value); }
        }

        public bool DataChanged
        {
            get { return this._DataChanged; }
            set { this._DataChanged = value; }
        }

        private static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TextBoxRtf rtb = obj as TextBoxRtf;
            if (obj == null)
            {
                return;
            }

            if (rtb.isGetText == false)
            {
                rtb.SetRtfText(args.NewValue as string);
            }
        }

        private static void OnIsReadOnlyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxRtf rtfTextBox = d as TextBoxRtf;

            if (e.NewValue != e.OldValue)
            {
                rtfTextBox.IsReadOnly = (bool)e.NewValue;
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<TextBoxRtf>(ApplyIsReadOnly), rtfTextBox);
        }

        private static void ApplyIsReadOnly(TextBoxRtf rtfTextBox)
        {
            if (rtfTextBox != null)
            {
                if (rtfTextBox.IsReadOnly == true)
                {
                    rtfTextBox.ToolBarOben.IsEnabled = false;
                    rtfTextBox.RichTextControl.IsReadOnly = true;
                    rtfTextBox.RichTextControl.Background = Brushes.LightYellow;
                }
                else
                {
                    rtfTextBox.ToolBarOben.IsEnabled = true;
                    rtfTextBox.RichTextControl.IsReadOnly = false;
                    rtfTextBox.RichTextControl.Background = Brushes.Transparent;
                }
            }
        }

        private static void OnIsShowToolbarChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxRtf rtfTextBox = d as TextBoxRtf;

            if (e.NewValue != e.OldValue)
            {
                rtfTextBox.IsShowToolbar = (bool)e.NewValue;
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<TextBoxRtf>(ApplyIsShowToolbar), rtfTextBox);
        }

        private static void ApplyIsShowToolbar(TextBoxRtf rtfTextBox)
        {
            if (rtfTextBox != null)
            {
                if (rtfTextBox.IsShowToolbar == false)
                {
                    rtfTextBox.ToolBarOben.Visibility = Visibility.Collapsed;
                    rtfTextBox.StatusbarUnten.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rtfTextBox.ToolBarOben.Visibility = Visibility.Visible;
                    rtfTextBox.StatusbarUnten.Visibility = Visibility.Visible;
                }
            }
        }

        private static void OnIsShowIOButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxRtf rtfTextBox = d as TextBoxRtf;

            if (e.NewValue != e.OldValue)
            {
                rtfTextBox.IsShowToolbar = (bool)e.NewValue;
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<TextBoxRtf>(ApplyIsShowIOButton), rtfTextBox);
        }

        private static void ApplyIsShowIOButton(TextBoxRtf rtfTextBox)
        {
            if (rtfTextBox != null)
            {
                if (rtfTextBox.IsShowIOButton == false)
                {
                    rtfTextBox.BtnOpenText.Visibility = Visibility.Collapsed;
                    rtfTextBox.BtnSaveText.Visibility = Visibility.Collapsed;
                    rtfTextBox.SeparatorIOButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rtfTextBox.BtnOpenText.Visibility = Visibility.Visible;
                    rtfTextBox.BtnSaveText.Visibility = Visibility.Visible;
                    rtfTextBox.SeparatorIOButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Schrifttypen- und -größen-Initialisierung
            TextRange range = new TextRange(this.RichTextControl.Selection.Start, this.RichTextControl.Selection.End);
            Fonttype.SelectedValue = range.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString();
            Fontheight.SelectedValue = range.GetPropertyValue(FlowDocument.FontSizeProperty).ToString();

            // aktuelle Zeilen- und Spaltenpositionen angeben
            this.runLine.Text = this.GetLinenumber().ToString();
            this.runColumn.Text = GetColumnnumber().ToString();

            this.RichTextControl.Focus();
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            // markierten Text holen
            TextRange selectionRange = new TextRange(this.RichTextControl.Selection.Start, this.RichTextControl.Selection.End);

            if (selectionRange.GetPropertyValue(FontWeightProperty).ToString() == "Bold")
            {
                this.ToolStripButtonBold.IsChecked = true;
            }
            else
            {
                this.ToolStripButtonBold.IsChecked = false;
            }

            if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Left")
            {
                ToolStripButtonAlignLeft.IsChecked = true;
            }

            if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Center")
            {
                ToolStripButtonAlignCenter.IsChecked = true;
            }

            if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Right")
            {
                ToolStripButtonAlignRight.IsChecked = true;
            }

            // Get selected font and height and update selection in ComboBoxes
            Fonttype.SelectedValue = selectionRange.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString();
            Fontheight.SelectedValue = selectionRange.GetPropertyValue(FlowDocument.FontSizeProperty).ToString();

            // aktuelle Zeilen- und Spaltenpositionen angeben
            this.runLine.Text = this.GetLinenumber().ToString();
            this.runColumn.Text = GetColumnnumber().ToString();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.DataChanged = true;
            this.isGetText = true;

            try
            {
                this.Document = this.GetRtfText();
            }
            finally
            {
                this.isGetText = false;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            this.DataChanged = true;
            this.RichTextControl.MakeUrlsClickable();
        }

        private void OnFontheightDropDownClosed(object sender, EventArgs e)
        {
            string fontHeight = (string)Fontheight.SelectedItem;

            if (fontHeight != null)
            {
                RichTextControl.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, fontHeight);
                RichTextControl.Focus();
            }
        }

        private void OnFonttypeDropDownClosed(object sender, EventArgs e)
        {
            string fontName = (string)Fonttype.SelectedItem;

            if (fontName != null)
            {
                RichTextControl.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
                RichTextControl.Focus();
            }
        }

        private int GetLinenumber()
        {
            TextPointer caretLineStart = this.RichTextControl.CaretPosition.GetLineStartPosition(0);
            TextPointer p = this.RichTextControl.Document.ContentStart.GetLineStartPosition(0);
            int currentLineNumber = 1;

            // Vom Anfang des RTF-Box Inhaltes wird vorwärts solange weitergezählt, bis die aktuelle Cursorposition erreicht ist.
            while (true)
            {
                if (caretLineStart.CompareTo(p) < 0)
                {
                    break;
                }
                int result;
                p = p.GetLineStartPosition(1, out result);
                if (result == 0)
                {
                    break;
                }
                currentLineNumber++;
            }
            return currentLineNumber;
        }

        private int GetColumnnumber()
        {
            TextPointer caretPos = this.RichTextControl.CaretPosition;
            TextPointer p = this.RichTextControl.CaretPosition.GetLineStartPosition(0);
            int currentColumnNumber = Math.Max(p.GetOffsetToPosition(caretPos) - 1, 0);

            return currentColumnNumber;
        }

        private void Clear()
        {
            this.DataChanged = false;
            this.RichTextControl.Document.Blocks.Clear();
        }

        private void SetRtfText(string rtfText)
        {
            TextRange range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);

            // Exception abfangen für StreamReader und MemoryStream, ArgumentException abfangen für range.Load bei rtf=null oder rtf=""          
            try
            {
                // um die Load Methode eines TextRange Objektes zu benutzen wird ein MemoryStream Objekt erzeugt
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        rtfStreamWriter.Write(rtfText);
                        rtfStreamWriter.Flush();
                        rtfMemoryStream.Seek(0, SeekOrigin.Begin);

                        range.Load(rtfMemoryStream, DataFormats.Rtf);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetRtfText()
        {
            TextRange range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);

            // Exception abfangen für StreamReader und MemoryStream
            try
            {
                // um die Load Methode eines TextRange Objektes zu benutzen wird ein MemoryStream Objekt erzeugt
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        range.Save(rtfMemoryStream, DataFormats.Rtf);

                        rtfMemoryStream.Flush();
                        rtfMemoryStream.Position = 0;
                        StreamReader sr = new StreamReader(rtfMemoryStream);
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OnStrikethrough(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);

            TextDecorationCollection tdc = (TextDecorationCollection)RichTextControl.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            if (tdc == null || !tdc.Equals(TextDecorations.Strikethrough))
            {
                tdc = TextDecorations.Strikethrough;

            }
            else
            {
                tdc = new TextDecorationCollection();
            }

            range.ApplyPropertyValue(Inline.TextDecorationsProperty, tdc);
        }

        private void OnSaveText(object sender, RoutedEventArgs e)
        {
            string textRtf = this.GetRtfText();

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = string.Empty;
            dlg.DefaultExt = ".rtf"; // Default file extension
            dlg.Filter = "RichText Datei (.rtf)|*.rtf|Text documents (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                File.WriteAllText(dlg.FileName, textRtf);
            }
        }

        private void OnOpenText(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = string.Empty; 
            dlg.DefaultExt = ".rtf"; // Default file extension
            dlg.Filter = "RichText Datei (.rtf)|*.rtf|Text documents (.txt)|*.txt"; 

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string path = dlg.FileName;
                TextRange range;
                FileStream fStream;

                if (File.Exists(path) == true)
                {
                    range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);
                    fStream = new FileStream(path, FileMode.OpenOrCreate);
                    range.Load(fStream, DataFormats.Rtf);
                    fStream.Close();
                }
            }
        }

        /// <summary>
        /// Spezifisches Kontextmenü erstellen
        /// </summary>
        /// <returns></returns>
        private ContextMenu BuildContextMenu()
        {
            ContextMenu textBoxContextMenu = new ContextMenu();
            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "Kopiere";
            copyMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy, 16);
            copyMenu.Command = ApplicationCommands.Copy;
            textBoxContextMenu.Items.Add(copyMenu);

            MenuItem pasteMenu = new MenuItem();
            pasteMenu.Header = "Einfügen";
            pasteMenu.Command = ApplicationCommands.Paste;
            pasteMenu.Icon = Icons.GetPathGeometry(Icons.IconPaste, 16);
            textBoxContextMenu.Items.Add(pasteMenu);

            MenuItem spezialMenu = new MenuItem();
            spezialMenu.Header = "Spezial";
            spezialMenu.Icon = Icons.GetPathGeometry(Icons.Icon3DotMenu, 16);
            textBoxContextMenu.Items.Add(spezialMenu);

            MenuItem setDateMenu = new MenuItem();
            setDateMenu.Header = "Setze Datum";
            setDateMenu.Icon = Icons.GetPathGeometry(Icons.IconClock, 16);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(setDateMenu, "Click", this.OnSetDateMenu);
            spezialMenu.Items.Add(setDateMenu);

            textBoxContextMenu.Items.Add(new Separator());

            MenuItem formatBoldMenu = new MenuItem();
            formatBoldMenu.Header = "Fett";
            formatBoldMenu.Command = EditingCommands.ToggleBold;
            formatBoldMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatBold, 16);
            textBoxContextMenu.Items.Add(formatBoldMenu);

            MenuItem formatItalicMenu = new MenuItem();
            formatItalicMenu.Header = "Italic";
            formatItalicMenu.Command = EditingCommands.ToggleItalic;
            formatItalicMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatItalic, 16);
            textBoxContextMenu.Items.Add(formatItalicMenu);

            MenuItem formatUnderlineMenu = new MenuItem();
            formatUnderlineMenu.Header = "Unterstreichen";
            formatUnderlineMenu.Command = EditingCommands.ToggleUnderline;
            formatUnderlineMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatUnderline, 16);
            textBoxContextMenu.Items.Add(formatUnderlineMenu);

            MenuItem formatStrikeoutMenu = new MenuItem();
            formatStrikeoutMenu.Header = "Durchstreichen";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(formatStrikeoutMenu, "Click", this.OnStrikethrough);
            formatStrikeoutMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatStrikethrough, 16);
            textBoxContextMenu.Items.Add(formatStrikeoutMenu);

            textBoxContextMenu.Items.Add(new Separator());

            MenuItem insertImageMenu = new MenuItem();
            insertImageMenu.Header = "Einfügen Bild";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(insertImageMenu, "Click", this.OnInsertImageMenu);
            insertImageMenu.Icon = Icons.GetPathGeometry(Icons.IconInsertInmage, 16);
            textBoxContextMenu.Items.Add(insertImageMenu);

            MenuItem insertLinkeMenu = new MenuItem();
            insertLinkeMenu.Header = "Einfügen Link";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(insertLinkeMenu, "Click", this.OnInsertLinkMenu);
            insertLinkeMenu.Icon = Icons.GetPathGeometry(Icons.IconInsertLink,16);
            textBoxContextMenu.Items.Add(insertLinkeMenu);

            textBoxContextMenu.Items.Add(new Separator());

            MenuItem changeTextColorMenu = new MenuItem();
            changeTextColorMenu.Header = "Textfarbe";
            changeTextColorMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor,16);
            textBoxContextMenu.Items.Add(changeTextColorMenu);

            MenuItem changeBlueMenu = new MenuItem();
            changeBlueMenu.Header = "Blau";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(changeBlueMenu, "Click", this.OnChangeBlueMenu);
            changeBlueMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor,Colors.Blue,16);
            changeTextColorMenu.Items.Add(changeBlueMenu);

            MenuItem changeRedMenu = new MenuItem();
            changeRedMenu.Header = "Rot";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(changeRedMenu, "Click", this.OnChangeRedMenu);
            changeRedMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor, Colors.Red,16);
            changeTextColorMenu.Items.Add(changeRedMenu);

            MenuItem changeGreenMenu = new MenuItem();
            changeGreenMenu.Header = "Grün";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(changeGreenMenu, "Click", this.OnChangeGreenMenu);
            changeGreenMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor, Colors.Green,16);
            changeTextColorMenu.Items.Add(changeGreenMenu);

            return textBoxContextMenu;
        }

        private void OnChangeBlueMenu(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
            range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(Colors.Blue));
        }

        private void OnChangeRedMenu(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
            range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(Colors.Red));
        }

        private void OnChangeGreenMenu(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
            range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(Colors.Green));
        }

        private void OnSetDateMenu(object sender, RoutedEventArgs e)
        {
            this.RichTextControl.CaretPosition.InsertTextInRun(DateTime.Now.ToString());
        }

        private void OnInsertImageMenu(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = string.Empty;
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "Bild (.png)|*.png|Bild (.jpg)|*.jpg";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string imageFile = dlg.FileName;
                if (File.Exists(imageFile) == true)
                {
                    System.Windows.Controls.Image insertImage = null;
                    ImageSource imageSource = new BitmapImage(new Uri(imageFile));
                    insertImage = new System.Windows.Controls.Image()
                    { 
                        Source = imageSource, 
                        Stretch = Stretch.UniformToFill,
                        Width = imageSource.Width,
                        Height = imageSource.Height,
                    };

                    Clipboard.SetImage((BitmapImage)imageSource);
                    RichTextControl.Paste();
                }
            }
        }

        private void OnInsertLinkMenu(object sender, RoutedEventArgs e)
        {
        }
    }

    public sealed class FontHeight : ObservableCollection<string>
    {
        public FontHeight()
        {
            this.Add("8");
            this.Add("9");
            this.Add("10");
            this.Add("11");
            this.Add("12");
            this.Add("14");
            this.Add("16");
            this.Add("18");
            this.Add("20");
            this.Add("22");
            this.Add("24");
            this.Add("26");
            this.Add("28");
            this.Add("36");
            this.Add("48");
            this.Add("72");
        }
    }

    public sealed class FontList : ObservableCollection<string>
    {
        public FontList()
        {
            foreach (FontFamily f in Fonts.SystemFontFamilies)
            {
                this.Add(f.ToString());
            }
        }
    }

    /// <summary>
    /// Liste von Colors
    /// </summary>
    /// <example>
    /// https://stackoverflow.com/questions/29263904/wpf-combobox-as-system-windows-media-colors
    /// https://wpf-tutorial.com/list-controls/combobox-control/
    /// </example>
    public sealed class ListOfColors : ObservableCollection<ColorItem>
    {
        public ListOfColors()
        {
            PropertyInfo[] colorProperties = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo colorProperty in colorProperties)
            {
                if (colorProperty.PropertyType.Name == typeof(Color).Name)
                {
                    Color color = (Color)colorProperty.GetValue(null, null);
                    string colorName = colorProperty.Name;
                    SolidColorBrush brush = new SolidColorBrush(color);
                    Tuple<int, int, int, int> rgbCode = new Tuple<int, int, int, int>(brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A);
                    ColorItem item = new ColorItem() { Brush = brush, Name = colorName, RGBA = rgbCode };
                    this.Add(item);
                }
            }
        }
    }

    [DebuggerDisplay("Name: {this.Name}; Color: {this.Brush}")]
    public sealed class ColorItem
    {
        public SolidColorBrush Brush { get; set; }
        public Tuple<int,int,int,int> RGBA { get; set; }
        public string Name { get; set; }
    }

    public static class Icons
    {
        private const string ICON_COPY = "M19,21H8V7H19M19,5H8A2,2 0 0,0 6,7V21A2,2 0 0,0 8,23H19A2,2 0 0,0 21,21V7A2,2 0 0,0 19,5M16,1H4A2,2 0 0,0 2,3V17H4V3H16V1Z";
        private const string ICON_PASTE = "M19,20H5V4H7V7H17V4H19M12,2A1,1 0 0,1 13,3A1,1 0 0,1 12,4A1,1 0 0,1 11,3A1,1 0 0,1 12,2M19,2H14.82C14.4,0.84 13.3,0 12,0C10.7,0 9.6,0.84 9.18,2H5A2,2 0 0,0 3,4V20A2,2 0 0,0 5,22H19A2,2 0 0,0 21,20V4A2,2 0 0,0 19,2Z";
        private const string ICON_CLOCK = "M12,20A8,8 0 0,0 20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22C6.47,22 2,17.5 2,12A10,10 0 0,1 12,2M12.5,7V12.25L17,14.92L16.25,16.15L11,13V7H12.5Z";
        private const string ICON_FORMAT_BOLD = "M13.5,15.5H10V12.5H13.5A1.5,1.5 0 0,1 15,14A1.5,1.5 0 0,1 13.5,15.5M10,6.5H13A1.5,1.5 0 0,1 14.5,8A1.5,1.5 0 0,1 13,9.5H10M15.6,10.79C16.57,10.11 17.25,9 17.25,8C17.25,5.74 15.5,4 13.25,4H7V18H14.04C16.14,18 17.75,16.3 17.75,14.21C17.75,12.69 16.89,11.39 15.6,10.79Z";
        private const string ICON_FORMAT_ITALIC = "M10,4V7H12.21L8.79,15H6V18H14V15H11.79L15.21,7H18V4H10Z";
        private const string ICON_FORMAT_UNDERLINE = "M5,21H19V19H5V21M12,17A6,6 0 0,0 18,11V3H15.5V11A3.5,3.5 0 0,1 12,14.5A3.5,3.5 0 0,1 8.5,11V3H6V11A6,6 0 0,0 12,17Z";
        private const string ICON_FORMAT_STRIKETHROUGH = "M7.2 9.8C6 7.5 7.7 4.8 10.1 4.3C13.2 3.3 17.7 4.7 17.6 8.5H14.6C14.6 8.2 14.5 7.9 14.5 7.7C14.3 7.1 13.9 6.8 13.3 6.6C12.5 6.3 11.2 6.4 10.5 6.9C9 8.2 10.4 9.5 12 10H7.4C7.3 9.9 7.3 9.8 7.2 9.8M21 13V11H3V13H12.6C12.8 13.1 13 13.1 13.2 13.2C13.8 13.5 14.3 13.7 14.5 14.3C14.6 14.7 14.7 15.2 14.5 15.6C14.3 16.1 13.9 16.3 13.4 16.5C11.6 17 9.4 16.3 9.5 14.1H6.5C6.4 16.7 8.6 18.5 11 18.8C14.8 19.6 19.3 17.2 17.3 12.9L21 13Z";
        private const string ICON_INSERT_IMAGE = "M19,19H5V5H19M19,3H5A2,2 0 0,0 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5A2,2 0 0,0 19,3M13.96,12.29L11.21,15.83L9.25,13.47L6.5,17H17.5L13.96,12.29Z";
        private const string ICON_INSERT_LINK = "M3.9,12C3.9,10.29 5.29,8.9 7,8.9H11V7H7A5,5 0 0,0 2,12A5,5 0 0,0 7,17H11V15.1H7C5.29,15.1 3.9,13.71 3.9,12M8,13H16V11H8V13M17,7H13V8.9H17C18.71,8.9 20.1,10.29 20.1,12C20.1,13.71 18.71,15.1 17,15.1H13V17H17A5,5 0 0,0 22,12A5,5 0 0,0 17,7Z";
        private const string ICON_CHANGE_COLOR = "M19,11.5C19,11.5 17,13.67 17,15A2,2 0 0,0 19,17A2,2 0 0,0 21,15C21,13.67 19,11.5 19,11.5M5.21,10L10,5.21L14.79,10M16.56,8.94L7.62,0L6.21,1.41L8.59,3.79L3.44,8.94C2.85,9.5 2.85,10.47 3.44,11.06L8.94,16.56C9.23,16.85 9.62,17 10,17C10.38,17 10.77,16.85 11.06,16.56L16.56,11.06C17.15,10.47 17.15,9.5 16.56,8.94Z";
        private const string ICOM_3DOT_MENU = "M12,16A2,2 0 0,1 14,18A2,2 0 0,1 12,20A2,2 0 0,1 10,18A2,2 0 0,1 12,16M12,10A2,2 0 0,1 14,12A2,2 0 0,1 12,14A2,2 0 0,1 10,12A2,2 0 0,1 12,10M12,4A2,2 0 0,1 14,6A2,2 0 0,1 12,8A2,2 0 0,1 10,6A2,2 0 0,1 12,4Z";

        public static string IconCopy
        {
            get { return ICON_COPY; }
        }

        public static string IconPaste
        {
            get { return ICON_PASTE; }
        }

        public static string IconClock
        {
            get { return ICON_CLOCK; }
        }

        public static string IconFormatBold
        {
            get { return ICON_FORMAT_BOLD; }
        }

        public static string IconFormatItalic
        {
            get { return ICON_FORMAT_ITALIC; }
        }

        public static string IconFormatUnderline
        {
            get { return ICON_FORMAT_UNDERLINE; }
        }

        public static string IconFormatStrikethrough
        {
            get { return ICON_FORMAT_STRIKETHROUGH; }
        }

        public static string IconInsertInmage
        {
            get { return ICON_INSERT_IMAGE; }
        }

        public static string IconInsertLink
        {
            get { return ICON_INSERT_LINK; }
        }

        public static string IconChangeColor
        {
            get { return ICON_CHANGE_COLOR; }
        }

        public static string Icon3DotMenu
        {
            get { return ICOM_3DOT_MENU; }
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <param name="iconColor">Icon Farbe</param>
        /// <returns></returns>
        public static System.Windows.Shapes.Path GetPathGeometry(string iconString, Color iconColor, int size = 24)
        {
            var path = new System.Windows.Shapes.Path
            {
                Height = size,
                Width = size,
                Fill = new SolidColorBrush(iconColor),
                Data = Geometry.Parse(iconString)
            };

            return path;
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        public static System.Windows.Shapes.Path GetPathGeometry(string iconString, int size = 24)
        {
            return GetPathGeometry(iconString, Colors.Blue, size);
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        public static System.Windows.Shapes.Path GetPathGeometry(string iconString)
        {
            return GetPathGeometry(iconString, Colors.Blue);
        }
    }

    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Scan the content of a RichTextBox control and make any https URLs
        /// clickable.  The initial version of this method was written by Bing Chat,
        /// and then tidied up by me and Intellicode.
        /// </summary>
        /// <param name="self"></param>
        public static void MakeUrlsClickable(this System.Windows.Controls.RichTextBox self)
        {
            TextPointer pointer = self.Document.ContentStart;

            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, @"(http(s)?://[^\s]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    foreach (Match match in matches.Cast<Match>())
                    {
                        TextPointer start = pointer.GetPositionAtOffset(match.Index);
                        TextPointer end = start.GetPositionAtOffset(match.Length);
                        Hyperlink hyperlink = new(start, end)
                        {
                            NavigateUri = new Uri(match.Value)
                        };
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
