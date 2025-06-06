﻿namespace ModernUIDemo.MyControls
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Xml.Linq;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    using ModernIU.Controls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TreeViewControlsUC.xaml
    /// </summary>
    public partial class TreeViewControlsUC : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> treeSource;
        private string selectedTreeItem = string.Empty;
        private object selectedItem = null;
        public ICommand SelectedTreeItemChanged => new RelayCommand(this.SelectedTreeItemChangedHandler);
        public ICommand CloseTreeItemAllCommand => new RelayCommand(this.CloseTreeItemAllHandler);
        public ICommand ExpandTreeItemAllCommand => new RelayCommand(this.ExpandTreeItemAllHandler);
        public ICommand CloseTreeItemCommand => new RelayCommand(this.CloseTreeItemHandler);
        public ICommand ExpandTreeItemCommand => new RelayCommand(this.ExpandTreeItemHandler);
        public ICommand InsertTreeItemCommand => new RelayCommand(this.InsertTreeItemHandler);
        public ICommand RemoveTreeItemCommand => new RelayCommand(this.RemoveTreeItemHandler);

        public TreeViewControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.treeSource = new ObservableCollection<TreeViewItem>();
            this.DataContext = this;
        }

        public ObservableCollection<TreeViewItem> TreeSource
        {
            get { return this.treeSource; }
            set
            {
                SetField(ref this.treeSource, value);
            }
        }

        public string SelectedTreeItem
        {
            get { return this.selectedTreeItem; }
            set
            {
                SetField(ref this.selectedTreeItem, value);
            }
        }

        public object SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                SetField(ref this.selectedItem, value);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.TreeSource.Clear();
            TreeViewItem tr1 = new TreeViewItem("Root",true);
            this.TreeSource.Add(tr1);

            TreeViewItem level1 = new TreeViewItem("Level-1");
            level1.Add(new TreeViewItem("Level-1-1"),2);
            level1.Add(new TreeViewItem("Level-1-2"), 2);
            this.TreeSource.Add(level1);

            TreeViewItem level2 = new TreeViewItem("Level-2");
            level2.Add(new TreeViewItem("Level-2-1"), 2);
            level2.Add(new TreeViewItem("Level-2-2"), 2, false);
            level2.Add(new TreeViewItem("Level-2-3"), 2);
            this.TreeSource.Add(level2);

            TreeViewItem level3 = new TreeViewItem("Level-3");
            level3.Add(new TreeViewItem("Level-3-1"), 2);
            Guid l3 = level3.ChildTreeItem.LastOrDefault().NodeKey;
            level3.Add(l3, new TreeViewItem("Level-3-1-1"),3);
            level3.Add(l3, new TreeViewItem("Level-3-1-2"), 3);
            Guid l33 = level3.ChildTreeItem.LastOrDefault().ChildTreeItem.LastOrDefault().NodeKey;
            level3.Add(l33, new TreeViewItem("Level-3-1-2-1"),4);
            level3.Add(l3, new TreeViewItem("Level-3-1-3"), 3);
            level3.Add(new TreeViewItem("Level-3-2"), 2);
            this.TreeSource.Add(level3);

            TreeViewItem node = this.TreeViewFind(this.TreeSource, f => f.NodeName == "Level-2-1");
            this.SelectedItem = node;
        }

        #region TreeView Handler
        private void SelectedTreeItemChangedHandler(object obj)
        {
            if (obj != null)
            {
                if (((TreeViewItem)obj).IsEnabled == true)
                {
                    this.SelectedTreeItem = $"NodeName: {((TreeViewItem)obj).NodeName}; Level: {((TreeViewItem)obj).Level}";
                }
            }
        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (args.ButtonState == MouseButtonState.Pressed && args.ClickCount == 1)
            {
                if (((HeaderedItemsControl)sender).Header is TreeViewItem)
                {
                    if (((TreeViewItem)((HeaderedItemsControl)sender).Header).IsSelected == true)
                    {
                        string item = $"NodeName: {((TreeViewItem)((HeaderedItemsControl)sender).Header).NodeName}";
                        MessageBox.Show(item);

                        args.Handled = true;
                    }
                }
            }
        }

        private void CloseTreeItemAllHandler(object obj)
        {
            if (this.TreeSource != null)
            {
                foreach (TreeViewItem treeItem in this.TreeSource.Where(w => w.Level == 1))
                {
                    treeItem.IsExpanded = false;
                }
            }
        }

        private void ExpandTreeItemAllHandler(object obj)
        {
            if (this.TreeSource != null)
            {
                foreach (TreeViewItem treeItem in this.TreeSource.Where(w => w.Level == 1))
                {
                    treeItem.IsExpanded = true;
                }
            }
        }

        private void CloseTreeItemHandler(object obj)
        {
            if (obj != null && obj is TreeViewItem treeItem)
            {
                treeItem.IsExpanded = false;
            }
        }

        private void ExpandTreeItemHandler(object obj)
        {
            if (obj != null && obj is TreeViewItem treeItem)
            {
                treeItem.IsExpanded = true;
            }
        }

        private void InsertTreeItemHandler(object obj)
        {
            if (obj != null && obj is TreeViewItem treeItem)
            {
                if (treeItem.IsRoot == true)
                {
                    int num = this.TreeSource.Count<TreeViewItem>(c => c.Level == treeItem.Level) + 1;
                    TreeViewItem tr = new TreeViewItem($"Level-{num}");
                    this.TreeSource.Add(tr);
                }
                else if (treeItem.IsRoot == false)
                {
                    int treeItemLevel = treeItem.Level;
                    int num = treeItem.ChildTreeItem.Count<TreeViewItem>() + 1;
                    if (treeItem.ChildTreeItem.Count > 0)
                    {
                        int level = treeItem.ChildTreeItem.LastOrDefault().Level;
                        treeItem.Add(new TreeViewItem($"{treeItem.NodeName}-{num}"), level);
                    }
                    else
                    {
                        treeItem.Add(new TreeViewItem($"{treeItem.NodeName}-{num}"), treeItemLevel+1);
                    }
                }
            }
        }

        private void RemoveTreeItemHandler(object obj)
        {
            if (obj != null && obj is TreeViewItem treeItem)
            {
                bool isDeleted = TreeViewFindAndDelete(this.TreeSource, f => f.NodeKey == ((TreeViewItem)obj).NodeKey);
            }
        }

        private bool TreeViewFindAndDelete(ObservableCollection<TreeViewItem> treeSource, Func<TreeViewItem, bool> pPredicate)
        {
            foreach (TreeViewItem current in treeSource)
            {
                if (pPredicate(current) == true)
                {
                    if (current.IsRoot == true)
                    {
                        return false;
                    }

                    return treeSource.Remove(current);
                }
                else
                {
                    if (current.HasChildren == true)
                    {
                        foreach (TreeViewItem child2 in current.ChildTreeItem)
                        {
                            if (pPredicate(child2))
                            {
                                return current.ChildTreeItem.Remove(child2);
                            }
                            else
                            {
                                foreach (TreeViewItem child3 in child2.ChildTreeItem)
                                {
                                    if (pPredicate(child3))
                                    {
                                        return child2.ChildTreeItem.Remove(child3);
                                    }
                                    else
                                    {
                                        foreach (TreeViewItem child4 in child3.ChildTreeItem)
                                        {
                                            if (pPredicate(child4))
                                            {
                                                return child3.ChildTreeItem.Remove(child4);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return default;
        }

        private TreeViewItem TreeViewFind(ObservableCollection<TreeViewItem> treeSource, Func<TreeViewItem, bool> pPredicate)
        {
            foreach (TreeViewItem current in treeSource)
            {
                if (pPredicate(current) == true)
                {
                    return current;
                }
                else
                {
                    if (current.HasChildren == true)
                    {
                        foreach (TreeViewItem child2 in current.ChildTreeItem)
                        {
                            if (pPredicate(child2))
                            {
                                return child2;
                            }
                            else
                            {
                                foreach (TreeViewItem child3 in child2.ChildTreeItem)
                                {
                                    if (pPredicate(child3))
                                    {
                                        return child3;
                                    }
                                    else
                                    {
                                        foreach (TreeViewItem child4 in child3.ChildTreeItem)
                                        {
                                            if (pPredicate(child4))
                                            {
                                                return child4;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return default;
        }

        #endregion TreeView Handler

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }

    [DebuggerDisplay("Key={this.NodeKey};Name={this.NodeName};Level={this.Level}")]
    public class TreeViewItem : INotifyPropertyChanged, ITreeViewItem
    {
        private bool isSelected;
        private bool isExpanded;
        private bool isEnabled = true;
        private ObservableCollection<TreeViewItem> childTreeItem;
        private Guid nodeKey = Guid.Empty;


        public TreeViewItem(string description, int level = 1, bool isEnabled = true)
        {
            this.IsExpanded = true;
            this.IsSelected = false;

            this.NodeKey = Guid.NewGuid();
            this.NodeName = description;
            this.Level = level;
            this.IsEnabled = isEnabled;
            this.NodeSymbol = "[/]";
            this.NodeForeground = Brushes.Black;
            if (isEnabled == false)
            {
                this.NodeForeground = Brushes.LightGray;
            }
        }

        public TreeViewItem(string description, bool isRoot, bool isEnabled = true)
        {
            this.IsExpanded = true;
            this.IsSelected = false;

            this.NodeKey = Guid.NewGuid();
            this.NodeName = description;
            this.Level = 1;
            this.IsEnabled = isEnabled;
            this.IsRoot = isRoot;
            this.NodeSymbol = "[/]";
            this.NodeForeground = Brushes.Black;
            if (isEnabled == false)
            {
                this.NodeForeground = Brushes.LightGray;
            }
        }

        public int Count
        {
            get { return this.childTreeItem.Count; }
        }

        public bool IsRoot { get; private set; }

        public int Level { get; private set; }

        public Guid NodeKey
        {
            get { return this.nodeKey; }
            set
            {
                SetField(ref this.nodeKey, value);
            }
        }

        public string NodeName { get; set; }

        public string NodeSymbol { get; set; }

        public Brush NodeForeground { get; set; }

        public Brush NodeBackground { get; set; }

        public ObservableCollection<TreeViewItem> ChildTreeItem
        {
            get 
            { 
                if (this.childTreeItem == null)
                {
                    this.childTreeItem = new ObservableCollection<TreeViewItem>();
                }

                return this.childTreeItem; 
            }
        }

        public bool HasChildren
        {
            get { return this.childTreeItem == null ? false : this.childTreeItem.Any(); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                SetField(ref this.isEnabled, value);
                if (value == false)
                {
                    this.NodeForeground = Brushes.LightGray;
                    _ = Find(this, value);
                }
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                SetField(ref this.isSelected, value);
            }
        }

        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                SetField(ref this.isExpanded, value);
            }
        }

        public void Add(TreeViewItem childItem, int level = -1, bool isEnabled = true)
        {
            if (level > 0)
            {
                childItem.Level = level;
            }

            childItem.IsEnabled = isEnabled;

            this.ChildTreeItem.Add(childItem);
            this.IsExpanded = true;
            this.IsSelected = false;
        }

        public void Add(Guid childItem, TreeViewItem childItemNext, int level, bool isEnabled = true)
        {
            if (this.ChildTreeItem.Any() == true)
            {
                TreeViewItem node = this.Find(this.ChildTreeItem.LastOrDefault(), childItem);
                if (node != null)
                {
                    childItemNext.Level = level;
                    childItemNext.IsEnabled = isEnabled;
                    node.Add(childItemNext);
                }

                this.IsExpanded = true;
                this.IsSelected = false;
            }
        }

        public TreeViewItem Find(TreeViewItem node)
        {
            if (node == null)
            {
                return null;
            }

            foreach (TreeViewItem child in node.ChildTreeItem)
            {
                TreeViewItem found = Find(child);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private TreeViewItem Find(TreeViewItem node, Guid key)
        {
            if (node == null)
            {
                return null;
            }

            if (node.NodeKey == key)
            {
                return node;
            }

            foreach (TreeViewItem child in node.ChildTreeItem)
            {
                TreeViewItem found = Find(child, key);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private TreeViewItem Find(TreeViewItem node, bool isEnabled)
        {
            if (node == null)
            {
                return null;
            }

            foreach (TreeViewItem child in node.ChildTreeItem)
            {
                child.isEnabled = isEnabled;
                child.NodeForeground = Brushes.LightGray;
                TreeViewItem found = Find(child, isEnabled);
                if (found != null)
                {
                    found.isEnabled = isEnabled;
                    found.NodeForeground = Brushes.LightGray;
                    return found;
                }
            }

            return null;
        }

        public override string ToString()
        {
            if (this.ChildTreeItem != null)
            {
                return $"{string.Join("|", this.ChildTreeItem.Select(x => x.NodeName))}";
            }
            else
            {
                return string.Empty;
            }
        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }
}
