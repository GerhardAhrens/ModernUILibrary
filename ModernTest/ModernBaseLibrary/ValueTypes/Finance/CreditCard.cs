namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    using System.Collections.Generic;

    using global::ModernBaseLibrary.ValueTypes;

    public class CreditCard : Value
    {
        private readonly CreditCompany _company;
        private readonly Amount _creditLimit;
        public CreditCard(CreditCompany company, Amount creditLimit) => (_company, _creditLimit) = (company, creditLimit);

        protected override IEnumerable<ValueBase> GetValues() => Yield(_company, _creditLimit);
    }
}
