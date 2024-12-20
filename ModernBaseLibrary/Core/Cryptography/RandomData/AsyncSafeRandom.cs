//-----------------------------------------------------------------------
// <copyright file="SafeRandomAsync.cs" company="Lifeprojects.de">
//     Class: SafeRandomAsync
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.01.2021</date>
//
// <summary>
// The Class for create random Data
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using ModernBaseLibrary.Extension;

    /// <inheritdoc />
    public class SafeRandomAsync : ISafeRandomAsync
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly Random _random;

        /// <inheritdoc />
        public SafeRandomAsync(int seed)
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _random = new Random(seed);
        }

        /// <inheritdoc />
        public SafeRandomAsync()
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _random = new Random();
        }

        /// <inheritdoc />
        public async Task<int> NextAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.Next();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public int Next()
        {
            try
            {
                _semaphore.Wait();
                return _random.Next();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<int> NextAsync(int maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.Next(maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public int Next(int maxValue)
        {
            try
            {
                _semaphore.Wait();
                return _random.Next(maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<int> NextAsync(int minValue, int maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.Next(minValue, maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public int Next(int minValue, int maxValue)
        {
            try
            {
                _semaphore.Wait();
                return _random.Next(minValue, maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<double> NextDoubleAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.NextDouble(); ;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public double NextDouble()
        {
            try
            {
                _semaphore.Wait();
                return _random.NextDouble();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<double> NextDoubleAsync(double minValue, double maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.NextDouble(minValue, maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public double NextDouble(double minValue, double maxValue)
        {
            try
            {
                _semaphore.Wait();
                return _random.NextDouble(minValue, maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<double> NextDoubleAsync(double maxValue)
        {
            try
            {
                await _semaphore.WaitAsync();
                return _random.NextDouble(maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public double NextDouble(double maxValue)
        {
            try
            {
                _semaphore.Wait();
                return _random.NextDouble(maxValue);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task NextBytesAsync(byte[] buffer)
        {
            try
            {
                await _semaphore.WaitAsync();
                _random.NextBytes(buffer);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public void NextBytes(byte[] buffer)
        {
            try
            {
                _semaphore.Wait();
                _random.NextBytes(buffer);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}