namespace ModernBaseLibrary.ValueTypes
{
    public static class GroupExtensions
    {
        // Note: Any additions to this list must be reflected in the ValueBase implicit cast
        public static ValueGroup AsGroup(this IEnumerable<ValueBase> group) => new ValueGroup(group.ToArray());

        public static ValueGroup AsGroup(this IEnumerable<bool> group)
        {
            var source = group.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<byte> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<sbyte> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<char> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<decimal> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<double> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<float> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<int> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<uint> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<long> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<ulong> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<short> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<ushort> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<Guid> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
        public static ValueGroup AsGroup(this IEnumerable<string> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueGroup(values);
        }
    }
}
