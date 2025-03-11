namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Diagnostics;

    using global::ModernBaseLibrary.Collection;

    [DebuggerStepThrough]
    [Serializable]
    [DebuggerDisplay("Name = {Name}")]
    public class AuthorV0 : BaseModelDC, IBaseModelDC
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
                this.id =  value;
                this.OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public int Age
        {
            get { return this.age; }
            set
            {
                this.age = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime Birthday
        {
            get { return this.birthday; }
            set
            {
                this.birthday = value;
                this.OnPropertyChanged();
            }
        }

    }
}
