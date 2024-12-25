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

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Documents;

    public static class UIElementExtensions
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
    }
}
