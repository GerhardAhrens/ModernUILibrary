namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class MPasswordBox : IconTextBoxBase
    {
        private ToggleButton PART_SeePassword;
        private bool mIsHandledTextChanged = true;
        private StringBuilder mPasswordBuilder;

        public static readonly DependencyProperty IsCanSeePasswordProperty =
            DependencyProperty.Register("IsCanSeePassword", typeof(bool), typeof(MPasswordBox), new PropertyMetadata(true, IsCanSeePasswordChangedCallback));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(MPasswordBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ShowPasswordProperty =
            DependencyProperty.Register("ShowPassword", typeof(bool), typeof(MPasswordBox), new PropertyMetadata(false, ShowPasswordChanged));

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(MPasswordBox), new PropertyMetadata('●'));

        public static readonly DependencyProperty SetBorderProperty = 
            DependencyProperty.Register("SetBorder", typeof(bool), typeof(MPasswordBox), new PropertyMetadata(true, OnSetBorderChanged));

        #region Constructors

        public MPasswordBox()
        {
            this.Height = ControlBase.DefaultHeight;
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = ControlBase.DefaultMargin;
            this.Height = ControlBase.DefaultHeight;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.IsReadOnly = false;
            this.Focusable = true;
        }

        static MPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MPasswordBox), new FrameworkPropertyMetadata(typeof(MPasswordBox)));
        }

        #endregion

        [Bindable(true), Description("Holt oder setzt, ob das Passwort sichtbar ist")]
        public bool IsCanSeePassword
        {
            get { return (bool)GetValue(IsCanSeePasswordProperty); }
            set { SetValue(IsCanSeePasswordProperty, value); }
        }
        
        [Bindable(true), Description("Abrufen oder Setzen des aktuellen Passworts")]
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        

        [Bindable(true), Description("Holt oder setzt das Maskenzeichen der PasswordBox.")]
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        public bool ShowPassword
        {
            get { return (bool)GetValue(ShowPasswordProperty); }
            private set { SetValue(ShowPasswordProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_SeePassword = this.GetTemplateChild("PART_SeePassword") as ToggleButton;
            if(this.PART_SeePassword != null)
            {
                this.PART_SeePassword.Visibility = this.IsCanSeePassword ? Visibility.Visible : Visibility.Collapsed;
            }
            this.SetEvent();

            if (this.Password != null)
            {
                this.SetText(this.ConvertToPasswordChar(this.Password.Length));
            }

            this.CommandBindings.Add(new System.Windows.Input.CommandBinding(ApplicationCommands.Copy, CommandBinding_Executed, CommandBinding_CanExecute));

            /* Spezifisches Kontextmenü für Control übergeben */
            this.ContextMenu = this.BuildContextMenu();
        }

        public override void OnCornerRadiusChanged(CornerRadius newValue)
        {
            this.IconCornerRadius = new CornerRadius(newValue.TopLeft, 0, 0, newValue.BottomLeft);
        }

        private void SetEvent()
        {
            this.TextChanged += OnTextChanged;
            if(this.PART_SeePassword != null)
            {
                this.PART_SeePassword.Checked += (o, e) => 
                {
                    this.SetText(this.Password);
                    this.ShowPassword = true;
                };

                this.PART_SeePassword.Unchecked += (o, e) =>
                {
                    if (this.Password != null)
                    {
                        this.SetText(this.ConvertToPasswordChar(this.Password.Length));
                        this.ShowPassword = false;
                    }
                };
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MPasswordBox)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.BorderBrush = ControlBase.BorderBrush;
                        control.BorderThickness = ControlBase.BorderThickness;
                    }
                    else
                    {
                        control.BorderBrush = Brushes.Transparent;
                        control.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        private void SetText(string str)
        {
            this.mIsHandledTextChanged = false;
            this.Text = str;
            this.mIsHandledTextChanged = true;
        }

        private string ConvertToPasswordChar(int length)
        {
            if (mPasswordBuilder != null)
            {
                mPasswordBuilder.Clear();
            }
            else
            {
                mPasswordBuilder = new StringBuilder();
            }
                
            for (var i = 0; i < length; i++)
            {
                mPasswordBuilder.Append(this.PasswordChar);
            }
                
            return mPasswordBuilder.ToString();
        }

        private void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!this.mIsHandledTextChanged)
            {
                return;
            }

            foreach (TextChange c in e.Changes)
            {
                if (this.Password != null)
                {
                    this.Password = this.Password.Remove(c.Offset, c.RemovedLength);
                    this.Password = this.Password.Insert(c.Offset, Text.Substring(c.Offset, c.AddedLength));
                }
            }

            if(!this.ShowPassword)
            {
                this.SetText(ConvertToPasswordChar(Text.Length));
            }

            this.SelectionStart = this.Text.Length + 1;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Password) == false)
            {
                Clipboard.SetText(this.Password);
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private static void IsCanSeePasswordChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MPasswordBox passowrdBox = d as MPasswordBox;
            if (passowrdBox != null && passowrdBox.PART_SeePassword != null)
            {
                passowrdBox.PART_SeePassword.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void ShowPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MPasswordBox passwordBox = d as MPasswordBox;
            if (d != null)
            {
                if (passwordBox != null)
                {
                    passwordBox.SelectionStart = passwordBox.Text.Length + 1;
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
            Clipboard.SetText(this.Password);
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
