namespace ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class ValueEquatable<T> : ValueBase
        where T : IEquatable<T>
    {
        private readonly IEquatable<T> _equatable;
        public ValueEquatable(IEquatable<T> equatable) => _equatable = equatable;

        public override bool Equals([AllowNull] ValueBase other) => other switch
        {
            null => false,
            ValueEquatable<T> eq => _equatable.Equals(eq._equatable),
            _ => false
        };

        public override int GetHashCode() => _equatable.GetHashCode();

        public override string ToString() => $"Value({_equatable})";
    }
}
