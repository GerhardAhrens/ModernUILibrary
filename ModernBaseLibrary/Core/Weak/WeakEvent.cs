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
 * Implementation of the WeakEvent Pattern
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
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Runtime.CompilerServices;

    public class WeakEvent<TEventHandler>
    {
        private readonly List<WeakDelegate<TEventHandler>> _handlers;

        public WeakEvent()
        {
            _handlers = new List<WeakDelegate<TEventHandler>>();
        }

        public virtual void AddHandler(TEventHandler handler)
        {
            Delegate d = (Delegate)(object)handler;
            _handlers.Add(new WeakDelegate<TEventHandler>(d));
        }

        public virtual void RemoveHandler(TEventHandler handler)
        {
            // also remove "dead" (garbage collected) handlers
            _handlers.RemoveAll(wd => !wd.IsAlive || wd.Equals(handler));
        }

        public virtual void Raise(object sender, EventArgs e)
        {
            var handlers = _handlers.ToArray();
            foreach (var weakDelegate in handlers)
            {
                if (weakDelegate.IsAlive)
                {
                    weakDelegate.Invoke(sender, e);
                }
                else
                {
                    _handlers.Remove(weakDelegate);
                }
            }
        }

        protected List<WeakDelegate<TEventHandler>> Handlers
        {
            get { return _handlers; }
        }
    }

    public sealed class WeakEventEx<TEventArgs>
    {
        private ImmutableList<WeakReference<EventHandler<TEventArgs>>> _listeners = ImmutableList<WeakReference<EventHandler<TEventArgs>>>.Empty;

        // Keep the delegates alive with their target. This prevent anonymous delegates from being garbage collected prematurely.
        private readonly ConditionalWeakTable<object, List<object>> _delegateKeepAlive = new();

        public void AddHandler(EventHandler<TEventArgs> handler)
        {
            if (handler == null)
                return;

            var weakReference = new WeakReference<EventHandler<TEventArgs>>(handler);
            _listeners = _listeners.Add(weakReference);
            if (handler.Target != null)
            {
                _delegateKeepAlive.GetOrCreateValue(handler.Target).Add(handler);
            }
        }

        public void RemoveHandler(EventHandler<TEventArgs> handler)
        {
            if (handler == null)
                return;

            // Remove the handler and all handlers that have been garbage collected
            _listeners = _listeners.RemoveAll(wr => !wr.TryGetTarget(out var target) || handler.Equals(target));
            if (handler.Target != null && _delegateKeepAlive.TryGetValue(handler.Target, out var weakReference))
            {
                weakReference.Remove(handler);
            }
        }

        public void Raise(object sender, TEventArgs args)
        {
            foreach (var listener in _listeners)
            {
                if (listener.TryGetTarget(out var target))
                {
                    target.Invoke(sender, args);
                }
                else
                {
                    // Remove the listener if the target has been garbage collected
                    _listeners = _listeners.Remove(listener);
                }
            }
        }
    }
}
