namespace ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public sealed class Value<T> : ValueBase
        where T : notnull
    {
        public T Content { get; }
        public Value(T content) => Content = content;

        public override bool Equals([AllowNull] ValueBase other)
        {
            if (other is null) return false;
            if (other is Value<T> value) return Content.Equals(value.Content);
            return false;
        }
        
        public override int GetHashCode() => Content.GetHashCode();
        public override string ToString() => $"Value({Content})";
    }

    public abstract class Value : ValueBase
    {
        protected abstract IEnumerable<ValueBase> GetValues();
        public override bool Equals([AllowNull] ValueBase other)
        {
            if (other is null) return false;
            if (GetType() != other.GetType()) return false;
            return GetValues().SequenceEqual(((Value)other).GetValues());
        }

        public sealed override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var value in GetValues())
                hash.Add(value);
            return hash.ToHashCode();
        }
    }
}
