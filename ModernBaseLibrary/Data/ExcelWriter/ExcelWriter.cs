﻿/*
 * PicoXLSX is a small .NET library to generate XLSX (Microsoft Excel 2007 or newer) files in an easy and native way
 * Copyright Raphael Stoeckli © 2022
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

namespace ModernBaseLibrary.ExcelWriter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Threading.Tasks;

    using static ModernBaseLibrary.ExcelWriter.Style;

    /// <summary>
    /// Class representing a workbook
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class ExcelWriter : IDisposable
    {
        private bool disposed;
        private string filename;
        private List<Worksheet> worksheets;
        private Worksheet currentWorksheet;
        private Metadata workbookMetadata;
        private string workbookProtectionPassword;
        private bool lockWindowsIfProtected;
        private bool lockStructureIfProtected;
        private int selectedWorksheet;
        private Shortener shortener;
        private List<string> mruColors = new List<string>();

        /// <summary>
        /// Gets the shortener object for the current worksheet
        /// </summary>
        public Shortener WS
        {
            get { return shortener; }
        }

        /// <summary>
        /// Gets the current worksheet
        /// </summary>
        public Worksheet CurrentWorksheet
        {
            get { return currentWorksheet; }
        }

        /// <summary>
        /// Gets or sets the filename of the workbook
        /// </summary>
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        /// <summary>
        /// Gets a value indicating whether LockStructureIfProtected
        /// Gets whether the structure are locked if workbook is protected. See also <see cref="SetWorkbookProtection"/>
        /// </summary>
        public bool LockStructureIfProtected
        {
            get { return lockStructureIfProtected; }
        }

        /// <summary>
        /// Gets a value indicating whether LockWindowsIfProtected
        /// Gets whether the windows are locked if workbook is protected. See also <see cref="SetWorkbookProtection"/>
        /// </summary>
        public bool LockWindowsIfProtected
        {
            get { return lockWindowsIfProtected; }
        }

        /// <summary>
        /// Gets or sets the WorkbookMetadata
        /// Meta data object of the workbook
        /// </summary>
        public Metadata WorkbookMetadata
        {
            get { return workbookMetadata; }
            set { workbookMetadata = value; }
        }

        /// <summary>
        /// Gets the selected worksheet. The selected worksheet is not the current worksheet while design time but the selected sheet in the output file
        /// </summary>
        public int SelectedWorksheet
        {
            get { return selectedWorksheet; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UseWorkbookProtection
        /// Gets or sets whether the workbook is protected
        /// </summary>
        public bool UseWorkbookProtection { get; set; }

        /// <summary>
        /// Gets the password used for workbook protection. See also <see cref="SetWorkbookProtection"/>
        /// </summary>
        public string WorkbookProtectionPassword
        {
            get { return workbookProtectionPassword; }
        }

        /// <summary>
        /// Gets or sets the WorkbookProtectionPasswordHash
        /// Hash of the protected workbook, originated from <see cref="WorkbookProtectionPassword"/>
        /// </summary>
        public string WorkbookProtectionPasswordHash { get; internal set; }

        /// <summary>
        /// Gets the list of worksheets in the workbook
        /// </summary>
        public List<Worksheet> Worksheets
        {
            get { return worksheets; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Hidden
        /// Gets or sets whether the whole workbook is hidden
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWriter"/> class
        /// </summary>
        public ExcelWriter()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWriter"/> class
        /// </summary>
        /// <param name="createWorkSheet">If true, a default worksheet with the name 'Sheet1' will be crated and set as current worksheet.</param>
        public ExcelWriter(bool createWorkSheet)
        {
            Init();
            if (createWorkSheet)
            {
                AddWorksheet("Sheet1");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWriter"/> class
        /// </summary>
        /// <param name="sheetName">Filename of the workbook.  The name will be sanitized automatically according to the specifications of Excel.</param>
        public ExcelWriter(string sheetName)
        {
            Init();
            AddWorksheet(sheetName, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWriter"/> class
        /// </summary>
        /// <param name="filename">Filename of the workbook.  The name will be sanitized automatically according to the specifications of Excel.</param>
        /// <param name="sheetName">Name of the first worksheet. The name will be sanitized automatically according to the specifications of Excel.</param>
        public ExcelWriter(string filename, string sheetName)
        {
            Init();
            this.filename = filename;
            AddWorksheet(sheetName, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWriter"/> class
        /// </summary>
        /// <param name="filename">Filename of the workbook.</param>
        /// <param name="sheetName">Name of the first worksheet.</param>
        /// <param name="sanitizeSheetName">If true, the name of the worksheet will be sanitized automatically according to the specifications of Excel.</param>
        public ExcelWriter(string filename, string sheetName, bool sanitizeSheetName)
        {
            Init();
            this.filename = filename;
            if (sanitizeSheetName)
            {
                AddWorksheet(Worksheet.SanitizeWorksheetName(sheetName, this));
            }
            else
            {
                AddWorksheet(sheetName);
            }
        }

        /// <summary>
        /// Adds a color value (HEX; 6-digit RGB or 8-digit ARGB) to the MRU list
        /// </summary>
        /// <param name="color">RGB code in hex format (either 6 characters, e.g. FF00AC or 8 characters with leading alpha value). Alpha will be set to full opacity (FF) in case of 6 characters.</param>
        public void AddMruColor(string color)
        {
            if (color != null && color.Length == 6)
            {
                color = "FF" + color;
            }
            Fill.ValidateColor(color, true);
            mruColors.Add(color.ToUpper());
        }

        /// <summary>
        /// Gets the MRU color list
        /// </summary>
        /// <returns>Immutable list of color values.</returns>
        public IReadOnlyList<string> GetMruColors()
        {
            return mruColors;
        }

        /// <summary>
        /// Clears the MRU color list
        /// </summary>
        public void ClearMruColors()
        {
            mruColors.Clear();
        }

        /// <summary>
        /// Adds a style to the style repository. This method is deprecated since it has no direct impact on the generated file
        /// </summary>
        /// <param name="style">Style to add.</param>
        /// <returns>Returns the managed style of the style repository.</returns>
        [Obsolete("This method has no direct impact on the generated file and is deprecated.")]
        public Style AddStyle(Style style)
        {
            return StyleRepository.Instance.AddStyle(style);
        }

        /// <summary>
        /// Adds a style component to a style. This method is deprecated since it has no direct impact on the generated file
        /// </summary>
        /// <param name="baseStyle">Style to append a component.</param>
        /// <param name="newComponent">Component to add to the baseStyle.</param>
        /// <returns>Returns the modified style of the style repository.</returns>
        [Obsolete("This method has no direct impact on the generated file and is deprecated.")]
        public Style AddStyleComponent(Style baseStyle, AbstractStyle newComponent)
        {

            if (newComponent.GetType() == typeof(Border))
            {
                baseStyle.CurrentBorder = (Border)newComponent;
            }
            else if (newComponent.GetType() == typeof(CellXf))
            {
                baseStyle.CurrentCellXf = (CellXf)newComponent;
            }
            else if (newComponent.GetType() == typeof(Fill))
            {
                baseStyle.CurrentFill = (Fill)newComponent;
            }
            else if (newComponent.GetType() == typeof(Font))
            {
                baseStyle.CurrentFont = (Font)newComponent;
            }
            else if (newComponent.GetType() == typeof(NumberFormat))
            {
                baseStyle.CurrentNumberFormat = (NumberFormat)newComponent;
            }
            return StyleRepository.Instance.AddStyle(baseStyle);
        }

        /// <summary>
        /// Adding a new Worksheet. The new worksheet will be defined as current worksheet
        /// </summary>
        /// <param name="name">Name of the new worksheet.</param>
        public void AddWorksheet(string name)
        {
            foreach (Worksheet item in worksheets)
            {
                if (item.SheetName == name)
                {
                    throw new ExcelWorksheetException("The worksheet with the name '" + name + "' already exists.");
                }
            }
            int number = GetNextWorksheetId();
            Worksheet newWs = new Worksheet(name, number, this);
            currentWorksheet = newWs;
            worksheets.Add(newWs);
            shortener.SetCurrentWorksheetInternal(currentWorksheet);
        }

        /// <summary>
        /// Adding a new Worksheet with a sanitizing option. The new worksheet will be defined as current worksheet
        /// </summary>
        /// <param name="name">Name of the new worksheet.</param>
        /// <param name="sanitizeSheetName">If true, the name of the worksheet will be sanitized automatically according to the specifications of Excel.</param>
        public void AddWorksheet(string name, bool sanitizeSheetName)
        {
            if (sanitizeSheetName)
            {
                string sanitized = Worksheet.SanitizeWorksheetName(name, this);
                AddWorksheet(sanitized);
            }
            else
            {
                AddWorksheet(name);
            }
        }

        /// <summary>
        /// Adding a new Worksheet. The new worksheet will be defined as current worksheet
        /// </summary>
        /// <param name="worksheet">Prepared worksheet object.</param>
        public void AddWorksheet(Worksheet worksheet)
        {
            AddWorksheet(worksheet, false);
        }

        /// <summary>
        /// Adding a new Worksheet. The new worksheet will be defined as current worksheet
        /// </summary>
        /// <param name="worksheet">Prepared worksheet object.</param>
        /// <param name="sanitizeSheetName">If true, the name of the worksheet will be sanitized automatically according to the specifications of Excel.</param>
        public void AddWorksheet(Worksheet worksheet, bool sanitizeSheetName)
        {
            if (sanitizeSheetName)
            {
                string name = Worksheet.SanitizeWorksheetName(worksheet.SheetName, this);
                worksheet.SheetName = name;
            }
            else
            {
                if (string.IsNullOrEmpty(worksheet.SheetName))
                {
                    throw new ExcelWorksheetException("The name of the passed worksheet is null or empty.");
                }
                for (int i = 0; i < worksheets.Count; i++)
                {
                    if (worksheets[i].SheetName == worksheet.SheetName)
                    {
                        throw new ExcelWorksheetException("The worksheet with the name '" + worksheet.SheetName + "' already exists.");
                    }
                }
            }
            worksheet.SheetID = GetNextWorksheetId();
            currentWorksheet = worksheet;
            worksheets.Add(worksheet);
            worksheet.WorkbookReference = this;
        }

        /// <summary>
        /// Removes the passed style from the style sheet. This method is deprecated since it has no direct impact on the generated file
        /// </summary>
        /// <param name="style">Style to remove.</param>
        [Obsolete("This method has no direct impact on the generated file and is deprecated.")]
        public void RemoveStyle(Style style)
        {
            RemoveStyle(style, false);
        }

        /// <summary>
        /// Removes the defined style from the style sheet of the workbook. This method is deprecated since it has no direct impact on the generated file
        /// </summary>
        /// <param name="styleName">Name of the style to be removed.</param>
        [Obsolete("This method has no direct impact on the generated file and is deprecated.")]
        public void RemoveStyle(string styleName)
        {
            RemoveStyle(styleName, false);
        }

        /// <summary>
        /// Removes the defined style from the style sheet of the workbook
        /// </summary>
        /// <param name="style">Style to remove.</param>
        /// <param name="onlyIfUnused">If true, the style will only be removed if not used in any cell.</param>
        [Obsolete("This method has no direct impact on the generated file and is deprecated.")]
        public void RemoveStyle(Style style, bool onlyIfUnused)
        {
            if (style == null)
            {
                throw new ExcelStyleException("MissingReferenceException", "The style to remove is not defined");
            }
            RemoveStyle(style.Name, onlyIfUnused);
        }

        /// <summary>
        /// Removes the defined style from the style sheet of the workbook. This method is deprecated since it has no direct impact on the generated file
        /// </summary>
        /// <param name="styleName">Name of the style to be removed.</param>
        /// <param name="onlyIfUnused">If true, the style will only be removed if not used in any cell.</param>
        [Obsolete("This method has no direct impact on the generated file and is deprecated.")]
        public void RemoveStyle(string styleName, bool onlyIfUnused)
        {
            if (string.IsNullOrEmpty(styleName))
            {
                throw new ExcelStyleException("MissingReferenceException", "The style to remove is not defined (no name specified)");
            }
        }

        /// <summary>
        /// Removes the defined worksheet based on its name. If the worksheet is the current or selected worksheet, the current and / or the selected worksheet will be set to the last worksheet of the workbook
        /// Removes the defined worksheet based on its name. If the worksheet is the current or selected worksheet, the current and / or the selected worksheet will be set to the last worksheet of the workbook
        /// </summary>
        /// <param name="name">Name of the worksheet.</param>
        public void RemoveWorksheet(string name)
        {
            Worksheet worksheetToRemove = worksheets.FindLast(w => w.SheetName == name);
            if (worksheetToRemove == null)
            {
                throw new ExcelWorksheetException("The worksheet with the name '" + name + "' does not exist.");
            }
            int index = worksheets.IndexOf(worksheetToRemove);
            bool resetCurrentWorksheet = worksheetToRemove == currentWorksheet;
            RemoveWorksheet(index, resetCurrentWorksheet);
        }

        /// <summary>
        /// Removes the defined worksheet based on its index. If the worksheet is the current or selected worksheet, the current and / or the selected worksheet will be set to the last worksheet of the workbook
        /// Removes the defined worksheet based on its index. If the worksheet is the current or selected worksheet, the current and / or the selected worksheet will be set to the last worksheet of the workbook
        /// </summary>
        /// <param name="index">Index within the worksheets list.</param>
        public void RemoveWorksheet(int index)
        {
            if (index < 0 || index >= worksheets.Count)
            {
                throw new ExcelWorksheetException("The worksheet index " + index + " is out of range");
            }
            bool resetCurrentWorksheet = worksheets[index] == currentWorksheet;
            RemoveWorksheet(index, resetCurrentWorksheet);
        }

        /// <summary>
        /// Method to resolve all merged cells in all worksheets. Only the value of the very first cell of the locked cells range will be visible. The other values are still present (set to EMPTY) but will not be stored in the worksheet.<br/>
        /// This is an internal method. There is no need to use it
        /// </summary>
        internal void ResolveMergedCells()
        {
            foreach (Worksheet worksheet in worksheets)
            {
                worksheet.ResolveMergedCells();
            }
        }

        /// <summary>
        /// Saves the workbook
        /// </summary>
        public void Save()
        {
            LowLevel l = new LowLevel(this);
            l.Save();
        }

        /// <summary>
        /// Saves the workbook asynchronous
        /// </summary>
        /// <returns>Task object (void).</returns>
        public async Task SaveAsync()
        {
            LowLevel l = new LowLevel(this);
            await l.SaveAsync();
        }

        /// <summary>
        /// Saves the workbook with the defined name
        /// </summary>
        /// <param name="fileName">filename of the saved workbook.</param>
        public void SaveAs(string fileName)
        {
            string backup = fileName;
            filename = fileName;
            LowLevel l = new LowLevel(this);
            l.Save();
            filename = backup;
        }

        /// <summary>
        /// Saves the workbook with the defined name asynchronous
        /// </summary>
        /// <param name="fileName">filename of the saved workbook.</param>
        /// <returns>Task object (void).</returns>
        public async Task SaveAsAsync(string fileName)
        {
            string backup = fileName;
            filename = fileName;
            LowLevel l = new LowLevel(this);
            await l.SaveAsync();
            filename = backup;
        }

        /// <summary>
        /// Save the workbook to a writable stream
        /// </summary>
        /// <param name="stream">Writable stream.</param>
        /// <param name="leaveOpen">Optional parameter to keep the stream open after writing (used for MemoryStreams; default is false).</param>
        public void SaveAsStream(Stream stream, bool leaveOpen = false)
        {
            LowLevel l = new LowLevel(this);
            l.SaveAsStream(stream, leaveOpen);
        }

        /// <summary>
        /// Save the workbook to a writable stream asynchronous
        /// </summary>
        /// <param name="stream">>Writable stream.</param>
        /// <param name="leaveOpen">Optional parameter to keep the stream open after writing (used for MemoryStreams; default is false).</param>
        /// <returns>Task object (void).</returns>
        public async Task SaveAsStreamAsync(Stream stream, bool leaveOpen = false)
        {
            LowLevel l = new LowLevel(this);
            await l.SaveAsStreamAsync(stream, leaveOpen);
        }

        /// <summary>
        /// Sets the current worksheet
        /// </summary>
        /// <param name="name">Name of the worksheet.</param>
        /// <returns>Returns the current worksheet.</returns>
        public Worksheet SetCurrentWorksheet(string name)
        {
            currentWorksheet = worksheets.FirstOrDefault(w => w.SheetName == name);
            if (currentWorksheet == null)
            {
                throw new ExcelWorksheetException("The worksheet with the name '" + name + "' does not exist.");
            }
            shortener.SetCurrentWorksheetInternal(currentWorksheet);
            return currentWorksheet;
        }

        /// <summary>
        /// Sets the current worksheet
        /// </summary>
        /// <param name="worksheetIndex">Zero-based worksheet index.</param>
        /// <returns>Returns the current worksheet.</returns>
        public Worksheet SetCurrentWorksheet(int worksheetIndex)
        {
            if (worksheetIndex < 0 || worksheetIndex > worksheets.Count - 1)
            {
                throw new ExcelRangeException("OutOfRangeException", "The worksheet index " + worksheetIndex + " is out of range");
            }
            currentWorksheet = worksheets[worksheetIndex];
            shortener.SetCurrentWorksheetInternal(currentWorksheet);
            return currentWorksheet;
        }

        /// <summary>
        /// Sets the current worksheet
        /// </summary>
        /// <param name="worksheet">Worksheet object (must be in the collection of worksheets).</param>
        public void SetCurrentWorksheet(Worksheet worksheet)
        {
            int index = worksheets.IndexOf(worksheet);
            if (index < 0)
            {
                throw new ExcelWorksheetException("The passed worksheet object is not in the worksheet collection.");
            }
            currentWorksheet = worksheets[index];
            shortener.SetCurrentWorksheetInternal(worksheet);
        }

        /// <summary>
        /// Sets the selected worksheet in the output workbook
        /// </summary>
        /// <param name="name">Name of the worksheet.</param>
        public void SetSelectedWorksheet(string name)
        {
            selectedWorksheet = worksheets.FindIndex(w => w.SheetName == name);
            if (selectedWorksheet < 0)
            {
                throw new ExcelWorksheetException("The worksheet with the name '" + name + "' does not exist.");
            }
            ValidateWorksheets();
        }

        /// <summary>
        /// Sets the selected worksheet in the output workbook
        /// </summary>
        /// <param name="worksheetIndex">Zero-based worksheet index.</param>
        public void SetSelectedWorksheet(int worksheetIndex)
        {
            if (worksheetIndex < 0 || worksheetIndex > worksheets.Count - 1)
            {
                throw new ExcelRangeException("OutOfRangeException", "The worksheet index " + worksheetIndex + " is out of range");
            }
            selectedWorksheet = worksheetIndex;
            ValidateWorksheets();
        }

        /// <summary>
        /// Sets the selected worksheet in the output workbook
        /// </summary>
        /// <param name="worksheet">Worksheet object (must be in the collection of worksheets).</param>
        public void SetSelectedWorksheet(Worksheet worksheet)
        {
            selectedWorksheet = worksheets.IndexOf(worksheet);
            if (selectedWorksheet < 0)
            {
                throw new ExcelWorksheetException("The passed worksheet object is not in the worksheet collection.");
            }
            ValidateWorksheets();
        }

        /// <summary>
        /// Gets a worksheet from this workbook by name
        /// </summary>
        /// <param name="name">Name of the worksheet.</param>
        /// <returns>Worksheet with the passed name.</returns>
        public Worksheet GetWorksheet(string name)
        {
            int index = worksheets.FindIndex(w => w.SheetName == name);
            if (index < 0)
            {
                throw new ExcelWorksheetException("No worksheet with the name '" + name + "' was found in this workbook.");
            }
            return worksheets[index];
        }

        /// <summary>
        /// Gets a worksheet from this workbook by index
        /// </summary>
        /// <param name="index">Index of the worksheet.</param>
        /// <returns>Worksheet with the passed index.</returns>
        public Worksheet GetWorksheet(int index)
        {
            if (index < 0 || index > worksheets.Count - 1)
            {
                throw new ExcelRangeException("OutOfRangeException", "The worksheet index " + index + " is out of range");
            }
            return worksheets[index];
        }

        /// <summary>
        /// Sets or removes the workbook protection. If protectWindows and protectStructure are both false, the workbook will not be protected
        /// </summary>
        /// <param name="state">If true, the workbook will be protected, otherwise not.</param>
        /// <param name="protectWindows">If true, the windows will be locked if the workbook is protected.</param>
        /// <param name="protectStructure">If true, the structure will be locked if the workbook is protected.</param>
        /// <param name="password">Optional password. If null or empty, no password will be set in case of protection.</param>
        public void SetWorkbookProtection(bool state, bool protectWindows, bool protectStructure, string password)
        {
            lockWindowsIfProtected = protectWindows;
            lockStructureIfProtected = protectStructure;
            workbookProtectionPassword = password;
            WorkbookProtectionPasswordHash = LowLevel.GeneratePasswordHash(password);
            if (protectWindows == false && protectStructure == false)
            {
                UseWorkbookProtection = false;
            }
            else
            {
                UseWorkbookProtection = state;
            }
        }

        /// <summary>
        /// Copies a worksheet of the current workbook by its name
        /// </summary>
        /// <param name="sourceWorksheetName">Name of the worksheet to copy, originated in this workbook.</param>
        /// <param name="newWorksheetName">Name of the new worksheet (copy).</param>
        /// <param name="sanitizeSheetName">If true, the new name will be automatically sanitized if a name collision occurs.</param>
        /// <returns>Copied worksheet.</returns>
        public Worksheet CopyWorksheetIntoThis(string sourceWorksheetName, string newWorksheetName, bool sanitizeSheetName = true)
        {
            Worksheet sourceWorksheet = GetWorksheet(sourceWorksheetName);
            return CopyWorksheetTo(sourceWorksheet, newWorksheetName, this, sanitizeSheetName);
        }

        /// <summary>
        /// Copies a worksheet of the current workbook by its index
        /// </summary>
        /// <param name="sourceWorksheetIndex">Index of the worksheet to copy, originated in this workbook.</param>
        /// <param name="newWorksheetName">Name of the new worksheet (copy).</param>
        /// <param name="sanitizeSheetName">If true, the new name will be automatically sanitized if a name collision occurs.</param>
        /// <returns>Copied worksheet.</returns>
        public Worksheet CopyWorksheetIntoThis(int sourceWorksheetIndex, string newWorksheetName, bool sanitizeSheetName = true)
        {
            Worksheet sourceWorksheet = GetWorksheet(sourceWorksheetIndex);
            return CopyWorksheetTo(sourceWorksheet, newWorksheetName, this, sanitizeSheetName);
        }

        /// <summary>
        /// Copies a worksheet of any workbook into the current workbook
        /// </summary>
        /// <param name="sourceWorksheet">Worksheet to copy.</param>
        /// <param name="newWorksheetName">Name of the new worksheet (copy).</param>
        /// <param name="sanitizeSheetName">If true, the new name will be automatically sanitized if a name collision occurs.</param>
        /// <returns>Copied worksheet.</returns>
        public Worksheet CopyWorksheetIntoThis(Worksheet sourceWorksheet, string newWorksheetName, bool sanitizeSheetName = true)
        {
            return CopyWorksheetTo(sourceWorksheet, newWorksheetName, this, sanitizeSheetName);
        }

        /// <summary>
        /// Copies a worksheet of the current workbook by its name into another workbook
        /// </summary>
        /// <param name="sourceWorksheetName">Name of the worksheet to copy, originated in this workbook.</param>
        /// <param name="newWorksheetName">Name of the new worksheet (copy).</param>
        /// <param name="targetWorkbook">Workbook to copy the worksheet into.</param>
        /// <param name="sanitizeSheetName">If true, the new name will be automatically sanitized if a name collision occurs.</param>
        /// <returns>Copied worksheet.</returns>
        public Worksheet CopyWorksheetTo(string sourceWorksheetName, string newWorksheetName, ExcelWriter targetWorkbook, bool sanitizeSheetName = true)
        {
            Worksheet sourceWorksheet = GetWorksheet(sourceWorksheetName);
            return CopyWorksheetTo(sourceWorksheet, newWorksheetName, targetWorkbook, sanitizeSheetName);
        }

        /// <summary>
        /// Copies a worksheet of the current workbook by its index into another workbook
        /// </summary>
        /// <param name="sourceWorksheetIndex">Index of the worksheet to copy, originated in this workbook.</param>
        /// <param name="newWorksheetName">Name of the new worksheet (copy).</param>
        /// <param name="targetWorkbook">Workbook to copy the worksheet into.</param>
        /// <param name="sanitizeSheetName">If true, the new name will be automatically sanitized if a name collision occurs.</param>
        /// <returns>Copied worksheet.</returns>
        public Worksheet CopyWorksheetTo(int sourceWorksheetIndex, string newWorksheetName, ExcelWriter targetWorkbook, bool sanitizeSheetName = true)
        {
            Worksheet sourceWorksheet = GetWorksheet(sourceWorksheetIndex);
            return CopyWorksheetTo(sourceWorksheet, newWorksheetName, targetWorkbook, sanitizeSheetName);
        }

        /// <summary>
        /// Copies a worksheet of any workbook into the another workbook
        /// </summary>
        /// <param name="sourceWorksheet">Worksheet to copy.</param>
        /// <param name="newWorksheetName">Name of the new worksheet (copy).</param>
        /// <param name="targetWorkbook">Workbook to copy the worksheet into.</param>
        /// <param name="sanitizeSheetName">If true, the new name will be automatically sanitized if a name collision occurs.</param>
        /// <returns>Copied worksheet.</returns>
        public static Worksheet CopyWorksheetTo(Worksheet sourceWorksheet, string newWorksheetName, ExcelWriter targetWorkbook, bool sanitizeSheetName = true)
        {
            if (targetWorkbook == null)
            {
                throw new ExcelWorksheetException("The target workbook cannot be null");
            }
            if (sourceWorksheet == null)
            {
                throw new ExcelWorksheetException("The source worksheet cannot be null");
            }
            Worksheet copy = sourceWorksheet.Copy();
            copy.SetSheetName(newWorksheetName);
            Worksheet currentWorksheet = targetWorkbook.CurrentWorksheet;
            targetWorkbook.AddWorksheet(copy, sanitizeSheetName);
            targetWorkbook.SetCurrentWorksheet(currentWorksheet);
            return copy;
        }

        /// <summary>
        /// Validates the worksheets regarding several conditions that must be met:<br/>
        /// - At least one worksheet must be defined<br/>
        /// - A hidden worksheet cannot be the selected one<br/>
        /// - At least one worksheet must be visible<br/>
        /// If one of the conditions is not met, an exception is thrown
        /// </summary>
        internal void ValidateWorksheets()
        {
            int woksheetCount = worksheets.Count;
            if (woksheetCount == 0)
            {
                throw new ExcelWorksheetException("The workbook must contain at least one worksheet");
            }
            for (int i = 0; i < woksheetCount; i++)
            {
                if (worksheets[i].Hidden)
                {
                    if (i == selectedWorksheet)
                    {
                        throw new ExcelWorksheetException("The worksheet with the index " + selectedWorksheet + " cannot be set as selected, since it is set hidden");
                    }
                }
            }
        }

        /// <summary>
        /// Removes the worksheet at the defined index and relocates current and selected worksheet references
        /// </summary>
        /// <param name="index">Index within the worksheets list.</param>
        /// <param name="resetCurrentWorksheet">If true, the current worksheet will be relocated to the last worksheet in the list.</param>
        private void RemoveWorksheet(int index, bool resetCurrentWorksheet)
        {
            worksheets.RemoveAt(index);
            if (worksheets.Count > 0)
            {
                for (int i = 0; i < worksheets.Count; i++)
                {
                    worksheets[i].SheetID = i + 1;
                }
                if (resetCurrentWorksheet)
                {
                    currentWorksheet = worksheets[worksheets.Count - 1];
                }
                if (selectedWorksheet == index || selectedWorksheet > worksheets.Count - 1)
                {
                    selectedWorksheet = worksheets.Count - 1;
                }
            }
            else
            {
                currentWorksheet = null;
                selectedWorksheet = 0;
            }
            ValidateWorksheets();
        }

        /// <summary>
        /// Gets the next free worksheet ID
        /// </summary>
        /// <returns>Worksheet ID.</returns>
        private int GetNextWorksheetId()
        {
            if (worksheets.Count == 0)
            {
                return 1;
            }
            return worksheets.Max(w => w.SheetID) + 1;
        }

        /// <summary>
        /// Init method called in the constructors
        /// </summary>
        private void Init()
        {
            worksheets = new List<Worksheet>();
            workbookMetadata = new Metadata();
            shortener = new Shortener(this);
        }

        /// <summary>
        /// Class to provide access to the current worksheet with a shortened syntax. Note: The WS object can be null if the workbook was created without a worksheet. The object will be available as soon as the current worksheet is defined
        /// </summary>
        [SupportedOSPlatform("windows")]
        public class Shortener
        {
            /// <summary>
            /// Defines the currentWorksheet
            /// </summary>
            private Worksheet currentWorksheet;

            /// <summary>
            /// Defines the workbookReference
            /// </summary>
            private readonly ExcelWriter workbookReference;

            /// <summary>
            /// Initializes a new instance of the <see cref="Shortener"/> class
            /// </summary>
            /// <param name="reference">Workbook reference.</param>
            public Shortener(ExcelWriter reference)
            {
                this.workbookReference = reference;
                this.currentWorksheet = reference.CurrentWorksheet;
            }

            /// <summary>
            /// Sets the worksheet accessed by the shortener
            /// </summary>
            /// <param name="worksheet">Current worksheet.</param>
            public void SetCurrentWorksheet(Worksheet worksheet)
            {
                workbookReference.SetCurrentWorksheet(worksheet);
                currentWorksheet = worksheet;
            }

            /// <summary>
            /// Sets the worksheet accessed by the shortener, invoked by the workbook
            /// </summary>
            /// <param name="worksheet">Current worksheet.</param>
            internal void SetCurrentWorksheetInternal(Worksheet worksheet)
            {
                currentWorksheet = worksheet;
            }

            /// <summary>
            /// Sets a value into the current cell and moves the cursor to the next cell (column or row depending on the defined cell direction)
            /// </summary>
            /// <param name="value">Value to set.</param>
            public void Value(object value)
            {
                NullCheck();
                currentWorksheet.AddNextCell(value);
            }

            /// <summary>
            /// Sets a value with style into the current cell and moves the cursor to the next cell (column or row depending on the defined cell direction)
            /// </summary>
            /// <param name="value">Value to set.</param>
            /// <param name="style">Style to apply.</param>
            public void Value(object value, Style style)
            {
                NullCheck();
                currentWorksheet.AddNextCell(value, style);
            }

            /// <summary>
            /// Sets a formula into the current cell and moves the cursor to the next cell (column or row depending on the defined cell direction)
            /// </summary>
            /// <param name="formula">Formula to set.</param>
            public void Formula(string formula)
            {
                NullCheck();
                currentWorksheet.AddNextCellFormula(formula);
            }

            /// <summary>
            /// Sets a formula with style into the current cell and moves the cursor to the next cell (column or row depending on the defined cell direction)
            /// </summary>
            /// <param name="formula">Formula to set.</param>
            /// <param name="style">Style to apply.</param>
            public void Formula(string formula, Style style)
            {
                NullCheck();
                currentWorksheet.AddNextCellFormula(formula, style);
            }

            /// <summary>
            /// Moves the cursor one row down
            /// </summary>
            public void Down()
            {
                NullCheck();
                currentWorksheet.GoToNextRow();
            }

            /// <summary>
            /// Moves the cursor the number of defined rows down
            /// </summary>
            /// <param name="numberOfRows">Number of rows to move.</param>
            /// <param name="keepColumnPosition">If true, the column position is preserved, otherwise set to 0.</param>
            public void Down(int numberOfRows, bool keepColumnPosition = false)
            {
                NullCheck();
                currentWorksheet.GoToNextRow(numberOfRows, keepColumnPosition);
            }

            /// <summary>
            /// Moves the cursor one row up
            /// </summary>
            public void Up()
            {
                NullCheck();
                currentWorksheet.GoToNextRow(-1);
            }

            /// <summary>
            /// Moves the cursor the number of defined rows up
            /// </summary>
            /// <param name="numberOfRows">Number of rows to move.</param>
            /// <param name="keepColumnosition">If true, the column position is preserved, otherwise set to 0.</param>
            public void Up(int numberOfRows, bool keepColumnosition = false)
            {
                NullCheck();
                currentWorksheet.GoToNextRow(-1 * numberOfRows, keepColumnosition);
            }

            /// <summary>
            /// Moves the cursor one column to the right
            /// </summary>
            public void Right()
            {
                NullCheck();
                currentWorksheet.GoToNextColumn();
            }

            /// <summary>
            /// Moves the cursor the number of defined columns to the right
            /// </summary>
            /// <param name="numberOfColumns">Number of columns to move.</param>
            /// <param name="keepRowPosition">If true, the row position is preserved, otherwise set to 0.</param>
            public void Right(int numberOfColumns, bool keepRowPosition = false)
            {
                NullCheck();
                currentWorksheet.GoToNextColumn(numberOfColumns, keepRowPosition);
            }

            /// <summary>
            /// Moves the cursor one column to the left
            /// </summary>
            public void Left()
            {
                NullCheck();
                currentWorksheet.GoToNextColumn(-1);
            }

            /// <summary>
            /// Moves the cursor the number of defined columns to the left
            /// </summary>
            /// <param name="numberOfColumns">Number of columns to move.</param>
            /// <param name="keepRowRowPosition">If true, the row position is preserved, otherwise set to 0.</param>
            public void Left(int numberOfColumns, bool keepRowRowPosition = false)
            {
                NullCheck();
                currentWorksheet.GoToNextColumn(-1 * numberOfColumns, keepRowRowPosition);
            }

            /// <summary>
            /// Internal method to check whether the worksheet is null
            /// </summary>
            private void NullCheck()
            {
                if (currentWorksheet == null)
                {
                    throw new ExcelWorksheetException("No worksheet was defined");
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposed == true)
            {
                return;
            }

            if (disposing == true)
            {
                this.worksheets = null;
                this.currentWorksheet = null;
                this.workbookMetadata = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
