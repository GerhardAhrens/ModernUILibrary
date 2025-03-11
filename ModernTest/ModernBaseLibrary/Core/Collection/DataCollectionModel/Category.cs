namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Diagnostics;
    using Core;

    using global::ModernBaseLibrary.Collection;

    [DebuggerStepThrough]
    [Serializable]
    [DebuggerDisplay("Name = {Name}")]
    public class Category : BindableBaseDC
    {
        public Category(bool isNotifyProperty = false)
        {
            this.IsNotifyProperty = isNotifyProperty;
        }

        public Guid Id
        {
            get { return this.Get<Guid>(); }
            set
            {
                this.Set(value);
            }
        }

        public string Name
        {
            get { return this.Get<string>(); }
            set
            {
                this.Set(value);
            }
        }
    }
}
