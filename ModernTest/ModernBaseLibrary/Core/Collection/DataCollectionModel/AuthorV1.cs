namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Diagnostics;

    using global::ModernBaseLibrary.Collection;

    [DebuggerStepThrough]
    [Serializable]
    [DebuggerDisplay("Name = {Name}")]
    public class AuthorV1 : BaseModelDC, IBaseModelDC
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }
    }
}
