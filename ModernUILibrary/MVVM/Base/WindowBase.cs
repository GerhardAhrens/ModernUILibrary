/*
 * <copyright file="WindowBase.cs" company="Lifeprojects.de">
 *     Class: WindowBase
 *     Copyright © Lifeprojects.de 2024
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

    using Microsoft.Xaml.Behaviors;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBase"/> class.
        /// </summary>
        public WindowBase()
        {
            this.FontFamily = new FontFamily("Tahoma");
            this.FontWeight = FontWeights.Medium;
            this.className = this.GetType().Name;
        }

        public WindowBase(Type inheritsType)
        {
            this.BaseType = inheritsType;
            this.FontFamily = new FontFamily("Tahoma");
            this.FontWeight = FontWeights.Medium;
            this.className = this.GetType().Name;
        }

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

        public Type BaseType { get; set; }

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        public EventAggregator EventAgg { get; } = new EventAggregator();

        public int DisplayRowCount { get; set; }

        public virtual void InitCommands() { }

        public virtual void OnViewIsClosing(CancelEventArgs eventArgs)
        {
        }

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
