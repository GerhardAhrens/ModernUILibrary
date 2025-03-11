namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Diagnostics;
    using Core;

    using global::ModernBaseLibrary.Collection;

    [DebuggerStepThrough]
    [Serializable]
    [DebuggerDisplay("Name = {Name}")]
    public class Contact : BindableBaseDC
    {
        public Contact(bool isNotifyProperty = false)
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

        public int IdCounter
        {
            get { return this.Get<int>(); }
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

        public int Age
        {
            get { return this.Get<int>(); }
            set
            {
                this.Set(value);
            }
        }

        public DateTime Birthday
        {
            get { return this.Get<DateTime>(); }
            set
            {
                this.Set(value);
            }
        }
    }
}
