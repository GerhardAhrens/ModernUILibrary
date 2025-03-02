namespace ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        // Note: Any additions to this list must be reflected in the ValueBase implicit cast
        public static ValueSequence AsValues(this IEnumerable<ValueBase> sequence) => new ValueSequence(sequence.ToArray());

        public static ValueSequence AsValues(this IEnumerable<bool> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<byte> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<sbyte> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<char> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<decimal> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<double> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<float> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<int> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<uint> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<long> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<ulong> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<short> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<ushort> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<Guid> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
        public static ValueSequence AsValues(this IEnumerable<string> sequence)
        {
            var source = sequence.ToArray();
            ValueBase[] values = new ValueBase[source.Length];
            for (int i = 0; i < source.Length; i++)
                values[i] = source[i];
            return new ValueSequence(values);
        }
    }
}
