﻿namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxStringControlsUC.xaml
    /// </summary>
    public partial class TextBoxStringControlsUC : UserControl
    {
        public TextBoxStringControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Student> list = new List<Student>();
            /*
            for (int i = 0; i < 1_000; i++)
            {
                list.Add(new Student() { ID = i.ToString(), Name = i.ToString() });
            }*/

            list.Add(new Student() { ID = 1, Name = "gerhard.ahrens@lifeprojects.de" });
            list.Add(new Student() { ID = 2, Name = "gerhard.ahrens@pta.de" });
            list.Add(new Student() { ID = 3, Name = "gahrens@contractors.com" });

            this.AutoCompleteBox.ItemsSource = list;
            this.AutoCompleteBox.DisplayMemberPath = "Name";

        }

        private void IconTextBox_EnterKeyClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IconTextBox si = (IconTextBox)sender;

            var content = ((TextBox)e.OriginalSource).Text;
            MessageBox.Show($"{si.Name}; {content}");
        }
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
