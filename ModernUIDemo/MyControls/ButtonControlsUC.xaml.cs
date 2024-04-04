namespace ModernUIDemo.MyControls
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;

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
            list.Add("全部");
            list.Add("主任医师");
            list.Add("副主任医师");
            list.Add("住院医生");
            list.Add("其他");

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

    }
}
