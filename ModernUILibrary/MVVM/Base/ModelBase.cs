//-----------------------------------------------------------------------
// <copyright file="ModelBase.cs" company="Lifeprojects.de">
//     Class: ModelBase
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 13:29:40</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUILibrary.MVVM.Base
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Core.LINQ;

    //[DebuggerStepThrough]
    [Serializable]
    public abstract class ModelBase<TModel> : IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ConcurrentDictionary<string, object> originalValues = new ConcurrentDictionary<string, object>();
        private readonly ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
        private bool classIsDisposed = false;
        private bool isPropertyChanged = false;
        private readonly string className = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        ~ModelBase()
        {
            this.Dispose(false);
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

        public T ToClone<T>()
        {
            return (T)this.MemberwiseClone();
        }

        public override string ToString()
        {
            try
            {
                PropertyInfo[] propInfo = this.GetType().GetProperties();
                StringBuilder outText = new StringBuilder();
                outText.AppendFormat("{0} ", this.GetType().Name);
                foreach (PropertyInfo propItem in propInfo)
                {
                    outText.AppendFormat("{0}:{1};", propItem.Name, propItem.GetValue(this, null));
                }

                return outText.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Compare<TModel>(TModel object1, TModel object2)
        {
            Type type = typeof(TModel);

            if (object1 == null || object2 == null)
            {
                return false;
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string object1Value = string.Empty;
                    string object2Value = string.Empty;

                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                    {
                        object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    }

                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                    {
                        object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    }

                    if (object1Value.Trim() != object2Value.Trim())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int CalculateHash()
        {
            int result = 0;
            var hash = new HashCode();

            try
            {
                PropertyInfo[] propInfo = this.GetType().GetProperties();
                foreach (PropertyInfo propItem in propInfo)
                {
                    hash.Add(propItem.GetValue(this, null));
                }

                result = hash.ToHashCode();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public int CalculateHash<T>(params Expression<Func<T, object>>[] expressions)
        {
            int result = 0;
            HashCode hash = new HashCode();
            Type type = typeof(T);

            try
            {
                foreach (var property in expressions)
                {
                    string propertyName = ExpressionPropertyName.For<T>(property);
                    object propertyValue = type.GetProperty(propertyName).GetValue(this);
                    if (propertyValue != null)
                    {
                        hash.Add(propertyValue);
                    }
                }

                result = hash.ToHashCode();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
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
                AddOriginalValues(propertyName, value);
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

        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                }
            }

            this.classIsDisposed = true;
        }
        #endregion Dispose

        #region Property Tracking
        public int CountChanges { get { return this.originalValues.Count(); } }

        public ConcurrentDictionary<string, object> OriginalValues { get { return this.originalValues; } }

        public bool IsChanged { get; private set; }

        public void AddOriginalValues(string key, object originalValue)
        {
            if (this.originalValues.ContainsKey(key) == false)
            {
                this.originalValues[key] = originalValue;
                this.IsChanged = true;
            }
        }

        public void ResetChanged()
        {
            this.originalValues.Clear();
            this.IsChanged = false;
        }

        #endregion Property Tracking
    }
}
