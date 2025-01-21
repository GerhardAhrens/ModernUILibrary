namespace ModernMVVMDemo.View
{
    using System.Windows.Controls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für ContentA_UC.xaml
    /// </summary>
    public partial class ContentA_UC : UserControlBase
    {
        public ContentA_UC() : base(typeof(ContentA_UC))
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
    }
}
