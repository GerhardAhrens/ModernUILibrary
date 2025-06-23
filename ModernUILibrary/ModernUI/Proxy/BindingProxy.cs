//-----------------------------------------------------------------------
// <copyright file="BindingProxy.cs" company="Lifeprojects.de">
//     Class: BindingProxy
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>14.07.2020</date>
//
// <summary>
// Die Klasse stelt einen Proxy zum Binding im XAML zur verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;

    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Data
        {
            get { return (object)this.GetValue(DataProperty); }
            set { this.SetValue(DataProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
