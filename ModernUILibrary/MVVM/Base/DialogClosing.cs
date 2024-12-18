/*
 * <copyright file="ActionDialogClosing.cs" company="Lifeprojects.de">
 *     Class: ActionDialogClosing
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>31.03.2017</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Erweiterungsmethode ermöglich das Schließen eines Windows Dialog über ein ViewModel
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
    using System.ComponentModel;
    using System.Windows;


    public sealed class DialogClosing
    {
        public static readonly DependencyProperty IsClosingProperty =
            DependencyProperty.RegisterAttached("IsClosing", typeof(bool), typeof(DialogClosing), new PropertyMetadata(false, OnRegistration));

        public static bool GetIsClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsClosingProperty);
        }

        public static void SetIsClosing(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClosingProperty, value);
        }

        private static void OnRegistration(DependencyObject depObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            Window window = depObject as Window;
            if (window != null && GetIsClosing(depObject) == true)
            {
                window.Loaded += (s, e) => RegisterClosingAfterWindowIsLoaded(window);
            }
        }

        private static void RegisterClosingAfterWindowIsLoaded(Window window)
        {
            IDialogClosing vm = window.DataContext as IDialogClosing;
            if (vm != null)
            {
                window.Closing += (s, e) => vm.OnViewIsClosing(e);
                window.Closing += (s, e) => Unregister(window, vm);
            }
        }

        private static void Unregister(Window window, IDialogClosing vm)
        {
            window.Loaded -= (s, e) => RegisterClosingAfterWindowIsLoaded(window);
            window.Closing -= (s, e) => vm?.OnViewIsClosing(e);
            window.Closing -= (s, e) => Unregister(window, vm);
        }
    }

    public interface IDialogClosing
    {
        void OnViewIsClosing(CancelEventArgs eventArgs);
    }
}
