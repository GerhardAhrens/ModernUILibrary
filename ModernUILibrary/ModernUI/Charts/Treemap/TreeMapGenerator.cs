namespace ModernIU.Controls.Chart
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    [DebuggerDisplay("Label={this.Label}; Percentage={this.Percentage}")]
    public class TreeMapItem
    {
        public string Label { get; set; }
        public double Percentage { get; set; }
        public Brush Background { get; set; }
        public Brush Foreground { get; set; } = Brushes.Black;
        public object Id { get; set; }

        public override string ToString()
        {
            return $"{this.Label};{this.Percentage}";
        }
    }

    public class TreeMapControl : Canvas
    {
        private readonly Random random = new();
        private Popup currentPopup;
        private Brush pendingPopupBrush;
        private Grid pendingPopupContainer;
        private TreeMapItem pendingPopupItem;
        private DispatcherTimer popupTimer;

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<TreeMapItem>),
                typeof(TreeMapControl),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(TreeMapItem),
                typeof(TreeMapControl),
                new PropertyMetadata(null, OnSelectedItemChanged));

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(
                nameof(ItemClickCommand),
                typeof(ICommand),
                typeof(TreeMapControl));

        public static readonly DependencyProperty PopupDelayProperty =
            DependencyProperty.Register(
                nameof(PopupDelay),
                typeof(int),
                typeof(TreeMapControl),
                new PropertyMetadata(500)); // Default 500ms delay

        #endregion

        public TreeMapControl()
        {
            this.InitializePopup();
            this.InitializeTimer();
        }

        #region Properties

        public IEnumerable<TreeMapItem> ItemsSource
        {
            get => (IEnumerable<TreeMapItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public TreeMapItem SelectedItem
        {
            get => (TreeMapItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        public int PopupDelay
        {
            get => (int)GetValue(PopupDelayProperty);
            set => SetValue(PopupDelayProperty, value);
        }

        #endregion

        private void InitializePopup()
        {
            this.currentPopup = new Popup
            {
                AllowsTransparency = true,
                Placement = PlacementMode.Relative,
                PopupAnimation = PopupAnimation.Fade
            };
        }

        private void InitializeTimer()
        {
            this.popupTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(PopupDelay)
            };

            this.popupTimer.Tick += this.PopupTimer_Tick;
        }

        private void PopupTimer_Tick(object sender, EventArgs e)
        {
            this.popupTimer.Stop();
            if (this.pendingPopupItem != null && this.pendingPopupContainer != null)
            {
                ShowPopup(this.pendingPopupItem, this.pendingPopupContainer, this.pendingPopupBrush);
            }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeMapControl control)
            {
                control.UpdateTreeMap();
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeMapControl control)
            {
                control.UpdateHighlight();
            }
        }

        private void UpdateHighlight()
        {
            const double dimOpacity = 0.7;
            foreach (var child in Children)
            {
                if (child is Grid container)
                {
                    var item = container.Tag as TreeMapItem;
                    container.Opacity = SelectedItem == null || item == SelectedItem ? 1.0 : dimOpacity;
                }
            }
        }

        private void UpdateTreeMap()
        {
            this.Children.Clear();
            if (this.ItemsSource == null)
            {
                return;
            }

            List<TreeMapItem> items = new List<TreeMapItem>(ItemsSource);
            if (items == null || items.Count == 0)
            {
                return;
            }

            items.Sort((a, b) => b.Percentage.CompareTo(a.Percentage));
            this.LayoutItems(items);
        }

        private void LayoutItems(List<TreeMapItem> items)
        {
            double totalArea = ActualWidth * ActualHeight;
            double totalPercentage = items.Sum(item => item.Percentage);
            double currentX = 0, currentY = 0;
            double remainingWidth = ActualWidth, remainingHeight = ActualHeight;

            foreach (TreeMapItem item in items)
            {
                var (container, width, height) = CreateItemContainer(item, totalArea, totalPercentage, remainingWidth, remainingHeight);

                SetLeft(container, currentX);
                SetTop(container, currentY);
                Children.Add(container);

                if (remainingWidth > remainingHeight)
                {
                    currentX += width;
                    remainingWidth -= width;
                }
                else
                {
                    currentY += height;
                    remainingHeight -= height;
                }
            }
        }

        private (Grid container, double width, double height) CreateItemContainer(TreeMapItem item, double totalArea, double totalPercentage, double remainingWidth, double remainingHeight)
        {
            double percentage = item.Percentage / totalPercentage;
            double area = totalArea * percentage;
            double width, height;

            if (remainingWidth > remainingHeight)
            {
                width = area / remainingHeight;
                height = remainingHeight;
                if (width > remainingWidth)
                {
                    width = remainingWidth;
                    height = area / remainingWidth;
                }
            }
            else
            {
                height = area / remainingWidth;
                width = remainingWidth;
                if (height > remainingHeight)
                {
                    height = remainingHeight;
                    width = area / remainingHeight;
                }
            }

            var container = CreateVisualContainer(item, width, height);
            return (container, width, height);
        }

        private Grid CreateVisualContainer(TreeMapItem item, double width, double height)
        {
            var brush = item.Background ?? GetRandomBrush();
            var container = new Grid
            {
                Width = width,
                Height = height,
                Cursor = Cursors.Hand
            };

            var rect = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = brush,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };

            var label = new TextBlock
            {
                Text = $"{item.Label}\n{item.Percentage:F1}%",
                Foreground = item.Foreground,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    BlurRadius = 2,
                    Direction = 320,
                    ShadowDepth = 1,
                    Opacity = 0.8
                },
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5)
            };

            container.Children.Add(rect);
            container.Children.Add(label);
            container.Tag = item;

            // Event handlers
            container.MouseEnter += (s, e) => StartPopupTimer(item, container, brush);
            container.MouseLeave += (s, e) => CancelAndHidePopup();
            container.MouseMove += Container_MouseMove;
            container.MouseLeftButtonUp += Container_MouseLeftButtonUp;

            return container;
        }

        private void StartPopupTimer(TreeMapItem item, Grid container, Brush brush)
        {
            this.pendingPopupItem = item;
            this.pendingPopupContainer = container;
            this.pendingPopupBrush = brush;
            this.popupTimer.Start();
        }

        private void CancelAndHidePopup()
        {
            this.popupTimer.Stop();
            this.pendingPopupItem = null;
            this.pendingPopupContainer = null;
            this.pendingPopupBrush = null;
            this.HidePopup();
        }

        private void HidePopup()
        {
            this.currentPopup.IsOpen = false;
        }

        private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid container && container.Tag is TreeMapItem item)
            {
                SelectedItem = SelectedItem == item ? null : item;
                ItemClickCommand?.Execute(SelectedItem);
            }
        }

        private void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.currentPopup.IsOpen == true)
            {
                this.AdjustCurrentPopupPlacement();
            }
        }

        private void AdjustCurrentPopupPlacement()
        {
            var window = Window.GetWindow(this);
            var position = Mouse.GetPosition(window); 
            var popupSize = this.currentPopup.Child.DesiredSize;

            // Get screen position of the window and mouse position
            var cursorScreenPosition = window.PointToScreen(position);

            double offsetX = -50;
            double offsetY = -50;

            if (cursorScreenPosition.X + popupSize.Width + offsetX > SystemParameters.WorkArea.Width)
            {
                offsetX = -popupSize.Width - 10;
            }

            if (cursorScreenPosition.Y + popupSize.Height + offsetY > SystemParameters.WorkArea.Height)
            {
                offsetY = -popupSize.Height - 10;
            }

            this.currentPopup.HorizontalOffset = position.X + offsetX;
            this.currentPopup.VerticalOffset = position.Y + offsetY;
        }

        private Brush GetRandomBrush()
        {
            return new SolidColorBrush(Color.FromRgb((byte)random.Next(100, 200), (byte)random.Next(100, 200), (byte)random.Next(100, 200)));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            this.UpdateTreeMap();
        }

        private void ShowPopup(TreeMapItem item, Grid sourceContainer, Brush background)
        {
            Border popupContent = new Border
            {
                Background = background,
                BorderBrush = item.Foreground,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(10)
            };

            TextBlock popupText = new TextBlock
            {
                Text = $"{item.Label}\nPercentage: {item.Percentage:F2}%",
                Foreground = item.Foreground,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            };

            popupContent.Child = popupText;
            this.currentPopup.Child = popupContent;
            this.currentPopup.PlacementTarget = this;
            this.AdjustCurrentPopupPlacement();
            this.currentPopup.IsOpen = true;
        }
    }
}
 