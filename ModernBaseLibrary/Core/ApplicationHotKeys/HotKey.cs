//-----------------------------------------------------------------------
// <copyright file="HotKey.cs" company="Lifeprojects.de">
//     Class: HotKey
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <Framework>4.8</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.12.2022 12:54:33</date>
//
// <summary>
// Klasse für 
// </summary>

// <Website>
// https://www.codeproject.com/Tips/274003/Global-Hotkeys-in-WPF
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Windows.Input;

    [Serializable]
    public class HotKey : INotifyPropertyChanged, ISerializable, IEquatable<HotKey>
    {
        private Key key = Key.None;
        private ModifierKeys modifiers = ModifierKeys.None;
        private bool enabled = false;
        private string description = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<HotKeyEventArgs> HotKeyPressed;


        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey"/> class.
        /// Creates an HotKey object. This instance has to be registered in an HotKeyHost.
        /// </summary>
        public HotKey()
        {
        }

        // <summary>
        /// Creates an HotKey object. This instance has to be registered in an HotKeyHost.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="modifiers">The modifier. Multiple modifiers can be combined with or.</param>
        public HotKey(Key key, ModifierKeys modifiers) : this(key, modifiers, true)
        {
        }

        // <summary>
        /// Creates an HotKey object. This instance has to be registered in an HotKeyHost.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="modifiers">The modifier. Multiple modifiers can be combined with or.</param>
        /// <param name="enabled">Specifies whether the HotKey will be enabled when registered to an HotKeyHost</param>
        public HotKey(Key key, ModifierKeys modifiers, bool enabled)
        {
            this.Key = key;
            this.Modifiers = modifiers;
            this.Enabled = enabled;
        }

        public HotKey(Key key, ModifierKeys modifiers, string description, bool enabled = true)
        {
            this.Key = key;
            this.Modifiers = modifiers;
            this.Enabled = enabled;
            this.Description = description;
        }

        /// <summary>
        /// The Key. Must not be null when registering to an HotKeyHost.
        /// </summary>
        public Key Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                    this.OnPropertyChanged();
                }
            }
        }

        // <summary>
        /// The modifier. Multiple modifiers can be combined with or.
        /// </summary>
        public ModifierKeys Modifiers
        {
            get { return modifiers; }
            set
            {
                if (modifiers != value)
                {
                    modifiers = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value != enabled)
                {
                    enabled = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public override bool Equals(object obj)
        {
            HotKey hotKey = obj as HotKey;
            if (hotKey != null)
            {
                return Equals(hotKey);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(HotKey other)
        {
            return (this.Key == other.Key && this.Modifiers == other.Modifiers);
        }

        public override int GetHashCode()
        {
            return (int)Modifiers + 10 * (int)this.Key;
        }

        public override string ToString()
        {
            string isEnabled = this.Enabled ? "" : "Not ";
            return $"{this.Key} + {this.Modifiers} ({isEnabled}Enabled)";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnHotKeyPress()
        {
            if (this.HotKeyPressed != null)
            {
                this.HotKeyPressed(this, new HotKeyEventArgs(this));
            }
        }

        internal void RaiseOnHotKeyPressed()
        {
            this.OnHotKeyPress();
        }

        protected HotKey(SerializationInfo info, StreamingContext context)
        {
            this.Key = (Key)info.GetValue("Key", typeof(Key));
            this.Modifiers = (ModifierKeys)info.GetValue("Modifiers", typeof(ModifierKeys));
            this.Enabled = info.GetBoolean("Enabled");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Key", this.Key, typeof(Key));
            info.AddValue("Modifiers", this.Modifiers, typeof(ModifierKeys));
            info.AddValue("Enabled", this.Enabled);
        }
    }
}
