namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für ComboTreeControlsUC.xaml
    /// </summary>
    public partial class ComboTreeControlsUC : UserControl
    {
        public ComboTreeControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Dept> datas = new ObservableCollection<Dept>();

            for (int i = 0; i < 10; i++)
            {
                Dept dept = new Dept();
                dept.ID = i.ToString();
                dept.Name = "erste Ebene:" + i;
                if (i % 2 == 0)
                {
                    dept.Children = new ObservableCollection<Dept>();
                    for (int j = 0; j < 5; j++)
                    {
                        Dept child = new Dept();
                        child.ID = i.ToString() + j.ToString();
                        child.Name = "zweite Ebene:" + i.ToString() + j.ToString();

                        if (j % 2 == 0)
                        {
                            child.Children = new ObservableCollection<Dept>();
                            for (int k = 0; k < 2; k++)
                            {
                                Dept three = new Dept();
                                three.ID = i.ToString() + j.ToString() + k.ToString();
                                three.Name = "dritte Ebene" + i.ToString() + j.ToString() + k.ToString();
                                child.Children.Add(three);
                            }
                        }

                        dept.Children.Add(child);
                    }
                }

                datas.Add(dept);
            }

            this.comboTree.ItemsSource = datas;
            this.comboTree.DisplayMemberPath = "Name";
            this.comboTree.SelectedValuePath = "ID";
            this.comboTree.SelectedValue = "201";

            this.comboTree2.ItemsSource = datas;
            this.comboTree2.DisplayMemberPath = "Name";
            this.comboTree2.SelectedValuePath = "ID";
            this.comboTree2.SelectedValue = "201";
        }
    }
}
