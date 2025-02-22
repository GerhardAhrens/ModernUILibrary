﻿namespace ModernIU.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ModernBaseLibrary.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for TabView.xaml
    /// </summary>
    [ContentProperty("CustomItems")]
    public partial class TabView : UserControl
    {
        private delegate Point GetPosition(IInputElement element);
        private int LastTabClicked;
        private const int PlusWidth = 45; // const indentation from right top for "plus" button
        private const int TabWidthIfOver = 50; // const default width for selected tab when tabs width is too small
        private int DragTabTo; // index of tab that will be replaced when drop happened

        /// <summary>
        /// ContentProperty which contains tabs that will be added to custom tabcontrol
        /// Also, allows added tabs directly from XAML where this control used 
        /// </summary>
        public ObservableCollection<TabItem> CustomItems
        {
            get
            {
                return (ObservableCollection<TabItem>)GetValue(CustomItemsProperty);
            }
            set { SetValue(CustomItemsProperty, value); }
        }

        public static readonly DependencyProperty CustomItemsProperty =
            DependencyProperty.Register("CustomItems", typeof(ObservableCollection<TabItem>), typeof(TabView), new FrameworkPropertyMetadata(new ObservableCollection<TabItem>(),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(CurrentTabItems_PropertyChanged)));

        /// <summary>
        /// callback for entire tabs collection
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void CurrentTabItems_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TabView control = obj as TabView;
            control.Refresh();
        }


        public TabView()
        {
            this.InitializeComponent();
            this.Loaded += Control_Loaded;
            this.DataContext = this;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            CustomItems.CollectionChanged += OnItemCollectionChanged;
            Refresh();
        }

        /// <summary>
        /// callback for each new element of tab collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Adding tabs 
        /// </summary>
        public void Refresh()
        {
            foreach (TabItem item in this.CustomItems)
            {
                if (!MainTabView.Items.Contains(item))
                {
                    AddTabMethod(item as TabItem);
                }
                else
                {
                    continue;
                }
            }

            if (this.CustomItems.Count > 0)
            {
                this.CustomItems.Clear();
            }
        }

        private void MainTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainTabView_SelectionChangedMethod();
        }

        /// <summary>
        /// Setting width "TabWidthIfOver" when tabs width is too small
        /// </summary>
        private void MainTabView_SelectionChangedMethod()
        {
            if (MainTabView.Items.Count <= 1)
            {
                return;
            }

            double TabWidth = this.MainTabView.ActualWidth / (this.MainTabView.Items.Count - 1) - (PlusWidth / (this.MainTabView.Items.Count - 1));
            if (TabWidth < TabWidthIfOver)
            {
                this.UpdateTabsWidth();
                (this.MainTabView.Items[LastTabClicked] as TabItem).Width = TabWidthIfOver;
            }
        }

        /// <summary>
        /// Add tab by pressing "Plus" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.AddTabMethod(null);
        }

        /// <summary>
        /// adding tab
        /// </summary>
        /// <param name="TabForAdd"></param>
        private void AddTabMethod(TabItem TabForAdd)
        {
            TabForAdd = MakeHeaderWithButton(TabForAdd);
            this.MainTabView.Items.Insert(this.MainTabView.Items.Count - 1, TabForAdd);
            this.LastTabClicked = this.MainTabView.Items.Count - 2;
            this.Dispatcher.BeginInvoke((Action)(() => this.MainTabView.SelectedIndex = LastTabClicked));
            this.MainTabView_SelectionChangedMethod();
            this.UpdateTabsWidth();
        }

        /// <summary>
        /// make tab with header and close button
        /// </summary>
        /// <param name="TabForAdd"></param>
        /// <returns></returns>
        private TabItem MakeHeaderWithButton(TabItem TabForAdd)
        {
            Grid TabItemHeaderSP = new Grid();
            TabItemHeaderSP.ColumnDefinitions.Add(new ColumnDefinition());
            TabItemHeaderSP.ColumnDefinitions.Add(new ColumnDefinition());
            //TabItemHeaderSP.Orientation = Orientation.Horizontal;
            string TabSign = (TabForAdd != null) ? TabForAdd.Header as string : "Tab auswählen " + (this.MainTabView.Items.Count);
            TextBlock textBlock = new TextBlock()
            {
                Text = TabSign,
            };

            Assembly a = typeof(TabView).Assembly;
            string assemblyShortName = a.ToString().Split(',')[0];

            Grid.SetColumn(textBlock, 0);
            Button close = new Button();
            Grid.SetColumn(close, 1);
            BitmapImage cross = new BitmapImage();
            cross.BeginInit();
            cross.UriSource = new Uri($"pack://application:,,,/{assemblyShortName};component/Resources/Picture/close.png");
            cross.EndInit();

            Image CloseImg = new Image()
            {
                Source = cross
            };

            CloseImg.Width = 10;
            close.Margin = new Thickness(5, 0, 0, 0);
            close.ToolTip = new ToolTip()
            {
                Content = "Aktueller Tab schließen"
            };

            close.Content = CloseImg;
            close.Click += AnyTab_MouseClick;
            TabItemHeaderSP.Children.Add(textBlock);
            TabItemHeaderSP.Children.Add(close);

            TabForAdd = TabForAdd == null ? new TabItem() : TabForAdd;
            TabForAdd.Header = TabItemHeaderSP;
            TabForAdd.PreviewMouseDown += AnyTab_PreviewMouseDown;
            TabForAdd.MouseMove += AnyTab_MouseMove;
            var keybinding = new KeyBinding
            {
                Key = Key.W,
                Modifiers = ModifierKeys.Control,
                Command = CloseShortcutsCommand,
            };

            TabForAdd.InputBindings.Add( keybinding );
            TabForAdd.ToolTip = new ToolTip()
            {
                Content = TabSign
            };

            return TabForAdd;
        }

        /// <summary>
        /// set LastTabClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnyTab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LastTabClicked = MainTabView.Items.IndexOf(sender as TabItem);
        }

        /// <summary>
        /// Code for drag and drop tabs
        /// </summary>
        #region Drag n Drop
        private void AnyTab_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragTabTo = -1;
                DragTabTo = GetCurrentMark(e.GetPosition);
                Point CurrentCursorPoint = e.GetPosition(MainTabView);
