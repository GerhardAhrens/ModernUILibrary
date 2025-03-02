namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    using System.Collections.Generic;

    using global::ModernBaseLibrary.ValueTypes;

    public class Contact : Value
    {
        public string Name { get; }
        public Address? Address { get; }
        public Contact(string name, Address? address)
        {
            Name = name;
            Address = address;
        }

        protected override IEnumerable<ValueBase> GetValues() => Yield(Name, Address);
    }
}
