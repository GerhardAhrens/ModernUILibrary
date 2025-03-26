namespace ModernBaseLibrary.Core
{
    public class TypeMapEntry
    {
        public TypeMapEntry(string name, Type type)
        {
            this.Name = name;
            this.Types.Add(type);
        }
        public string Name { get; } 

        public List<Type> Types = new List<Type>();

        public Type Type => Types.Count > 0 ? Types[0] : null;

        internal Type GetType(string assemblyName)
        {
            foreach (var t in Types)
            {
                if (t.Assembly.GetName().Name == assemblyName)
                {
                    return t;
                }
            }
            return null;
        }
    }
}
