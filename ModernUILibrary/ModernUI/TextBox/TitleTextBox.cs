
namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;

    using ModernIU.Base;

    public enum TitleOrientationEnum
    {
        Horizontal,
        Vertical,
    }

    [TemplatePart(Name = "PART_ClearText", Type = typeof(Path))]
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    public class TitleTextBox : TextBox
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title" , typeof(string), typeof(TitleTextBox));

        public static readonly DependencyProperty IsShowTitleProperty = DependencyProperty.Register("IsShowTitle", typeof(bool), typeof(TitleTextBox), new PropertyMetadata(true));

        public static readonly DependencyProperty CanClearTextProperty = DependencyProperty.Register("CanClearText", typeof(bool), typeof(TitleTextBox));

        public static readonly DependencyProperty TitleOrientationProperty = DependencyProperty.Register("TitleOrientation", typeof(TitleOrientationEnum), typeof(TitleTextBox));

        private Path PART_ClearText;
        private ScrollViewer PART_ScrollViewer;

        static TitleTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleTextBox), new FrameworkPropertyMetadata(typeof(TitleTextBox)));
        }


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool IsShowTitle
        {
            get { return (bool)GetValue(IsShowTitleProperty); }
            set { SetValue(IsShowTitleProperty, value); }
        }


        public bool CanClearText
        {
            get { return (bool)GetValue(CanClearTextProperty); }
            set { SetValue(CanClearTextProperty, value); }
        }

        public TitleOrientationEnum TitleOrientation
        {
            get { return (TitleOrientationEnum)GetValue(TitleOrientationProperty); }
            set { SetValue(TitleOrientationProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_ClearText = VisualHelper.FindVisualElement<Path>(this, "PART_ClearText");
            if(this.PART_ClearText != null)
            {
                this.PART_ClearText.MouseLeftButtonDown += PART_ClearText_MouseLeftButtonDown;
            }

            this.PART_ScrollViewer = VisualHelper.FindVisualElement<ScrollViewer>(this, "PART_ContentHost");

            this.PreviewMouseWheel += TitleTextBox_PreviewMouseWheel;
        }

        private void TitleTextBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if(this.TitleOrientation == TitleOrientationEnum.Vertical && this.PART_ScrollViewer != null)
            {
                this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset - e.Delta);
            }
        }

        private void PART_ClearText_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Text = string.Empty;
        }
    }
}
