namespace ModernInsideVM.Views.ContentControls
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernBaseLibrary.Collection;
    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    using ModernInsideVM.Core;

    using ModernIU.Controls;

    using ModernUI.MVVM.Base;

    using static System.Net.Mime.MediaTypeNames;

    /// <summary>
    /// Interaktionslogik für DialogC.xaml
    /// </summary>
    public partial class DialogC : UserControlBase
    {
        private INotificationService notificationService = new NotificationService();

        public DialogC(ChangeViewEventArgs args) : base(typeof(DialogC))
        {
            this.InitializeComponent();
            this.InitCommands();
            this.CtorArgs = args;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, MouseWheelEventArgs>.AddHandler(this, "PreviewMouseWheel", this.OnPreviewMouseWheel);

            this.ValidationErrors = new ObservableDictionary<string, string>();

            this.Id = this.CtorArgs.EntityId.ToString();

            this.DataContext = this;
        }

        #region Properties
        public string Id
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Titel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value, this.CheckContent);
        }

        public string Description
        {
            get => base.GetValue<string>();
            set => base.SetValue(value,this.CheckContent);
        }

        /* Prüfen von Eingaben */
        public ObservableDictionary<string, string> ValidationErrors
        {
            get => base.GetValue<ObservableDictionary<string, string>>();
            set => base.SetValue(value);
        }

        public string ValidationErrorsSelected
        {
            get => base.GetValue<string>();
            set => base.SetValue(value, this.InputNavigationProperty);
        }

        private ChangeViewEventArgs CtorArgs { get; set; }
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);

            WeakEventManager<TitleTextBox, KeyEventArgs>.AddHandler(this.TxtTitel, "KeyDown", this.OnKeyDown);
            WeakEventManager<TitleTextBox, KeyEventArgs>.AddHandler(this.TxtDescription, "KeyDown", this.OnKeyDown);

            this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => { this.TxtTitel.Focus(); }));

            this.RegisterValidations();
            this.LoadDataHandler();
            this.IsUCLoaded = true;
        }

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.Save, new RelayCommand(this.SaveHandler, this.CanSaveHandler));
        }

        private void LoadDataHandler()
        {
            this.Titel = string.Empty;
            this.Description = string.Empty;
        }

        private bool CanSaveHandler(object obj)
        {
            if (ValidationErrors.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SaveHandler(object obj)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Home,
                FromPage = CommandButtons.DialogC,
                RowPosition = this.CtorArgs.RowPosition,
                IsRefresh = true,
            });
        }

        private void DialogBackHandler(object p1)
        {
            if (ValidationErrors.Count > 0)
            {
                NotificationBoxButton result = this.notificationService.IsSavedLastChanges();
                if (result == NotificationBoxButton.Yes)
                {
                    base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
                    {
                        Sender = this.GetType().Name,
                        MenuButton = CommandButtons.Home,
                        FromPage = CommandButtons.DialogC,
                        RowPosition = this.CtorArgs.RowPosition,
                        IsRefresh = true,
                    });
                }
            }
            else
            {
                base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
                {
                    Sender = this.GetType().Name,
                    MenuButton = CommandButtons.Home,
                    FromPage = CommandButtons.DialogC,
                    RowPosition = this.CtorArgs.RowPosition,
                    IsRefresh = false,
                });
            }
        }

        private void CheckContent<T>(T value, string propertyName)
        {
            PropertyInfo propInfo = this.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyName);
            if (propInfo != null)
            {
                this.ChangedContent(true);
            }

            this.ValidationErrors.Clear();
            foreach (string property in this.GetProperties(this))
            {
                Func<Result<string>> function = null;
                if (this.ValidationRules.TryGetValue(property, out function) == true)
                {
                    Result<string> ruleText = this.DoValidation(function, property);
                    if (string.IsNullOrEmpty(ruleText.Value) == false)
                    {
                        this.ValidationErrors.Add(property, ruleText.Value);
                    }
                }
            }
        }

        public override void ChangedContent(bool isPropertyChanged = false)
        {
            this.IsPropertyChanged = isPropertyChanged;
            if (isPropertyChanged == true)
            {
                StatusbarMain.Statusbar.SetNotification($"Geändert");
            }
            else
            {
                StatusbarMain.Statusbar.SetNotification($"Bereit");
            }
        }

        #region Register Validations
        private void RegisterValidations()
        {
            this.ValidationRules.Add(nameof(this.Titel), () =>
            {
                return ValidationRule<DialogC>.This(this).NotEmpty(x => x.Titel, "Titel");
            });

            this.ValidationRules.Add(nameof(this.Description), () =>
            {
                return ValidationRule<DialogC>.This(this).NotEmpty(x => x.Description, "Beschreibung");
            });
        }
        #endregion Register Validations

        /// <summary>
        /// Ausgewähltes Eingabeelement fokusieren und Hintergrundfarbe setzen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        private void InputNavigationProperty<T>(T value, string propertyName)
        {
            this.TxtTitel.Background = Brushes.Transparent;
            this.TxtDescription.Background = Brushes.Transparent;

            if (value == null)
            {
                return;
            }

            foreach (var ctrl in this.GetChildren())
            {
                if (ctrl is TitleTextBox textBox)
                {
                    Binding binding = BindingOperations.GetBinding(textBox, TitleTextBox.TextProperty);
                    if (value != null)
                    {
                        if (binding != null && binding.Path.Path.EndsWith(value.ToString()))
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => 
                            {
                                textBox.Focus();
                                textBox.Background = Brushes.Coral;
                            }));

                            break;
                        }
                    }
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            TitleTextBox tb = sender as TitleTextBox;
            if (tb != null && e.Key == Key.Tab)
            {
                Dictionary<string, Action<object>> dict = new Dictionary<string, Action<object>>();
                dict.Add("TxtTitel", new Action<object>(txt =>
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => { this.TxtDescription.Focus(); }));
                }));
                dict.Add("TxtDescription", new Action<object>(txt =>
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => { this.TxtTitel.Focus(); }));
                }));

                dict[tb.Name].Invoke(null);
            }
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
            {
                if (e.Delta > 0)
                {
                    if (this.Scalefactor.ScaleX <= 2.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX + 0.25;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY + 0.25;
                    }
                }

                if (e.Delta < 0)
                {
                    if (this.Scalefactor.ScaleX > 1.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX - 0.25;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY - 0.25;
                    }
                }
            }
        }
    }
}
