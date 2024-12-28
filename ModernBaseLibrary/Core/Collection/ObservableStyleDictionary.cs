//-----------------------------------------------------------------------
// <copyright file="ObservableStyleDictionary.cs" company="Lifeprojects.de">
//     Class: ObservableStyleDictionary
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>06.09.2020</date>
//
// <summary>
//  Definition of ObservableSortedDictionary Class
//  http://drwpf.com/blog/2007/09/16/can-i-bind-my-itemscontrol-to-a-dictionary/
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    public class ObservableStyleDictionary : ObservableSortedDictionary<string, Style>
    {
        public ObservableStyleDictionary() : base(new KeyComparer()) 
        {
        }

        private class KeyComparer : IComparer<DictionaryEntry>
        {
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                return string.Compare((string)entry1.Key, (string)entry2.Key, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
