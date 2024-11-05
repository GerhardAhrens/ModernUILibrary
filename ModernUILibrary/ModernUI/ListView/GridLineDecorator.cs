namespace ModernIU.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    [ContentProperty("Target")]
    public class GridLineDecorator : FrameworkElement
    {
        private ListView _target;
        private DrawingVisual _gridLinesVisual = new DrawingVisual();
        private GridViewHeaderRowPresenter _headerRowPresenter = null;

        public GridLineDecorator()
        {
            this.AddVisualChild(_gridLinesVisual);
            this.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(OnScrollChanged));
        }

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register(nameof(GridLineBrush), typeof(Brush), typeof(GridLineDecorator),
                new FrameworkPropertyMetadata(Brushes.LightGray, new PropertyChangedCallback(OnGridLineBrushChanged)));

        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        protected override int VisualChildrenCount
        {
            get { return Target == null ? 1 : 2; }
        }

        protected override System.Collections.IEnumerator LogicalChildren
        {
            get { yield return Target; }
        }

        public ListView Target
        {
            get { return this._target; }
            set
            {
                if (this._target != value)
                {
                    if (this._target != null)
                    {
                        this.Detach();
                    }

                    this.RemoveVisualChild(this._target);
                    this.RemoveLogicalChild(this._target);

                    this._target = value;

                    this.AddVisualChild(this._target);
                    this.AddLogicalChild(this._target);
                    if (this._target != null)
                    {
                        this.Attach();
                    }

                    this.InvalidateMeasure();
                }
            }
        }

        private static void OnGridLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GridLineDecorator)d).OnGridLineBrushChanged(e);
        }

        protected virtual void OnGridLineBrushChanged(DependencyPropertyChangedEventArgs e)
        {
            this.DrawGridLines();
        }

        private void GetGridViewHeaderPresenter()
        {
            if (this.Target == null)
            {
                this._headerRowPresenter = null;
                return;
            }

            this._headerRowPresenter = this.Target.GetDesendentChild<GridViewHeaderRowPresenter>();
        }

        private void DrawGridLines()
        {
            if (Target == null) return;
            if (_headerRowPresenter == null) return;

            var itemCount = Target.Items.Count;
            if (itemCount == 0) return;

            var gridView = Target.View as GridView;
            if (gridView == null) return;

            var drawingContext = _gridLinesVisual.RenderOpen();
            var startPoint = new Point(0, 0);

            var dpiFactor = this.GetDpiFactor();
            var pen = new Pen(this.GridLineBrush, 1 * dpiFactor);
            var halfPenWidth = pen.Thickness / 2;
            var guidelines = new GuidelineSet();

            var headerOffset = _headerRowPresenter.TranslatePoint(startPoint, this);
            var headerSize = _headerRowPresenter.RenderSize;
            var headerBottomY = headerOffset.Y + headerSize.Height;

            var item0 = _target.ItemContainerGenerator.ContainerFromIndex(0);
            if (item0 == null) return;

            var scrollViewer = item0.GetAncestor<ScrollViewer>();
            if (scrollViewer == null) return;

            var contentElement = scrollViewer.Content as UIElement;
            var maxLineX = scrollViewer.ViewportWidth;
            var maxLineY = headerBottomY + contentElement.RenderSize.Height;

            var vLineY = 0.0;

            for (int i = 0; i < itemCount; i++)
            {
                var item = Target.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                if (item != null)
                {
                    var renderSize = item.RenderSize;
                    var offset = item.TranslatePoint(startPoint, this);

                    var hLineX1 = offset.X;
                    var hLineX2 = offset.X + renderSize.Width;
                    var hLineY = offset.Y + renderSize.Height;
                    vLineY = hLineY;

                    if (hLineY <= headerBottomY) continue;

                    if (hLineY > maxLineY) break;

                    if (hLineX2 > maxLineX) hLineX2 = maxLineX;

                    guidelines.GuidelinesY.Add(hLineY + halfPenWidth);
                    drawingContext.PushGuidelineSet(guidelines);
                    drawingContext.DrawLine(pen, new Point(hLineX1, hLineY), new Point(hLineX2, hLineY));
                    drawingContext.Pop();
                }
            }

            var columns = gridView.Columns;
            var vLineX = headerOffset.X;
            if (vLineY > maxLineY) vLineY = maxLineY;

            foreach (var column in columns)
            {
                var columnWidth = column.GetColumnWidth();
                vLineX += columnWidth;

                if (vLineX > maxLineX) break;

                guidelines.GuidelinesX.Add(vLineX + halfPenWidth);
                drawingContext.PushGuidelineSet(guidelines);
                drawingContext.DrawLine(pen, new Point(vLineX, headerBottomY), new Point(vLineX, vLineY));
                drawingContext.Pop();
            }

            drawingContext.Close();
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return this._target;
            }

            if (index == 1)
            {
                return this._gridLinesVisual;
            }

            throw new IndexOutOfRangeException(string.Format("Index of visual child '{0}' is out of range", index));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Target != null)
            {
                this.Target.Measure(availableSize);
                return this.Target.DesiredSize;
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Target != null)
            {
                this.Target.Arrange(new Rect(new Point(0, 0), finalSize));
            }

            return base.ArrangeOverride(finalSize);
        }

        private void Attach()
        {
            this._target.Loaded += this.OnTargetLoaded;
            this._target.Unloaded += this.OnTargetUnloaded;
            this._target.SizeChanged += this.OnTargetSizeChanged;
        }

        private void Detach()
        {
            this._target.Loaded -= this.OnTargetLoaded;
            this._target.Unloaded -= this.OnTargetUnloaded;
            this._target.SizeChanged -= this.OnTargetSizeChanged;
        }

        private void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            if (_headerRowPresenter == null)
            {
                this.GetGridViewHeaderPresenter();
            }

            this.DrawGridLines();
        }

        private void OnTargetUnloaded(object sender, RoutedEventArgs e)
        {
            this.DrawGridLines();
        }

        private void OnTargetSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.DrawGridLines();
        }

        private void OnScrollChanged(object sender, RoutedEventArgs e)
        {
            this.DrawGridLines();
        }
    }
}
