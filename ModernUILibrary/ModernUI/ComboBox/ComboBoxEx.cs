//-----------------------------------------------------------------------
// <copyright file="MComboBox.cs" company="Lifeprojects.de">
//     Class: MComboBox
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.07.2018</date>
//
// <summary>Class for UI Control ComboBoxEx</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using ModernIU.Base;

    [SupportedOSPlatform("windows")]
    public class ComboBoxEx : ComboBox
    {
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(ComboBoxEx), new PropertyMetadata(1000));
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register("ReadOnlyBackgroundColor", typeof(Brush), typeof(ComboBoxEx), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(222, 222, 222))));
        public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register("IsNumeric", typeof(bool), typeof(ComboBoxEx), new PropertyMetadata(false));
        public static readonly DependencyProperty IsEnabledContextMenuProperty = DependencyProperty.Register("IsEnabledContextMenu", typeof(bool), typeof(ComboBoxEx), new FrameworkPropertyMetadata(true, OnEnabledContextMenu));
        public static new readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ComboBoxEx), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsReadOnlyChangedCallback));
        public static readonly DependencyProperty HighlightedItemProperty = DependencyProperty.RegisterAttached("HighlightedItem",   typeof(object), typeof(ComboBoxEx));


        private static int selectedIndex = -1;

        private TextBox _comboBoxTextBox;
        private Brush _defaultBackgroundBorder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxEx"/> class.
        /// </summary>
        public ComboBoxEx() : base()
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
            this.MinHeight = 18;
            this.ClipToBounds = false;
            this.Focusable = true;

            WeakEventManager<ComboBox, SelectionChangedEventArgs>.AddHandler(this, "SelectionChanged", this.OnSelectionChanged);

            this.ReadOnlyBackgroundColor = Brushes.LightYellow;
            this.IsNumeric = false;
            this.IsReadOnly = false;
            this.IsEditable = true;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text) == true)
            {
                //this.SelectedIndex = -1;
                //this.SelectedValue = null;
            }
        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { this.SetValue(MaxLengthProperty, value); }
        }

        public Brush ReadOnlyBackgroundColor
        {
            get { return GetValue(ReadOnlyBackgroundColorProperty) as Brush; }
            set { this.SetValue(ReadOnlyBackgroundColorProperty, value); }
        }

        public bool IsNumeric
        {
            get { return (bool)GetValue(IsNumericProperty); }
            set { this.SetValue(IsNumericProperty, value); }
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

        public object HighlightedItem
        {
            get { return (object)GetValue(HighlightedItemProperty); }
            set { this.SetValue(HighlightedItemProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Set max input length
            this._comboBoxTextBox = this.Template.FindName("PART_EditableTextBox", this) as TextBox;
            if (this._comboBoxTextBox != null)
            {
                this._comboBoxTextBox.MaxLength = this.MaxLength;
                WeakEventManager<TextBox, TextCompositionEventArgs>.AddHandler(this._comboBoxTextBox, "PreviewTextInput", this.OnPreviewTextInput);
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (this.IsReadOnly == true)
            {
                if (e.IsDown == true || e.IsUp == true)
                {
                    this.SelectedIndex = selectedIndex;
                }
            }
        }

        protected void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char character = Convert.ToChar(e.Text);

            if (this.IsNumeric)
            {
                if (char.IsNumber(character))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private static void OnEnabledContextMenu(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxEx comboBox = d as ComboBoxEx;

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

        private static void OnIsReadOnlyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = d as ComboBox;

            if (e.NewValue != e.OldValue)
            {
                comboBox.IsReadOnly = (bool)e.NewValue;
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<ComboBoxEx>(ApplyIsReadOnly), comboBox);
        }

        private static void ApplyIsReadOnly(ComboBoxEx pComboBox)
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
                    backgroundBorder.Background = ((ComboBoxEx)pComboBox).ReadOnlyBackgroundColor;

                    if (buttonBorder != null)
                    {
                        buttonBorder.Background = ((ComboBoxEx)pComboBox).ReadOnlyBackgroundColor;
                    }

                    toggleButton.IsEnabled = false;
                    selectedIndex = pComboBox.SelectedIndex;
                }
                else
                {
                    backgroundBorder.Background = ((ComboBoxEx)pComboBox)._defaultBackgroundBorder;

                    if (buttonBorder != null)
                    {
                        buttonBorder.Background = ((ComboBoxEx)pComboBox)._defaultBackgroundBorder;
                    }

                    toggleButton.IsEnabled = true;
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
            Clipboard.SetText(this.Text);
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

    public static class ComboBoxHelper
    {
        public static readonly DependencyProperty EditBackgroundProperty = 
            DependencyProperty.RegisterAttached("EditBackground", typeof(Brush), typeof(ComboBoxHelper), new PropertyMetadata(default(Brush), EditBackgroundChanged));

        private static void EditBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var combo = dependencyObject as ComboBox;
            if (combo != null)
            {
                if (!combo.IsLoaded)
                {
                    RoutedEventHandler comboOnLoaded = null;
                    comboOnLoaded = delegate (object sender, RoutedEventArgs eventArgs)
                    {
                        EditBackgroundChanged(dependencyObject, args);
                        combo.Loaded -= comboOnLoaded;
                    };
                    combo.Loaded += comboOnLoaded;

                    return;
                }

                var part = combo.Template.FindName("PART_EditableTextBox", combo);
                var tb = part as TextBox;
                if (tb != null)
                {
                    var parent = tb.Parent as Border;
                    if (parent != null)
                    {
                        parent.Background = (Brush)args.NewValue;
                    }
                }
            }
        }

        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static void SetEditBackground(DependencyObject element, Brush value)
        {
            element.SetValue(EditBackgroundProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static Brush GetEditBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(EditBackgroundProperty);
        }
    }
}