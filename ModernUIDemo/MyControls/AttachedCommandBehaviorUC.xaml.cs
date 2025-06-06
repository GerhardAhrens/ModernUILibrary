﻿namespace ModernUIDemo.MyControls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Base;

    using ModernUI.MVVM;

    /// <summary>
    /// Interaktionslogik für AttachedCommandBehaviorUC.xaml
    /// </summary>
    public partial class AttachedCommandBehaviorUC : UserControl, INotifyPropertyChanged
    {
        public ICommand MouseDownCommand => new UIButtonCommand(this.MouseDownCommandHandler);

        public CommandBehaviorCollection behaviorCollection = new CommandBehaviorCollection();

        public AttachedCommandBehaviorUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void MouseDownCommandHandler(object obj)
        {
            MessageBox.Show(obj.ToString());
        }


        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }
}
