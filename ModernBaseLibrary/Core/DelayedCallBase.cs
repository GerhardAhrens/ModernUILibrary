﻿//-----------------------------------------------------------------------
// <copyright file="DelayedCallBase.cs" company="Lifeprojects.de">
//     Class: DelayedCallBase
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2017</date>
//
// <summary>Definition of DelayedCall Base Class</summary>
//  <auto-generated />
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class DelayedCallBase : IDisposable
    {
        protected static List<DelayedCallBase> instances = new List<DelayedCallBase>();
        protected System.Timers.Timer timer;
        protected object timerLock;
        protected bool isCancelled;
        protected SynchronizationContext context;
        private bool isDisposed;
        private Callback callback;


        protected DelayedCallBase()
        {
            this.timerLock = new object();
        }

        ~DelayedCallBase()
        {
            this.Dispose(false);
        }

        public delegate void Callback();

        public static bool SupportsSynchronization
        {
            get
            {
                return SynchronizationContext.Current != null;
            }
        }

        public static int RegisteredCount
        {
            get
            {
                lock (instances)
                {
                    return instances.Count;
                }
            }
        }

        public static bool IsAnyWaiting
        {
            get
            {
                lock (instances)
                {
                    foreach (DelayedCallBase dc in instances)
                    {
                        if (dc.IsWaiting)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public int Milliseconds
        {
            get
            {
                lock (this.timerLock)
                {
                    return (int)this.timer.Interval;
                }
            }

            set
            {
                lock (this.timerLock)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", "The new timeout must be 0 or greater.");
                    }
                    else if (value == 0)
                    {
                        this.Cancel();
                        this.FireNow();
                        Unregister(this);
                    }
                    else
                    {
                        this.timer.Interval = value;
                    }
                }
            }
        }

        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        public bool IsWaiting
        {
            get
            {
                lock (this.timerLock)
                {
                    return this.timer.Enabled && !this.isCancelled;
                }
            }
        }

        public static DelayedCallBase Create(Callback cb, int milliseconds)
        {
            DelayedCallBase dc = new DelayedCallBase();
            PrepareDCObject(dc, milliseconds, false);
            dc.callback = cb;
            return dc;
        }

        public static DelayedCallBase CreateAsync(Callback cb, int milliseconds)
        {
            DelayedCallBase dc = new DelayedCallBase();
            PrepareDCObject(dc, milliseconds, true);
            dc.callback = cb;
            return dc;
        }

        public static DelayedCallBase Start(Callback cb, int milliseconds)
        {
            DelayedCallBase dc = Create(cb, milliseconds);
            if (milliseconds > 0)
            {
                dc.Start();
            }
            else if (milliseconds == 0)
            {
                dc.FireNow();
            }

            return dc;
        }

        public static DelayedCallBase StartAsync(Callback cb, int milliseconds)
        {
            DelayedCallBase dc = CreateAsync(cb, milliseconds);
            if (milliseconds > 0)
            {
                dc.Start();
            }
            else if (milliseconds == 0)
            {
                dc.FireNow();
            }

            return dc;
        }

        public static void CancelAll()
        {
            lock (instances)
            {
                foreach (DelayedCallBase dc in instances)
                {
                    dc.Cancel();
                }
            }
        }

        public static void FireAll()
        {
            lock (instances)
            {
                foreach (DelayedCallBase dc in instances)
                {
                    dc.Fire();
                }
            }
        }

        public static void DisposeAll()
        {
            lock (instances)
            {
                while (instances.Count > 0)
                {
                    instances[0].Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            lock (this.timerLock)
            {
                this.isCancelled = false;
                this.timer.Start();
                Register(this);
            }
        }

        public void Cancel()
        {
            lock (this.timerLock)
            {
                this.isCancelled = true;
                Unregister(this);
                this.timer.Stop();
            }
        }

        public void Fire()
        {
            lock (this.timerLock)
            {
                if (this.IsWaiting == false)
                {
                    return;
                }

                this.timer.Stop();
            }

            this.FireNow();
        }

        public void FireNow()
        {
            this.isCancelled = false;
            this.OnFire();
            Unregister(this);
        }

        public void Reset()
        {
            lock (this.timerLock)
            {
                this.Cancel();
                this.Start();
            }
        }

        public void Reset(int milliseconds)
        {
            lock (this.timerLock)
            {
                this.Cancel();
                this.Milliseconds = milliseconds;
                this.Start();
            }
        }

        protected static void PrepareDCObject(DelayedCallBase dc, int milliseconds, bool async)
        {
            if (milliseconds < 0)
            {
                throw new ArgumentOutOfRangeException("milliseconds", "The new timeout must be 0 or greater.");
            }

            dc.context = null;
            if (async == false)
            {
                dc.context = SynchronizationContext.Current;
                if (dc.context == null)
                {
                    throw new InvalidOperationException("Cannot delay calls synchronously on a non-UI thread. Use the *Async methods instead.");
                }
            }

            if (dc.context == null)
            {
                dc.context = new SynchronizationContext();
            }

            dc.timer = new System.Timers.Timer();
            if (milliseconds > 0)
            {
                dc.timer.Interval = milliseconds;
            }

            dc.timer.AutoReset = false;
            dc.timer.Elapsed += dc.Timer_Elapsed;

            Register(dc);
        }

        protected static void Register(DelayedCallBase dc)
        {
            lock (instances)
            {
                if (instances.Contains(dc) == false)
                {
                    instances.Add(dc);
                }
            }
        }

        protected static void Unregister(DelayedCallBase dc)
        {
            lock (instances)
            {
                instances.Remove(dc);
            }
        }

        protected virtual void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.OnFire();
            Unregister(this);
        }

        protected virtual void OnFire()
        {
            this.context.Post(
                delegate
                {
                    lock (this.timerLock)
                    {
                        if (this.isCancelled == true)
                        {
                            return;
                        }

                        this.isCancelled = true;
                    }

                    if (this.callback != null)
                    {
                        this.callback();
                    }
                },
                null);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed == false)
            {
                if (disposing)
                {
                    this.Cancel();
                    this.timer.Dispose();
                }

                this.isDisposed = true;
            }
        }
    }
}
