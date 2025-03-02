namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.ValueTypes;

    public sealed class Amount : Value
    {
        public decimal Value { get; }

        public Amount(decimal amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
            if (Math.Round(amount, 2) != amount) throw new ArgumentOutOfRangeException(nameof(amount), "Amount can only have 2-digits of precision.");

            Value = amount;
        }

        protected override IEnumerable<ValueBase> GetValues() => Yield(Value);
    }
}
