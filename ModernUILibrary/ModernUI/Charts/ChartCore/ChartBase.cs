namespace ModernIU.Controls.Chart
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;


    public class ChartBase : Control
    {
        #region Fields

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption",
            typeof(string),
            typeof(ChartBase),
            new PropertyMetadata("EasyChart"));

        public static readonly DependencyProperty ChartBackgroundBorderColorProperty =
            DependencyProperty.Register("ChartBackgroundBorderColor",
            typeof(Brush),
            typeof(ChartBase),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xB1, 0xB1, 0x4A))));

        public static readonly DependencyProperty ChartBackgroundBorderThicknessProperty =
            DependencyProperty.Register("ChartBackgroundBorderThickness",
            typeof(Thickness),
            typeof(ChartBase),
            new PropertyMetadata(new Thickness(1, 1, 1, 1)));

        public static readonly DependencyProperty ChartBackgroundCornerRadiusProperty =
            DependencyProperty.Register("ChartBackgroundCornerRadius",
            typeof(CornerRadius),
            typeof(ChartBase),
            new PropertyMetadata(new CornerRadius(3, 3, 0, 0)));

        public static readonly DependencyProperty ChartBackgroundProperty =
            DependencyProperty.Register("ChartBackground",
            typeof(Brush),
            typeof(ChartBase),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xD9, 0xD9, 0x98))));

        public static readonly DependencyProperty ChartBorderColorProperty =
            DependencyProperty.Register("ChartBorderColor",
            typeof(Brush),
            typeof(ChartBase),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))));

        public static readonly DependencyProperty ChartBorderThicknessProperty =
            DependencyProperty.Register("ChartBorderThickness",
            typeof(Thickness),
            typeof(ChartBase),
            new PropertyMetadata(new Thickness(0.5, 0.5, 0.5, 0.5)));

        public static readonly DependencyProperty ChartColorMouseOverProperty =
            DependencyProperty.Register("ChartColorMouseOver",
            typeof(Brush),
            typeof(ChartBase),
            new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public static readonly DependencyProperty ChartColorProperty =
            DependencyProperty.Register("ChartColor",
            typeof(Brush),
            typeof(ChartBase),
            new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public static readonly DependencyProperty ChartLegendProperty =
            DependencyProperty.Register("ChartLegend",
            typeof(ICollection),
            typeof(ChartBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ChartLegendVisibilityProperty =
            DependencyProperty.Register("ChartLegendVisibility",
            typeof(Visibility),
            typeof(ChartBase),
            new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty ChartMarginProperty =
            DependencyProperty.Register("ChartMargin",
            typeof(Thickness),
            typeof(ChartBase),
            new PropertyMetadata(new Thickness(1, 1, 1, 1)));
        #endregion Fields

        #region Constructors

        public ChartBase()
        {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ChartBase_DataContextChanged);

            ChartLegend = new List<ChartLegendItem>();

            InitializeChartComponent();
            InitializeChartData();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get
            {
                return (string)GetValue(CaptionProperty);
            }
            set
            {
                SetValue(CaptionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart background.
        /// </summary>
        /// <value>The chart background.</value>
        public Brush ChartBackground
        {
            get
            {
                return (Brush)GetValue(ChartBackgroundProperty);
            }
            set
            {
                SetValue(ChartBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the chart background border.
        /// </summary>
        /// <value>The color of the chart background border.</value>
        public Brush ChartBackgroundBorderColor
        {
            get
            {
                return (Brush)GetValue(ChartBackgroundBorderColorProperty);
            }
            set
            {
                SetValue(ChartBackgroundBorderColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart background border thickness.
        /// </summary>
        /// <value>The chart background border thickness.</value>
        public Thickness ChartBackgroundBorderThickness
        {
            get
            {
                return (Thickness)GetValue(ChartBackgroundBorderThicknessProperty);
            }
            set
            {
                SetValue(ChartBackgroundBorderThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart background corner radius.
        /// </summary>
        /// <value>The chart background corner radius.</value>
        public CornerRadius ChartBackgroundCornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(ChartBackgroundCornerRadiusProperty);
            }
            set
            {
                SetValue(ChartBackgroundCornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart margin.
        /// </summary>
        /// <value>The chart margin.</value>
        public Thickness ChartMargin
        {
            get
            {
                return (Thickness)GetValue(ChartMarginProperty);
            }
            set
            {
                SetValue(ChartMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the chart border.
        /// </summary>
        /// <value>The color of the chart border.</value>
        public Brush ChartBorderColor
        {
            get
            {
                return (Brush)GetValue(ChartBorderColorProperty);
            }
            set
            {
                SetValue(ChartBorderColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart border thickness.
        /// </summary>
        /// <value>The chart border thickness.</value>
        public Thickness ChartBorderThickness
        {
            get
            {
                return (Thickness)GetValue(ChartBorderThicknessProperty);
            }
            set
            {
                SetValue(ChartBorderThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the chart.
        /// </summary>
        /// <value>The color of the chart.</value>
        public Brush ChartColor
        {
            get
            {
                return (Brush)GetValue(ChartColorProperty);
            }
            set
            {
                SetValue(ChartColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart color mouse over.
        /// </summary>
        /// <value>The chart color mouse over.</value>
        public Brush ChartColorMouseOver
        {
            get
            {
                return (Brush)GetValue(ChartColorMouseOverProperty);
            }
            set
            {
                SetValue(ChartColorMouseOverProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart legend.
        /// </summary>
        /// <value>The chart legend.</value>
        public ICollection ChartLegend
        {
            get
            {
                return (ICollection)GetValue(ChartLegendProperty);
            }
            set
            {
                SetValue(ChartLegendProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the chart legend visibility.
        /// </summary>
        /// <value>The chart legend visibility.</value>
        public Visibility ChartLegendVisibility
        {
            get
            {
                return (Visibility)GetValue(ChartLegendVisibilityProperty);
            }
            set
            {
                SetValue(ChartLegendVisibilityProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the DataContextChanged event of the ChartBase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ChartBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (myCollectionView != null)
            {
                foreach (object item in myCollectionView)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        INotifyPropertyChanged observable = (INotifyPropertyChanged)item;
                        observable.PropertyChanged += new PropertyChangedEventHandler(observable_PropertyChanged);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the chart component.
        /// </summary>
        private void InitializeChartComponent()
        {
            LinearGradientBrush chartBrush = new LinearGradientBrush();
            chartBrush.StartPoint = new Point(0, 0.032);
            chartBrush.EndPoint = new Point(0, 1);
            chartBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 186, 218, 243), 1));
            chartBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 208, 240, 252), 0));

            LinearGradientBrush chartMouseOverBrush = new LinearGradientBrush();
            chartMouseOverBrush.StartPoint = new Point(0, 0.03);
            chartMouseOverBrush.EndPoint = new Point(0, 1);
            chartMouseOverBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 202, 233, 255), 1));
            chartMouseOverBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 221, 245, 255), 0));

            ChartColor = chartBrush;
            ChartColorMouseOver = chartMouseOverBrush;
        }

        /// <summary>
        /// Initializes the chart data.
        /// </summary>
        private void InitializeChartData()
        {
            List<ChartRow> tempRows = new List<ChartRow>();
            tempRows.Add(new ChartRow("Beispiel 1", 20, new Color()));
            tempRows.Add(new ChartRow("Beispiel 2", 35, new Color()));
            tempRows.Add(new ChartRow("Beispiel 3", 80, new Color()));
            tempRows.Add(new ChartRow("Beispiel 4", 65, new Color()));
            tempRows.Add(new ChartRow("Beispiel 5", 50, new Color()));

            DataContext = tempRows;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the observable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (myCollectionView != null)
            {
                myCollectionView.Refresh();
            }
        }

        #endregion Methods
    }
}