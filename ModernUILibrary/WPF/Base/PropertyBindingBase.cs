//-----------------------------------------------------------------------
// <copyright file="PropertyBindingBase.cs" company="Lifeprojects.de">
//     Class: PropertyBindingBase
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>20.12.2024</date>
//
// <summary>
// Klasse zum Binden eines Property im XAML Code an ein Control
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class PropertyBindingBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}
