
namespace System.Windows
{
    using System.Windows.Data;

    internal static class DependencyObjectExtensions
    {
        public static void SetBinding(this DependencyObject run, object source, DependencyProperty dProp, string propName = null)
        {
            Binding binding = new Binding(propName);
            binding.Source = source;
            BindingOperations.SetBinding(run, dProp, binding);
        }
    }
}
