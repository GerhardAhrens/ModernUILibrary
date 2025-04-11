namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für ExceptionControlsUC.xaml
    /// </summary>
    public partial class ExceptionControlsUC : UserControl, INotifyPropertyChanged
    {
        public ExceptionControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

        private void btnExceptionA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> errors = null;
                errors.Clear();
            }
            catch (Exception ex)
            {
                ExceptionView.Show(ex);
            }
        }

        private void btnExceptionB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExceptionTest et = new ExceptionTest();
                et.MethodeA();
            }
            catch (Exception ex)
            {
                ex.Data.Add("Handler", "btnExceptionB_Click");
                ExceptionView.Show(ex);
            }
        }
    }

    public class ExceptionTest
    {
        private List<string> errors = null;

        public ExceptionTest() { }

        public void MethodeA()
        {
            try
            {
                errors.Clear();
            }
            catch (Exception ex)
            {
                ex.Data.Add("Methode", "MethodeA");
                string errorText = ex.Message;
                throw;
            }
        }
    }
}
