//-----------------------------------------------------------------------
// <copyright file="MTreeViewItem.cs" company="Lifeprojects.de">
//     Class: MTreeViewItem
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    public enum EnumTreeNodeType
    {
        RootNode,
        MiddleNode,
        LeafNode,
    }
    public class MTreeViewItem : TreeViewItem
    {
        #region DependencyProperty

        #region TreeNodeType

        public EnumTreeNodeType TreeNodeType
        {
            get { return (EnumTreeNodeType)GetValue(TreeNodeTypeProperty); }
            set { SetValue(TreeNodeTypeProperty, value); }
        }
        
        public static readonly DependencyProperty TreeNodeTypeProperty =
            DependencyProperty.Register(nameof(TreeNodeType), typeof(EnumTreeNodeType), typeof(MTreeViewItem), new PropertyMetadata(EnumTreeNodeType.RootNode));

        #endregion

        #endregion

        #region Constructors

        static MTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTreeViewItem), new FrameworkPropertyMetadata(typeof(MTreeViewItem)));
        }

        #endregion

        #region Override

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            MTreeViewItem treeViewItem = element as MTreeViewItem;

            if (treeViewItem.HasItems)
            {
                treeViewItem.TreeNodeType = EnumTreeNodeType.MiddleNode;
            }
            else
            {
                treeViewItem.TreeNodeType = EnumTreeNodeType.LeafNode;
            }
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
