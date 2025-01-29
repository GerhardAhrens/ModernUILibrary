namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// you can ignore field or property by adding this attribute to property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
    public class IgnoreFieldAttribute : Attribute
    {
    }
}
