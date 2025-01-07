/*
 * <copyright file="WeakEvent.cs" company="Lifeprojects.de">
 *     Class: WeakEvent
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>20.08.2018</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Implementation of the WeakPropertyChange Pattern
 * </summary>
 * 
 * <WebLink>
 *    http://agsmith.wordpress.com/2008/04/07/propertydescriptor-addvaluechanged-alternative/
 * </WebLink>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Core
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;

    public class WeakPropertyChange : DependencyObject, IDisposable
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
        typeof(object), typeof(WeakPropertyChange), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        private readonly WeakReference _propertySource;

        public WeakPropertyChange(DependencyObject propertySource, string path) : this(propertySource, new PropertyPath(path))
        {
        }

        public WeakPropertyChange(DependencyObject propertySource, DependencyProperty property) : this(propertySource, new PropertyPath(property))
        {
        }

        public WeakPropertyChange(DependencyObject propertySource, PropertyPath property)
        {
            if (null == propertySource)
            {
                throw new ArgumentNullException("propertySource");
            }

            if (null == property)
            {
                throw new ArgumentNullException("property");
            }

            this._propertySource = new WeakReference(propertySource);

            Binding binding = new Binding();
            binding.Path = property;
            binding.Mode = BindingMode.OneWay;
            binding.Source = propertySource;
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        public event EventHandler ValueChanged;

        public DependencyObject PropertySource
        {
            get
            {
                try
                {
                    // note, it is possible that accessing the target property
                    // will result in an exception so i’ve wrapped this check
                    // in a try catch
                    return this._propertySource.IsAlive
                    ? this._propertySource.Target as DependencyObject
                    : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns/sets the value of the property
        /// </summary>
        /// <seealso cref="ValueProperty"/>
        [Description("Returns/sets the value of the property")]
        [Category("Behavior")]
        [Bindable(true)]
        public object Value
        {
            get
            {
                return (object)this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WeakPropertyChange notifier = (WeakPropertyChange)d;
            if (null != notifier.ValueChanged)
                notifier.ValueChanged(notifier, EventArgs.Empty);
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ValueProperty);
        }
    }
}
