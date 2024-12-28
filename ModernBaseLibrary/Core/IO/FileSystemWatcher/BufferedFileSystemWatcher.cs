//-----------------------------------------------------------------------
// <copyright file="BufferedFileSystemWatcher.cs" company="Lifeprojects.de">
//     Class: BufferedFileSystemWatcher
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.06.2023</date>
//
// <summary>
// Class with BufferedFileSystemWatcher Definition
// </summary>
// <Website>
// https://decatec.de/programmierung/filesystemwatcher-events-werden-mehrfach-ausgeloest-loesungsansaetze/
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Timers;
    using System.Windows;

    /// <summary>
    /// Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.
    /// The events are not raised immediately, but are buffered so that events which are raised twice by the included FileSystemWatcher are only raised once.
    /// </summary>
    public class BufferedFileSystemWatcher : FileSystemWatcher
    {
        private const string StandardFilter = "*.*";
        private const double StandardBufferDelay = 400;

        private IList<string> changedList;
        private IList<string> createdList;
        private IList<string> deletedList;
        private IList<string> renamedList;

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class.
        /// </summary>
        public BufferedFileSystemWatcher() : this(string.Empty, StandardFilter, StandardBufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class.
        /// </summary>
        /// <param name="bufferedChangeTypes">The BufferedChangeTypes specifying which events should be buffered.</param>
        public BufferedFileSystemWatcher(BufferedChangeTypes bufferedChangeTypes) : this(string.Empty, StandardFilter, StandardBufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specifed buffer delay time.
        /// </summary>
        /// <param name="bufferDelay">The time in milliseconds the raised events should be buffered.</param>
        public BufferedFileSystemWatcher(long bufferDelay) : this(string.Empty, StandardFilter, bufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specifed buffer delay time.
        /// </summary>
        /// <param name="bufferDelay">The time in milliseconds the raised events should be buffered.</param>
        /// <param name="bufferedChangeTypes">The BufferedChangeTypes specifying which events should be buffered.</param>
        public BufferedFileSystemWatcher(long bufferDelay, BufferedChangeTypes bufferedChangeTypes) : this(string.Empty, StandardFilter, bufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        public BufferedFileSystemWatcher(string path) : this(path, StandardFilter, StandardBufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="bufferedChangeTypes">The BufferedChangeTypes specifying which events should be buffered.</param>
        public BufferedFileSystemWatcher(string path, BufferedChangeTypes bufferedChangeTypes) : this(path, StandardFilter, StandardBufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory to monitor and buffer delay time.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="bufferDelay">The time in milliseconds the raised events should be buffered.</param>
        public BufferedFileSystemWatcher(string path, long bufferDelay) : this(path, StandardFilter, bufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory and type of files to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        public BufferedFileSystemWatcher(string path, string filter) : this(path, filter, StandardBufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory and type of files to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        /// <param name="bufferedChangeTypes">The BufferedChangeTypes specifying which events should be buffered.</param>
        public BufferedFileSystemWatcher(string path, string filter, BufferedChangeTypes bufferedChangeTypes) : this(path, filter, StandardBufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory, type of files to monitor, buffer delay time and the types of events which should be handled delayed.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        /// <param name="bufferDelay">The time in milliseconds the raised events should be buffered.</param>
        /// <param name="bufferedChangeTypes">The BufferedChangeTypes specifying which events should be buffered.</param>
        public BufferedFileSystemWatcher(string path, string filter, double bufferDelay, BufferedChangeTypes bufferedChangeTypes) : base(path, filter)
        {
            this.changedList = new List<string>();
            this.createdList = new List<string>();
            this.deletedList = new List<string>();
            this.renamedList = new List<string>();

            this.Path = path;
            this.Filter = filter;
            this.BufferDelay = bufferDelay;
            this.BufferedChangeTypes = BufferedChangeTypes.All;

            base.Changed += this.BufferedFileSystemWatcher_Changed;
            base.Created += this.BufferedFileSystemWatcher_Created;
            base.Deleted += this.BufferedFileSystemWatcher_Deleted;
            base.Renamed += this.BufferedFileSystemWatcher_Renamed;
        }

        /// <summary>
        /// Occurs when a file or directory in the specified Path is changed.
        /// </summary>
        public new event FileSystemEventHandler Changed;

        /// <summary>
        /// Occurs when a file or directory in the specified Path is created.
        /// </summary>
        public new event FileSystemEventHandler Created;

        /// <summary>
        /// Occurs when a file or directory in the specified Path is deleted.
        /// </summary>
        public new event FileSystemEventHandler Deleted;

        /// <summary>
        /// Occurs when a file or directory in the specified Path is renamed.
        /// </summary>
        public new event FileSystemEventHandler Renamed;

        /// <summary>
        /// Gets or sets the buffer delay time in milliseconds, i.e. the time in which the events raised should be buffered.
        /// </summary>
        public double BufferDelay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the BufferedChangeTypes, i.e. the types of events which should be buffered by the BufferedFileSystemWatcher.
        /// If this property is set to DelayedChangeTypes.None, the BufferedFileSystemWatcher will behave like a normal FileSystemWatcher.
        /// </summary>
        public BufferedChangeTypes BufferedChangeTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Invoked whenever a file or directory in the specified Path is changed (buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual void OnBufferedChanged(FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;

            lock (this.changedList)
            {
                if (this.changedList.Contains(fullPath))
                {
                    return;
                }
                else
                {
                    this.changedList.Add(fullPath);
                }
            }

            var timer = new Timer(this.BufferDelay) { AutoReset = false };

            timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
            {
                FileSystemEventHandler tmp = this.Changed;

                if (tmp != null)
                {
                    tmp(this, e);
                }

                lock (this.changedList)
                {
                    this.changedList.Remove(fullPath);
                }
            };

            timer.Start();
        }

        /// <summary>
        ///  Invoked whenever a file or directory in the specified Path is changed (not buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual new void OnChanged(FileSystemEventArgs e)
        {
            FileSystemEventHandler tmp = this.Changed;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        /// <summary>
        /// Inviked whenever a file or directory in the specified Path is created (buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual void OnBufferedCreated(FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;

            lock (this.createdList)
            {
                if (this.createdList.Contains(fullPath))
                {
                    return;
                }
                else
                {
                    this.createdList.Add(fullPath);
                }
            }

            var timer = new Timer(this.BufferDelay) { AutoReset = false };

            timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
            {
                FileSystemEventHandler tmp = this.Created;

                if (tmp != null)
                {
                    tmp(this, e);
                }

                lock (this.createdList)
                {
                    this.createdList.Remove(fullPath);
                }
            };

            timer.Start();
        }

        /// <summary>
        /// Inviked whenever a file or directory in the specified Path is created (not buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual new void OnCreated(FileSystemEventArgs e)
        {
            FileSystemEventHandler tmp = this.Created;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        /// <summary>
        /// Invoked whenever a file or directory in the specified Path is deleted (buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual void OnBufferedDeleted(FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;

            lock (this.deletedList)
            {
                if (this.deletedList.Contains(fullPath))
                {
                    return;
                }
                else
                {
                    this.deletedList.Add(fullPath);
                }
            }

            var timer = new Timer(this.BufferDelay) { AutoReset = false };

            timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
            {
                FileSystemEventHandler tmp = this.Deleted;

                if (tmp != null)
                {
                    tmp(this, e);
                }

                lock (this.deletedList)
                {
                    this.deletedList.Remove(fullPath);
                }
            };

            timer.Start();
        }

        /// <summary>
        /// Invoked whenever a file or directory in the specified Path is deleted (not buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual new void OnDeleted(FileSystemEventArgs e)
        {
            FileSystemEventHandler tmp = this.Deleted;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        /// <summary>
        /// Invkoed whenever a file or directory in the specified Path is renamed (buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual void OnBufferedRenamed(RenamedEventArgs e)
        {
            string fullPath = e.FullPath;

            lock (this.renamedList)
            {
                if (this.renamedList.Contains(fullPath))
                {
                    return;
                }
                else
                {
                    this.renamedList.Add(fullPath);
                }
            }

            var timer = new Timer(this.BufferDelay) { AutoReset = false };

            timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
            {
                FileSystemEventHandler tmp = this.Renamed;

                if (tmp != null)
                {
                    tmp(this, e);
                }

                lock (this.renamedList)
                {
                    this.renamedList.Remove(fullPath);
                }
            };

            timer.Start();
        }

        /// <summary>
        /// Invkoed whenever a file or directory in the specified Path is renamed (not buffered).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual new void OnRenamed(RenamedEventArgs e)
        {
            FileSystemEventHandler tmp = this.Renamed;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        private void BufferedFileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Changed) == BufferedChangeTypes.Changed)
            {
                this.OnBufferedChanged(e);
            }
            else
            {
                this.OnChanged(e);
            }
        }

        private void BufferedFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Created) == BufferedChangeTypes.Created)
            {
                this.OnBufferedCreated(e);
            }
            else
            {
                this.OnCreated(e);
            }
        }

        private void BufferedFileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Deleted) == BufferedChangeTypes.Deleted)
            {
                this.OnBufferedDeleted(e);
            }
            else
            {
                this.OnDeleted(e);
            }
        }

        private void BufferedFileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Renamed) == BufferedChangeTypes.Renamed)
            {
                this.OnBufferedRenamed(e);
            }
            else
            {
                this.OnRenamed(e);
            }
        }
    }
}
