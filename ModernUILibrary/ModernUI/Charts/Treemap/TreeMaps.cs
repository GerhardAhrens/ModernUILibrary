
namespace ModernIU.Controls.Chart
{
    using System.Windows;
    using System.Windows.Controls;

    public class TreeMaps : ItemsControl
    {
        public static DependencyProperty TreeMapModeProperty
          = DependencyProperty.Register("TreeMapMode", typeof(TreeMapAlgo), typeof(TreeMaps), new FrameworkPropertyMetadata(TreeMapAlgo.TreeMap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static DependencyProperty ValuePropertyNameProperty
          = DependencyProperty.Register("ValuePropertyName", typeof(string), typeof(TreeMaps), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static DependencyProperty MaxDepthProperty
          = DependencyProperty.Register("MaxDepth", typeof(int), typeof(TreeMaps), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinAreaProperty
          = DependencyProperty.Register("MinArea", typeof(int), typeof(TreeMaps), new FrameworkPropertyMetadata(64, FrameworkPropertyMetadataOptions.AffectsRender));

        static TreeMaps()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeMaps), new FrameworkPropertyMetadata(typeof(TreeMaps)));
        }

        public TreeMapAlgo TreeMapMode
        {
            get { return (TreeMapAlgo)this.GetValue(TreeMaps.TreeMapModeProperty); }
            set { this.SetValue(TreeMaps.TreeMapModeProperty, value); }
        }

        public int MaxDepth
        {
            get { return (int)this.GetValue(TreeMaps.MaxDepthProperty); }
            set { this.SetValue(TreeMaps.MaxDepthProperty, value); }
        }

        public int MinArea
        {
            get { return (int)this.GetValue(TreeMaps.MinAreaProperty); }
            set { this.SetValue(TreeMaps.MinAreaProperty, value); }
        }

        public string ValuePropertyName
        {
            get { return (string)this.GetValue(TreeMaps.ValuePropertyNameProperty); }
            set { this.SetValue(TreeMaps.ValuePropertyNameProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeMapItem(1, this.MaxDepth, this.MinArea, this.ValuePropertyName);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TreeMapItem);
        }
    }

    public enum TreeMapAlgo
    {
        Standard,
        TreeMap
    }
}
