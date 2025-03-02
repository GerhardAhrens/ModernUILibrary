namespace ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public sealed class ValueSequence : ValueBase, IEquatable<ValueSequence>
    {
        private ValueBase[] Values { get; }
        public ValueSequence(ValueBase[] values) => Values = values;


        public bool Equals([AllowNull] ValueSequence other) => !(other is null) && Values.SequenceEqual(other.Values);

        public override bool Equals([AllowNull] ValueBase other) => Equals(other as ValueSequence);
        public override int GetHashCode()
        {
            var hash = new HashCode();

            var hashValues = Values.Select(v => v.GetHashCode());

            foreach (var item in hashValues)
                hash.Add(item);

            return hash.ToHashCode();
        }
    }
}
