//-----------------------------------------------------------------------
// <copyright file="UIElementExtensions.cs" company="Lifeprojects.de">
//     Class: UIElementExtensions
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.01.2021</date>
//
// <summary>Extenstion Class for UIElement Definition</summary>
//-----------------------------------------------------------------------

namespace ModernIU.BehaviorsBase
{
    using System;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Documents;

    [SupportedOSPlatform("windows")]
    public static partial class UIElementExtensions
    {
        public static Adorner GetOrAddAdorner(this UIElement uIElement,Type type) {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(uIElement);
            if (adornerLayer == null)
            {
                throw new Exception("VisualParents Must have AdornerDecorator!");
            }

            var adorner = adornerLayer.GetAdorners(uIElement)?.FirstOrDefault(x => x?.GetType() == type);

            if (adorner == null)
            {
                lock (uIElement)
                {
                    if (adorner == null)
                    {
                        adorner = (Adorner)Activator.CreateInstance(type, new object[] { uIElement });
                        adornerLayer.Add(adorner);
                    }
                }
            }

            return adorner;
        }

        public static Adorner GetAdorner(this UIElement uIElement, Type type)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(uIElement);
            if (adornerLayer == null)
            {
                return null;
            }

            return adornerLayer.GetAdorners(uIElement)?.FirstOrDefault(x => x?.GetType() == type);
        }

        /// <summary>
        ///     Sets the value of the <paramref name="property" /> only if it hasn't been explicitely set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool SetIfDefault<T>(this DependencyObject o, DependencyProperty property, T value)
        {
            if (!property.PropertyType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Type of dependency property is incompatible with value.");
            }

            if (DependencyPropertyHelper.GetValueSource(o, property).BaseValueSource == BaseValueSource.Default)
            {
                o.SetValue(property, value);

                return true;
            }

            return false;
        }
    }
}
