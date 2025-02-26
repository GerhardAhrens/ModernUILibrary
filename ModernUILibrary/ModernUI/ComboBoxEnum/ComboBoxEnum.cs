namespace ModernIU.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernIU.Base;
    using ModernIU.Converters;

    /// <summary>
    /// Represents a combo box with specific support for enum types.
    /// <para>Implemented as a derived class of the <see cref="ComboBox"/> parent.</para>
    /// </summary>
    /// <remarks>
    /// <para>. Retain all features of a standard combo box.</para>
    /// <para>. Include an additional property to use for binding specifically on an enum property.</para>
    /// <para>. Provide support for localized value names by way of a converter property, to convert all values to their localized content.</para>
    /// <para>. Measure the largest selectable value before it reports its own size.</para>
    /// </remarks>
    public class ComboBoxEnum : ComboBox
    {
        private bool IsUserSelecting;
        private CultureInfo ConversionCulture = CultureInfo.CurrentCulture;
        private static int selectedIndex = -1;

        private TextBox _comboBoxTextBox;
        private Brush _defaultBackgroundBorder;

        public static readonly DependencyProperty EnumBindingProperty = 
            DependencyProperty.Register("EnumBinding", typeof(object), typeof(ComboBoxEnum), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEnumBindingChanged));

        public static readonly DependencyProperty NameConverterProperty = 
            DependencyProperty.Register("NameConverter", typeof(IValueConverter), typeof(ComboBoxEnum), new PropertyMetadata(new IdentityStringConverter()));

        public static readonly DependencyProperty NameConverterParameterProperty =
            DependencyProperty.Register("NameConverterParameter", typeof(object), typeof(ComboBoxEnum), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty NameConverterCultureProperty = 
            DependencyProperty.Register("NameConverterCulture", typeof(CultureInfo), typeof(ComboBoxEnum), new PropertyMetadata(CultureInfo.CurrentCulture));

        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = 
            DependencyProperty.Register("ReadOnlyBackgroundColor", typeof(Brush), typeof(ComboBoxEnum), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(222, 222, 222))));

        public static new readonly DependencyProperty IsReadOnlyProperty = 
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ComboBoxEnum), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsReadOnlyChangedCallback));

        public static readonly DependencyProperty IsEnabledContextMenuProperty = 
            DependencyProperty.Register("IsEnabledContextMenu", typeof(bool), typeof(ComboBoxEnum), new FrameworkPropertyMetadata(true, OnEnabledContextMenu));

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxEnum"/> class.
        /// </summary>
        public ComboBoxEnum()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.Height = ControlBase.DefaultHeight;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.MinHeight = 20;
            this.ClipToBounds = false;
            this.Focusable = true;
            this.EnumNameCollection = new ObservableCollection<string>();
            this.IsUserSelecting = false;
            this.ReadOnlyBackgroundColor = Brushes.LightYellow;
            this.IsReadOnly = false;
            this.IsEditable = true;
        }

        public object EnumBinding
        {
            get { return GetValue(EnumBindingProperty); }
            set { SetValue(EnumBindingProperty, value); }
        }

        public IValueConverter NameConverter
        {
            get { return (IValueConverter)GetValue(NameConverterProperty); }
            set { SetValue(NameConverterProperty, value); }
        }

        public object NameConverterParameter
        {
            get { return GetValue(NameConverterParameterProperty); }
            set { SetValue(NameConverterParameterProperty, value); }
        }

        public CultureInfo NameConverterCulture
        {
            get { return (CultureInfo)GetValue(NameConverterCultureProperty); }
            set { SetValue(NameConverterCultureProperty, value); }
        }

        public Brush ReadOnlyBackgroundColor
        {
            get { return GetValue(ReadOnlyBackgroundColorProperty) as Brush; }
            set { this.SetValue(ReadOnlyBackgroundColorProperty, value); }
        }

        public new bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        public bool IsEnabledContextMenu
        {
            get { return (bool)GetValue(IsEnabledContextMenuProperty); }
            set { this.SetValue(IsEnabledContextMenuProperty, value); }
        }

        private int EnumBindingAsIndex
        {
            get
            {
                int result = -1;

                if (EnumBinding != null)
                {
                    Type EnumType = EnumBinding.GetType();
                    if (EnumType.IsEnum)
                    {
                        Array Values = EnumType.GetEnumValues();
                        object CurrentValue = GetValue(EnumBindingProperty);

                        for (int i = 0; i < Values.Length; i++)
                        {
                            object EnumValue = Values.GetValue(i)!;
                            if (EnumValue.Equals(CurrentValue))
                            {
                                result = i;
                            }
                        }
                    }
                }

                return result;
            }
        }

        public ObservableCollection<string> EnumNameCollection { get; private set; }

        /// <summary>
        /// Gets the culture that was used during conversion of enum values to their localized names.
        /// </summary>

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Set max input length
            this._comboBoxTextBox = this.Template.FindName("PART_EditableTextBox", this) as TextBox;
            if (this._comboBoxTextBox != null)
            {
                this._comboBoxTextBox.MaxLength = 200;
            }

            // Read default backgroud color from combobox, to restore the original color.
            Border border = GetTemplateFrameworkElement(this, "Border") as Border;
            if (border != null)
            {
                this._defaultBackgroundBorder = border.Background;
            }

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            /* Spezifisches Kontextmenü für Control übergeben */
            if (this.IsEnabledContextMenu == true)
            {
                this._comboBoxTextBox.ContextMenu = this.BuildContextMenu();
            }
            else
            {
                this._comboBoxTextBox.ContextMenu = null;
            }
        }

        private static void OnEnumBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxEnum Ctrl = (ComboBoxEnum)d;
            Ctrl.OnEnumBindingChanged(e);
        }

        private void OnEnumBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.IsUserSelecting == true)
            {
                return;
            }

            if (e.NewValue == null)
            {
                this.ResetContent();
            }
            else if (e.OldValue == null || e.OldValue.GetType() != e.NewValue.GetType())
            {
                this.UpdateContent(e.NewValue.GetType());
            }

            int Index = this.EnumBindingAsIndex;
            if (SelectedIndex != Index)
            {
                this.SelectedIndex = Index;
            }
        }

        private static void OnIsReadOnlyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = d as ComboBox;

            if (e.NewValue != e.OldValue)
            {
                comboBox.IsReadOnly = (bool)e.NewValue;
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<ComboBoxEnum>(ApplyIsReadOnly), comboBox);
        }

        private static void ApplyIsReadOnly(ComboBoxEnum pComboBox)
        {
            ToggleButton toggleButton = null;
            Border buttonBorder = null;
            Border backgroundBorder = null;

            backgroundBorder = GetTemplateFrameworkElement(pComboBox, "Border") as Border;
            toggleButton = GetTemplateFrameworkElement(pComboBox, "toggleButton") as ToggleButton;
            buttonBorder = GetTemplateFrameworkElement(toggleButton, "splitBorder") as Border;

            // Toggle readonly modus
            if (backgroundBorder != null)
            {
                if (pComboBox.IsReadOnly)
                {
                    backgroundBorder.Background = ((ComboBoxEnum)pComboBox).ReadOnlyBackgroundColor;

                    if (buttonBorder != null)
                    {
                        buttonBorder.Background = ((ComboBoxEnum)pComboBox).ReadOnlyBackgroundColor;
                    }

                    toggleButton.IsEnabled = false;
                    selectedIndex = pComboBox.SelectedIndex;
                }
                else
                {
                    backgroundBorder.Background = ((ComboBoxEnum)pComboBox)._defaultBackgroundBorder;

                    if (buttonBorder != null)
                    {
                        buttonBorder.Background = ((ComboBoxEnum)pComboBox)._defaultBackgroundBorder;
                    }

                    toggleButton.IsEnabled = true;
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (this.IsReadOnly == true)
            {
                if (e.Key == Key.Down)
                {
                    if (this.SelectedIndex > this.EnumNameCollection.Count)
                    {
                        this.SelectedIndex = this.EnumNameCollection.Count;
                    }
                    this.SelectedIndex++;
                }
                else if (e.Key == Key.Up)
                {
                    if (this.SelectedIndex < 0)
                    {
                        this.SelectedIndex = 0;
                    }
                    this.SelectedIndex--;
                }
            }
        }

        private static void OnEnabledContextMenu(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxEnum comboBox = d as ComboBoxEnum;

            if (e.NewValue != e.OldValue)
            {
                /* Spezifisches Kontextmenü für Control übergeben */
                if (comboBox.IsEnabledContextMenu == true)
                {
                    comboBox._comboBoxTextBox.ContextMenu = comboBox.BuildContextMenu();
                }
                else
                {
                    if (comboBox._comboBoxTextBox != null)
                    {
                        comboBox._comboBoxTextBox.ContextMenu = null;
                    }
                }
            }
        }

        private static FrameworkElement GetTemplateFrameworkElement(Control pBasisControl, string pTemplateName)
        {
            FrameworkElement foundElement = null;

            if (pBasisControl != null && !string.IsNullOrEmpty(pTemplateName))
            {
                pBasisControl.ApplyTemplate();
                foundElement = pBasisControl.Template.FindName(pTemplateName, pBasisControl) as FrameworkElement;
            }

            return foundElement;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size BaseSize = base.MeasureOverride(constraint);

            double AddedWidth = 0;
            double AddedHeight = 0;
            Size SelectedEnumSize;

            if (SelectedIndex >= 0 && SelectedIndex < EnumNameCollection.Count)
            {
                SelectedEnumSize = GetTextSize(EnumNameCollection[SelectedIndex]);
            }
            else
            {
                SelectedEnumSize = BaseSize;
            }

            if (BaseSize.Width > SelectedEnumSize.Width)
            {
                AddedWidth = BaseSize.Width - SelectedEnumSize.Width;
            }

            if (BaseSize.Height > SelectedEnumSize.Height)
            {
                AddedHeight = BaseSize.Height - SelectedEnumSize.Height;
            }

            double MeasuredWidth = 0;
            double MeasuredHeight = 0;
            for (int i = 0; i < EnumNameCollection.Count; i++)
            {
                Size EnumSize = GetTextSize(EnumNameCollection[i]);

                if (MeasuredWidth < EnumSize.Width)
                {
                    MeasuredWidth = EnumSize.Width;
                }

                if (MeasuredHeight < EnumSize.Height)
                {
                    MeasuredHeight = EnumSize.Height;
                }
            }

            if (MeasuredWidth > 0 && MeasuredHeight > 0)
            {
                return new Size(Math.Min(MeasuredWidth + AddedWidth, constraint.Width), Math.Min(MeasuredHeight + AddedHeight, constraint.Height));
            }
            else
            {
                return BaseSize;
            }
        }

        private Size GetTextSize(string text)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            FormattedText EnumFormattedText = new FormattedText(text, ConversionCulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground);
#pragma warning restore CS0618 // Type or member is obsolete
            double EnumWidth = EnumFormattedText.WidthIncludingTrailingWhitespace;
            double EnumHeight = EnumFormattedText.Height;

            return new Size(EnumWidth, EnumHeight);
        }

        /// <summary>
        /// Override the <see cref="OnSelectionChanged"/> event handler, to update the bound enum property.
        /// </summary>
        /// <param name="e">Provides data for <see cref="SelectionChangedEventArgs"/>.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (EnumBinding != null)
            {
                Type EnumType = EnumBinding.GetType();
                if (EnumType.IsEnum)
                {
                    Array Values = EnumType.GetEnumValues();
                    if (SelectedIndex >= 0 && SelectedIndex < Values.Length)
                    {
                        this.IsUserSelecting = true;
                        SetValue(EnumBindingProperty, Values.GetValue(SelectedIndex));
                        this.IsUserSelecting = false;
                    }
                }
            }
        }

        private void ResetContent()
        {
            EnumNameCollection.Clear();
            SelectedIndex = -1;
        }

        private void UpdateContent(Type enumType)
        {
            ObservableCollection<string> NewEnumNameCollection = new ObservableCollection<string>();

            if (enumType.IsEnum)
            {
                IValueConverter Converter = NameConverter;
                object ConverterParameter = NameConverterParameter;
                ConversionCulture = NameConverterCulture;

                string[] EnumNames = enumType.GetEnumNames();
                foreach (string EnumName in EnumNames)
                {
                    string ConvertedText = (string)Converter.Convert(EnumName, typeof(string), ConverterParameter, ConversionCulture);
                    NewEnumNameCollection.Add(ConvertedText);
                }
            }

            EnumNameCollection = NewEnumNameCollection;

            if (SelectedIndex >= EnumNameCollection.Count)
            {
                SelectedIndex = -1;
            }

            this.ItemsSource = EnumNameCollection;
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = Border.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = Border.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyBackgroundColor });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
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
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen";
                pasteMenu.Icon = Icons.GetPathGeometry(Icons.IconPaste);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Ausschneiden";
                deleteMenu.Icon = Icons.GetPathGeometry(Icons.IconDelete);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(deleteMenu, "Click", this.OnDeleteMenu);
                textBoxContextMenu.Items.Add(deleteMenu);
            }

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            this._comboBoxTextBox.SelectAll();
            Clipboard.SetText(this._comboBoxTextBox.Text);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this.Text = Clipboard.GetText();
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
            this.Text = string.Empty;
        }
    }
}
