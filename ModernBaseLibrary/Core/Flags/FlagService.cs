//-----------------------------------------------------------------------
// <copyright file="FlagService.cs" company="Lifeprojects.de">
//     Class: FlagService
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>22.07.2025 12:52:06</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public interface IFlagService<TEnum>
    {
        Dictionary<TEnum, bool> FlagItems { get; }

        void SetFlags(string flagItems);

        void SetFlag(TEnum rightflag, bool flag);

        string GetFlags();

        bool IsFlag(TEnum flag);
    }

    public class FlagService<TEnum> : IDisposable, IFlagService<TEnum>
    {
        private bool classIsDisposed = false;
        private Dictionary<TEnum, bool> _FlagItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagService"/> class.
        /// </summary>
        public FlagService()
        {
            this.FlagItems = null;
        }

        public FlagService(Dictionary<TEnum, bool> flagItems)
        {
            if (flagItems == null && flagItems.Count == 0)
            {
                throw new ArgumentException("Im Dictionary 'flagItems' muß mindestens ein Eintrag haben.");
            }

            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("Das Dictionary 'flagItems' muß als Key ein gültiges Enum haben.");
            }

            this.FlagItems = flagItems.OrderBy(k => k.Key.ToString()).ToDictionary<TEnum, bool>();
        }

        public int Count { get { return this.FlagItems != null ? this.FlagItems.Count() : 0; } }


        public Dictionary<TEnum, bool> FlagItems
        {
            get { return _FlagItems; }
            set { _FlagItems = value; }
        }

        public string GetFlags()
        {
            string result = string.Empty;
            if (this.FlagItems != null && this.FlagItems.Count > 0)
            {
                result = string.Join("",this.FlagItems.Select(x => string.Format("{0}", x.Value == true ? "1" : "0")));
            }

            return result;
        }

        public void SetFlags(string flagItems)
        {
            if (string.IsNullOrEmpty(flagItems) == false)
            {
                if (flagItems.Length != this.FlagItems.Count)
                {
                    throw new ArgumentException($"Die Anzahl von Items im Dictionary ({this.FlagItems.Count}) muß gleich der Länge des Strings ({flagItems.Length}) 'flagItems' sein.");
                }

                int step = 0;
                foreach (KeyValuePair<TEnum, bool> item in this.FlagItems)
                {
                    if (this.FlagItems.ContainsKey(item.Key) == true)
                    {
                        bool value = flagItems.Substring(step, 1) == "0" ? false : true;
                        this.FlagItems[item.Key] = value;
                        step++;
                    }
                }
            }
            else
            {
                int step = 0;
                foreach (KeyValuePair<TEnum, bool> item in this.FlagItems)
                {
                    if (this.FlagItems.ContainsKey(item.Key) == true)
                    {
                        this.FlagItems[item.Key] = false;
                        step++;
                    }
                }
            }
        }

        public void SetFlag(TEnum rightflag, bool flag)
        {
            if (this.FlagItems.ContainsKey(rightflag) == true)
            {
                this.FlagItems[rightflag] = flag;
            }
        }

        public bool IsFlag(TEnum flag)
        {
            bool result = false;

            if (this.FlagItems.ContainsKey(flag) == true)
            {
                result = this.FlagItems[flag];
            }

            return result;
        }

        public void Add(Dictionary<TEnum, bool> flagItems)
        {
            if (flagItems == null && flagItems.Count == 0)
            {
                throw new ArgumentException("Im Dictionary 'flagItems' muß mindestens ein Eintrag haben.");
            }

            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("Das Dictionary 'flagItems' muß als Key ein gültiges Enum haben.");
            }

            this.FlagItems = flagItems.OrderBy(k => k.Key.ToString()).ToDictionary<TEnum, bool>();
        }

        public void Add(KeyValuePair<TEnum, bool> flagItem)
        {
            if (this.FlagItems.ContainsKey(flagItem.Key) == false)
            {
                this.FlagItems.Add(flagItem.Key,flagItem.Value);
                this.FlagItems = this.FlagItems.OrderBy(k => k.Key.ToString()).ToDictionary<TEnum, bool>();
            }
            else
            {
                throw new ArgumentException($"Der Key '{flagItem.Key.ToString()}' ist bereits vorhanden.");
            }
        }

        public string ToString(bool active = true)
        {
            string result = string.Empty;
            if (this.FlagItems != null && this.FlagItems.Count > 0)
            {
                result = string.Join(", ", this.FlagItems.Where(v => v.Value == active).Select(x => x.Key.ToString()));
            }

            return result;
        }

        public string ToDescription(bool active = true)
        {
            string result = string.Empty;
            if (this.FlagItems != null && this.FlagItems.Count > 0)
            {
                result = string.Join(", ", this.FlagItems.Where(v => v.Value == active).Select(x => this.GetDescription(x.Key)));
            }

            return result;
        }

        private string GetDescription(TEnum enumKey)
        {
            Type type = enumKey.GetType();
            if (type.IsEnum == false)
            {
                throw new ArgumentException($"Der Typ '{type.Name}' muss vom Typ Enum sein", "enumKey");
            }

            MemberInfo[] memberInfo = type.GetMember(enumKey.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] descAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descAttribute != null && descAttribute.Length > 0)
                {
                    return ((DescriptionAttribute)descAttribute[0]).Description;
                }
            }

            return enumKey.ToString();
        }


        #region Implement Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing == true)
                {
                    this.FlagItems = null;
                }
            }

            this.classIsDisposed = true;
        }

        #endregion Implement Dispose
    }
}
