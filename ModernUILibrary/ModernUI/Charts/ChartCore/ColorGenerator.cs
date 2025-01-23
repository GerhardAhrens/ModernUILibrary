namespace ModernIU.Controls.Chart
{
    using System.Collections.Generic;
    using System.Windows.Media;


    public class ColorGenerator
    {
        #region Fields

        private static ColorGenerator _instance = null;
        private static bool _isInitialized = false;
        private static object _syncLock = new object();

        private List<Color> _colors = new List<Color>();
        private Color _nextColor;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ColorGenerator class.
        /// </summary>
        public ColorGenerator()
        {
            _colors.Add(Colors.Red);
            _colors.Add(Colors.Green);
            _colors.Add(Colors.Blue);
            _colors.Add(Colors.Yellow);
            _colors.Add(Colors.BlueViolet);
            _colors.Add(Colors.Brown);
            _colors.Add(Colors.BurlyWood);
            _colors.Add(Colors.DarkCyan);
            _colors.Add(Colors.Orange);
            _colors.Add(Colors.SteelBlue);
            _colors.Add(Colors.YellowGreen);
            _colors.Add(Colors.AliceBlue);
            _colors.Add(Colors.AntiqueWhite);

            _nextColor = _colors[0];
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of the colorgenerator
        /// </summary>
        /// <value>The instance.</value>
        public static ColorGenerator Instance
        {
            get
            {
                if (!_isInitialized)
                {
                    lock (_syncLock)
                    {
                        if (!_isInitialized)
                        {
                            _instance = new ColorGenerator();
                            _instance.Initialize();
                            _isInitialized = true;
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the next color.
        /// </summary>
        /// <returns></returns>
        public Color GetNext()
        {
            Color result = _nextColor;
            int index = _colors.IndexOf(result);
            if (index == _colors.Count - 1)
            {
                _nextColor = _colors[0];
            }
            else
            {
                _nextColor = _colors[index + 1];
            }

            return result;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
        }

        #endregion Methods
    }
}