// <copyright file="FileFilter.cs" company="Lifeprojects.de">
//     Class: FileFilter
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Class with FileFilter Definition</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FileFilter : IDisposable
    {
        private static int filterIndex = 0;
        private bool classIsDisposed = false;
        private List<FilterContent> fileFilterList;
        private int defaultFilterIndex = -1;
        private string defaultExtension = string.Empty;

        public FileFilter()
        {
            this.fileFilterList = new List<FilterContent>();
            filterIndex = 0;
        }

        public int DefaultFilterIndex
        {
            get
            {
                FilterContent filterContent = this.fileFilterList.Where(p => p.Default == true).First();
                this.defaultFilterIndex = filterContent.Index;
                return this.defaultFilterIndex;
            }

            private set { this.defaultFilterIndex = value; }
        }

        public string DefaultExtension
        {
            get
            {
                FilterContent defaultFilter = this.fileFilterList.Where(p => p.Default == true).First();
                this.defaultExtension = string.Format(".{0}", defaultFilter.Filter);
                return this.defaultExtension;
            }

            private set { this.defaultExtension = value; }
        }

        public void AddFilter(string name, string filter, bool pDefault = false)
        {
            if (this.fileFilterList == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(filter) == false)
            {
                filterIndex++;
                FilterContent fc = new FilterContent();
                fc.Name = name;
                fc.Filter = filter;
                fc.Default = pDefault;
                fc.Index = filterIndex;
                this.fileFilterList.Add(fc);
            }
        }

        public void SetDefaultFilter(string name)
        {
            if (this.fileFilterList == null)
            {
                return;
            }

            this.fileFilterList.SingleOrDefault(s => s.Name == name).Default = true;
        }

        public string GetFilterName(string filter)
        {
            string getFilterName = string.Empty;

            foreach (FilterContent item in this.fileFilterList)
            {
                if (item.Filter.ToUpper().Contains(filter.ToUpper().Replace(".", string.Empty)) == true)
                {
                    getFilterName = item.Name;
                    break;
                }
            }

            return getFilterName;
        }

        public string GetFileFilter()
        {
            string retFilter = string.Empty;

            /*
             txt files (*.txt)|*.txt
             all files (*.*)|*.*
             Documents (*.txt, *.doc)|*.txt*;*.doc|All Files|*.*
            */

            char[] charSeparators = { ',', ';' };
            StringBuilder filter = new StringBuilder();
            foreach (FilterContent item in this.fileFilterList)
            {
                string[] ext = item.Filter.Split(charSeparators);
                if (ext.Length == 1)
                {
                    if (ext[0].Contains(".") == true)
                    {
                        filter.AppendFormat("{0} ({1})|{1}|", item.Name, item.Filter);
                    }
                    else
                    {
                        filter.AppendFormat("{0} (*.{1})|*.{1}|", item.Name, item.Filter);
                    }
                }

                if (ext.Length > 1)
                {
                    string extText = string.Empty;
                    for (int i = 0; i < ext.Length; i++)
                    {
                        if (ext[i].Contains(".") == true)
                        {
                            extText += string.Format("{0};", ext[i]);
                        }
                        else
                        {
                            extText += string.Format("*.{0};", ext[i]);
                        }
                    }

                    extText.Remove(extText.Length - 1, 1);
                    filter.AppendFormat("{0} ({1})|{1}|", item.Name, extText);
                }
            }

            filter.Remove(filter.Length - 1, 1);

            return filter.ToString();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Dispose

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                    this.fileFilterList = null;
                }
            }

            this.classIsDisposed = true;
        }
        #endregion Dispose

        private class FilterContent
        {
            public string Name { get; set; }

            public string Filter { get; set; }

            public bool Default { get; set; }

            public int Index { get; set; }

            public override string ToString()
            {
                return $"{this.Name}-{this.Filter}-{this.Default}";
            }
        }
    }
}