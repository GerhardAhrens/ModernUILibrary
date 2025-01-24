namespace ModernIU.Controls.Chart
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    public class StackedChartRow : ChartCoreBase
    {
        #region Fields

        private string _caption;
        private Brush _chartBrush;
        private Guid _id;
        private List<ChartRow> _values = new List<ChartRow>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        public StackedChartRow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        /// <param name="caption"></param>
        public StackedChartRow(string caption)
        {
            _caption = caption;
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
        public StackedChartRow(string caption, Brush chartBrush)
        {
            _caption = caption;
            _chartBrush = chartBrush;
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the ChartRow class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="caption"></param>
        /// <param name="value"></param>
        public StackedChartRow(string caption, Color color)
        {
            _caption = caption;
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

        public Brush ChartBrush
        {
            get { return _chartBrush; }
            set
            {
                _chartBrush = value;
                OnPropertyChanged();
            }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public List<ChartRow> Values
        {
            get
            {
                return _values;
            }
        }

        #endregion Properties

    }
}