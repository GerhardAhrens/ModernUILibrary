/*
 * <copyright file="EventObserver.cs" company="PTA GmbH">
 *     Class: EventObserver
 *     Copyright © PTA GmbH 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - PTA GmbH</author>
 * <email>gerhard.ahrens@pta.de</email>
 * <date>15.10.2022</date>
 * <Project>ModernBaseLibrary</Project>
 *
 * <summary>
 * Implementierung der Klasse EventObserver
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

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// The subscriptions.
        /// </summary>
        private static readonly IDictionary<Type, IList> subscriptions = new Dictionary<Type, IList>();

        public IDictionary<Type, IList> Subscriptions { get { return subscriptions; } }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="payload">The payload.</param>
        public void Publish<TPayload>(TPayload payload) where TPayload : IEventAggregatorArgs
        {
            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }

            Type messageType = typeof(TPayload);

            if (subscriptions.ContainsKey(messageType))
            {
                var subscriptionList
                    = new List<ISubscription<TPayload>>(subscriptions[messageType].Cast<ISubscription<TPayload>>());

                foreach (var subscription in subscriptionList)
                {
                    subscription.Action(payload);
                }
            }
        }

        public bool ExistKey<TPayload>() where TPayload : IEventAggregatorArgs
        {
            bool result = false;
            Type messageType = typeof(TPayload);

            if (subscriptions.ContainsKey(messageType) == true)
            {
                result = true;
            }

            return result;
        }

        public int Count()
        {
            int result = 0;
            if (subscriptions != null && subscriptions.Any() == true)
            {
                result = subscriptions.Count;
            }

            return result;
        }

        public int CountValues<TPayload>() where TPayload : IEventAggregatorArgs
        {
            int result = 0;
            Type messageType = typeof(TPayload);

            if (subscriptions.ContainsKey(messageType) == true)
            {
                result = subscriptions[messageType].Count;
            }

            return result;
        }

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The subscription.</returns>
        public ISubscription<TPayload> Subscribe<TPayload>(Action<TPayload> action) where TPayload : IEventAggregatorArgs
        {
            Type messageType = typeof(TPayload);

            var subscription = new Subscription<TPayload>(this, action);

            if (subscriptions.ContainsKey(messageType))
            {
                subscriptions[messageType].Add(subscription);
            }
            else
            {
                subscriptions.Add(messageType, new List<ISubscription<TPayload>> { subscription });
            }

            return subscription;
        }

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="subscription">The subscription.</param>
        public void UnSubscribe<TPayload>(ISubscription<TPayload> subscription) where TPayload : IEventAggregatorArgs
        {
            Type messageType = typeof(TPayload);

            if (subscriptions.ContainsKey(messageType))
            {
                subscriptions.Remove(messageType);
            }
        }

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="action">The action.</param>
        public void UnSubscribe<TPayload>(Action<TPayload> action) where TPayload : IEventAggregatorArgs
        {
            Type messageType = typeof(TPayload);

            if (subscriptions.ContainsKey(messageType))
            {
                subscriptions.Remove(messageType);
            }
        }

        public void UnSubscribe<TPayload>() where TPayload : IEventAggregatorArgs
        {
            Type messageType = typeof(TPayload);

            if (subscriptions.ContainsKey(messageType))
            {
                subscriptions.Remove(messageType);
            }
        }

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        public void ClearAllSubscriptions()
        {
            this.ClearAllSubscriptions(null);
        }

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        /// <param name="excepTPayloads">The except messages.</param>
        public void ClearAllSubscriptions(Type[] excepTPayloads)
        {
            foreach (var messageSubscriptions in new Dictionary<Type, IList>(subscriptions))
            {
                bool canDelete = true;
                if (excepTPayloads != null)
                {
                    canDelete = !excepTPayloads.Contains(messageSubscriptions.Key);
                }

                if (canDelete)
                {
                    subscriptions.Remove(messageSubscriptions);
                }
            }
        }
    }
}