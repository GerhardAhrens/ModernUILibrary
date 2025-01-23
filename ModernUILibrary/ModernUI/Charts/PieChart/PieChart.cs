namespace ModernIU.Controls.Chart
{
    using System.Windows;

    public class PieChart : ChartBase
    {
        /// <summary>
        /// Initializes the <see cref="PieChart"/> class.
        /// </summary>
        static PieChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PieChart), new FrameworkPropertyMetadata(typeof(PieChart)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        public PieChart()
        {
        }
    }
}