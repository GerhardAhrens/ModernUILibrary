//-----------------------------------------------------------------------
// <copyright file="EnumBase.cs" company="Lifeprojects.de">
//     Class: EnumBase
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Class of EnumMap Implemation</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;

    using ModernBaseLibrary.Extension;

    public sealed class EnumMap<T> where T : struct, Enum
    {
        private sealed class Mapper
        {
            public readonly Dictionary<string, T> valueLookUp;
            public readonly Dictionary<T, string> nameLookUp;

            public Mapper()
            {
                string[] names = Enum.GetNames(typeof(T));

                valueLookUp = new Dictionary<string, T>(names.Length);
                nameLookUp = new Dictionary<T, string>(names.Length);

                foreach (string name in names)
                {
                    T valueOut;
                    if (Enum.TryParse<T>(name, out valueOut) == true)
                    {
                        if (valueLookUp.ContainsKey(name) == false)
                        {
                            valueLookUp.Add(name, valueOut);
                        }

                        if (nameLookUp.ContainsKey(valueOut) == false)
                        {
                            nameLookUp.Add(valueOut, name);
                        }
                    }
                }
            }
        }

        private readonly Lazy<Mapper> lazy = new Lazy<Mapper>(() => { return new Mapper(); });

        public string GetName(T value)
        {
            if (lazy.Value.nameLookUp.TryGetValue(value, out string name))
            {
                return name;
            }

            return default;
        }

        public int GetValue(T value)
        {
            if (lazy.Value.nameLookUp.TryGetValue(value, out string name))
            {
                return value.ToInt();
            }

            return default;
        }

        public string GetDescription(T value)
        {
            if (lazy.Value.nameLookUp.TryGetValue(value, out string name))
            {
                return value.ToDescription();
            }

            return default;
        }

        public T Parse(string value)
        {
            if (TryParse(value, out T result))
            {
                return result;
            }

            throw new ArgumentException(null, nameof(value));
        }

        public bool TryParse(string value, out T result)
        {
            if (value is null)
            {
                result = default;
                return false;
            }

            return lazy.Value.valueLookUp.TryGetValue(value, out result);
        }
    }
}
