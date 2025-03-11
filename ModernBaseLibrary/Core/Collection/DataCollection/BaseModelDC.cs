namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerStepThrough]
    [Serializable]
    public class BaseModelDC : IBaseModelDC, INotifyPropertyChanged
    {

        public bool IsModified { get; set; }

        protected void SetProperty<TValue>(ref TValue member, TValue newValue, IEqualityComparer<TValue> equalityComparer, [CallerMemberName] string propertyName = "", PropertyChangedEventHandler eventHandler = null)
        {
            bool changed = !equalityComparer.Equals(member, newValue);
            if (changed == true)
            {
                member = newValue;

                if (eventHandler != null)
                {
                    eventHandler(member, new PropertyChangedEventArgs(propertyName));
                }

                this.IsModified = true;
            }
        }

        protected void SetProperty<TValue>(ref TValue member, TValue newValue, [CallerMemberName] string propertyName = "", PropertyChangedEventHandler eventHandler = null)
        {
            SetProperty(ref member, newValue, EqualityComparer<TValue>.Default, propertyName, eventHandler);
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