namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;

    [TemplatePart(Name = "PART_InnerHyperlink", Type = typeof(Hyperlink))]
    public class LinkLabel : Label
    {
        public static readonly DependencyProperty UrlProperty = null;
        public static readonly DependencyProperty HyperlinkStyleProperty = null;
        public static readonly DependencyProperty HoverForegroundProperty = null;
        public static readonly DependencyProperty LinkLabelBehaviorProperty = null;
        public static readonly DependencyProperty CommandProperty = null;
        public static readonly DependencyProperty CommandParameterProperty = null;
        public static readonly DependencyProperty CommandTargetProperty = null;

        [Category("Behavior")]
        public static readonly RoutedEvent ClickEvent;

        [Category("Behavior")]
        public static readonly RoutedEvent RequestNavigateEvent;

        static LinkLabel()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkLabel),  new FrameworkPropertyMetadata(typeof(LinkLabel)));

            ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LinkLabel));
            RequestNavigateEvent = EventManager.RegisterRoutedEvent(nameof(RequestNavigate), RoutingStrategy.Bubble, typeof(RequestNavigateEventHandler), typeof(LinkLabel));

            UrlProperty = DependencyProperty.Register(nameof(Url), typeof(Uri), typeof(LinkLabel));
            HyperlinkStyleProperty = DependencyProperty.Register(nameof(HyperlinkStyle), typeof(Style), typeof(LinkLabel));
            HoverForegroundProperty = DependencyProperty.Register(nameof(HoverForeground), typeof(Brush), typeof(LinkLabel));
            LinkLabelBehaviorProperty = DependencyProperty.Register(nameof(LinkLabelBehavior), typeof(LinkLabelBehavior), typeof(LinkLabel));
            CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(LinkLabel));
            CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(LinkLabel));
            CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(LinkLabel));
        }

        [Category("Common Properties"), Bindable(true)]
        public Uri Url
        {
            get { return GetValue(UrlProperty) as Uri; }
            set { SetValue(UrlProperty, value); }
        }

        public Style HyperlinkStyle
        {
            get { return GetValue(HyperlinkStyleProperty) as Style; }
            set { SetValue(HyperlinkStyleProperty, value); }
        }

        [Category("Brushes"), Bindable(true)]
        public Brush HoverForeground
        {
            get { return GetValue(HoverForegroundProperty) as Brush; }
            set { SetValue(HoverForegroundProperty, value); }
        }

        [Category("Common Properties"), Bindable(true)]
        public LinkLabelBehavior LinkLabelBehavior
        {
            get { return (LinkLabelBehavior)GetValue(LinkLabelBehaviorProperty); }
            set { SetValue(LinkLabelBehaviorProperty, value); }
        }

        [Localizability(LocalizationCategory.NeverLocalize), Bindable(true), Category("Action")]
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        [Localizability(LocalizationCategory.NeverLocalize), Bindable(true), Category("Action")]
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        [Bindable(true), Category("Action")]
        public IInputElement CommandTarget
        {
            get { return (IInputElement)this.GetValue(CommandTargetProperty); }
            set { this.SetValue(CommandTargetProperty, value); }
        }

        public event RoutedEventHandler Click
        {
            add
            {
                base.AddHandler(ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(ClickEvent, value);
            }
        }

        public event RequestNavigateEventHandler RequestNavigate
        {
            add
            {
                base.AddHandler(RequestNavigateEvent, value);
            }
            remove
            {
                base.RemoveHandler(RequestNavigateEvent, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Hyperlink innerHyperlink = GetTemplateChild("PART_InnerHyperlink") as Hyperlink;
            if (innerHyperlink != null)
            {
                innerHyperlink.Click += new RoutedEventHandler(this.InnerHyperlink_Click);
                innerHyperlink.RequestNavigate += new RequestNavigateEventHandler(this.InnerHyperlink_RequestNavigate);
            }
        }

        private void InnerHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            RequestNavigateEventArgs args = new RequestNavigateEventArgs(e.Uri, String.Empty);
            args.Source = this;
            args.RoutedEvent = LinkLabel.RequestNavigateEvent;
            this.RaiseEvent(args);
        }

        
        private void InnerHyperlink_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(LinkLabel.ClickEvent, this));
        }
    }
}
