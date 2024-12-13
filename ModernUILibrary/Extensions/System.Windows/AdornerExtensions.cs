/*
 * <copyright file="AdornerExtensions.cs" company="Lifeprojects.de">
 *     Class: AdornerExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>31.03.2017</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Erweiterungsmethode ermöglich die Behandlung der Adorner Klasse
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

namespace ModernIU.BehaviorsBase
{
    using System.Windows;
    using System.Windows.Documents;

    public static class AdornerExtensions
    {
        public static void TryRemoveAdorners<T>(this UIElement @this) where T : System.Windows.Documents.Adorner
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(@this);
            if (adornerLayer != null)
            {
                adornerLayer.RemoveAdorners<T>(@this);
            }
        }

        public static void RemoveAdorners<T>(this AdornerLayer @this, UIElement elem)
            where T : System.Windows.Documents.Adorner
        {
            System.Windows.Documents.Adorner[] adorners = @this.GetAdorners(elem);

            if (adorners == null) return;

            for (int i = adorners.Length - 1; i >= 0; i--)
            {
                if (adorners[i] is T)
                {
                    if (adorners[i] != null)
                    {
                        @this.Remove(adorners[i]);
                    }
                }
            }
        }

        public static void TryAddAdorner<T>(this UIElement @this, System.Windows.Documents.Adorner adorner) where T : System.Windows.Documents.Adorner
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(@this);
            if (adornerLayer != null && !adornerLayer.ContainsAdorner<T>(@this))
            {
                if (adorner != null)
                {
                    adornerLayer.Add(adorner);
                }
            }
        }

        public static bool ContainsAdorner<T>(this AdornerLayer @this, UIElement elem)
            where T : System.Windows.Documents.Adorner
        {
            System.Windows.Documents.Adorner[] adorners = @this.GetAdorners(elem);

            if (adorners == null) return false;

            for (int i = adorners.Length - 1; i >= 0; i--)
            {
                if (adorners[i] is T)
                    return true;
            }
            return false;
        }

        public static void RemoveAllAdorners(this AdornerLayer @this, UIElement elem)
        {
            System.Windows.Documents.Adorner[] adorners = @this.GetAdorners(elem);

            if (adorners == null)
            {
                return;
            }

            foreach (var toRemove in adorners)
            {
                @this.Remove(toRemove);
            }
        }
    }
}
