//-----------------------------------------------------------------------
// <copyright file="SimpleGrid.cs" company="Lifeprojects.de">
//     Class: SimpleGrid
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>22.08.2019</date>
//
// <summary>
// Class for UI Control SimpleGrid
// </summary>
// < Website >
// https://thomaslevesque.com/tag/wpf/page/2/
// </ Website >
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    using ModernIU.Converters;

    [TypeConverter(typeof(GridLengthCollectionConverter))]
    public class GridLengthCollection : ReadOnlyCollection<GridLength>
    {
        public GridLengthCollection(IList<GridLength> lengths) : base(lengths)
        {
        }
    }
}