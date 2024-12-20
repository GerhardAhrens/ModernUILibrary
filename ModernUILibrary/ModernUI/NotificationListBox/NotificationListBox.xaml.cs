//-----------------------------------------------------------------------
// <copyright file="NotificationListBox.cs" company="Lifeprojects.de">
//     Class: NotificationListBox
//     Copyright © company="Lifeprojects.de" 2018
// </copyright>
//
// <author>Gerhard Ahrens - company="Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>24.08.2018</date>
//
// <summary>Implementierung einer Custom NotificationListBox</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernBaseLibrary.Extension;

    using ModernIU.Base;

    /// <summary>
    /// Interaktionslogik für NotificationListBox.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class NotificationListBox : Window
    {
        private static NotificationResult customDialogResult = NotificationResult.None;
        private static MessageBoxButton customMessageBoxButton = MessageBoxButton.YesNoCancel;
        private static ImageSource instructionIconSource = null;
        private readonly CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        private readonly bool isLanguageDE = true;
        private DispatcherTimer timerAutoClose;
        private int sumSeconds = 0;
        private readonly IInputElement oldFocusedControl = null;
        private readonly bool isOptionSetting = false;

        [SupportedOSPlatform("windows")]
        public NotificationListBox(Window owner, string caption, string instructionHeading, IEnumerable<string> instructionSource, int autoCloseDialogTime)
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ShowInTaskbar = false;
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = false;

            this.isOptionSetting = false;

            WeakEventManager<Window, MouseButtonEventArgs>.AddHandler(this, "MouseLeftButtonDown", this.WindowsMouseLeftButtonDown);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, KeyEventArgs>.AddHandler(this, "KeyDown", this.OnKeyDown);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button1, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button2, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button3, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button1, "LostFocus", this.OnTimerStopLostFocus);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button2, "LostFocus", this.OnTimerStopLostFocus);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button3, "LostFocus", this.OnTimerStopLostFocus);

            if (cultureInfo.TwoLetterISOLanguageName.ToUpper() == "DE")
            {
                isLanguageDE = true;
            }
            else
            {
                isLanguageDE = false;
            }


            if (owner != null)
            {
                this.Owner = owner;
                this.oldFocusedControl = FocusManager.GetFocusedElement(this.Owner);
            }
            else
            {
                this.Owner = Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsActive == true);
                if (this.Owner != null)
                {
                    this.oldFocusedControl = FocusManager.GetFocusedElement(this.Owner);
                }
                else
                {
                    Window current = Application.Current.MainWindow;
                    this.oldFocusedControl = FocusManager.GetFocusedElement(current);
                }
            }


            if (string.IsNullOrEmpty(caption) == false)
            {
                this.TbCaption.Text = caption;
            }
            else
            {
                this.TbCaption.Text = "Application";
            }

            if (string.IsNullOrEmpty(instructionHeading) == false)
            {
                this.TbInstructionHeading.Text = instructionHeading;
            }
            else
            {
                this.TbInstructionHeading.Text = string.Empty;
            }

            if (instructionSource != null)
            {
                this.LBInstructionSource.ItemsSource = instructionSource;
            }
            else
            {
                this.LBInstructionSource.ItemsSource = null;
            }

            if (instructionIconSource == null)
            {
                instructionIconSource = NotifiactionBoxIcons.Application;
            }

            if (autoCloseDialogTime > 0)
            {
                timerAutoClose = new DispatcherTimer();
                timerAutoClose.Interval = TimeSpan.FromSeconds(1);
                WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(timerAutoClose, "Tick", this.TimerAutoCloseTick);
                timerAutoClose.Tag = autoCloseDialogTime;
                timerAutoClose.Start();
            }
        }

        public NotificationListBox(NotificationBoxOption messageBoxOption)
        {
            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ShowInTaskbar = false;
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = messageBoxOption.Topmost;

            this.isOptionSetting = true;

            WeakEventManager<Window, MouseButtonEventArgs>.AddHandler(this, "MouseLeftButtonDown", this.WindowsMouseLeftButtonDown);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoadedCustom);
            WeakEventManager<Window, KeyEventArgs>.AddHandler(this, "KeyDown", this.OnKeyDown);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button1, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button2, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button3, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button1, "LostFocus", this.OnTimerStopLostFocus);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button2, "LostFocus", this.OnTimerStopLostFocus);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.Button3, "LostFocus", this.OnTimerStopLostFocus);

            if (cultureInfo.TwoLetterISOLanguageName.ToUpper() == "DE")
            {
                isLanguageDE = true;
            }
            else
            {
                isLanguageDE = false;
            }

            if (this.Width != messageBoxOption.DialogWidth)
            {
                if (messageBoxOption.DialogWidth > 450)
                {
                    this.Width = messageBoxOption.DialogWidth;
                }
            }

            if (this.TbInstructionHeading.FontSize != messageBoxOption.InstructionHeadingFontSize)
            {
                this.TbInstructionHeading.FontSize = messageBoxOption.InstructionHeadingFontSize;
            }

            if (messageBoxOption.Onwer != null)
            {
                this.Owner = messageBoxOption.Onwer;
                this.oldFocusedControl = FocusManager.GetFocusedElement(this.Owner);
            }
            else
            {
                this.Owner = Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsActive == true);
                if (this.Owner != null)
                {
                    this.oldFocusedControl = FocusManager.GetFocusedElement(this.Owner);
                }
                else
                {
                    Window current = Application.Current.MainWindow;
                    this.oldFocusedControl = FocusManager.GetFocusedElement(current);
                }
            }

            if (string.IsNullOrEmpty(messageBoxOption.Caption) == false)
            {
                this.TbCaption.Text = messageBoxOption.Caption;
            }
            else
            {
                this.TbCaption.Text = "Application";
            }

            if (string.IsNullOrEmpty(messageBoxOption.InstructionHeading) == false)
            {
                this.TbInstructionHeading.Text = messageBoxOption.InstructionHeading;
            }
            else
            {
                this.TbInstructionHeading.Text = string.Empty;
            }

            if (messageBoxOption.InstructionSource != null)
            {
                this.LBInstructionSource.ItemsSource = messageBoxOption.InstructionSource;
            }
            else
            {
                this.LBInstructionSource.ItemsSource = null;
            }

            if (instructionIconSource == null)
            {
                instructionIconSource = NotifiactionBoxIcons.Application;
            }

            this.Button1.Content = messageBoxOption.ButtonRight;
            this.Button2.Content = messageBoxOption.ButtonMiddle;
            this.Button3.Content = messageBoxOption.ButtonLeft;

            if (messageBoxOption.AutoCloseDialogTime > 0)
            {
                timerAutoClose = new DispatcherTimer();
                timerAutoClose.Interval = TimeSpan.FromSeconds(1);
                WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(timerAutoClose, "Tick", this.TimerAutoCloseTick);
                timerAutoClose.Tag = messageBoxOption.AutoCloseDialogTime;
                timerAutoClose.Start();
            }
        }

        private void OnTimerStopLostFocus(object sender, RoutedEventArgs e)
        {
            if (timerAutoClose != null)
            {
                timerAutoClose.Stop();
                timerAutoClose = null;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is NotificationListBox)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control && e.SystemKey == Key.F4)
                {
                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
                {
                    if (string.IsNullOrEmpty(this.TbInstructionHeading.Text) == false)
                    {
                        string clipboardText = $"{this.TbInstructionHeading.Text}";
                        Clipboard.SetText(clipboardText);
                    }

                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.B)
                {
                    if (this.LBInstructionSource.ItemsSource != null)
                    {

                        string clipboardText = string.Join("\n", this.LBInstructionSource.Items);
                        Clipboard.SetText(clipboardText);
                    }

                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
                {
                    StringBuilder msgText = new StringBuilder();
                    if (string.IsNullOrEmpty(this.TbInstructionHeading.Text) == false && this.LBInstructionSource.ItemsSource != null)
                    {
                        msgText.AppendLine(this.TbCaption.Text);
                        msgText.AppendLine("-".Repeat(30));
                        msgText.AppendLine("[1]" + this.TbInstructionHeading.Text);
                        msgText.AppendLine("[2]" + string.Join("\n", this.LBInstructionSource.Items));
                        msgText.AppendLine("-".Repeat(30));

                        if (this.Button1.Visibility == Visibility.Visible)
                        {
                            msgText.Append(this.Button1.Content.ToString());
                        }

                        if (this.Button2.Visibility == Visibility.Visible)
                        {
                            msgText.Append(" - ");
                            msgText.Append(this.Button2.Content.ToString());
                        }

                        if (this.Button3.Visibility == Visibility.Visible)
                        {
                            msgText.Append(" - ");
                            msgText.Append(this.Button3.Content.ToString());
                        }

                        Clipboard.SetText(msgText.ToString());
                    }
                    else if (string.IsNullOrEmpty(this.TbInstructionHeading.Text) == false)
                    {
                        string clipboardText = $"{this.TbCaption}\n{this.TbInstructionHeading.Text}";
                        Clipboard.SetText(clipboardText);
                    }
                    else if (this.LBInstructionSource.Items != null)
                    {
                        string clipboardText = string.Join("\n", this.LBInstructionSource.Items);
                        Clipboard.SetText(clipboardText);
                    }

                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.P)
                {
                    using (ScreenCapturerTo screenCapturer = new ScreenCapturerTo())
                    {
                        screenCapturer.Capture();
                    }

                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.W)
                {
                    string clipboardText = $"{this.Owner.ToString()}";
                    Clipboard.SetText(clipboardText);

                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.Escape)
                {
                    if (this.isOptionSetting == false)
                    {
                        if (customMessageBoxButton.In(MessageBoxButton.YesNo, MessageBoxButton.OKCancel, MessageBoxButton.YesNoCancel) == true)
                        {
                            customDialogResult = NotificationResult.No;
                        }
                        else
                        {
                            customDialogResult = NotificationResult.Ok;
                        }
                    }
                    else
                    {
                        if (customMessageBoxButton.In(MessageBoxButton.YesNo, MessageBoxButton.OKCancel, MessageBoxButton.YesNoCancel) == true)
                        {
                            customDialogResult = NotificationResult.ButtonRight;
                        }
                        else
                        {
                            customDialogResult = NotificationResult.Ok;
                        }
                    }

                    this.DialogResult = true;
                    e.Handled = true;
                    this.Close();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Button1.Visibility = Visibility.Collapsed;
            this.Button1.Tag = NotificationResult.None;
            this.Button2.Visibility = Visibility.Collapsed;
            this.Button2.Tag = NotificationResult.None;
            this.Button3.Visibility = Visibility.Collapsed;
            this.Button3.Tag = NotificationResult.None;

            this.ImgInstructionIcon.Source = instructionIconSource;

            this.CreateNormalButton();
        }

        private void OnLoadedCustom(object sender, RoutedEventArgs e)
        {
            this.Button1.Visibility = Visibility.Collapsed;
            this.Button1.Tag = NotificationResult.None;
            this.Button2.Visibility = Visibility.Collapsed;
            this.Button2.Tag = NotificationResult.None;
            this.Button3.Visibility = Visibility.Collapsed;
            this.Button3.Tag = NotificationResult.None;

            this.ImgInstructionIcon.Source = instructionIconSource;

            this.CreateCustomButton();
        }

        private void TimerAutoCloseTick(object sender, EventArgs e)
        {
            DispatcherTimer dt = (DispatcherTimer)sender;
            this.sumSeconds++;

            if (this.sumSeconds > (int)dt.Tag)
            {
                timerAutoClose.Stop();
                this.DialogResult = false;
            }

            if (this.Button1.IsDefault == true)
            {
                string btnCaption = this.Button1.Content.ToString();
                if (btnCaption.EndsWith(")") == false)
                {
                    this.Button1.Content = $"{btnCaption} ({this.sumSeconds})";
                }
                else
                {
                    int pos = btnCaption.IndexOf("(");
                    this.Button1.Content = $"{btnCaption.Substring(0, pos - 1)} ({this.sumSeconds})";
                }
            }
            else if (this.Button2.IsDefault == true)
            {
                string btnCaption = this.Button2.Content.ToString();
                if (btnCaption.EndsWith(")") == false)
                {
                    this.Button2.Content = $"{btnCaption} ({this.sumSeconds})";
                }
                else
                {
                    int pos = btnCaption.IndexOf("(");
                    this.Button2.Content = $"{btnCaption.Substring(0,pos-1)} ({this.sumSeconds})";
                }
            }
            else if (this.Button3.IsDefault == true)
            {
                string btnCaption = this.Button3.Content.ToString();
                if (btnCaption.EndsWith(")") == false)
                {
                    this.Button3.Content = $"{btnCaption} ({this.sumSeconds})";
                }
                else
                {
                    int pos = btnCaption.IndexOf("(");
                    this.Button3.Content = $"{btnCaption.Substring(0, pos - 1)} ({this.sumSeconds})";
                }
            }
        }

        private void CreateNormalButton()
        {
            switch (customMessageBoxButton)
            {
                case MessageBoxButton.OK:
                    this.Button1.Visibility = Visibility.Visible;
                    this.Button1.Content = "Ok";
                    this.Button1.Tag = NotificationResult.Ok;
                    this.Button1.IsDefault = true;
                    this.Button1.Focus();
                    break;

                case MessageBoxButton.OKCancel:
                    this.Button1.Visibility = Visibility.Visible;
                    this.Button1.Content = isLanguageDE == true ? "Abbruch" : "Cancel";
                    this.Button1.Tag = NotificationResult.Cancel;

                    this.Button2.Visibility = Visibility.Visible;
                    this.Button2.Content = "Ok";
                    this.Button2.Tag = NotificationResult.Ok;

                    if (customDialogResult == NotificationResult.Cancel)
                    {
                        this.Button1.IsDefault = true;
                        this.Button1.Focus();
                    }
                    else if (customDialogResult == NotificationResult.Ok)
                    {
                        this.Button2.IsDefault = true;
                        this.Button2.Focus();
                    }

                    break;

                case MessageBoxButton.YesNo:
                    this.Button1.Visibility = Visibility.Visible;
                    this.Button1.Content = isLanguageDE == true ? "Nein" : "No";
                    this.Button1.Tag = NotificationResult.No;

                    this.Button2.Visibility = Visibility.Visible;
                    this.Button2.Content = isLanguageDE == true ? "Ja" : "Yes";
                    this.Button2.Tag = NotificationResult.Yes;

                    if (customDialogResult == NotificationResult.No)
                    {
                        this.Button1.IsDefault = true;
                        this.Button1.Focus();
                    }
                    else if (customDialogResult == NotificationResult.Yes)
                    {
                        this.Button2.IsDefault = true;
                        this.Button2.Focus();
                    }

                    break;

                case MessageBoxButton.YesNoCancel:
                    Button1.Visibility = Visibility.Visible;
                    this.Button1.Content = isLanguageDE == true ? "Abbruch" : "Cancel";
                    Button1.Tag = NotificationResult.Cancel;

                    Button2.Visibility = Visibility.Visible;
                    Button2.Content = isLanguageDE == true ? "Nein" : "No";
                    Button2.Tag = NotificationResult.No;

                    Button3.Visibility = Visibility.Visible;
                    Button3.Content = isLanguageDE == true ? "Ja" : "Yes";
                    Button3.Tag = NotificationResult.Yes;

                    if (customDialogResult == NotificationResult.Cancel)
                    {
                        this.Button1.IsDefault = true;
                        this.Button1.Focus();
                    }
                    else if (customDialogResult == NotificationResult.No)
                    {
                        this.Button2.IsDefault = true;
                        this.Button2.Focus();
                    }
                    else if (customDialogResult == NotificationResult.Yes)
                    {
                        this.Button3.IsDefault = true;
                        this.Button3.Focus();
                    }

                    break;
            }
        }

        private void CreateCustomButton()
        {
            switch (customMessageBoxButton)
            {
                case MessageBoxButton.OK:
                    this.Button1.Visibility = Visibility.Visible;
                    this.Button1.Tag = NotificationResult.ButtonRight;
                    this.Button1.IsDefault = true;
                    this.Button1.Focus();
                    break;

                case MessageBoxButton.OKCancel:
                    this.Button1.Visibility = Visibility.Visible;
                    this.Button1.Tag = NotificationResult.ButtonRight;

                    this.Button2.Visibility = Visibility.Visible;
                    this.Button2.Tag = NotificationResult.ButtonMiddle;

                    if (customDialogResult == NotificationResult.ButtonRight)
                    {
                        this.Button1.IsDefault = true;
                        this.Button1.Focus();
                    }
                    else if (customDialogResult == NotificationResult.ButtonMiddle)
                    {
                        this.Button2.IsDefault = true;
                        this.Button2.Focus();
                    }

                    break;

                case MessageBoxButton.YesNo:
                    this.Button1.Visibility = Visibility.Visible;
                    this.Button1.Tag = NotificationResult.ButtonRight;

                    this.Button2.Visibility = Visibility.Visible;
                    this.Button2.Tag = NotificationResult.ButtonMiddle;

                    if (customDialogResult == NotificationResult.ButtonRight)
                    {
                        this.Button1.IsDefault = true;
                        this.Button1.Focus();
                    }
                    else if (customDialogResult == NotificationResult.ButtonMiddle)
                    {
                        this.Button2.IsDefault = true;
                        this.Button2.Focus();
                    }

                    break;

                case MessageBoxButton.YesNoCancel:
                    Button1.Visibility = Visibility.Visible;
                    Button1.Tag = NotificationResult.ButtonRight;

                    Button2.Visibility = Visibility.Visible;
                    Button2.Tag = NotificationResult.ButtonMiddle;

                    Button3.Visibility = Visibility.Visible;
                    Button3.Tag = NotificationResult.ButtonLeft;

                    if (customDialogResult == NotificationResult.ButtonRight)
                    {
                        this.Button1.IsDefault = true;
                        this.Button1.Focus();
                    }
                    else if (customDialogResult == NotificationResult.ButtonMiddle)
                    {
                        this.Button2.IsDefault = true;
                        this.Button2.Focus();
                    }
                    else if (customDialogResult == NotificationResult.ButtonLeft)
                    {
                        this.Button3.IsDefault = true;
                        this.Button3.Focus();
                    }

                    break;
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Tag != null)
            {
                customDialogResult = (NotificationResult)Enum.Parse(typeof(NotificationResult), btn.Tag.ToString(), true);
                this.DialogResult = true;
            }
            else
            {
                customDialogResult = NotificationResult.None;
                this.DialogResult = true;
            }
        }

        public static NotificationResult Show(NotificationBoxOption messageBoxOption)
        {
            customMessageBoxButton = messageBoxOption.MessageBoxButton;
            if (messageBoxOption.InstructionIcon == NotificationIcon.None)
            {
                instructionIconSource = messageBoxOption.Icon;
            }
            else
            {
                instructionIconSource = NotifiactionBoxIcons.GetIcon(messageBoxOption.InstructionIcon);
            }

            customDialogResult = messageBoxOption.NotificationResult;
            Create(null, messageBoxOption.Caption, messageBoxOption.InstructionHeading, messageBoxOption.InstructionSource, messageBoxOption.AutoCloseDialogTime).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string instructionHeading)
        {
            customMessageBoxButton = MessageBoxButton.OK;
            instructionIconSource = NotifiactionBoxIcons.Application;
            Create(null, string.Empty, instructionHeading, null, 0).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading)
        {
            customMessageBoxButton = MessageBoxButton.OK;
            instructionIconSource = NotifiactionBoxIcons.Application;
            Create(null, caption, instructionHeading, null, 0).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading, IEnumerable<string> instructionSource, MessageBoxButton messageBoxButton)
        {
            customMessageBoxButton = messageBoxButton;
            instructionIconSource = NotifiactionBoxIcons.Application;
            Create(null, caption, instructionHeading, instructionSource, 0).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading, IEnumerable<string> instructionSource, MessageBoxButton messageBoxButton, NotificationIcon instructionIcon)
        {
            customMessageBoxButton = messageBoxButton;
            instructionIconSource = NotifiactionBoxIcons.GetIcon(instructionIcon);
            Create(null, caption, instructionHeading, instructionSource, 0).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading, IEnumerable<string> instructionSource, MessageBoxButton messageBoxButton, NotificationIcon instructionIcon, NotificationResult dialogResult)
        {
            customMessageBoxButton = messageBoxButton;
            instructionIconSource = NotifiactionBoxIcons.GetIcon(instructionIcon);
            customDialogResult = dialogResult;
            Create(null, caption, instructionHeading, instructionSource, 0).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading, IEnumerable<string> instructionSource, int autoCloseDialogTime = 0)
        {
            customMessageBoxButton = MessageBoxButton.OK;
            Create(null, caption, instructionHeading, instructionSource, autoCloseDialogTime).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading, IEnumerable<string> instructionSource, MessageBoxButton messageBoxButton, int autoCloseDialogTime = 0)
        {
            customMessageBoxButton = messageBoxButton;
            Create(null, caption, instructionHeading, instructionSource, autoCloseDialogTime).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult Show(string caption, string instructionHeading, IEnumerable<string> instructionSource, MessageBoxButton messageBoxButton, NotificationResult dialogResult, int autoCloseDialogTime = 0)
        {
            customMessageBoxButton = messageBoxButton;
            customDialogResult = dialogResult;
            Create(null, caption, instructionHeading, instructionSource, autoCloseDialogTime).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult ShowWithOwner(Window owner, string caption, string instructionHeading, IEnumerable<string> instructionSource, MessageBoxButton messageBoxButton, NotificationIcon instructionIcon, NotificationResult dialogResult)
        {
            customMessageBoxButton = messageBoxButton;
            instructionIconSource = NotifiactionBoxIcons.GetIcon(instructionIcon);
            customDialogResult = dialogResult;
            Create(owner, caption, instructionHeading, instructionSource, 0).ShowDialog();
            return customDialogResult;
        }

        public static NotificationResult ShowCustom(NotificationBoxOption messageBoxOption)
        {
            customMessageBoxButton = messageBoxOption.MessageBoxButton;
            if (messageBoxOption.InstructionIcon == NotificationIcon.None)
            {
                instructionIconSource = messageBoxOption.Icon;
            }
            else
            {
                instructionIconSource = NotifiactionBoxIcons.GetIcon(messageBoxOption.InstructionIcon);
            }

            customDialogResult = messageBoxOption.NotificationResult;
            CreateCustom(messageBoxOption).ShowDialog();
            return customDialogResult;
        }

        private static NotificationListBox Create(Window owner, string caption, string instructionHeading, IEnumerable<string> instructionSource, int autoCloseDialogTime)
        {
            return new NotificationListBox(owner, caption, instructionHeading, instructionSource, autoCloseDialogTime);
        }

        private static NotificationListBox CreateCustom(NotificationBoxOption messageBoxOption)
        {
            return new NotificationListBox(messageBoxOption);
        }

        private void WindowsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (timerAutoClose != null)
            {
                timerAutoClose.Stop();
                timerAutoClose = null;
            }

            if (this.oldFocusedControl != null)
            {
                Action focusAction = () => this.oldFocusedControl.Focus();
                this.Dispatcher.BeginInvoke(focusAction, DispatcherPriority.Normal);
            }

            WeakEventManager<Window, MouseButtonEventArgs>.RemoveHandler(this, "MouseLeftButtonDown", this.WindowsMouseLeftButtonDown);
            WeakEventManager<Window, CancelEventArgs>.RemoveHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Window, RoutedEventArgs>.RemoveHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, KeyEventArgs>.RemoveHandler(this, "KeyDown", this.OnKeyDown);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this.Button1, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this.Button2, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this.Button3, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this.Button1, "LostFocus", this.OnTimerStopLostFocus);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this.Button2, "LostFocus", this.OnTimerStopLostFocus);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this.Button3, "LostFocus", this.OnTimerStopLostFocus);
        }

    }
}