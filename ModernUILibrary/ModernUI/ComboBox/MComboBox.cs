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
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernIU.Base;

    [SupportedOSPlatform("windows")]
    public class MComboBox : ComboBox
    {
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(MComboBox), new PropertyMetadata(1000));
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register("ReadOnlyBackgroundColor", typeof(Brush), typeof(MComboBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(222, 222, 222))));
        public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register("IsNumeric", typeof(bool), typeof(MComboBox), new PropertyMetadata(false));
        public static new readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MComboBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsReadOnlyChangedCallback));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(MComboBox), new PropertyMetadata(true, OnSetBorderChanged));

        private static int selectedIndex = -1;

        private TextBox _comboBoxTextBox;
        private Brush _defaultBackgroundBorder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxEx"/> class.
        /// </summary>
        public MComboBox() : base()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.Height = ControlBase.DefaultHeight;
            this.Margin = ControlBase.DefaultMargin;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(0);
            this.MinHeight = 18;
            this.ClipToBounds = false;
            this.Focusable = true;

            this.ReadOnlyBackgroundColor = Brushes.LightYellow;
            this.IsNumeric = false;
            this.IsReadOnly = false;
            this.IsEditable = true;
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

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
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
                //this._defaultBackgroundBorder = border.Background;
                /* Rahmen für Control festlegen */
                if (SetBorder == true)
                {
                    this._defaultBackgroundBorder = Brushes.Green;
                    this.BorderThickness = new Thickness(1);
                }
                else
                {
                    this._defaultBackgroundBorder = Brushes.Transparent;
                    this.BorderThickness = new Thickness(0);
                }
            }

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            /* Spezifisches Kontextmenü für Control übergeben */
            this._comboBoxTextBox.ContextMenu = this.BuildContextMenu();

            /* Rahmen für Control festlegen */
            if (SetBorder == true)
            {
                this.BorderBrush = Brushes.Green;
                this.BorderThickness = new Thickness(1);
            }
            else
            {
                this.BorderBrush = Brushes.Transparent;
                this.BorderThickness = new Thickness(0);
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
            else
            {
                selectedIndex = this.SelectedIndex;
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

        private static void OnIsReadOnlyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = d as ComboBox;

            if (e.NewValue != e.OldValue)
            {
                comboBox.IsReadOnly = (bool)e.NewValue;
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<MComboBox>(ApplyIsReadOnly), comboBox);
        }

        private static void ApplyIsReadOnly(MComboBox pComboBox)
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
                    backgroundBorder.Background = ((MComboBox)pComboBox).ReadOnlyBackgroundColor;

                    if (buttonBorder != null)
                    {
                        buttonBorder.Background = ((MComboBox)pComboBox).ReadOnlyBackgroundColor;
                    }

                    toggleButton.IsEnabled = false;
                    selectedIndex = pComboBox.SelectedIndex;
                }
                else
                {
                    backgroundBorder.Background = ((MComboBox)pComboBox)._defaultBackgroundBorder;

                    if (buttonBorder != null)
                    {
                        buttonBorder.Background = ((MComboBox)pComboBox)._defaultBackgroundBorder;
                    }

                    toggleButton.IsEnabled = true;
                }
            }
        }

        private static FrameworkElement GetTemplateFrameworkElement(Control pBasisControl, string pTemplateName)
        {
            FrameworkElement foundElement = null;

            if (pBasisControl != null &&
                !string.IsNullOrEmpty(pTemplateName))
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
            copyMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen";
                pasteMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconPaste);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Ausschneiden";
                deleteMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconDelete);
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

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MComboBox)d;
                var backgroundBorder = GetTemplateFrameworkElement(control, "Border") as Border;
                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        backgroundBorder.BorderBrush = Brushes.Green;
                        backgroundBorder.BorderThickness = new Thickness(1);
                    }
                    else
                    {
                        backgroundBorder.BorderBrush = Brushes.Transparent;
                        backgroundBorder.BorderThickness = new Thickness(0);
                    }
                }
            }
        }
    }
}