//-----------------------------------------------------------------------
// <copyright file="EventHandlerExtensions.cs" company="Lifeprojects.de">
//     Class: EventHandlerExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.01.2020</date>
//
// <summary>EventHandlerExtensions Definition</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// An EventHandler extension method that raises the event event.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        public static void Raise(this EventHandler @this, object sender)
        {
            if (@this != null)
            {
                @this(sender, null);
            }
        }

        public static void Raise(this EventHandler @this, object sender, EventArgs e)
        {
            if (@this != null)
            {
                @this(sender, e);
            }
        }

        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> @this, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (@this != null)
            {
                @this(sender, e);
            }
        }

        /// <summary>
        /// An EventHandler&lt;TEventArgs&gt; extension method that raises the event event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the event arguments.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> @this, object sender) where TEventArgs : EventArgs
        {
            if (@this != null)
            {
                @this(sender, Activator.CreateInstance<TEventArgs>());
            }
        }
    }
}
