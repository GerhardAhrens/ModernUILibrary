namespace ModernIU.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public class WaterfallPanel : Canvas
    {
        private int column;
        private double listWidth = 180;

        static WaterfallPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaterfallPanel), new FrameworkPropertyMetadata(typeof(WaterfallPanel)));
        }

        public double ListWidth
        {
            get { return listWidth; }
            set
            {
                listWidth = value;
                this.SetColumn();
            }
        }

        public WaterfallPanel()
        {
            this.Loaded += delegate
            {
                this.SetColumn();
                Margin = new Thickness(Margin.Left);
            };

            SizeChanged += delegate
            {
                this.SetColumn();
            };
        }

        private void SetColumn()
        {
            column = (int)(ActualWidth / listWidth);
            if (column <= 0)
            {
                column = 1;
            }
                
            this.Refresh();
        }

        public void Add(FrameworkElement element)
        {
            element.Width = ListWidth;
            if (element is Grid)
            {
                if ((element as Grid).Children.Count > 0)
                {
                    ((element as Grid).Children[0] as FrameworkElement).Margin = new Thickness(Margin.Left);
                }
            }

            this.Children.Add(element);
            this.Refresh();
        }

        public class Point
        {
            public Point(int index, double height, double buttom) { Index = index; Height = height; Buttom = buttom; }

            public int Index { get; set; }
            public double Buttom { get; set; }
            public double Height { get; set; }
        }

        private void Refresh()
        {
            var maxHeight = 0.0;
            var list = new Dictionary<int, Point>();
            var nlist = new Dictionary<int, Dictionary<int, Point>>();

            for (int i = 0; i < this.Children.Count; i++)
            {
                (this.Children[i] as FrameworkElement).UpdateLayout();
                list.Add(i, new Point(i, (this.Children[i] as FrameworkElement).ActualHeight, 0.0));
            }

            for (int i = 0; i < column; i++)
            {
                nlist.Add(i, new Dictionary<int, Point>());
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (i < column)
                {
                    list[i].Buttom = list[i].Height;
                    nlist[i].Add(nlist[i].Count, list[i]);
                }
                else
                {
                    var b = 0.0;
                    var l = 0;
                    for (int j = 0; j < column; j++)
                    {
                        var jh = nlist[j][nlist[j].Count - 1].Buttom + list[i].Height;
                        if (b == 0.0 || jh < b)
                        {
                            b = jh;
                            l = j;
                        }
                    }

                    list[i].Buttom = b;
                    nlist[l].Add(nlist[l].Count, list[i]);
                }
            }

            for (int i = 0; i < nlist.Count; i++)
            {
                for (int j = 0; j < nlist[i].Count; j++)
                {
                    this.Children[nlist[i][j].Index].SetValue(LeftProperty, i * ActualWidth / column);
                    this.Children[nlist[i][j].Index].SetValue(TopProperty, nlist[i][j].Buttom - nlist[i][j].Height);
                    this.Children[nlist[i][j].Index].SetValue(WidthProperty, ActualWidth / column);

                    if (this.Children[nlist[i][j].Index] is Grid)
                    {
                        ((this.Children[nlist[i][j].Index] as Grid).Children[0] as FrameworkElement).Margin = Margin;
                    }
                }

                if (nlist.ContainsKey(i))
                {
                    if (nlist[i].ContainsKey(nlist[i].Count - 1))
                    {
                        var mh = nlist[i][nlist[i].Count - 1].Buttom;
                        maxHeight = mh > maxHeight ? mh : maxHeight;
                    }
                }
            }

            this.Height = maxHeight;
            list.Clear();
            nlist.Clear();
        }

        public void Remove(UIElement element)
        {
            this.Children.Remove(element);
            this.Refresh();
        }
    }
}
