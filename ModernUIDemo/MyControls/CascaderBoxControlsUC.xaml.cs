namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für CascaderBoxControlsUC.xaml
    /// </summary>
    public partial class CascaderBoxControlsUC : UserControl, INotifyPropertyChanged
    {
        public CascaderBoxControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
            this.TreeData = this.GetDemoDate();
        }

        private ObservableCollection<Dept> _TreeData;

        public ObservableCollection<Dept> TreeData
        {
            get { return _TreeData; }
            set 
            { 
                _TreeData = value; 
                this.OnPropertyChanged();
            }
        }

        private object _SelectedItem;

        public object SelectedItem
        {
            get { return _SelectedItem; }
            set 
            { 
                _SelectedItem = value;
                this.OnPropertyChanged();
            }
        }


        private ObservableCollection<Dept> GetDemoDate()
        {
            ObservableCollection<Dept> datas = new ObservableCollection<Dept>();
            object item = new object();

            for (int i = 0; i < 10; i++)
            {
                Dept dept = new Dept();
                dept.ID = i.ToString();
                dept.Name = "erste Ebene" + i;
                if (i % 2 == 0)
                {
                    dept.Children = new ObservableCollection<Dept>();
                    for (int j = 0; j < 5; j++)
                    {
                        Dept child = new Dept();
                        child.ID = i.ToString() + j.ToString();
                        child.Name = "Stufe 2 Stufe 2 Stufe 2 Stufe 2" + i.ToString() + j.ToString();



                        if (j % 2 == 0)
                        {
                            if (j == 0)
                            {
                                child.Name = "Stufe 2, Level 2" + i.ToString() + j.ToString();
                            }
                            child.Children = new ObservableCollection<Dept>();
                            for (int k = 0; k < 2; k++)
                            {
                                Dept three = new Dept();
                                three.ID = i.ToString() + j.ToString() + k.ToString();
                                three.Name = "Stufe 2 Stufe 2" + i.ToString() + j.ToString() + k.ToString();
                                child.Children.Add(three);
                            }
                        }
                        dept.Children.Add(child);

                        if (i == 0 && j == 0)
                        {
                            item = dept;
                        }
                    }
                }

                datas.Add(dept);
            }

            return datas;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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
