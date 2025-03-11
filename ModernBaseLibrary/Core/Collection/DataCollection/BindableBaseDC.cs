namespace ModernBaseLibrary.Collection
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class BindableBaseDC : INotifyPropertyChanged, IBaseModelDC
    {
        protected bool IsNotifyProperty { get; set; }
        public bool IsModified { get; set; }

        private Dictionary<string, object> internalProperties = new Dictionary<string, object>();

        protected T Get<T>([CallerMemberName] string name = null)
        {
            object value = null;
            if (internalProperties.TryGetValue(name, out value))
            {
                return value == null ? default(T) : (T)value;
            }

            return default(T);
        }

        protected void Set<T>(T value, [CallerMemberName] string name = null)
        {
            if (Equals(value, Get<T>(name)))
            {
                return;
            }

            if (internalProperties.ContainsKey(name) == false)
            {
                this.IsModified = false;
                internalProperties[name] = value;

                if (this.IsNotifyProperty == true)
                {
                    this.OnPropertyChanged(name);
                }
            }
            else
            {
                if (internalProperties[name].Equals(value) == true)
                {
                    this.IsModified = false;
                }
                else
                {
                    this.IsModified = true;
                    internalProperties[name] = value;

                    if (this.IsNotifyProperty == true)
                    {
                        this.OnPropertyChanged(name);
                    }
                }
            }

        }

        public void ResetModified()
        {
            this.IsModified = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}