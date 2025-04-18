/*
 * <copyright file="WindowBase.cs" company="Lifeprojects.de">
 *     Class: WindowBase
 *     Copyright � Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>18.12.2024 19:17:53</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernUI.MVVM.Base
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Media;

    using ModernBaseLibrary.Core;

    using ModernIU.Controls;

    [DebuggerStepThrough]
    [Serializable]
    [SupportedOSPlatform("windows")]
    public class WindowBase : MWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ExpandoObject viewState = new ExpandoObject();
        private readonly string className;
        private readonly ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
        private bool _IsPropertyChanged = false;
        private int _RowPosition = 0;
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        public Dictionary<string, Func<Result<string>>> ValidationRules = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBase"/> class.
        /// </summary>
        public WindowBase()
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                this.ShowInTaskbar = true;
                this.WindowStartupLocation = WindowStartupLocation.Manual;
            }
            else
            {
                this.ShowInTaskbar = false;
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            this.FontFamily = new FontFamily("Tahoma");
            this.FontWeight = FontWeights.Medium;
            this.className = this.GetType().Name;
            this.ValidationRules = new Dictionary<string, Func<Result<string>>>();
        }

        public WindowBase(Type inheritsType)
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                this.ShowInTaskbar = true;
                this.WindowStartupLocation = WindowStartupLocation.Manual;
            }
            else
            {
                this.ShowInTaskbar = false;
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            this.BaseType = inheritsType;
            this.FontFamily = new FontFamily("Tahoma");
            this.FontWeight = FontWeights.Medium;
            this.className = this.GetType().Name;
            this.ValidationRules = new Dictionary<string, Func<Result<string>>>();
        }

        #region Properties
        public dynamic ViewState
        {
            get { return this.viewState; }
        }

        public Dictionary<string, object> ToDictionary
        {
            get { return this.values.ToDictionary(k => k.Key, v => v.Value); }
        }

        public bool IsPropertyChanged
        {
            get { return this._IsPropertyChanged; }
            set
            {
                this._IsPropertyChanged = value;
                this.SetProperty(ref _IsPropertyChanged, value);
            }
        }

        public int RowPosition
        {
            get { return this._RowPosition; }
            set
            {
                this._RowPosition = value;
                this.SetProperty(ref _RowPosition, value);
            }
        }

        public Type BaseType { get; set; }

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        public EventAggregator EventAgg { get; } = new EventAggregator();

        public int DisplayRowCount { get; set; }
        #endregion Properties

        public virtual void InitCommands() { }

        public virtual void OnViewIsClosing(CancelEventArgs eventArgs) { }

        public virtual void ChangedContent(bool isPropertyChanged = false)
        {
            this.IsPropertyChanged = isPropertyChanged;
        }

        #region Validation
        protected Result<string> DoValidation(Func<Result<string>> validationFunc, string propName)
        {
            Result<string> result = validationFunc.Invoke();

            if (errors.ContainsKey(propName) == true)
            {
                errors.Remove(propName);
            }
            else
            {
                if (string.IsNullOrEmpty(result.SuccessMessage) == false)
                {
                    errors[propName] = result.SuccessMessage;
                }
            }

            return result;
        }
        #endregion Validation

        #region Get/Set Implementierung
        private T GetPropertyValueInternal<T>(string propertyName)
        {
            if (values.ContainsKey(propertyName) == false)
            {
                values[propertyName] = default;
            }

            var value = values[propertyName];
            return value == null ? default : (T)value;
        }

        protected T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            var rightsKey = $"{this.className}.{propertyName}";

            return this.GetPropertyValueInternal<T>(propertyName);
        }

        protected void SetValueUnchecked<T>(T value, [CallerMemberName] string propertyName = "")
        {
            if (this.values.ContainsKey(propertyName) == true)
            {
                this.values[propertyName] = value;
            }
            else
            {
                this.values.TryAdd(propertyName, value);
            }
        }

        protected void SetValue<T>(T value, Func<T, string, bool> preAction, Action<T, string> postAction, [CallerMemberName] string propertyName = "")
        {
            if (preAction?.Invoke(value, propertyName) == true)
            {
                this.SetValue(value, propertyName);
            }

            if (postAction != null)
            {
                postAction?.Invoke(value, propertyName);
            }
        }

        protected void SetValue<T>(T value, Action<T, string> postAction, [CallerMemberName] string propertyName = "")
        {
            this.SetValue(value, propertyName);
            if (postAction != null)
            {
                postAction?.Invoke(value, propertyName);
            }
        }

        protected void SetValue<T>(T value, [CallerMemberName] string propertyName = "")
        {
            bool changed = !object.Equals(value, this.GetPropertyValueInternal<T>(propertyName));
            if (changed == true)
            {
                this.IsPropertyChanged = true;
                var rightsKey = $"{this.className}.{propertyName}";
                this.values[propertyName] = value;
                this.OnPropertyChanged(propertyName);
            }
        }
        #endregion Get/Set Implementierung

        #region INotifyPropertyChanged Implementierung
        protected void SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string property = "")
        {
            if (object.Equals(oldValue, newValue))
            {
                return;
            }

            oldValue = newValue;
            this.OnPropertyChanged(property);
        }

        protected virtual void OnPropertyChanged(string property)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion INotifyPropertyChanged Implementierung

        #region ViewState Implementierung
        protected void ClearViewState()
        {
            ((IDictionary<string, object>)this.ViewState).Clear();
        }

        public virtual void AddViewState(string key, object value)
        {
            if (((IDictionary<string, object>)this.ViewState).ContainsKey(key.ToLower()) == false)
            {
                ((IDictionary<string, object>)this.ViewState).Add(key.ToLower(), value);
            }
        }

        protected object GetViewState(string key)
        {
            if (this.viewState.Count(x => x.Key.ToLower() == key.ToLower()) > 0)
            {
                return this.viewState.Single(x => x.Key.ToLower() == key.ToLower()).Value;
            }
            else
            {
                return null;
            }
        }

        #endregion ViewState Implementierung

        public void CenterWindow(Window mainWindow, double factor = 300)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = mainWindow.Width + factor;
            double windowHeight = mainWindow.Height + (factor/ 2);
            mainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            mainWindow.Top = (screenHeight / 2) - (windowHeight / 2);
            mainWindow.Width = windowWidth;
            mainWindow.Height = windowHeight;

            if ((screenWidth - mainWindow.Width) <= factor)
            {
                mainWindow.Width = (screenWidth - 200);
                mainWindow.Left = (screenWidth / 2) - (mainWindow.Width / 2);
            }

            if ((screenHeight - mainWindow.Height) <= factor)
            {
                mainWindow.Height = (screenHeight - 100);
                mainWindow.Top = (screenHeight / 2) - (mainWindow.Height / 2);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod(int level = 1)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(level);

            return sf.GetMethod().Name;
        }
    }
}
