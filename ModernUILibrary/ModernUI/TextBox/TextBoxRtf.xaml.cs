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
            /*this.RichTextControl.MakeUrlsClickable();*/
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
            copyMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            copyMenu.Command = ApplicationCommands.Copy;
            textBoxContextMenu.Items.Add(copyMenu);

            MenuItem pasteMenu = new MenuItem();
            pasteMenu.Header = "Einfügen";
            pasteMenu.Command = ApplicationCommands.Paste;
            pasteMenu.Icon = Icons.GetPathGeometry(Icons.IconPaste,22);
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
            formatBoldMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatBold);
            textBoxContextMenu.Items.Add(formatBoldMenu);

            MenuItem formatItalicMenu = new MenuItem();
            formatItalicMenu.Header = "Italic";
            formatItalicMenu.Command = EditingCommands.ToggleItalic;
            formatItalicMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatItalic);
            textBoxContextMenu.Items.Add(formatItalicMenu);

            MenuItem formatUnderlineMenu = new MenuItem();
            formatUnderlineMenu.Header = "Unterstreichen";
            formatUnderlineMenu.Command = EditingCommands.ToggleUnderline;
            formatUnderlineMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatUnderline);
            textBoxContextMenu.Items.Add(formatUnderlineMenu);

            MenuItem formatStrikeoutMenu = new MenuItem();
            formatStrikeoutMenu.Header = "Durchstreichen";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(formatStrikeoutMenu, "Click", this.OnStrikethrough);
            formatStrikeoutMenu.Icon = Icons.GetPathGeometry(Icons.IconFormatStrikethrough);
            textBoxContextMenu.Items.Add(formatStrikeoutMenu);

            textBoxContextMenu.Items.Add(new Separator());

            MenuItem insertImageMenu = new MenuItem();
            insertImageMenu.Header = "Einfügen Bild";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(insertImageMenu, "Click", this.OnInsertImageMenu);
            insertImageMenu.Icon = Icons.GetPathGeometry(Icons.IconInsertInmage);
            textBoxContextMenu.Items.Add(insertImageMenu);

            MenuItem insertLinkeMenu = new MenuItem();
            insertLinkeMenu.Header = "Einfügen Link";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(insertLinkeMenu, "Click", this.OnInsertLinkMenu);
            insertLinkeMenu.Icon = Icons.GetPathGeometry(Icons.IconInsertLink);
            textBoxContextMenu.Items.Add(insertLinkeMenu);

            textBoxContextMenu.Items.Add(new Separator());

            MenuItem changeTextColorMenu = new MenuItem();
            changeTextColorMenu.Header = "Textfarbe";
            changeTextColorMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor);
            textBoxContextMenu.Items.Add(changeTextColorMenu);

            MenuItem changeBlueMenu = new MenuItem();
            changeBlueMenu.Header = "Blau";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(changeBlueMenu, "Click", this.OnChangeBlueMenu);
            changeBlueMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor,Colors.Blue);
            changeTextColorMenu.Items.Add(changeBlueMenu);

            MenuItem changeRedMenu = new MenuItem();
            changeRedMenu.Header = "Rot";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(changeRedMenu, "Click", this.OnChangeRedMenu);
            changeRedMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor, Colors.Red);
            changeTextColorMenu.Items.Add(changeRedMenu);

            MenuItem changeGreenMenu = new MenuItem();
            changeGreenMenu.Header = "Grün";
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(changeGreenMenu, "Click", this.OnChangeGreenMenu);
            changeGreenMenu.Icon = Icons.GetPathGeometry(Icons.IconChangeColor, Colors.Green);
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
            string linkName = Clipboard.GetText();

            Paragraph para = new Paragraph();
            para.Margin = new Thickness(0);

            Hyperlink link = new Hyperlink();
            link.IsEnabled = true;
            link.Inlines.Add(linkName);
            link.NavigateUri = new Uri(linkName);
            link.RequestNavigate += (sender, args) => Process.Start(args.Uri.ToString());

            para.Inlines.Add(link);

            this.RichTextControl.Document.Blocks.Add(para);
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
                        if (match != null)
                        {
                            TextPointer start = pointer.GetPositionAtOffset(match.Index);
                            TextPointer end = start.GetPositionAtOffset(match.Length);

                            Hyperlink hyperlink = new(start, end)
                            {
                                NavigateUri = new Uri(match.Value)
                            };
                        }
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
