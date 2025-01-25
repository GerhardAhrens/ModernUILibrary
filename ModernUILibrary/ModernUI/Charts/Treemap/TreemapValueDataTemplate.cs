namespace ModernIU.Controls.Chart
{
    using System.Windows;
    using System.Windows.Data;

    public class TreemapValueDataTemplate : HierarchicalDataTemplate
    {
        private BindingBase _valueBinding;

        public TreemapValueDataTemplate()
        {
        }

        public TreemapValueDataTemplate(object dataType) : base(dataType)
        {
        }

        public BindingBase AreaValue
        {
            get { return _valueBinding; }
            set { _valueBinding = value; }
        }
    }
}
