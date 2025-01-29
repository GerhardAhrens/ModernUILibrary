namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldNameAsAttribute : Attribute
    {
        public FieldNameAsAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
