//-----------------------------------------------------------------------
// <copyright file="HotKeyHost.cs" company="Lifeprojects.de">
//     Class: HotKeyHost
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <Framework>4.8</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.12.2022 13:06:36</date>
//
// <summary>
// Klasse für 
// </summary>
//
// <Website>
// https://www.codeproject.com/Tips/274003/Global-Hotkeys-in-WPF
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Input;
    using System.Windows.Interop;

    public class HotKeyHost : IDisposable
    {
        private bool disposed;
        private HwndSourceHook hook;
        private HwndSource hwndSource;
        private Dictionary<int, HotKey> hotKeys = new Dictionary<int, HotKey>();
        private static readonly SerialCounter idGen = new SerialCounter(1);

        public IEnumerable<HotKey> HotKeys { get { return hotKeys.Values; } }

        public event EventHandler<HotKeyEventArgs> HotKeyPressed;


        #region HotKey Interop

        private const int WM_HotKey = 786;

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, int modifiers, int key);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int UnregisterHotKey(IntPtr hwnd, int id);

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyHost"/> class.
        /// </summary>
        /// <summary>
        /// Creates a new HotKeyHost
        /// </summary>
        /// <param name="hwndSource">The handle of the window. Must not be null.</param>
        public HotKeyHost(HwndSource hwndSource)
        {
            if (hwndSource == null)
            {
                throw new ArgumentNullException("hwndSource");
            }

            this.hook = new HwndSourceHook(WndProc);
            this.hwndSource = hwndSource;
            hwndSource.AddHook(hook);
        }

        ~HotKeyHost()
        {
            this.Dispose(false);
        }

        public void AddHotKey(HotKey hotKey)
        {
            if (hotKey == null)
            {
                throw new ArgumentNullException("value");
            }

            if (hotKey.Key == 0)
            {
                throw new ArgumentNullException("value.Key");
            }

            if (hotKeys.ContainsValue(hotKey))
            {
                throw new HotKeyAlreadyRegisteredException("HotKey already registered!", hotKey);
            }

            int id = idGen.Next();
            if (hotKey.Enabled)
            {
                RegisterHotKey(id, hotKey);
            }

            hotKey.PropertyChanged += hotKey_PropertyChanged;
            hotKeys[id] = hotKey;
        }

        /// <summary>
        /// Removes an hotKey
        /// </summary>
        /// <param name="hotKey">The hotKey to be removed</param>
        /// <returns>True if success, otherwise false</returns>
        public bool RemoveHotKey(HotKey hotKey)
        {
            var kvPair = hotKeys.FirstOrDefault(h => h.Value == hotKey);
            if (kvPair.Value != null)
            {
                kvPair.Value.PropertyChanged -= hotKey_PropertyChanged;
                if (kvPair.Value.Enabled)
                {
                    UnregisterHotKey(kvPair.Key);
                }

                return this.hotKeys.Remove(kvPair.Key);
            }
            return false;
        }

        public Dictionary<int, HotKey> GetHotkeys()
        {
            return this.hotKeys;
        }

        public List<string> HotkeysToList(string separator = ";")
        {
            List<string> keys = new List<string>();

            foreach (KeyValuePair<int, HotKey> hotkey in this.hotKeys)
            {
                string modifiers = hotkey.Value.Modifiers.ToString();
                string key = hotkey.Value.Key.ToString();
                string desc = hotkey.Value.Description.ToString();
                if (string.IsNullOrEmpty(desc) == false)
                {
                    keys.Add($"{modifiers}-{key}{separator}{desc}");
                }
                else
                {
                    keys.Add($"{modifiers}-{key}");
                }
            }

            return keys;
        }

        private void hotKey_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var kvPair = hotKeys.FirstOrDefault(h => h.Value == sender);
            if (kvPair.Value != null)
            {
                if (e.PropertyName == "Enabled")
                {
                    if (kvPair.Value.Enabled)
                    {
                        RegisterHotKey(kvPair.Key, kvPair.Value);
                    }
                    else
                    {
                        UnregisterHotKey(kvPair.Key);
                    }
                }
                else if (e.PropertyName == "Key" || e.PropertyName == "Modifiers")
                {
                    if (kvPair.Value.Enabled)
                    {
                        UnregisterHotKey(kvPair.Key);
                        RegisterHotKey(kvPair.Key, kvPair.Value);
                    }
                }
            }
        }

        private void RegisterHotKey(int id, HotKey hotKey)
        {
            if ((int)hwndSource.Handle != 0)
            {
                RegisterHotKey(hwndSource.Handle, id, (int)hotKey.Modifiers, KeyInterop.VirtualKeyFromKey(hotKey.Key));
                int error = Marshal.GetLastWin32Error();

                if (error != 0)
                {
                    Exception e = new Win32Exception(error);

                    if (error == 1409)
                    {
                        throw new HotKeyAlreadyRegisteredException(e.Message, hotKey, e);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Handle is invalid");
            }
        }

        private void UnregisterHotKey(int id)
        {
            if ((int)hwndSource.Handle != 0)
            {
                UnregisterHotKey(hwndSource.Handle, id);
                int error = Marshal.GetLastWin32Error();
                if (error != 0)
                {
                    throw new Win32Exception(error);
                }
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HotKey)
            {
                if (hotKeys.ContainsKey((int)wParam))
                {
                    HotKey h = hotKeys[(int)wParam];
                    h.RaiseOnHotKeyPressed();
                    if (HotKeyPressed != null)
                    {
                        HotKeyPressed(this, new HotKeyEventArgs(h));
                    }
                }
            }

            return new IntPtr(0);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                hwndSource.RemoveHook(hook);
            }

            for (int i = hotKeys.Count - 1; i >= 0; i--)
            {
                RemoveHotKey(hotKeys.Values.ElementAt(i));
            }


            disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class SerialCounter
        {
            public SerialCounter(int start)
            {
                Current = start;
            }

            public int Current { get; private set; }

            public int Next()
            {
                return ++Current;
            }
        }
    }
}
