namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Xml.Linq;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TreeViewControlsUC.xaml
    /// </summary>
    public partial class TreeViewControlsUC : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> treeSource;
        private string selectedTreeItem = string.Empty;
        public ICommand SelectedTreeItemChanged => new RelayCommand(this.SelectedTreeItemChangedHandler);

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


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem tr1 = new TreeViewItem("Root");
            treeSource.Add(tr1);

            TreeViewItem level1 = new TreeViewItem("Level-1");
            level1.Add(new TreeViewItem("Level-1-1"));
            level1.Add(new TreeViewItem("Level-1-2"));
            treeSource.Add(level1);

            TreeViewItem level2 = new TreeViewItem("Level-2");
            level2.Add(new TreeViewItem("Level-2-1"));
            level2.Add(new TreeViewItem("Level-2-2"));
            level2.Add(new TreeViewItem("Level-2-3"));
            treeSource.Add(level2);

            TreeViewItem level3 = new TreeViewItem("Level-3");
            level3.Add(new TreeViewItem("Level-3-1"));
            Guid l3 = level3.ChildTreeItem.LastOrDefault().NodeKey;
            level3.Add(l3, new TreeViewItem("Level-3-1-1"));
            level3.Add(l3, new TreeViewItem("Level-3-1-2"));
            Guid l33 = level3.ChildTreeItem.LastOrDefault().ChildTreeItem.LastOrDefault().NodeKey;
            level3.Add(l33, new TreeViewItem("Level-3-1-3-1"));
            level3.Add(l3, new TreeViewItem("Level-3-1-3"));
            level3.Add(new TreeViewItem("Level-3-2"));

            treeSource.Add(level3);

        }

        private void SelectedTreeItemChangedHandler(object obj)
        {
            this.SelectedTreeItem = $"NodeName: {((TreeViewItem)obj).NodeName}";
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

    [DebuggerDisplay("Key={this.NodeKey};Name={this.NodeName}")]
    public class TreeViewItem : INotifyPropertyChanged
    {
        private bool isSelected;
        private bool isExpanded;
        private ObservableCollection<TreeViewItem> childTreeItem;
        private Guid nodeKey = Guid.Empty;


        public TreeViewItem(string description)
        {
            this.IsExpanded = true;
            this.IsSelected = false;

            this.NodeKey = Guid.NewGuid();
            this.NodeName = description;
        }

        public int Count
        {
            get { return childTreeItem.Count; }
        }

        public Guid NodeKey
        {
            get { return this.nodeKey; }
            set
            {
                SetField(ref this.nodeKey, value);
            }
        }

        public string NodeName { get; set; }

        public string NodeSymbol { get; set; } = "[/]";

        public ObservableCollection<TreeViewItem> ChildTreeItem
        {
            get 
            { 
                if (childTreeItem == null)
                {
                    childTreeItem = new ObservableCollection<TreeViewItem>();
                }

                return childTreeItem; 
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

        public void Add(TreeViewItem childItem)
        {
            this.ChildTreeItem.Add(childItem);
        }

        public void Add(Guid childItem, TreeViewItem childItemNext)
        {
            if (this.ChildTreeItem.Any() == true)
            {
                TreeViewItem node = this.Find(this.ChildTreeItem.LastOrDefault(), childItem);
                if (node != null)
                {
                    node.Add(childItemNext);
                }
            }
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

        private void SetExpandAll()
        {
            this.IsExpanded = true;
        }

        private void SetCloseAll(bool expand)
        {
            this.IsExpanded = expand;
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
