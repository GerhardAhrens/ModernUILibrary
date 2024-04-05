namespace ModernUIDemo.MyControls
{
    using System.Data;
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
            list.Add("Vielichet");
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

        }
    }
}
