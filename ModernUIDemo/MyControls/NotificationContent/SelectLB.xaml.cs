namespace ModernUIDemo
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für SelectLB.xaml
    /// </summary>
    public partial class SelectLB : UserControl, INotificationServiceMessage
    {
        public SelectLB()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.LBItems.ItemsSource = this.CreateFooItems();
            this.LBItems.SelectedValuePath = "Id";
            this.LBItems.DisplayMemberPath = "Full";
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Yes, LBItems.SelectedItem);
            window.DialogResult = true;
            window.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.No, null);
            window.DialogResult = false;
            window.Close();
        }

        private List<FooItem> CreateFooItems()
        {
            List<FooItem> items = new List<FooItem>();
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Max", Age=36});
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Moriz", Age = 38 });
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Buddy", Age = 31 });
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Micky Mouse", Age = 67 });
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Paulchen Panter", Age = 44 });
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Donald Duck", Age = 55 });
            items.Add(new FooItem() { Id = Guid.NewGuid(), Name = "Kater Karlo", Age = 55 });

            return items;
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Yes, this.LBItems.SelectedItem);
            window.DialogResult = true;
            window.Close();
        }
    }

    public class FooItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public string Full 
        {
            get { return $"Name: {this.Name}; Alter: {this.Age}"; }
        }
    }
}
