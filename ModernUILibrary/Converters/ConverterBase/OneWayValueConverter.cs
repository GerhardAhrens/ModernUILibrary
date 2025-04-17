namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Eine abstrakte Basisklasse für Wertkonverter, die nur in eine Richtung konvertieren (von der Quelle zur Bindung).
    /// </summary>
    public abstract class OneWayValueConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{GetType().Name} can only convert one-way.");
        }
    }

    /// <summary>
    /// Eine abstrakte Basisklasse für mehrwertige Konverter, die nur in eine Richtung konvertieren (von der Quelle zur Bindung).
    /// </summary>
    public abstract class OneWayMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// Wandelt Quellwerte in einen Wert für das Bindungsziel um. 
        /// Die Datenbindungs-Engine ruft diese Methode auf, wenn sie die Werte aus den Quellbindungen auf das Bindungsziel überträgt.
        /// </summary>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{GetType().Name} can only convert one-way.");
        }
    }
}
