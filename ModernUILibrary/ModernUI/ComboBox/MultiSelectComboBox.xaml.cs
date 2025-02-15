namespace ModernIU.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<MultiSelectNode> _nodeList = null;
        public static readonly RoutedEvent SelectedItemsEvent;

        public MultiSelectComboBox()
        {
            InitializeComponent();
            this._nodeList = new ObservableCollection<MultiSelectNode>();
        }

        #region Dependency Properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public Dictionary<string, object> ItemsSource
        {
            get { return (Dictionary<string, object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public Dictionary<string, object> SelectedItems
        {
            get { return (Dictionary<string, object>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));


        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsCommandProperty =
            DependencyProperty.Register("SelectedItemsCommand", typeof(ICommand), typeof(MultiSelectComboBox), new PropertyMetadata(null));

        public ICommand SelectedItemsCommand
        {
            get { return (ICommand)GetValue(SelectedItemsCommandProperty); }
            set { SetValue(SelectedItemsCommandProperty, value); }
        }

        #endregion

        #region Events
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            if (clickedBox.Content.ToString() == "Alle" && this._nodeList.Count(x => x.IsSelected == true && x.Title == "Alle") == 1)
            {
                foreach (MultiSelectNode node in this._nodeList)
                {
                    node.IsSelected = true;
                }
            }
            else if (clickedBox.Content.ToString() == "Alle" && this._nodeList.Count(x => x.IsSelected == false && x.Title == "Alle") == 1)
            {
                foreach (MultiSelectNode node in this._nodeList)
                {
                    node.IsSelected = false;
                }
            }
            else
            {
                int selectedCount = 0;
                foreach (MultiSelectNode s in this._nodeList)
                {
                    if (s.IsSelected && s.Title != "Alle")
                    {
                        selectedCount++;
                    }
                }
                if (selectedCount == this._nodeList.Count - 1)
                {
                    this._nodeList.FirstOrDefault(i => i.Title == "Alle").IsSelected = true;
                }
                else
                {
                    this._nodeList.FirstOrDefault(i => i.Title == "Alle").IsSelected = false;
                }
            }

            this.SetSelectedItems();
            this.SetText();

        }
        #endregion


        #region Methods
        private void SelectNodes()
        {
            foreach (KeyValuePair<string, object> keyValue in this.SelectedItems)
            {
                MultiSelectNode node = this._nodeList.FirstOrDefault(i => i.Title == keyValue.Key);
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }

            if (this.SelectedItems != null)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(SelectedItemsEvent);
                ((MultiSelectComboBox)this).RaiseEvent(newEventArgs);
                this.SelectedItemsCommand?.Execute(((MultiSelectComboBox)this).SelectedItems);
            }
        }

        private void SetSelectedItems()
        {
            try
            {
                if (this.SelectedItems == null)
                {
                    this.SelectedItems = new Dictionary<string, object>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            this.SelectedItems.Clear();
            foreach (MultiSelectNode node in _nodeList)
            {
                if (node.IsSelected && node.Title != "Alle")
                {
                    if (this.ItemsSource.Count > 0)
                    {
                        this.SelectedItems.Add(node.Title, this.ItemsSource[node.Title]);
                    }
                }
            }
        }

        private void DisplayInControl()
        {
            _nodeList.Clear();
            if (this.ItemsSource.Count > 0)
            {
                _nodeList.Add(new MultiSelectNode("Alle"));
            }

            foreach (KeyValuePair<string, object> keyValue in this.ItemsSource)
            {
                MultiSelectNode node = new MultiSelectNode(keyValue.Key);
                _nodeList.Add(node);
            }

            MultiSelectCombo.ItemsSource = _nodeList;
        }

        private void SetText()
        {
            if (this.SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (MultiSelectNode s in _nodeList)
                {
                    if (s.IsSelected == true && s.Title == "Alle")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("Alle");
                        break;
                    }
                    else if (s.IsSelected == true && s.Title != "Alle")
                    {
                        displayText.Append(s.Title);
                        displayText.Append(',');
                    }
                }
                this.Text = displayText.ToString().TrimEnd(new char[] { ',' }); 
            }           
            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }       
        #endregion
    }

    public class MultiSelectNode : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private bool _isSelected;

        public MultiSelectNode(string title)
        {
            this.Title = title;
        }

        #region Properties
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
