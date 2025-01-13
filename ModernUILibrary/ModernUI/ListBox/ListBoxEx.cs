//-----------------------------------------------------------------------
// <copyright file="ListBox.cs" company="Lifeprojects.de">
//     Class: ListBox
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.07.2018</date>
//
// <summary>
// Class for UI Control ListBox
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ListBoxEx :ListBox
    {
        public ListBoxEx()
        {
            this.Style = Base.ResourceReader.Instance.ReadAs<Style>("ValidationListBoxExCoreStyle");
            this.ApplyFontSize();
            this.ApplyBorderBrush();

            this.IsSynchronizedWithCurrentItem = true;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(2);
            this.Margin = new Thickness(2);
            this.ClipToBounds = false;
            this.Resources.Add(SystemColors.WindowBrushKey, Brushes.WhiteSmoke);
            this.Resources.Add(SystemColors.WindowTextBrushKey, Colors.White);
            this.Resources.Add(SystemColors.HighlightColorKey, Colors.Gray);
        }

        private void ApplyBorderBrush()
        {
            this.BorderBrush = Base.ResourceReader.Instance.ReadAsBrush("CellBorderColor");
        }

        private void ApplyFontSize()
        {
            this.FontSize = Base.ResourceReader.Instance.ReadAs<double>("DefaultFontSize");
        }
    }
}
