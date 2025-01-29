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
            this._caption = caption;
            this._value = value;
            this._id = Guid.NewGuid();
            this._chartBrush = new SolidColorBrush(ColorGenerator.Instance.GetNext());
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
            this._caption = caption;
            this._chartBrush = chartBrush;
            this._id = Guid.NewGuid();
            this._value = value;
        }

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="caption"></param>
        /// <param name="value"></param>
        public ChartRow(string caption, double value, Color color)
        {
            this._caption = caption;
            this._value = value;
            this._id = Guid.NewGuid();
            this._chartBrush = new SolidColorBrush(color);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return this._caption; }
            set
            {
                this._caption = value;
                base.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the chart brush.
        /// </summary>
        /// <value>The chart brush.</value>
        public Brush ChartBrush
        {
            get { return this._chartBrush; }
            set
            {
                this._chartBrush = value;
                base.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id
        {
            get { return this._id; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                base.OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}