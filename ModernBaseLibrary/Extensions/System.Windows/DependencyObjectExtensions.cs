
namespace ModernBaseLibrary.Extension
{
    using System.Windows;
    using System.Windows.Data;

    public static class DependencyObjectExtensions
    {
        public static void SetBinding(this DependencyObject run, object source, DependencyProperty dProp, string propName = null)
        {
            Binding binding = new Binding(propName);
            binding.Source = source;
            BindingOperations.SetBinding(run, dProp, binding);
        }
    }
}
