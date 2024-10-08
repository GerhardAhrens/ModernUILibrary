﻿//-----------------------------------------------------------------------
// <copyright file="ListBox.cs" company="Lifeprojects.de">
//     Class: ListBox
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.05.2023</date>
//
// <summary>
// Class for UI Control ListBox
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class ListTextBox : Control
    {
        
        private TextBox txtSearchinput;//For user input
        private Popup listTextBoxPopup;// For ListTextBox Popup
        private DataGrid listTextBoxDataGrid;// DataGrid To Display List Of Filtered Items

        public static readonly DependencyProperty ListTextBoxColumnsProperty = DependencyProperty.Register("ListTextBoxColumns", typeof(ObservableCollection<DataGridColumn>), typeof(ListTextBox));
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(IEnumerable<object>), typeof(ListTextBox));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ListTextBox));
        public static readonly DependencyProperty ValueMemberPathProperty = DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(ListTextBox));
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ListTextBox));
        public static readonly DependencyProperty ListTextBoxPlacementDependencyProperty = DependencyProperty.Register("ListTextBoxPlacement", typeof(PlacementMode), typeof(ListTextBox));
        public static readonly DependencyProperty ListTextBoxPlacementTargetDependencyProperty = DependencyProperty.Register("ListTextBoxPlacementTarget", typeof(UIElement), typeof(ListTextBox));
        public static readonly DependencyProperty ListTextBoxHorizontalOffsetDependencyProperty = DependencyProperty.Register("ListTextBoxHorizontalOffset", typeof(double), typeof(ListTextBox));
        public static readonly DependencyProperty ListTextBoxVerticalOffsetDependencyProperty = DependencyProperty.Register("ListTextBoxVerticalOffset", typeof(double), typeof(ListTextBox));
        public static readonly DependencyProperty ListTextBoxWidthDependencyProperty = DependencyProperty.Register("ListTextBoxWidth", typeof(double), typeof(ListTextBox));
        public static readonly DependencyProperty ListTextBoxHeightDependencyProperty = DependencyProperty.Register("ListTextBoxHeight", typeof(double), typeof(ListTextBox));

        public ObservableCollection<DataGridColumn> ListTextBoxColumns
        {
            get
            {
                return (ObservableCollection<DataGridColumn>)GetValue(ListTextBoxColumnsProperty);
            }
            set
            {
                SetValue(ListTextBoxColumnsProperty, value);
            }
        }

        public IEnumerable<object> ItemSource
        {
            get
            {
                return (IEnumerable<object>)GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }

        public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }

        public string ValueMemberPath
        {
            get
            {
                return (string)GetValue(ValueMemberPathProperty);
            }
            set
            {
                SetValue(ValueMemberPathProperty, value);
            }
        }

        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);

                if (value != null)
                {
                    string displayPropertyName = this.DisplayMemberPath;
                    if (string.IsNullOrEmpty(displayPropertyName) == false)
                    {
                        this.txtSearchinput.Text = value.GetType().GetProperty(displayPropertyName).GetValue(value, null).ToString();
                    }
                    else
                    {
                        this.txtSearchinput.Text = value.ToString();
                    }

                    this.txtSearchinput.Select(txtSearchinput.Text.Length, 0);

                    if (this.OnSelectedItemChange != null)
                    {
                        ListTextBoxSelectedEventArgs args = new ListTextBoxSelectedEventArgs();
                        string valuePropertyName = this.ValueMemberPath;
                        if (string.IsNullOrEmpty(valuePropertyName) == false)
                        {
                            args.SelectedValue = value.GetType().GetProperty(valuePropertyName).GetValue(value, null).ToString();
                        }

                        if (string.IsNullOrEmpty(displayPropertyName) == false)
                        {
                            args.SelectedItem = listTextBoxDataGrid.SelectedItem;
                        }
                        else
                        {
                            args.SelectedItem = value;
                        }

                        this.OnSelectedItemChange.Invoke(this, args);
                    }
                }
            }
        }

        public PlacementMode ListTextBoxPlacement
        {
            get
            {
                return (PlacementMode)GetValue(ListTextBoxPlacementDependencyProperty);
            }
            set
            {
                SetValue(ListTextBoxPlacementDependencyProperty, value);
            }
        }

        public UIElement ListTextBoxPlacementTarget
        {
            get
            {
                return (UIElement)GetValue(ListTextBoxPlacementTargetDependencyProperty);
            }
            set
            {
                SetValue(ListTextBoxPlacementTargetDependencyProperty, value);
            }
        }

        public double ListTextBoxHorizontalOffset
        {
            get { return (double)GetValue(ListTextBoxHorizontalOffsetDependencyProperty); }
            set { SetValue(ListTextBoxHorizontalOffsetDependencyProperty, value); }
        }

        public double ListTextBoxVerticalOffset
        {
            get { return (double)GetValue(ListTextBoxVerticalOffsetDependencyProperty); }
            set { SetValue(ListTextBoxVerticalOffsetDependencyProperty, value); }
        }

        public double ListTextBoxWidth
        {
            get { return (double)GetValue(ListTextBoxWidthDependencyProperty); }
            set
            {
                SetValue(ListTextBoxWidthDependencyProperty, value);
            }
        }

        public double ListTextBoxHeight
        {
            get { return (double)GetValue(ListTextBoxHeightDependencyProperty); }
            set
            {
                SetValue(ListTextBoxHeightDependencyProperty, value);
            }
        }

        // allows programmer to filter records as per input
        public event TextChangedEventHandler OnTextChange;

        // to perform operation on ItemSelection Changed
        public event EventHandler<ListTextBoxSelectedEventArgs> OnSelectedItemChange;

        public ListTextBox()
        {
            this.ListTextBoxColumns = new ObservableCollection<DataGridColumn>(); 
        }
               
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.txtSearchinput = this.Template.FindName("ListTextBox_SearchInput", this) as TextBox;
            this.txtSearchinput.TextChanged += this.OnListTextBoxTextChanged;
            this.txtSearchinput.PreviewKeyDown += this.OnListTextBoxPreviewKeyDown;

            this.listTextBoxPopup = this.Template.FindName("ListTextBox_Popup", this) as Popup;

            this.listTextBoxDataGrid = this.Template.FindName("ListTextBox_DataGrid", this) as DataGrid;
            this.listTextBoxDataGrid.MouseLeftButtonUp += TextBoxDataGridMouseLeftButtonUp;
            foreach (DataGridColumn column in this.ListTextBoxColumns)
            {
                this.listTextBoxDataGrid.Columns.Add(column);
            }
        }

        //Select Item From List On Mouse Click
        private void TextBoxDataGridMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = this.listTextBoxDataGrid.SelectedItem;
            if (item == null)
            {
                this.SelectedItem = null;
                return;
            }

            if (this.listTextBoxPopup.IsOpen)
            {
                this.SelectedItem = item;
                this.listTextBoxPopup.IsOpen = false;
            }
        } 

        // Navigatse through Items using Up/Down keyboard keys and select item on Enter key
        private void OnListTextBoxPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Down)
            {
                if (listTextBoxDataGrid.Items.Count > 0)
                {
                    int selectedIndex = this.listTextBoxDataGrid.SelectedIndex;
                    if (selectedIndex < this.listTextBoxDataGrid.Items.Count)
                    {
                        this.listTextBoxDataGrid.SelectedIndex++;
                    }
                }
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                if (listTextBoxDataGrid.Items.Count > 0)
                {
                    int selectedIndex = this.listTextBoxDataGrid.SelectedIndex;
                    if (selectedIndex > 0)
                    {
                        this.listTextBoxDataGrid.SelectedIndex--;
                    }
                }
            }
            else if (e.Key == System.Windows.Input.Key.Enter)
            {
                var item = this.listTextBoxDataGrid.SelectedItem;
                if (item == null)
                {
                    this.SelectedItem = null;
                    return;
                }

                if (listTextBoxPopup.IsOpen)
                {
                    this.SelectedItem = item;
                    this.listTextBoxPopup.IsOpen = false;
                }
            }
        }

        //Displays AutoComplete Pupup On User Input And Fire Event  'OnTextChange' to filter records
        private void OnListTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.SelectedItem != null && this.SelectedItem.ToString() != this.txtSearchinput.Text)
            {
                this.SelectedItem = null;
            }

            if (string.IsNullOrEmpty(this.txtSearchinput.Text))
            {
                listTextBoxPopup.IsOpen = false;
            }
            else
            {
                listTextBoxPopup.IsOpen = true;
            }

            if (this.OnTextChange != null)
            {
                this.OnTextChange.Invoke(sender, e);
            }
        }
    }

    public class ListTextBoxSelectedEventArgs : EventArgs
    {
        public object SelectedItem { get; set; }

        public object SelectedValue { get; set; }
    }
}
