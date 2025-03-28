namespace ModernUIDemo.MyControls
{
    using System.Data;
    using System.Net.WebSockets;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für ButtonControlsUC.xaml
    /// </summary>
    public partial class ButtonControlsUC : UserControl
    {
        DataTable dt = new DataTable();

        public ButtonControlsUC()
        {
            this.InitializeComponent();

            List<string> list = new List<string>();
            list.Add("Alles");
            list.Add("Nix");
            list.Add("Vieleicht");
            list.Add("Oder nicht");
            list.Add("Nee");

            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");
            DataRow dr = dt.NewRow();
            dr["ID"] = "1";
            dr["NAME"] = "Gerhard";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["ID"] = "2";
            dr["NAME"] = "Charlie";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["ID"] = "3";
            dr["NAME"] = "Buddy";
            dt.Rows.Add(dr);

            this.BtnGroupButtonSource.ItemsSource = list;
            this.BtnGroupButtonDataTable.ItemsSource = dt.AsDataView();
        }

        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FlatButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SegmentItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SegmentItem si = (SegmentItem)sender;
            MessageBox.Show(si.Name);
        }

        private void BtnGroupButtonClick_ItemClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ButtonGroup si = (ButtonGroup)sender;

            var content = ((ContentControl)e.NewValue).Content;
            if ((((ContentControl)e.NewValue).Content).GetType() == typeof(string))
            {
                MessageBox.Show($"{si.Name}; {content}");
            }
            else
            {
                var column = ((System.Data.DataRowView)content).Row["NAME"];
                MessageBox.Show($"{si.Name}; {column}");
            }
        }

        private void BtnIconGroupButtonClick_ItemClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ButtonGroup si = (ButtonGroup)sender;
            var content = ((ContentControl)e.NewValue).Content;
            if ((((ContentControl)e.NewValue).Content).GetType() == typeof(System.Windows.Shapes.Path))
            {
                string ucName = ((ContentControl)e.NewValue).Name;
                MessageBox.Show($"{ucName}");
            }
        }
    }
}
