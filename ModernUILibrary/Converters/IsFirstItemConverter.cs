namespace ModernIU.Converters
{
    using System.Windows.Controls;
    using System.Windows.Data;

    public class IsFirstItemConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ContentControl contentPresenter = value[0] as ContentControl;
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(contentPresenter);

            bool flag = false;
            if(itemsControl != null)
            {
                int index = itemsControl.ItemContainerGenerator.IndexFromContainer(contentPresenter);
                flag = (index == 0);
            }

            return flag;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
