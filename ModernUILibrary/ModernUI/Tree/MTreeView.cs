namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MTreeView : TreeView
    {
        /*private CheckBox PART_CheckBox;*/

        #region IsShowCheckBox
        public static readonly DependencyProperty IsShowCheckBoxProperty = DependencyProperty.Register(nameof(IsShowCheckBox) , typeof(bool), typeof(MTreeView), new PropertyMetadata(false));

        public bool IsShowCheckBox
        {
            get { return (bool)GetValue(IsShowCheckBoxProperty); }
            set { SetValue(IsShowCheckBoxProperty, value); }
        }

        #endregion

        #region Constructors
        static MTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTreeView), new FrameworkPropertyMetadata(typeof(MTreeView)));
        }
        #endregion

        #region Override

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            MTreeViewItem treeViewItem = element as MTreeViewItem;
            treeViewItem.TreeNodeType = EnumTreeNodeType.RootNode;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MTreeViewItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion
    }
}
