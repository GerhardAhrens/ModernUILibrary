namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Diagnostics;

    using global::ModernBaseLibrary.Collection;

    [DebuggerStepThrough]
    [Serializable]
    [DebuggerDisplay("Name = {Name}")]
    public class Author : BaseModelDC, IBaseModelDC
    {
        private Guid id = Guid.Empty;
        private string name = string.Empty;
        private int age;
        private DateTime birthday;

        public Guid Id
        {
            get { return this.id; }
            set
            {
                this.SetProperty(ref this.id, value);
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.SetProperty(ref this.name, value);
            }
        }

        public int Age
        {
            get { return this.age; }
            set
            {
                this.SetProperty(ref this.age, value);
            }
        }

        public DateTime Birthday
        {
            get { return this.birthday; }
            set
            {
                this.SetProperty(ref this.birthday, value);
            }
        }

    }
}
