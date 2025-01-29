//-----------------------------------------------------------------------
// <copyright file="FocusBinding.cs" company="Lifeprojects.de">
//     Class: FocusBinding
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.02.2023</date>
//
// <summary>
// Klasse zur Focussteuerung zwischen Controls
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System;
    using System.Windows;

    public class FocusBinding : BindingDecoratorBase
    {
        public override object ProvideValue(IServiceProvider provider)
        {
            DependencyObject elem;
            DependencyProperty prop;
            if (base.TryGetTargetItems(provider, out elem, out prop))
            {
                FocusController.SetFocusableProperty(elem, prop);
            }

            return base.ProvideValue(provider);
        }
    }
}