//-----------------------------------------------------------------------
// <copyright file="UIHelper.cs" company="Lifeprojects.de">
//     Class: UIHelper
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>11.02.2025 10:47:18</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernInsideVM.Core
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class UIHelper
    {
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter or null if not found</returns> 
        public static T FindByName<T>(DependencyObject userControl, string controlName) where T : DependencyObject
        {
            T foundChild = null;
            if (userControl != null)
            {
                foundChild = ((UserControl)userControl).FindName(controlName) as T;
            }

            return (T)foundChild;
        }
    }
}
