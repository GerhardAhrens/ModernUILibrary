namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Win32;

    using ModernIU.Base;

    public class ChooseBox : TextBox
    {
        #region private fields
        private Button PART_ChooseButton;
        #endregion

        #region DependencyProperty

        #region ChooseButtonStyle
        public Style ChooseButtonStyle
        {
            get { return (Style)GetValue(ChooseButtonStyleProperty); }
            set { SetValue(ChooseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ChooseButtonStyleProperty =
            DependencyProperty.Register("ChooseButtonStyle", typeof(Style), typeof(ChooseBox));

        #endregion

        #region ChooseBoxType

        public EnumChooseBoxType ChooseBoxType
        {
            get { return (EnumChooseBoxType)GetValue(ChooseBoxTypeProperty); }
            set { SetValue(ChooseBoxTypeProperty, value); }
        }

        public static readonly DependencyProperty ChooseBoxTypeProperty =
            DependencyProperty.Register("ChooseBoxType", typeof(EnumChooseBoxType), typeof(ChooseBox), new PropertyMetadata(EnumChooseBoxType.SingleFile));

        #endregion

        #region ExtensionFilter

        public string ExtensionFilter
        {
            get { return (string)GetValue(ExtensionFilterProperty); }
            set { SetValue(ExtensionFilterProperty, value); }
        }

        public static readonly DependencyProperty ExtensionFilterProperty =
            DependencyProperty.Register("ExtensionFilter", typeof(string), typeof(ChooseBox), new PropertyMetadata(string.Empty));

        #endregion

        #region DefaultExtension

        public string DefaultExtension
        {
            get { return (string)GetValue(DefaultExtensionProperty); }
            set { SetValue(DefaultExtensionProperty, value); }
        }

        public static readonly DependencyProperty DefaultExtensionProperty =
            DependencyProperty.Register("DefaultExtension", typeof(string), typeof(ChooseBox), new PropertyMetadata("Alle|*.*"));

        #endregion

        #region ChooseButtonWidth

        public double ChooseButtonWidth
        {
            get { return (double)GetValue(ChooseButtonWidthProperty); }
            set { SetValue(ChooseButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty ChooseButtonWidthProperty =
            DependencyProperty.Register("ChooseButtonWidth", typeof(double), typeof(ChooseBox), new PropertyMetadata(20d));

        #endregion

        #endregion

        #region Constructors

        static ChooseBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChooseBox), new FrameworkPropertyMetadata(typeof(ChooseBox)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_ChooseButton = this.GetTemplateChild("PART_ChooseButton") as Button;
            if(this.PART_ChooseButton != null)
            {
                this.PART_ChooseButton.Click += PART_ChooseButton_Click;
            }
        }
        
        #endregion

        #region private function

        #endregion

        #region Event Implement Function
        private void PART_ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            switch (this.ChooseBoxType)
            {
                case EnumChooseBoxType.SingleFile:
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = false;

                    if (string.IsNullOrEmpty(this.ExtensionFilter) == false)
                    {
                        //Alles|*.*|C# File|*.cs|Xaml-File|*.xaml
                        openFileDialog.Filter = this.ExtensionFilter;
                    }

                    if (string.IsNullOrEmpty(this.DefaultExtension) == false)
                    {
                        openFileDialog.DefaultExt = this.DefaultExtension;
                    }

                    if (openFileDialog.ShowDialog() == true)
                    {
                        this.Text = openFileDialog.FileName;
                    }

                    break;
                case EnumChooseBoxType.MultiFile:
                    break;
                case EnumChooseBoxType.SaveFile:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.CheckFileExists = true;
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.RestoreDirectory = true;
                    if (string.IsNullOrEmpty(this.ExtensionFilter) == false)
                    {
                        //Alles|*.*|C# File|*.cs|Xaml-File|*.xaml
                        saveFileDialog.Filter = this.ExtensionFilter;
                    }

                    if (string.IsNullOrEmpty(this.DefaultExtension) == false)
                    {
                        saveFileDialog.DefaultExt = this.DefaultExtension;
                    }

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        this.Text = saveFileDialog.FileName;
                    }

                    break;
                case EnumChooseBoxType.Folder:
                    OpenFolderDialog folderDialog = new OpenFolderDialog();
                    if(folderDialog.ShowDialog() == true)
                    {
                        this.Text = folderDialog.FolderName;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
