/*
 * <copyright file="UserControlBase.cs" company="Lifeprojects.de">
 *     Class: UserControlBase
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
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Core;

    [DebuggerStepThrough]
    [Serializable]
    [SupportedOSPlatform("windows")]
    public class UserControlBase : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ExpandoObject viewState = new ExpandoObject();
        private readonly string className;
        private readonly ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
        private bool isPropertyChanged = false;
        private int rowPosition = 0;
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        public Dictionary<string, Func<Result<string>>> ValidationRules = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlBase"/> class.
        /// </summary>
        public UserControlBase()
        {
            this.className = this.GetType().Name;
            this.FontFamily = new System.Windows.Media.FontFamily("Tahoma");
            this.FontWeight = FontWeights.Normal;
            this.ValidationRules = new Dictionary<string, Func<Result<string>>>();
        }

        public UserControlBase(Type inheritsType)
        {
            this.BaseType = inheritsType;
            this.className = this.GetType().Name;
            this.FontFamily = new System.Windows.Media.FontFamily("Tahoma");
            this.FontWeight = FontWeights.Normal;
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
            get { return this.isPropertyChanged; }
            set
            {
                this.isPropertyChanged = value;
                this.SetProperty(ref isPropertyChanged, value);
            }
        }

        public int RowPosition
        {
            get { return this.rowPosition; }
            set
            {
                this.rowPosition = value;
                this.SetProperty(ref rowPosition, value);
            }
        }

        public Type BaseType { get; set; }

        public bool IsUCLoaded { get; set; } = false;

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        public EventAggregator EventAgg { get; } = new EventAggregator();

        public int DisplayRowCount { get; set; }
        #endregion Properties

        public virtual void InitCommands() { }

        public virtual void ChangedContent(bool isPropertyChanged = false)
        {
            this.IsPropertyChanged = isPropertyChanged;
        }

        #region Validation

        public HashSet<string> GetProperties(UserControl userControl)
        {
            HashSet<string> propertyNames = new HashSet<string>();

            propertyNames = userControl.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(s => s.Name).ToHashSet();

            return propertyNames;
        }

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

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod(int level = 1)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(level);

            return sf.GetMethod().Name;
        }
    }
}
