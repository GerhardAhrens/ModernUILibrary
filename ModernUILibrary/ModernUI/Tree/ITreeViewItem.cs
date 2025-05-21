//-----------------------------------------------------------------------
// <copyright file="ITreeViewItem.cs" company="Lifeprojects.de">
//     Class: ITreeViewItem
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>21.05.2025 14:31:49</date>
//
// <summary>
// Interface Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows.Media;

    public interface ITreeViewItem
    {
        int Count { get; }

        public bool HasChildren { get; }

        public bool IsRoot { get; }

        public int Level { get; }

        public Guid NodeKey { get; set; }

        string NodeName { get; set; }

        string NodeSymbol { get; set; }

        bool IsEnabled { get; set; }

        bool IsSelected { get; set; }

        bool IsExpanded { get; set; }

        Brush NodeForeground { get; set; }

        Brush NodeBackground { get; set; }
    }
}
