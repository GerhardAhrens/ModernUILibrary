namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaktionslogik für ColorControlsUC.xaml
    /// </summary>
    public partial class ColorControlsUC : UserControl
    {
        public ColorControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, MouseWheelEventArgs>.AddHandler(this, "PreviewMouseWheel", this.OnPreviewMouseWheel);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
            {
                if (e.Delta > 0)
                {
                    if (this.Scalefactor.ScaleX <= 2.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX + 0.20;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY + 0.20;
                    }
                }

                if (e.Delta < 0)
                {
                    if (this.Scalefactor.ScaleX > 1.00)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX - 0.20;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY - 0.20;
                    }
                }
            }
        }
    }
}
