
namespace ModernIU.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using ModernIU.Base;

    internal sealed class MessageBoxModule : Window
    {
        private Button PART_CloseButton;
        private Storyboard openStoryboard;
        private Storyboard closedStoryboard;

        public static readonly DependencyProperty TypeProperty;
        public static readonly DependencyProperty MessageTextProperty;
        public static readonly DependencyProperty ButtonCollectionProperty;
        public static readonly DependencyProperty YesButtonTextProperty;
        public static readonly DependencyProperty NoButtonTextProperty;
        public static readonly DependencyProperty OkButtonTextProperty;
        public static readonly DependencyProperty CancelButtonTextProperty;

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MessageBoxModule), new PropertyMetadata(new CornerRadius(3d)));

        public EnumPromptType Type
        {
            get { return (EnumPromptType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public string MessageText
        {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public ObservableCollection<Button> ButtonCollection
        {
            get { return (ObservableCollection<Button>)GetValue(ButtonCollectionProperty); }
            private set { SetValue(ButtonCollectionProperty, value); }
        }

        public string YesButtonText
        {
            get { return (string)GetValue(YesButtonTextProperty); }
            set { SetValue(YesButtonTextProperty, value); }
        }

        public string NoButtonText
        {
            get { return (string)GetValue(NoButtonTextProperty); }
            set { SetValue(NoButtonTextProperty, value); }
        }

        public string OkButtonText
        {
            get { return (string)GetValue(OkButtonTextProperty); }
            set { SetValue(OkButtonTextProperty, value); }
        }

        public string CancelButtonText
        {
            get { return (string)GetValue(CancelButtonTextProperty); }
            set { SetValue(CancelButtonTextProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #region Constructors
        static MessageBoxModule()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxModule), new FrameworkPropertyMetadata(typeof(MessageBoxModule)));
            MessageBoxModule.TypeProperty = DependencyProperty.Register("Type", typeof(EnumPromptType), typeof(MessageBoxModule), new PropertyMetadata(EnumPromptType.Info));
            MessageBoxModule.MessageTextProperty = DependencyProperty.Register("MessageText", typeof(string), typeof(MessageBoxModule));
            MessageBoxModule.ButtonCollectionProperty = DependencyProperty.Register("ButtonCollection", typeof(ObservableCollection<Button>), typeof(MessageBoxModule));
            MessageBoxModule.YesButtonTextProperty = DependencyProperty.Register("YesButtonText", typeof(string), typeof(MessageBoxModule), new PropertyMetadata("是"));
            MessageBoxModule.NoButtonTextProperty = DependencyProperty.Register("NoButtonText", typeof(string), typeof(MessageBoxModule), new PropertyMetadata("否"));
            MessageBoxModule.OkButtonTextProperty = DependencyProperty.Register("OkButtonText", typeof(string), typeof(MessageBoxModule), new PropertyMetadata("确定"));
            MessageBoxModule.CancelButtonTextProperty = DependencyProperty.Register("CancelButtonText", typeof(string), typeof(MessageBoxModule), new PropertyMetadata("取消"));
        }

        public MessageBoxModule()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.AllowsTransparency = true;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ShowInTaskbar = false;
            this.Topmost = false;
            this.ButtonCollection = new ObservableCollection<Button>();

            this.Loaded += MessageBoxModule_Loaded;
        }

        private void MessageBoxModule_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Loaded", true);


        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_CloseButton = this.GetTemplateChild("PART_CloseButton") as Button;
            Grid s = this.GetTemplateChild("grid") as Grid;


            if (this.PART_CloseButton != null)
            {
                this.PART_CloseButton.Click += CloseWindow;
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.PART_CloseButton.Click -= CloseWindow;
            this.Close();
        }

        public static MessageBoxResult Show(string messageBoxText)
        {
            return MessageBoxModule.Show(messageBoxText, "");
        }

        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return MessageBoxModule.Show(messageBoxText, caption, MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return MessageBoxModule.Show(null, messageBoxText, caption, button);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return MessageBoxModule.Show(owner, messageBoxText, "", MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBoxResult defaultResult = MessageBoxResult.None;
            switch (button)
            {
                case MessageBoxButton.OK:
                    defaultResult = MessageBoxResult.OK;
                    break;
                case MessageBoxButton.OKCancel:
                    defaultResult = MessageBoxResult.Cancel;
                    break;
                case MessageBoxButton.YesNoCancel:
                    defaultResult = MessageBoxResult.Cancel;
                    break;
                case MessageBoxButton.YesNo:
                    defaultResult = MessageBoxResult.No;
                    break;
                default:
                    break;
            }

            return MessageBoxModule.Show(owner, messageBoxText, caption, button, defaultResult);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption , MessageBoxButton button, MessageBoxResult defaultResult)
        {
            return MessageBoxModule.Show(owner, messageBoxText, caption, button, defaultResult, EnumPromptType.Info);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption , MessageBoxButton button, MessageBoxResult defaultResult, EnumPromptType type)
        {
            MessageBoxModule messageBox = new MessageBoxModule();

            if (owner != null)
            {
                Grid layer = new Grid() { Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)) };
                UIElement original = owner.Content as UIElement;
                owner.Content = null;
                Grid container = new Grid();
                container.Children.Add(original);
                container.Children.Add(layer);
                owner.Content = container;
                messageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                messageBox.ShowInTaskbar = false;
            }

            messageBox.Owner = owner;
            messageBox.Title = caption;
            messageBox.MessageText = messageBoxText;
            messageBox.Type = type;

            switch (button)
            {
                case MessageBoxButton.OK:
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Ok", FlatButtonSkinEnum.primary, MessageBoxResult.OK));
                    break;
                case MessageBoxButton.OKCancel:
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Abbruch", FlatButtonSkinEnum.ghost, MessageBoxResult.Cancel));
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Ok", FlatButtonSkinEnum.primary, MessageBoxResult.OK));
                    break;
                case MessageBoxButton.YesNoCancel:
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Abbruch", FlatButtonSkinEnum.Default, MessageBoxResult.Cancel));
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Nein", FlatButtonSkinEnum.ghost, MessageBoxResult.No));
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Ja", FlatButtonSkinEnum.primary, MessageBoxResult.Yes));
                    break;
                case MessageBoxButton.YesNo:
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Nein", FlatButtonSkinEnum.ghost, MessageBoxResult.No));
                    messageBox.ButtonCollection.Add(CreateButton(messageBox, "Ja", FlatButtonSkinEnum.primary, MessageBoxResult.Yes));
                    break;
                default:
                    break;
            }

            bool? result = messageBox.ShowDialog();
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    {
                        return result == true ? MessageBoxResult.OK : result == false ? MessageBoxResult.Cancel : MessageBoxResult.None;
                    }
                case MessageBoxButton.YesNo:
                    {
                        return result == true ? MessageBoxResult.Yes : MessageBoxResult.No;
                    }
                case MessageBoxButton.YesNoCancel:
                    {
                        return result == true ? MessageBoxResult.Yes : result == false ? MessageBoxResult.No :  MessageBoxResult.Cancel;
                    }
                case MessageBoxButton.OK:
                default:
                    {
                        return result == true ? MessageBoxResult.OK : MessageBoxResult.None;
                    }
            }
        }

        public static MessageBoxResult Show(string messageBoxText, EnumPromptType type)
        {
            return Show(messageBoxText, string.Empty, type);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, EnumPromptType type)
        {
            return MessageBoxModule.Show(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxResult.OK, type);
        }
        #endregion

        private static string GetDefaultCaption(EnumPromptType type)
        {
            string caption = string.Empty;
            switch (type)
            {
                case EnumPromptType.Info:
                    caption = "Auf etwas aufmerksam machen";
                    break;
                case EnumPromptType.Warn:
                    caption = "Eine Warnung";
                    break;
                case EnumPromptType.Error:
                    caption = "Ein Fehler";
                    break;
                case EnumPromptType.Success:
                    caption = "Erledigt";
                    break;
                default:
                    break;
            }

            return caption;
        }

        private static Button CreateButton(MessageBoxModule messageBox, string content, FlatButtonSkinEnum buttonType
            , MessageBoxResult dialogResult)
        {
            FlatButton button = new FlatButton();
            button.Content = content;
            button.Type = buttonType;
            //button.Width = 70;
            button.Height = 30;
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.Margin = new Thickness(10, 0, 10, 10);
            button.Click += (o, e) =>
            {
                bool? flag = null;
                switch (dialogResult)
                {
                    case MessageBoxResult.None:
                        flag = null;
                        break;
                    case MessageBoxResult.OK:
                        flag = true;
                        break;
                    case MessageBoxResult.Cancel:
                        flag = false;
                        break;
                    case MessageBoxResult.Yes:
                        flag = true;
                        break;
                    case MessageBoxResult.No:
                        flag = false;
                        break;
                    default:
                        break;
                }
                messageBox.DialogResult = flag;
            };

            return button;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.Owner != null)
            {
                Grid grid = this.Owner.Content as Grid;
                UIElement original = VisualTreeHelper.GetChild(grid, 0) as UIElement;
                grid.Children.Remove(original);
                this.Owner.Content = original;
            }

            VisualStateManager.GoToState(this, "Closed", true);
        }
    }

    public class MMessageBox
    {
        public static MessageBoxResult Show(string messageBoxText)
        {
            return MessageBoxModule.Show(messageBoxText);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return MessageBoxModule.Show(messageBoxText, caption);
        }

        public static MessageBoxResult Show(string messageBoxText, EnumPromptType type)
        {
            return MessageBoxModule.Show(messageBoxText, type);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, EnumPromptType type)
        {
            return MessageBoxModule.Show(messageBoxText, caption, type);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return MessageBoxModule.Show(owner, messageBoxText);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            return MessageBoxModule.Show(owner, messageBoxText, caption, MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return MessageBoxModule.Show(messageBoxText, caption, button);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            return MessageBoxModule.Show(owner, messageBoxText, caption, button);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, EnumPromptType type)
        {
            return MessageBoxModule.Show(owner, messageBoxText, caption, button, MessageBoxResult.OK, type);
        }
    }
}