namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für CascaderBoxControlsUC.xaml
    /// </summary>
    public partial class CascaderBoxControlsUC : UserControl
    {
        public CascaderBoxControlsUC()
        {
            this.InitializeComponent();
            //WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

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

            this.treeView.ItemsSource = datas;
            this.treeView.ChildMemberPath = "Children";
            this.treeView.DisplayMemberPath = "Name";
            this.treeView.SelectedItem = item;

            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
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

            this.treeView.ItemsSource = datas;
            this.treeView.ChildMemberPath = "Children";
            this.treeView.DisplayMemberPath = "Name";
            this.treeView.SelectedItem = item;

        }
    }
}
