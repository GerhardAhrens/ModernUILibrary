//-----------------------------------------------------------------------
// <copyright file="ComboTree.cs" company="Lifeprojects.de">
//     Class: ComboTree
//     Copyright © Gerhard Ahrens, 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.09.2024</date>
//
// <summary>Class for UI Control ComboBoxEx</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using ModernIU.BehaviorsBase;

    public class ComboTree : ItemsControl
    {
        private System.Windows.Controls.TreeView PART_TreeView;
        private Popup PART_Popup;
        private List<object> selectedList = new List<object>();

        #region DependencyProperty

        #region MaxDropDownHeight

        public double MaxDropDownHeight
        {
            get { return (double)GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }
        
        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register(nameof(MaxDropDownHeight), typeof(double), typeof(ComboTree), new PropertyMetadata(300d));

        #endregion

        #region Content

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(ComboTree), new PropertyMetadata(null));

        #endregion

        #region SelectedItem

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ComboTree), new PropertyMetadata(null));

        #endregion

        #region SelectedValue

        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(ComboTree));

        #endregion

        #region SelectedValuePath

        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register(nameof(SelectedValuePath), typeof(string), typeof(ComboTree), new PropertyMetadata(string.Empty));

        #endregion

        #region IsDropDownOpen

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(ComboTree), new PropertyMetadata(false));

        #endregion

        #region DisplayMemberPath

        [Bindable(true)]
        public new string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        
        public static readonly new DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(ComboTree), new PropertyMetadata(string.Empty));

        #endregion

        #region IsCloseWhenSelected

        public bool IsCloseWhenSelected
        {
            get { return (bool)GetValue(IsCloseWhenSelectedProperty); }
            set { SetValue(IsCloseWhenSelectedProperty, value); }
        }
        
        public static readonly DependencyProperty IsCloseWhenSelectedProperty =
            DependencyProperty.Register(nameof(IsCloseWhenSelected), typeof(bool), typeof(ComboTree), new PropertyMetadata(true));

        #endregion

        #endregion

        #region Constructors

        static ComboTree()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboTree), new FrameworkPropertyMetadata(typeof(ComboTree)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_TreeView = this.GetTemplateChild("PART_TreeView") as System.Windows.Controls.TreeView;
            this.PART_Popup = this.GetTemplateChild("PART_Popup") as Popup;
            if(this.PART_Popup != null)
            {
                this.PART_Popup.Opened += PART_Popup_Opened;
            }
            this.AddHandler(TreeViewItem.MouseDoubleClickEvent, new RoutedEventHandler(TreeNode_MouseDoubleClick), true);
        }
        
        #endregion

        #region private function

        private object GetPropertyValue(object obj, string path)
        {
            Type type = obj.GetType();
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(path);
            return propertyInfo.GetValue(obj, null);
        }

        private void SetNodeSelected(System.Windows.Controls.ItemsControl targetItemContainer)
        {
            if (targetItemContainer == null) return;
            if (targetItemContainer.Items == null) return;
            foreach (var item in targetItemContainer.Items)
            {
                TreeViewItem treeItem = targetItemContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem == null) continue;

                if (this.SelectedValue.Equals(this.GetPropertyValue(item, this.SelectedValuePath)))
                {
                    treeItem.IsExpanded = true;
                    treeItem.IsSelected = true;
                }
                else
                {
                    this.SetNodeSelected(treeItem);
                }
            }
        }

        private TreeViewItem GetNode(System.Windows.Controls.ItemsControl targetItemContainer)
        {
            if (targetItemContainer == null) return null;
            if (targetItemContainer.Items == null) return null;
            foreach (var item in targetItemContainer.Items)
            {
                TreeViewItem treeItem = targetItemContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem == null) continue;

                if (this.SelectedItem == item)
                {
                    return treeItem;
                }
                else
                {
                    //SetNodeSelected(treeItem);
                }
            }

            return null;
        }

        #endregion

        #region Event Implement Function

        private void TreeNode_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (this.PART_TreeView == null)
            {
                return;
            }

            if (this.PART_TreeView.SelectedItem == null)
            {
                return;
            }

            this.SelectedItem = this.PART_TreeView.SelectedItem;

            TreeViewItem treeViewItem = this.GetNode(this.PART_TreeView);
            this.SetSelected(treeViewItem);

            this.IsDropDownOpen = !this.IsCloseWhenSelected;

            this.Content = string.IsNullOrEmpty(this.DisplayMemberPath) ? this.SelectedItem : this.GetPropertyValue(this.SelectedItem, this.DisplayMemberPath);
            this.SelectedValue = string.IsNullOrEmpty(this.SelectedValuePath) ? string.Empty : this.GetPropertyValue(this.SelectedItem, this.SelectedValuePath);
        }

        private void SetSelected(TreeViewItem item)
        {
            while ((item = item.GetAncestor<TreeViewItem>()) != null)
            {
                this.selectedList.Insert(0, item.DataContext);
            }
        }

        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            if (this.SelectedValue != null && !string.IsNullOrEmpty(this.SelectedValuePath))
            {
                this.SetNodeSelected(this.PART_TreeView);
            }
        }
        #endregion
    }
}
