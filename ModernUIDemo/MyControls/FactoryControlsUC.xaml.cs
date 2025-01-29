namespace ModernUIDemo.MyControls
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Core;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für FactoryControlsUC.xaml
    /// </summary>
    public partial class FactoryControlsUC : UserControl, INotifyPropertyChanged
    {
        public FactoryControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.EM_A, "Click", this.OnClick_EMA);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.EM_B, "Click", this.OnClick_EMB);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.EM_C, "Click", this.OnClick_EMC);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.EM_D, "Click", this.OnClick_EMD);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.EM_E, "Click", this.OnClick_EME);
        }

        private void OnClick_EMA(object sender, RoutedEventArgs e)
        {
            ExecuteMethodVoid(ParameterTyp.Eins);
        }

        private void OnClick_EMB(object sender, RoutedEventArgs e)
        {
            bool r1 = ExecuteMethodWithReturn<bool>(ParameterTyp.Eins, 1);
        }

        private void OnClick_EMC(object sender, RoutedEventArgs e)
        {
            bool r2 = ExecuteMethodWithReturn<bool>(ParameterTyp.Zwei, 2);
        }

        private void OnClick_EMD(object sender, RoutedEventArgs e)
        {
            bool result = ExecuteMethodWithReturn<bool>(ParameterTyp.ExecuteWithReturn, "ExecuteWithReturn-1");
        }

        private void OnClick_EME(object sender, RoutedEventArgs e)
        {
            bool result = ExecuteMethodWithReturn<bool>(ParameterTyp.ExecuteParamsWithReturn, "ExecuteParamsWithReturn-1", "ExecuteParamsWithReturn-2");
        }

        private static void ExecuteMethodVoid(Enum commandParam)
        {
            Type className = MethodBase.GetCurrentMethod().DeclaringType;

            var methodes = new CommandsHandler();
            MethodInfo myMethod = ExecuteHandlerFactory.ExecuteMethod<CommandsHandler>(commandParam.ToString());
            myMethod.Invoke(methodes, new object[] { className.Name, commandParam.ToString() });
        }

        private static void ExecuteMethodVoid(object commandParam)
        {
            Type className = MethodBase.GetCurrentMethod().DeclaringType;

            var methodes = new CommandsHandler();
            MethodInfo myMethod = ExecuteHandlerFactory.ExecuteMethod<CommandsHandler>(commandParam.ToString());
            myMethod.Invoke(methodes, new object[] { className.Name, commandParam });
        }

        private static TResult ExecuteMethodWithReturn<TResult>(Enum commandParam, object args)
        {
            Type className = MethodBase.GetCurrentMethod().DeclaringType;

            var methodes = new CommandsHandler();
            MethodInfo myMethod = ExecuteHandlerFactory.ExecuteMethod<CommandsHandler>(commandParam.ToString());
            var result = myMethod.Invoke(methodes, new object[] { className.Name, args });

            return (TResult)result;
        }

        private static TResult ExecuteMethodWithReturn<TResult>(Enum commandParam,params object[] args)
        {
            Type className = MethodBase.GetCurrentMethod().DeclaringType;

            var methodes = new CommandsHandler();
            MethodInfo myMethod = ExecuteHandlerFactory.ExecuteMethod<CommandsHandler>(commandParam.ToString());
            var result = myMethod.Invoke(methodes, new object[] { className.Name, args });

            return (TResult)result;
        }

        private static TResult ExecuteMethodWithReturn<TResult>(string commandParam, object args)
        {
            Type className = MethodBase.GetCurrentMethod().DeclaringType;

            var methodes = new CommandsHandler();
            MethodInfo myMethod = ExecuteHandlerFactory.ExecuteMethod<CommandsHandler>(commandParam.ToString());
            var result = myMethod.Invoke(methodes, new object[] { className.Name, commandParam, args });

            return (TResult)result;
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
    }

    public enum ParameterTyp : int
    {
        None = 0,
        Eins = 1,
        Zwei = 2,
        ExecuteWithReturn = 3,
        ExecuteParamsWithReturn =4,
    }

    public class CommandsHandler
    {
        [ExecuteMethodeHandler("Eins")]
        public bool EinsHandler(string sender, object param)
        {
            MessageBox.Show($"Nachricht von '{sender}', Parameter '{param}'.");

            return true;
        }

        [ExecuteMethodeHandler("Zwei")]
        public bool ZweiHandler(string sender, object param)
        {
            MessageBox.Show($"Nachricht von '{sender}', Parameter '{param}'.");

            return true;
        }

        [ExecuteMethodeHandler("ExecuteWithReturn")]
        public bool ApplicationHelpHandler(string sender, string param)
        {
            MessageBox.Show($"Nachricht von '{sender}', Parameter '{param}'.");

            return true;
        }

        [ExecuteMethodeHandler("ExecuteParamsWithReturn")]
        public bool ApplicationParamsHandler(string sender, params object[] param)
        {
            MessageBox.Show($"Nachricht von '{sender}', Parameter '{string.Join(";",param)}'.");

            return true;
        }

        [ExecuteMethodeHandler("ExecuteWithoutReturn")]
        public void ApplicationInfoHandler(string sender, string param)
        {
            MessageBox.Show($"Nachricht von '{sender}', Parameter '{param}'.");
        }
    }
}
