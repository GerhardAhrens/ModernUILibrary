namespace ModernUIDemo.MyControls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    using ModernIU.Controls;
    using ModernIU.WPF.Base;

    /// <summary>
    /// Interaktionslogik für HotKeyControlsUC.xaml
    /// </summary>
    public partial class HotKeyControlsUC : UserControl, INotifyPropertyChanged, IFocusMover
    {
        public HotKeyControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private HotKeyHost HotKeys { get; set; }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set 
            { 
                firstName = value; 
                this.OnPropertyChanged();
            }
        }

        private int age;

        public int Age
        {
            get { return age; }
            set { age = value; }
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.HotKeys = new HotKeyHost((HwndSource)HwndSource.FromVisual(App.Current.MainWindow));
            this.HotKeys.AddHotKey(new HotKeyToMessageBox("ShowMessageBox", Key.C, ModifierKeys.Alt, "Show MessageBox"));

            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnHotkey, "Click", this.OnHotkeyClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnMoveFocusF, "Click", this.OnMoveToFHandler);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnMoveFocusA, "Click", this.OnMoveToAHandler);

            this.txtFirstName.CaretBrush = (this.txtFirstName.CaretBrush == Brushes.Red) ? Brushes.Blue : Brushes.Red;
        }

        private void OnHotkeyClick(object sender, RoutedEventArgs e)
        {
            List<string> hotKeys = this.HotKeys.HotkeysToList(" - ");
            string hotkeyList = hotKeys.ToStringAll<string>();

            MessageBox.Show(hotkeyList, "Liste Hotkeys");
        }

        private void OnMoveToFHandler(object sender, RoutedEventArgs e)
        {
            this.RaiseMoveFocus("FirstName");
        }

        private void OnMoveToAHandler(object sender, RoutedEventArgs e)
        {
            this.RaiseMoveFocus("Age");
        }

        private void cbContent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                ListBox lb = e.OriginalSource as ListBox;
                object li = lb.SelectedItem;
                object val = lb.SelectedValue;
                //this.tbSelectetItem.Text = li.ToString();
            }
        }

        #region IFocusMover Members

        public event EventHandler<MoveFocusEventArgs> MoveFocusUC;

        private void RaiseMoveFocus(string focusedProperty)
        {
            var handler = this.MoveFocusUC;
            if (handler != null)
            {
                var args = new MoveFocusEventArgs(focusedProperty);
                handler(this, args);
            }
        }

        #endregion

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

    public class LookupExtension : BindingDecoratorBase
    {
        //A key which will trigger a lookup when pressed.
        public Key LookupKey { get; set; }

        /// <summary>
        /// This method is being invoked during initialization.
        /// </summary>
        /// <param name="provider">Provides access to the bound items.</param>
        /// <returns>The binding expression that is created by
        /// the base class.</returns>
        public override object ProvideValue(IServiceProvider provider)
        {
            //delegate binding creation etc. to the base class
            object provideValue = base.ProvideValue(provider);


            //try to get bound items for our custom work
            DependencyObject targetObject;
            DependencyProperty targetProperty;
            bool status = TryGetTargetItems(provider, out targetObject, out targetProperty);

            if (status == true)
            {
                //associate an input listener with the control
                if (LookupKey == Key.F5)
                {
                    InputHandler.RegisterHandler(targetObject, LookupKey);
                }
                else if (LookupKey == Key.F6)
                {
                    InputHandler2.RegisterHandler(targetObject, LookupKey);
                }
            }

            return provideValue;
        }
    }

    public class InputHandler
    {
        private readonly Key lookupKey;


        /// <summary>
        /// Creates a handler with a given input control and a lookup key.
        /// </summary>
        /// <param name="inputControl"></param>
        /// <param name="lookupKey"></param>
        private InputHandler(DependencyObject inputControl, Key lookupKey)
        {
            this.lookupKey = lookupKey;
            Keyboard.AddKeyUpHandler(inputControl, OnKeyUp);
        }



        //this is were the custom work would be made. for this sample, just
        //show a message box
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            string controlName = string.Empty;
            if (e.Key != lookupKey)
            {
                return;
            }

            if (sender.GetType().BaseType == typeof(TextBox))
            {
                controlName = string.IsNullOrEmpty(((TitleTextBox)sender).Name) ? sender.GetType().Name : ((TitleTextBox)sender).Name;
            }

            string msg = $"Lookup key '{lookupKey}' from Control '{controlName}' was pressed";
            MessageBox.Show(msg);

            if (sender is TextBox)
            {
                ((TextBox)sender).Text = "Gerhard";
            }
        }


        public static void RegisterHandler(DependencyObject inputControl, Key lookupKey)
        {
            InputHandler handler = new InputHandler(inputControl, lookupKey);
        }
    }

    public class InputHandler2
    {
        private readonly Key lookupKey;


        /// <summary>
        /// Creates a handler with a given input control and a lookup key.
        /// </summary>
        /// <param name="inputControl"></param>
        /// <param name="lookupKey"></param>
        private InputHandler2(DependencyObject inputControl, Key lookupKey)
        {
            this.lookupKey = lookupKey;
            Keyboard.AddKeyUpHandler(inputControl, OnKeyUp);
        }



        //this is were the custom work would be made. for this sample, just
        //show a message box
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            string controlName = string.Empty;
            if (e.Key != lookupKey)
            {
                return;
            }

            if (sender.GetType().BaseType == typeof(TextBox))
            {
                controlName = string.IsNullOrEmpty(((TitleTextBox)sender).Name) ? sender.GetType().Name : ((TitleTextBox)sender).Name;
            }

            string msg = $"Lookup key '{lookupKey}' from Control '{controlName}' was pressed";
            MessageBox.Show(msg);

            if (sender is TextBox)
            {
                ((TextBox)sender).Text = "Charlie";
            }
        }


        public static void RegisterHandler(DependencyObject inputControl, Key lookupKey)
        {
            InputHandler2 handler = new InputHandler2(inputControl, lookupKey);
        }
    }

    [TypeConverter(typeof(ModernBaseLibrary.Core.EnumDescriptionTypeConverter))]
    public enum StatusEnum
    {
        [Description("This is horrible")]
        Horrible,
        [Description("This is bad")]
        Bad,
        [Description("This is so so")]
        SoSo,
        [Description("Good")]
        Good,
        [Description("Better")]
        Better,
        [Description("Best")]
        Best
    }

    [TypeConverter(typeof(ModernBaseLibrary.Core.EnumDescriptionTypeConverter))]
    [Flags]
    public enum StatusType
    {
        [Description("Kunden")]
        Kunden = 0,
        [Description("Rechnungen")]
        Rechnungen = 1,
        [Description("Adressen")]
        Adressen = 2,
        [Description("Lieferanten")]
        Lieferanten = 4,
    }
}
