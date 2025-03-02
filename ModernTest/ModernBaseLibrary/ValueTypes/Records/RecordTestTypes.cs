namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    using System.Collections.Generic;

    using global::ModernBaseLibrary.ValueTypes;

    public record ThermalUnit
    {
        private ThermalUnit() { }
        public string Symbol { get; init; } = string.Empty;

        public static ThermalUnit Fahrenheit => new() { Symbol = "F" };
        public static ThermalUnit Celsius => new() { Symbol = "C" };
    }

    public class Temperature : Value
    {
        ThermalUnit Unit { get; }
        int Amount { get; }

        public Temperature(ThermalUnit unit, int amount) => (Unit, Amount) = (unit, amount);

        protected override IEnumerable<ValueBase> GetValues() => Yield(Amount, Unit.AsValue());
    }

    public sealed class Chemical : Value
    {
        private readonly string _commonName;
        private readonly string _chemicalName;
        private Chemical(string commonName, string chemicalName) => (_commonName, _chemicalName) = (commonName, chemicalName);
        protected override IEnumerable<ValueBase> GetValues() => Yield(_commonName, _chemicalName);

        public static Chemical Alcohol => new("Alcohol", "Ethanol");
        public static Chemical Chalk => new("Chalk", "Calcium Carbonate");
    }

    public record ChemicalCompound
    {
        public Chemical Chemical { get; }
        public string Formula { get; }

        public ChemicalCompound(Chemical chemical, string formula) => (Chemical, Formula) = (chemical, formula);
    }
}
