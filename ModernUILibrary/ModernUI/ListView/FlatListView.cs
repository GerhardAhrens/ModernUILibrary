namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class FlatListView : ListView
    {
        static FlatListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatListView), new FrameworkPropertyMetadata(typeof(FlatListView)));
        }
    }
}