#if DEBUG
                //System.Diagnostics.Debug.WriteLine("x: " + CurrentCursorPoint.X + "; y: " + CurrentCursorPoint.Y);
                System.Diagnostics.Debug.WriteLine("GRAG TO " + DragTabTo);
#endif

                System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.SizeAll);


                if (DragTabTo != -1 && DragTabTo != LastTabClicked && DragTabTo != MainTabView.Items.Count - 1)
                {
                    ReorderTabs();
                    LastTabClicked = DragTabTo;
                    MainTabView_SelectionChangedMethod();
                }
            }
            else
            {
                DragTabTo = -1;
            }
        }

        private void ReorderTabs()
        {
            int from = DragTabTo;
            int to = LastTabClicked;
            if (DragTabTo > LastTabClicked)
            {
                from = LastTabClicked;
                to = DragTabTo;
            }
            for (int i = from; i < to; i++)
            {
                TabItem TabItemTemp = (MainTabView.Items[i] as TabItem);
                MainTabView.Items.RemoveAt(i);
                MainTabView.Items.Insert(i + 1, TabItemTemp);
            }
            MainTabView.SelectedIndex = DragTabTo;
        }

        private bool GetMouseTargetMark(Visual theTarget, GetPosition position)
        {
            if (theTarget == null) return false;
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);

            return rect.Contains(point);
        }
        private int GetCurrentMark(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < MainTabView.Items.Count; i++)
            {
                if (GetMouseTargetMark(MainTabView.Items[i] as TabItem, pos))
                {
                    curIndex = MainTabView.Items.IndexOf(MainTabView.Items[i] as TabItem);
                    break;
                }
            }
            return curIndex;
        }
        #endregion

        /// <summary>
        /// closing tabs by pressing close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnyTab_MouseClick(object sender, EventArgs e)
        {
            CloseTab();
        }
        private void CloseTab()
        {
            if (LastTabClicked == MainTabView.Items.Count - 1) return;
            MainTabView.Items.RemoveAt(LastTabClicked);
            LastTabClicked = MainTabView.Items.Count <= 1 ? 0 : MainTabView.Items.Count - 2;
            Dispatcher.BeginInvoke((Action)(() => MainTabView.SelectedIndex = LastTabClicked));
            MainTabView_SelectionChangedMethod();
            UpdateTabsWidth();
            GC.Collect();
        }

        /// <summary>
        /// update tabs width while resizing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTabView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTabsWidth();
        }
        private void UpdateTabsWidth()
        {
            if (this.MainTabView.Items.Count == 1 || this.MainTabView.ActualWidth <= 0) return;
            double TabWidth = this.MainTabView.ActualWidth / (this.MainTabView.Items.Count - 1) -
                (PlusWidth / (this.MainTabView.Items.Count - 1));

            while (TabWidth * (this.MainTabView.Items.Count - 1) + PlusWidth + TabWidthIfOver > this.MainTabView.ActualWidth - (PlusWidth / (this.MainTabView.Items.Count - 1)))
            {
                TabWidth--;
            }

            for (int i = 0; i < this.MainTabView.Items.Count - 1; i++)
            {
                if (i == this.MainTabView.SelectedIndex && TabWidth < TabWidthIfOver)
                {
                    (this.MainTabView.Items[i] as TabItem).Width = TabWidthIfOver;
                }
                else
                {
                    (this.MainTabView.Items[i] as TabItem).Width = TabWidth;
                }
            }
        }

        /// <summary>
        /// Command for closing tab by Shortcuts ctrl + w
        /// </summary>
        RelayCommand closeShortcutsCommand;
        public RelayCommand CloseShortcutsCommand
        {
            get
            {
                return closeShortcutsCommand ?? (closeShortcutsCommand = new RelayCommand((o) =>
                   {
                       this.CloseTab();
                   }
                   ));
            }
        }

        /// <summary>
        /// Command for adding tab by Shortcuts ctrl + w
        /// </summary>
        RelayCommand addShortcutsCommand;
        public RelayCommand AddShortcutsCommand
        {
            get
            {
                return addShortcutsCommand ?? (addShortcutsCommand = new RelayCommand((o) =>
                   {
                       this.AddTabMethod(null);
                   }
                   ));
            }
        }
    }
}
