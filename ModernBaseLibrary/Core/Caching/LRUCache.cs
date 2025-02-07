namespace ModernBaseLibrary.Core
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Represents a Least Recently Used (LRU) cache.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the cache.</typeparam>
    /// <typeparam name="TValue">The type of the values in the cache.</typeparam>
    public class LRUCache<TKey, TValue> : IDisposable where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, LinkedListNode<(TKey Key, TValue Value)>> _cacheMap;
        private readonly int _capacity;
        private readonly object _lock = new();
        private readonly LinkedList<(TKey Key, TValue Value)> _lruList;
        private bool _disposedValue;

        /// <summary>
        /// Gets the number of elements contained in the cache.
        /// </summary>
        public int Count => _cacheMap.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="LRUCache{TKey, TValue}"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The maximum number of items that the cache can hold.</param>
        /// <exception cref="ArgumentException">Thrown when the capacity is less than or equal to zero.</exception>
        public LRUCache(int capacity = 100)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
            }

            this._capacity = capacity;
            this._cacheMap = new ConcurrentDictionary<TKey, LinkedListNode<(TKey Key, TValue Value)>>();
            this._lruList = new LinkedList<(TKey Key, TValue Value)>();
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the key is not found in the cache.</exception>
        /// <exception cref="ArgumentException">Thrown when setting a null value.</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }

                throw new KeyNotFoundException($"The key '{key}' was not found in the cache.");
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException($"Invalid null value for key '{key}'");
                }

                this.Set(key, value);
            }
        }

        /// <summary>
        /// Clears all items from the cache.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                foreach (var node in _lruList)
                {
                    DisposeIfRequired(node.Value);
                }

                this._lruList.Clear();
                this._cacheMap.Clear();
            }
        }

        /// <summary>
        /// Disposes the cache and releases all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets all the values currently stored in the cache.
        /// </summary>
        /// <returns>An enumerable of values in the cache, in order from most recently used to least recently used.</returns>
        public IEnumerable<TValue> GetAllValues()
        {
            lock (_lock)
            {
                foreach (var node in _lruList)
                {
                    yield return node.Value;
                }
            }
        }

        /// <summary>
        /// Thread-safe method to retrieve or add an item.
        /// </summary>
        /// <param name="key">The key of the value to retrieve or add.</param>
        /// <param name="valueFactory">The function to create the value if it does not exist.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TValue AddOrUpdate(TKey key, Func<TValue> valueFactory)
        {
            // Fast path: Try to get the value without locking
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }

            // Slow path: Ensure only one thread can add the value
            lock (this._lock)
            {
                if (TryGetValue(key, out value)) // Double-check
                {
                    return value;
                }

                // Add new value
                value = valueFactory();
                this.Set(key, value);
                return value;
            }
        }

        /// <summary>
        /// Adds the specified key and value to the cache.
        /// If the key already exists, updates the value and moves the entry to the front of the cache.
        /// </summary>
        /// <param name="key">The key of the value to add.</param>
        /// <param name="value">The value to add.</param>
        public void Set(TKey key, TValue value)
        {
            lock (this._lock)
            {
                if (this._cacheMap.TryGetValue(key, out var existingNode))
                {
                    this._lruList.Remove(existingNode);
                    DisposeIfRequired(existingNode.Value.Value);
                    existingNode.Value = (key, value);
                    this._lruList.AddFirst(existingNode);
                }
                else
                {
                    var newNode = new LinkedListNode<(TKey Key, TValue Value)>((key, value));

                    this._lruList.AddFirst(newNode);
                    this._cacheMap[key] = newNode;

                    if (this._cacheMap.Count > this._capacity)
                    {
                        var lruNode = this._lruList.Last;
                        if (lruNode != null)
                        {
                            this._lruList.RemoveLast();
                            this._cacheMap.TryRemove(lruNode.Value.Key, out _);
                            DisposeIfRequired(lruNode.Value.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the cache contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this._cacheMap.TryGetValue(key, out var node))
            {
                // Move accessed node to the front of the LRU list
                lock (this._lock)
                {
                    this._lruList.Remove(node);
                    this._lruList.AddFirst(node);
                }
                value = node.Value.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Tries to remove the value with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key of the value to remove.</param>
        /// <param name="value">When this method returns, contains the value removed from the cache, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the value was removed; otherwise, <c>false</c>.</returns>
        public bool TryRemove(TKey key, out TValue value)
        {
            if (this._cacheMap.TryRemove(key, out var node))
            {
                lock (this._lock)
                {
                    DisposeIfRequired(node.Value.Value);
                    this._lruList.Remove(node);
                }
                value = node.Value.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Disposes the cache and releases all resources.
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the method was called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposedValue)
            {
                return;
            }

            if (disposing)
            {
                this.Clear();
            }

            this._disposedValue = true;
        }

        /// <summary>
        /// Disposes the value if it implements IDisposable.
        /// </summary>
        /// <param name="value">The value to dispose.</param>
        private static void DisposeIfRequired(TValue value)
        {
            if (value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}