//-----------------------------------------------------------------------
// <copyright file="Cache.cs" company="Lifeprojects.de">
//     Class: Cache
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>25.07.2019</date>
//
// <summary>Definition of Simple Cache Class</summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public class Cache<K, T> : IDisposable
    {
        private readonly Dictionary<K, T> cache = new Dictionary<K, T>();
        private readonly Dictionary<K, Timer> timers = new Dictionary<K, Timer>();
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private bool disposed = false;

        public Cache()
        {
        }

        public T this[K key] => Get(key);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed == false)
            {
                disposed = true;

                if (disposing == true)
                {
                    Clear();
                    locker.Dispose();
                }
            }
        }

        public void Clear()
        {
            locker.EnterWriteLock();
            try
            {
                try
                {
                    foreach (Timer t in timers.Values)
                    {
                        t.Dispose();
                    }
                }
                catch
                { }

                timers.Clear();
                cache.Clear();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public int Count()
        {
            if (cache == null)
            {
                return -1;
            }

            return cache.Count;
        }

        public void AddOrUpdate(K key, T cacheObject, int cacheTimeout, bool restartTimerIfExists = false)
        {
            if (disposed == true)
            {
                return;
            }

            if (cacheTimeout != Timeout.Infinite && cacheTimeout < 1)
            {
                throw new ArgumentOutOfRangeException("cacheTimeout must be greater than zero.");
            }

            locker.EnterWriteLock();
            try
            {
                this.CheckTimer(key, cacheTimeout, restartTimerIfExists);

                if (!cache.ContainsKey(key))
                {
                    cache.Add(key, cacheObject);
                }
                else
                {
                    cache[key] = cacheObject;
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void AddOrUpdate(K key, T cacheObject)
        {
            this.AddOrUpdate(key, cacheObject, Timeout.Infinite);
        }

        public T Get(K key)
        {
            if (disposed)
            {
                return default(T);
            }

            locker.EnterReadLock();
            try
            {
                T rv;
                if (cache.TryGetValue(key, out rv) == true)
                {
                    return rv;
                }
                else
                {
                    return default(T);
                }
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public TResult Get<TResult>(K key)
        {
            if (disposed)
            {
                return default(TResult);
            }

            locker.EnterReadLock();
            try
            {

                T rv;
                if (cache.TryGetValue(key, out rv) == true)
                {
                    TResult result = rv == null ? default(TResult) : (TResult)Convert.ChangeType(rv, typeof(TResult), CultureInfo.InvariantCulture);
                    return result;
                }
                else
                {
                    return default(TResult);
                }
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public bool TryGet(K key, out T value)
        {
            if (disposed == true)
            {
                value = default(T);
                return false;
            }

            locker.EnterReadLock();
            try
            {
                return cache.TryGetValue(key, out value);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public void Remove(Predicate<K> keyPattern)
        {
            if (disposed == true)
            {
                return;
            }

            locker.EnterWriteLock();
            try
            {
                var removers = (from k in cache.Keys.Cast<K>()
                                where keyPattern(k)
                                select k).ToList();

                foreach (K workKey in removers)
                {
                    try { timers[workKey].Dispose(); }
                    catch { }
                    timers.Remove(workKey);
                    cache.Remove(workKey);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void Remove(K key)
        {
            if (disposed == true)
            {
                return;
            }

            locker.EnterWriteLock();
            try
            {
                if (cache.ContainsKey(key))
                {
                    try
                    {
                        timers[key].Dispose();
                    }
                    catch { }
                    timers.Remove(key);
                    cache.Remove(key);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public bool Exists(K key)
        {
            if (disposed)
            {
                return false;
            }

            locker.EnterReadLock();
            try
            {
                return cache.ContainsKey(key);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public IEnumerable<K> GetKeys()
        {
            if (cache == null)
            {
                return null;
            }

            return cache.Keys.AsEnumerable<K>();
        }

        public IEnumerable<string> GetTyps()
        {
            if (this.cache == null)
            {
                return null;
            }

            List<string> cacheResult = new List<string>();
            foreach (T item in this.cache.Values)
            {
                cacheResult.Add(typeof(T).ToFriendlyName());
            }

            return cacheResult.AsEnumerable();
        }

        private void CheckTimer(K key, int cacheTimeout, bool restartTimerIfExists)
        {
            Timer timer;

            if (timers.TryGetValue(key, out timer))
            {
                if (restartTimerIfExists)
                {
                    timer.Change((cacheTimeout == Timeout.Infinite ? Timeout.Infinite : cacheTimeout * 1000), Timeout.Infinite);
                }
            }
            else
            {
                timers.Add(
                    key,
                    new Timer(new TimerCallback(this.RemoveByTimer), key, (cacheTimeout == Timeout.Infinite ? Timeout.Infinite : cacheTimeout * 1000),
                        Timeout.Infinite));
            }
        }

        private void RemoveByTimer(object state)
        {
            Remove((K)state);
        }
    }

    public class Cache<T> : Cache<string, T>
    {
    }

    public class Cache : Cache<string, object>
    {
        private static Lazy<Cache> global = new Lazy<Cache>();

        public static Cache Global => global.Value;
    }
}