//-----------------------------------------------------------------------
// <copyright file="MultiSelectListbox.cs" company="Lifeprojects.de">
//     Class: MultiSelectListbox
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>16.02.2024 14:38:09</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    using ModernBaseLibrary.Reader;

    using ModernIU.Base;

    public class MultiSelectListbox : ListBox
    {
        private Type boundType;

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultiSelectListbox), new PropertyMetadata(false, OnIsReadOnlyChanged));
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultiSelectListbox), new PropertyMetadata(OnSelectedItemsListChange));
        private static MultiSelectListbox self;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectListbox"/> class.
        /// </summary>
        public MultiSelectListbox()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.SelectionMode = SelectionMode.Extended;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            WeakEventManager<MultiSelectListbox, RoutedEventArgs>.AddHandler(this, "Loaded", OnLoaded);
            WeakEventManager<MultiSelectListbox, SelectionChangedEventArgs>.AddHandler(this, "SelectionChanged", OnSelectionChanged);
            self = this;

            /* Spezifisches Kontextmenü für Control übergeben */
            this.ContextMenu = this.BuildContextMenu();
        }

        static MultiSelectListbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectListbox), new FrameworkPropertyMetadata(typeof(ListBox)));
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static IList GetSelectedItemsList(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemsListProperty) as IList;
        }

        public static void SetSelectedItemsList(DependencyObject obj, IList value)
        {
            Type boundType = (obj as MultiSelectListbox).boundType;
            if (boundType != null)
            {
                Type genericType = typeof(List<>);
                Type returnType = genericType.MakeGenericType(boundType);

                IList castValue = System.Activator.CreateInstance(returnType) as IList;
                foreach (object o in value)
                {
                    castValue.Add(Convert.ChangeType(o, boundType));
                }

                obj.SetValue(SelectedItemsListProperty, castValue);
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            MultiSelectListbox lb = sender as MultiSelectListbox;
            if (lb != null)
            {
                BindingExpression be = lb.GetBindingExpression(SelectedItemsListProperty);
                if (be != null)
                {
                    FrameworkElement rs = be.ResolvedSource as FrameworkElement;
                    if (rs != null)
                    {
                        PropertyInfo lpi = rs.DataContext.GetType().GetProperty(be.ResolvedSourcePropertyName);
                        lb.boundType = lpi.PropertyType.GetGenericArguments().FirstOrDefault();

                        DataTemplate dt = new DataTemplate(typeof(MultiSelectListbox));
                        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(StackPanel))
                        { 
                            Name = "ItemTemplate" 
                        };

                        fef.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                        FrameworkElementFactory cb = new FrameworkElementFactory(typeof(CheckBox));
                        cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(CheckedChanged));
                        cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(CheckedChanged));
                        cb.SetValue(CheckBox.MarginProperty, new Thickness(0,0,5,0));
                        fef.AppendChild(cb);

                        FrameworkElementFactory tb = new FrameworkElementFactory(typeof(TextBlock));
                        tb.SetBinding(TextBlock.TextProperty, new Binding(lb.DisplayMemberPath));
                        fef.AppendChild(tb);

                        dt.VisualTree = fef;

                        lb.DisplayMemberPath = null;
                        lb.ItemTemplate = dt;

                        if (be.ParentBinding.Mode != BindingMode.TwoWay)
                        {
                            throw new NotSupportedException("SelectedItemsList Binding Mode must be TwoWay");
                        }
                    }
                }
            }
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MultiSelectListbox lb = sender as MultiSelectListbox;
            if (lb != null)
            {
                IList selectedItemsList = GetSelectedItemsList(lb);
                if (lb.SelectedItems == null)
                {
                    return;
                }
                if (selectedItemsList == null)
                {
                    selectedItemsList = new List<object>();
                }

                foreach (object addedItem in e.AddedItems)
                {
                    selectedItemsList.Add(addedItem);
                    ListBoxItem lbi = lb.ItemContainerGenerator.ContainerFromItem(addedItem) as ListBoxItem;
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = true;
                }

                foreach (object removedItem in e.RemovedItems)
                {
                    selectedItemsList.Remove(removedItem);
                    ListBoxItem lbi = lb.ItemContainerGenerator.ContainerFromItem(removedItem) as ListBoxItem;
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = false;
                }

                SetSelectedItemsList(lb, selectedItemsList);
            }
        }

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MultiSelectListbox)d;
                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.IsEnabled = false;
                    }
                    else
                    {
                        control.IsEnabled = true;
                    }
                }
            }
        }

        private static void OnSelectedItemsListChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MultiSelectListbox)d;
                if (e.NewValue.GetType().IsGenericType == true)
                {
                    var selectItems = (IList)e.NewValue;
                    foreach (var item in control.ItemsSource)
                    {
                        foreach (var selectItem in selectItems)
                        {
                            if (item.Equals(selectItem) == true)
                            {
                                ListBoxItem lbi = control.ItemContainerGenerator.ContainerFromItem(selectItem) as ListBoxItem;
                                WeakEventManager<MultiSelectListbox, SelectionChangedEventArgs>.RemoveHandler(self, "SelectionChanged", OnSelectionChanged);
                                FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = true;
                                WeakEventManager<MultiSelectListbox, SelectionChangedEventArgs>.AddHandler(self, "SelectionChanged", OnSelectionChanged);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spezifisches Kontextmenü erstellen
        /// </summary>
        /// <returns></returns>
        private ContextMenu BuildContextMenu()
        {
            ContextMenu textBoxContextMenu = new ContextMenu();

            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "Alle Kopieren";
            copyMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopy);
            textBoxContextMenu.Items.Add(copyMenu);

            MenuItem allCheckMenu = new MenuItem();
            allCheckMenu.Header = "Alle markieren";
            allCheckMenu.Icon = Icons.GetPathGeometry(Icons.IconCheckAll);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(allCheckMenu, "Click", this.OnCheckAll);
            textBoxContextMenu.Items.Add(allCheckMenu);

            MenuItem copyCheckMenu = new MenuItem();
            copyCheckMenu.Header = "Markierte Kopieren";
            copyCheckMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyCheckMenu, "Click", this.OnCheckCopy);
            textBoxContextMenu.Items.Add(copyCheckMenu);

            MenuItem unCheckMenu = new MenuItem();
            unCheckMenu.Header = "Keine markieren";
            unCheckMenu.Icon = Icons.GetPathGeometry(Icons.IconUnCheck);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(unCheckMenu, "Click", this.OnUnCheck);
            textBoxContextMenu.Items.Add(unCheckMenu);

            return textBoxContextMenu;
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object item in self.ItemsSource)
            {
                ListBoxItem lbi = self.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (lbi != null)
                {
                    sb.Append(lbi.Content.ToString()).Append(";");
                }
            }

            Clipboard.SetText(sb.ToString());
        }

        private void OnCheckAll(object sender, RoutedEventArgs e)
        {
            foreach (object item in self.ItemsSource)
            {
                ListBoxItem lbi = self.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (lbi != null)
                {
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = true;
                }
            }
        }

        private void OnCheckCopy(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object item in self.ItemsSource)
            {
                ListBoxItem lbi = self.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (lbi != null)
                {
                    if (FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked == true)
                    {
                        sb.Append(lbi.Content.ToString()).Append(";");
                    }
                }
            }

            Clipboard.SetText(sb.ToString());
        }

        private void OnUnCheck(object sender, RoutedEventArgs e)
        {
            foreach (object item in self.ItemsSource)
            {
                ListBoxItem lbi = self.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (lbi != null)
                {
                    FindChildrenOfType<CheckBox>(lbi as DependencyObject).First().IsChecked = false;
                }
            }
        }

        private static void CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ListBoxItem lbi = GetParentOfType<ListBoxItem>(cb);
            lbi.IsSelected = cb.IsChecked.Value;
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightYellow });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
        }

        private static T GetParentOfType<T>(DependencyObject control) where T : System.Windows.DependencyObject
        {
            DependencyObject ParentControl = control;

            do
                ParentControl = VisualTreeHelper.GetParent(ParentControl);
            while (ParentControl != null && !(ParentControl is T));

            return ParentControl as T;
        }

        private static List<T> FindChildrenOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> Children = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        Children.Add(child as T);
                    Children.AddRange(FindChildrenOfType<T>(child));
                }
            }
            return Children;
        }
    }
}
