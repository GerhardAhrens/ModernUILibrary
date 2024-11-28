namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Input;

    using ModernIU.Base;

    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    /// <summary>
    /// Interaktionslogik für PasswordGeneratorView.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class PasswordGeneratorView : Window, INotifyPropertyChanged
    {
        public ICommand PasswordGeneratorCommand => new UIButtonCommand(this.GeneratePasswordHandler);
        public ICommand UsedFolderCommand => new UIButtonCommand(p => this.OnUsedFolderHandle(p));
        public ICommand CancelButtonCommand => new UIButtonCommand(p => this.OnCancelButtonClick());
        public ICommand OkButtonCommand => new UIButtonCommand(p => this.OnOKButtonClick(), p2 => this.CheckButtonStatus());

        private Dictionary<int, string> letterTypSource = null;
        private int letterTypSelected = 0;
        private int passwordLength = 0;
        private bool setNumbers = false;
        private bool setSpecialChars = false;
        private string passwordResultSelected = null;
        private ObservableCollection<string> passwordResultSource = null;
        private string generatorText = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public PasswordGeneratorView()
        {
            this.InitializeComponent();
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public ObservableCollection<string> PasswordResultSource
        {
            get { return this.passwordResultSource; }
            set 
            { 
                if (this.passwordResultSource != value)
                {
                    this.passwordResultSource = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string PasswordResultSelected
        {
            get { return this.passwordResultSelected; }
            set
            { 
                if (this.passwordResultSelected != value)
                {
                    this.passwordResultSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Dictionary<int, string> LetterTypSource
        {
            get { return this.letterTypSource; }
            set
            {
                if (this.letterTypSource != value)
                {
                    this.letterTypSource = value;
                    this.OnPropertyChanged();
                }
            }
        }


        public int LetterTypSelected
        {
            get { return this.letterTypSelected; }
            set 
            {
                if (this.letterTypSelected != value)
                {
                    this.letterTypSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool SetNumbers
        {
            get { return this.setNumbers; }
            set 
            { 
                if (this.setNumbers != value)
                {
                    this.setNumbers = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool SetSpecialChars
        {
            get { return this.setSpecialChars; }
            set 
            {
                if (this.setSpecialChars != value)
                {
                    this.setSpecialChars = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int PasswordLength
        {
            get { return this.passwordLength; }
            set 
            { 
                if (this.passwordLength != value)
                {
                    this.passwordLength = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string GeneratorText
        {
            get { return this.generatorText; }
            set 
            { 
                if (this.generatorText != value)
                {
                    this.generatorText = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.LetterTypSource = this.GenerateLetterTyp();
            this.GeneratorText = "Kein Password";
            this.PasswordLength = 10;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
        }

        private void OnCancelButtonClick()
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OnOKButtonClick()
        {
            this.DialogResult = true;
            this.Close();
        }

        private bool CheckButtonStatus()
        {
            if (this.PasswordResultSource == null || this.PasswordResultSource.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [SupportedOSPlatform("windows")]
        private void GeneratePasswordHandler(object args)
        {
            IEnumerable<string> pwdList = null;
            using (PasswordGenerator pw = new PasswordGenerator())
            {
                pw.PasswordLength = this.PasswordLength;
                pwdList = pw.CreatePwdCollection(this.LetterTypSelected, this.PasswordLength, this.SetNumbers, this.SetSpecialChars);
            }

            this.PasswordResultSource = new ObservableCollection<string>(pwdList);
        }

        private void OnUsedFolderHandle(object p1)
        {
            this.DialogResult = true;
            this.Close();
        }

        public static PasswordGeneratorResult Execute(Window owner, string headerText = null)
        {
            return ExecuteInternal(owner, headerText);
        }

        public static PasswordGeneratorResult Execute(string headerText = null)
        {
            Window actualWindow = Application.Current.Windows.Cast<Window>().Last(l => l.IsActive == true);

            if (headerText != null)
            {
                headerText = "Passwortgenerator";
            }

            return ExecuteInternal(actualWindow, headerText);
        }

        public PasswordGeneratorResult Execute()
        {
            PasswordGeneratorResult result = null;
            PasswordGeneratorEventArgs args = null;
            bool? resultDialog = false;

            try
            {
                resultDialog = this.ShowDialog();
                args = new PasswordGeneratorEventArgs();
                if (resultDialog == true)
                {
                    args.Cancel = false;
                    args.Result = this.PasswordResultSelected;
                }
                else
                {
                    args.Cancel = true;
                    args.Result = string.Empty;
                }
            }
            catch (Exception ex)
            {
                args.Error = ex;
            }
            finally
            {
                result = new PasswordGeneratorResult(args);
            }

            return result;
        }

        internal static PasswordGeneratorResult ExecuteInternal(Window owner, string headerText)
        {
            PasswordGeneratorView dialog = new PasswordGeneratorView();
            dialog.Owner = owner;

            if (headerText != null)
            {
                dialog.txtHeaderText.Text = headerText;
            }
            else
            {
                dialog.txtHeaderText.Text = "Passwortgenerator";
            }

            return dialog.Execute();
        }

        private Dictionary<int, string> GenerateLetterTyp()
        {
            return new Dictionary<int, string>() { { 0, "Keine" }, { 1, "Groß- und Klein" }, { 2, "Klein" }, { 3, "Groß" } }; ;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}
