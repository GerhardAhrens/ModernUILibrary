//-----------------------------------------------------------------------
// <copyright file="BindableBase.cs" company="Lifeprojects.de">
//     Class: BindableBase
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>06.11.2023 15:19:08</date>
//
// <summary>
// Klasse zum Binden eines Property im XAML Code an ein Control
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class XamlPropertyBase : INotifyPropertyChanged
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
