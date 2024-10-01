namespace ModernIU.Controls
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interop;

    using Microsoft.Win32;

    using ModernIU.Base;

    public class ChooseBox : TextBox
    {
        private Button PART_ChooseButton;

        #region DependencyProperty

        #region Title

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ChooseBox), new PropertyMetadata(null));

        #endregion

        #region ChooseButtonStyle
        public Style ChooseButtonStyle
        {
            get { return (Style)GetValue(ChooseButtonStyleProperty); }
            set { SetValue(ChooseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ChooseButtonStyleProperty =
            DependencyProperty.Register(nameof(ChooseButtonStyle), typeof(Style), typeof(ChooseBox));

        #endregion

        #region ChooseBoxType

        public EnumChooseBoxType ChooseBoxType
        {
            get { return (EnumChooseBoxType)GetValue(ChooseBoxTypeProperty); }
            set { SetValue(ChooseBoxTypeProperty, value); }
        }

        public static readonly DependencyProperty ChooseBoxTypeProperty =
            DependencyProperty.Register(nameof(ChooseBoxType), typeof(EnumChooseBoxType), typeof(ChooseBox), new PropertyMetadata(EnumChooseBoxType.SingleFile));

        #endregion

        #region ExtensionFilter

        public string ExtensionFilter
        {
            get { return (string)GetValue(ExtensionFilterProperty); }
            set { SetValue(ExtensionFilterProperty, value); }
        }

        public static readonly DependencyProperty ExtensionFilterProperty =
            DependencyProperty.Register(nameof(ExtensionFilter), typeof(string), typeof(ChooseBox), new PropertyMetadata(string.Empty));

        #endregion

        #region DefaultExtension

        public string DefaultExtension
        {
            get { return (string)GetValue(DefaultExtensionProperty); }
            set { SetValue(DefaultExtensionProperty, value); }
        }

        public static readonly DependencyProperty DefaultExtensionProperty =
            DependencyProperty.Register(nameof(DefaultExtension), typeof(string), typeof(ChooseBox), new PropertyMetadata("Alle|*.*"));

        #endregion

        #region InitialDirectory

        public string InitialDirectory
        {
            get { return (string)GetValue(InitialDirectoryProperty); }
            set { SetValue(InitialDirectoryProperty, value); }
        }

        public static readonly DependencyProperty InitialDirectoryProperty =
            DependencyProperty.Register(nameof(InitialDirectory), typeof(string), typeof(ChooseBox), new PropertyMetadata(null));

        #endregion

        #region ChooseButtonWidth

        public double ChooseButtonWidth
        {
            get { return (double)GetValue(ChooseButtonWidthProperty); }
            set { SetValue(ChooseButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty ChooseButtonWidthProperty =
            DependencyProperty.Register(nameof(ChooseButtonWidth), typeof(double), typeof(ChooseBox), new PropertyMetadata(20d));

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
                WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.PART_ChooseButton, "Click", this.PART_ChooseButton_Click);
            }
        }
        
        #endregion

        #region Event Implement Function
        private void PART_ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            switch (this.ChooseBoxType)
            {
                case EnumChooseBoxType.SingleFile:
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = false;

                    if (string.IsNullOrEmpty(this.Title) == false)
                    {
                        openFileDialog.Title = this.Title;
                    }

                    if (string.IsNullOrEmpty(this.ExtensionFilter) == false)
                    {
                        //Alles|*.*|C# File|*.cs|Xaml-File|*.xaml
                        openFileDialog.Filter = this.ExtensionFilter;
                    }

                    if (string.IsNullOrEmpty(this.DefaultExtension) == false)
                    {
                        openFileDialog.DefaultExt = this.DefaultExtension;
                    }

                    if (string.IsNullOrEmpty(this.InitialDirectory) == false)
                    {
                        openFileDialog.InitialDirectory = this.InitialDirectory;
                    }
                    else 
                    {
                        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string initialDirectory = string.IsNullOrEmpty(this.InitialDirectory) == true ? myDocuments : this.InitialDirectory;
                        openFileDialog.InitialDirectory = initialDirectory;
                    }

                    if (openFileDialog.ShowDialog() == true)
                    {
                        this.Text = openFileDialog.FileName;
                    }

                    break;

                case EnumChooseBoxType.MultiFile:
                    OpenFileDialog openFileDialogMulti = new OpenFileDialog();
                    openFileDialogMulti.Multiselect = true;

                    if (string.IsNullOrEmpty(this.Title) == false)
                    {
                        openFileDialogMulti.Title = this.Title;
                    }

                    if (string.IsNullOrEmpty(this.ExtensionFilter) == false)
                    {
                        //Alles|*.*|C# File|*.cs|Xaml-File|*.xaml
                        openFileDialogMulti.Filter = this.ExtensionFilter;
                    }

                    if (string.IsNullOrEmpty(this.DefaultExtension) == false)
                    {
                        openFileDialogMulti.DefaultExt = this.DefaultExtension;
                    }

                    if (string.IsNullOrEmpty(this.InitialDirectory) == false)
                    {
                        openFileDialogMulti.InitialDirectory = this.InitialDirectory;
                    }
                    else
                    {
                        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string initialDirectory = string.IsNullOrEmpty(this.InitialDirectory) == true ? myDocuments : this.InitialDirectory;
                        openFileDialogMulti.InitialDirectory = initialDirectory;
                    }

                    if (openFileDialogMulti.ShowDialog() == true)
                    {
                        string[] files = openFileDialogMulti.FileNames;
                        this.Text = string.Join('|', files);
                    }

                    break;
                case EnumChooseBoxType.SaveFile:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.CheckFileExists = true;
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.CreatePrompt = false;
                    saveFileDialog.OverwritePrompt = true;

                    if (string.IsNullOrEmpty(this.Title) == false)
                    {
                        saveFileDialog.Title = this.Title;
                    }

                    if (string.IsNullOrEmpty(this.ExtensionFilter) == false)
                    {
                        //Alles|*.*|C# File|*.cs|Xaml-File|*.xaml
                        saveFileDialog.Filter = this.ExtensionFilter;
                    }

                    if (string.IsNullOrEmpty(this.DefaultExtension) == false)
                    {
                        saveFileDialog.DefaultExt = this.DefaultExtension;
                    }
                    else
                    {
                        saveFileDialog.DefaultExt = Path.GetExtension(this.Text);
                    }

                    if (string.IsNullOrEmpty(this.InitialDirectory) == false)
                    {
                        saveFileDialog.InitialDirectory = this.InitialDirectory;
                    }
                    else
                    {
                        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string initialDirectory = string.IsNullOrEmpty(this.InitialDirectory) == true ? myDocuments : this.InitialDirectory;
                        saveFileDialog.InitialDirectory = initialDirectory;
                    }

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        this.Text = saveFileDialog.FileName;
                    }

                    break;
                case EnumChooseBoxType.Folder:
                    OpenFolderDialog folderDialog = new OpenFolderDialog();
                    if (string.IsNullOrEmpty(this.Title) == false)
                    {
                        folderDialog.Title = this.Title;
                    }

                    if (string.IsNullOrEmpty(this.InitialDirectory) == false)
                    {
                        folderDialog.InitialDirectory = this.InitialDirectory;
                    }
                    else
                    {
                        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string initialDirectory = string.IsNullOrEmpty(this.InitialDirectory) == true ? myDocuments : this.InitialDirectory;
                        folderDialog.InitialDirectory = initialDirectory;
                    }

                    if (folderDialog.ShowDialog() == true)
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
