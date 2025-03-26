namespace ModernBaseLibrary.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public class AssemblyMap : IDictionary<string,AssemblyMapEntry>
    {
        Dictionary<string, AssemblyMapEntry> _map = new Dictionary<string, AssemblyMapEntry>();

        public virtual void AddAssembly(string name, Assembly a)
        {
            if (this._map.TryGetValue(name, out AssemblyMapEntry entry))
            {
                int index = entry.Assemblies.BinarySearch(a, new AssemblyComparer());
                if (index < 0)
                {
                    entry.Assemblies.Insert(~index, a);
                }
            }
            else
            {
                this._map[name] = new AssemblyMapEntry(name, a);
            }
        }

        public Assembly FindAssembly(string name)
        {
            if(this._map.TryGetValue(name, out AssemblyMapEntry entry))
            {
                return entry.Assembly;
            }
            return null;
        }
        public Assembly[] FindAssemblies(string name)
        {
            if (this._map.TryGetValue(name, out AssemblyMapEntry entry))
            {
                return entry.Assemblies.ToArray();
            }
            return new Assembly[0];
        }
        public AssemblyMapEntry this[string key] { get => ((IDictionary<string, AssemblyMapEntry>)this._map)[key]; set => ((IDictionary<string, AssemblyMapEntry>)this._map)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, AssemblyMapEntry>)this._map).Keys;

        public ICollection<AssemblyMapEntry> Values => ((IDictionary<string, AssemblyMapEntry>)this._map).Values;

        public int Count => ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).IsReadOnly;

        public void Add(string key, AssemblyMapEntry value)
        {
            ((IDictionary<string, AssemblyMapEntry>)this._map).Add(key, value);
        }

        public void Add(KeyValuePair<string, AssemblyMapEntry> item)
        {
            ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).Clear();
        }

        public bool Contains(KeyValuePair<string, AssemblyMapEntry> item)
        {
            return ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, AssemblyMapEntry>)this._map).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, AssemblyMapEntry>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, AssemblyMapEntry>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, AssemblyMapEntry>>)this._map).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, AssemblyMapEntry>)this._map).Remove(key);
        }

        public bool Remove(KeyValuePair<string, AssemblyMapEntry> item)
        {
            return ((ICollection<KeyValuePair<string, AssemblyMapEntry>>)this._map).Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out AssemblyMapEntry value)
        {
            return ((IDictionary<string, AssemblyMapEntry>)this._map).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._map).GetEnumerator();
        }
    }
}
