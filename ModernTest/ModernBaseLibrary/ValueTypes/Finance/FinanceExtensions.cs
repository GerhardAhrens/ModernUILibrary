namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    public static class FinanceExtensions
    {
        public static Amount ToAmount(this decimal amount) => new(amount);

        public static Money Dollars(this decimal amount) => new(Currency.USD, amount.ToAmount());
        public static Money Euros(this decimal amount) => new(Currency.EUR, amount.ToAmount());

        public static CreditCard For(this CreditCompany company, Amount limit) => new(company, limit);
    }
}
