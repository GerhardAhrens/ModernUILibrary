//-----------------------------------------------------------------------
// <copyright file="IFocusMover.cs" company="Lifeprojects.de">
//     Class: IFocusMover
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.02.2023</date>
//
// <summary>
// Interface Klasse für zur Focussteuerung zwischen Controls
// </summary>
//-----------------------------------------------------------------------


namespace ModernIU.WPF.Base
{
    using System;

    /// <summary>
    /// Implemented by a ViewModel that needs to control
    /// where input focus is in a View.
    /// </summary>
    public interface IFocusMover
    {
        /// <summary>
        /// Raised when the input focus should move to 
        /// a control whose 'active' dependency property 
        /// is bound to the specified property.
        /// </summary>
        event EventHandler<MoveFocusEventArgs> MoveFocus;
    }

    public class MoveFocusEventArgs : EventArgs
    {
        public MoveFocusEventArgs(string focusedProperty)
        {
            this.FocusedProperty = focusedProperty;
        }

        public string FocusedProperty { get; private set; }
    }
}