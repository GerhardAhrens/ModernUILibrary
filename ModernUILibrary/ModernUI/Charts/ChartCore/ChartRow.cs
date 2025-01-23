namespace ModernIU.Controls.Chart
{
    using System;
    using System.Windows.Media;


    /// <summary>
    /// Represents an Row in the bar-chart
    /// </summary>
    public class ChartRow : ChartCoreBase
    {
        #region Fields

        private string _caption;
        private Brush _chartBrush;
        private Guid _id;
        private double _value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        public ChartRow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="value1"></param>
        public ChartRow(string caption, double value)
        {
            _caption = caption;
            _value = value;
            _id = Guid.NewGuid();
            _chartBrush = new SolidColorBrush(ColorGenerator.Instance.GetNext());
        }

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <param name="chartBrush"></param>
        public ChartRow(string caption, double value, Brush chartBrush)
        {
            _caption = caption;
            _chartBrush = chartBrush;
            _id = Guid.NewGuid();
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="caption"></param>
        /// <param name="value"></param>
        public ChartRow(string caption, double value, Color color)
        {
            _caption = caption;
            _value = value;
            _id = Guid.NewGuid();
            _chartBrush = new SolidColorBrush(color);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the chart brush.
        /// </summary>
        /// <value>The chart brush.</value>
        public Brush ChartBrush
        {
            get { return _chartBrush; }
            set
            {
                _chartBrush = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}