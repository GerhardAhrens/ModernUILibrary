namespace ModernIU.Controls.Chart
{
    using System.Windows.Media;


    public class ChartLegendItem
    {
        #region Fields

        private string _caption;
        private Brush _itemBrush;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the StackedChartLegendItem class.
        /// </summary>
        /// <param name="caption"></param>
        /// /// <param name="color"></param>
        public ChartLegendItem(string caption, Color color)
        {
            _caption = caption;
            _itemBrush = new SolidColorBrush(color);
        }

        /// <summary>
        /// Initializes a new instance of the StackedChartLegendItem class.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="itemBrush"></param>
        public ChartLegendItem(string caption, Brush itemBrush)
        {
            _caption = caption;
            _itemBrush = itemBrush;
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
            }
        }

        /// <summary>
        /// Gets or sets the item brush.
        /// </summary>
        /// <value>The item brush.</value>
        public Brush ItemBrush
        {
            get { return _itemBrush; }
            set { _itemBrush = value; }
        }

        #endregion Properties
    }
}