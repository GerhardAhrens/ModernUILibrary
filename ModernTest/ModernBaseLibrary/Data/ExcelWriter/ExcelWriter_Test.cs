namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using global::ModernBaseLibrary.ExcelReader;
    using global::ModernBaseLibrary.ExcelWriter;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExcelWriter_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Directory.CreateDirectory(TempDirPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(TempDirPath))
            {
                Directory.Delete(TempDirPath, true);
            }
        }

        [TestMethod]
        public void AutomatischeAddressierung()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_01.xlsx");

            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddNextCell("Test");              // Add cell A1
                workbook.CurrentWorksheet.AddNextCell(123);                 // Add cell B1
                workbook.CurrentWorksheet.AddNextCell(true);                // Add cell C1
                workbook.CurrentWorksheet.GoToNextRow();                    // Go to Row 2
                workbook.CurrentWorksheet.AddNextCell(123.456d);            // Add cell A2
                workbook.CurrentWorksheet.AddNextCell(123.789f);            // Add cell B2
                workbook.CurrentWorksheet.AddNextCell(DateTime.Now);        // Add cell C2
                workbook.CurrentWorksheet.AddNextCell(new TimeSpan(12, 50, 30)); // Add cell D2
                workbook.CurrentWorksheet.GoToNextRow();                    // Go to Row 3
                workbook.CurrentWorksheet.AddNextCellFormula("B1*22");      // Add cell A3 as formula (B1 times 22)
                workbook.CurrentWorksheet.AddNextCellFormula("ROUNDDOWN(A2,1)"); // Add cell B3 as formula (Floor A2 with one decimal place)
                workbook.CurrentWorksheet.AddNextCellFormula("PI()");       // Add cell C3 as formula (Pi = 3.14.... )

                workbook.AddWorksheet("Addresses");                                                 // Add new worksheet
                workbook.CurrentWorksheet.CurrentCellDirection = Worksheet.CellDirection.Disabled;  // Disable automatic addressing
                workbook.CurrentWorksheet.AddCell("Default", 0, 0);                                  // Add a value
                Cell.Address address = new Cell.Address(1, 0, Cell.AddressType.Default);            // Create Address with default behavior
                workbook.CurrentWorksheet.AddCell(address.ToString(), 1, 0);                         // Add the string of the address
                workbook.CurrentWorksheet.AddCell("Fixed Column", 0, 1);                            // Add a value
                address = new Cell.Address(1, 1, Cell.AddressType.FixedColumn);                     // Create Address with fixed column
                workbook.CurrentWorksheet.AddCell(address.ToString(), 1, 1);                        // Add the string of the address
                workbook.CurrentWorksheet.AddCell("Fixed Row", 0, 2);                               // Add a value
                address = new Cell.Address(1, 2, Cell.AddressType.FixedRow);                        // Create Address with fixed row
                workbook.CurrentWorksheet.AddCell(address.ToString(), 1, 2);                        // Add the string of the address
                workbook.CurrentWorksheet.AddCell("Fixed Row and Column", 0, 3);                    // Add a value
                address = new Cell.Address(1, 3, Cell.AddressType.FixedRowAndColumn);               // Create Address with fixed row and column
                workbook.CurrentWorksheet.AddCell(address.ToString(), 1, 3);                        // Add the string of the address

                workbook.CurrentWorksheet.AddCell("Gerhard", "D5");             // Add cell D5

                workbook.Save();
            }
        }

        [TestMethod]
        public void DirekteAddressierung()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_02.xlsx");

            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddCell("Lifeprojects.de", "A1");
                workbook.CurrentWorksheet.AddCell("Gerhard", "B2");
                workbook.CurrentWorksheet.AddCell(DateTime.Now.ToString(), "C3");
                workbook.CurrentWorksheet.AddCell(4711, "D4");
                workbook.CurrentWorksheet.AddCell(77.99M, "e5");
                workbook.Save();
            }
        }

        [TestMethod]
        public void SpaltenbreitenZeilenhöhen()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_03.xlsx");

            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                List<object> values = new List<object>() { "Header1", "Header2", "Header3" };
                workbook.CurrentWorksheet.AddCellRange(values, new Cell.Address(0, 0), new Cell.Address(2, 0));
                workbook.CurrentWorksheet.Cells["A1"].SetStyle(Style.BasicStyles.Bold);
                workbook.CurrentWorksheet.Cells["B1"].SetStyle(Style.BasicStyles.Bold);
                workbook.CurrentWorksheet.Cells["C1"].SetStyle(Style.BasicStyles.Bold);

                workbook.CurrentWorksheet.SetAutoFilter(1, 3);

                Style styleDT = new Style();
                Style.NumberFormat nf = new Style.NumberFormat();
                nf.Number = Style.NumberFormat.FormatNumber.format_14;
                styleDT.CurrentNumberFormat = nf;

                workbook.CurrentWorksheet.GoToNextRow();                                                         // Go to Row 2
                workbook.CurrentWorksheet.AddNextCell(DateTime.Now, styleDT);                                    // Add cell A2
                workbook.CurrentWorksheet.AddNextCell(2);                                                        // Add cell B2
                workbook.CurrentWorksheet.AddNextCell(3);                                                        // Add cell B2
                workbook.CurrentWorksheet.GoToNextRow();                                                         // Go to Row 3
                workbook.CurrentWorksheet.AddNextCell(DateTime.Now.AddDays(1), styleDT);                                  // Add cell B1
                workbook.CurrentWorksheet.AddNextCell("B");                                                      // Add cell B2
                workbook.CurrentWorksheet.AddNextCell("C");                                                      // Add cell B3

                Style s = new Style();                                                                           // Create new style
                s.CurrentFill.SetColor("FF22FF11", Style.Fill.FillType.fillColor);                               // Set fill color
                s.CurrentFont.Underline = Style.Font.UnderlineValue.u_double;                                    // Set double underline
                s.CurrentCellXf.HorizontalAlign = Style.CellXf.HorizontalAlignValue.center;                      // Set alignment

                Style s2 = s.CopyStyle();                                                                        // Copy the previously defined style
                s2.CurrentFont.Italic = true;                                                                    // Change an attribute of the copied style

                workbook.CurrentWorksheet.Cells["B2"].SetStyle(s);                                               // Assign style to cell
                workbook.CurrentWorksheet.GoToNextRow();                                                         // Go to Row 3
                workbook.CurrentWorksheet.AddNextCell(DateTime.Now.AddDays(2), styleDT);                                  // Add cell B1
                workbook.CurrentWorksheet.AddNextCell(true);                                                     // Add cell B2
                workbook.CurrentWorksheet.AddNextCell(false, s2);                                                // Add cell B3 with style in the same step 
                workbook.CurrentWorksheet.Cells["C2"].SetStyle(Style.BasicStyles.BorderFrame);                   // Assign predefined basic style to cell

                Style s3 = Style.BasicStyles.Strike;                                                             // Create a style from a predefined style
                s3.CurrentCellXf.TextRotation = 45;                                                              // Set text rotation
                s3.CurrentCellXf.VerticalAlign = Style.CellXf.VerticalAlignValue.center;                         // Set alignment

                workbook.CurrentWorksheet.Cells["B4"].SetStyle(s3);                                              // Assign style to cell

                Style s4 = Style.BasicStyles.BoldItalic;                                                         // Create a style from a predefined style
                s4.CurrentCellXf.HorizontalAlign = Style.CellXf.HorizontalAlignValue.right;                      // Set text alignment
                s4.CurrentCellXf.Indent = 4;                                                                     // Set indentation
                workbook.CurrentWorksheet.AddCell("Text", 1, 4, s4);                                             // Assign style to cell B5

                workbook.CurrentWorksheet.SetColumnWidth(0, 20f);                                                // Set column width
                workbook.CurrentWorksheet.SetColumnWidth(1, 15f);                                                // Set column width
                workbook.CurrentWorksheet.SetColumnWidth(2, 25f);                                                // Set column width
                workbook.CurrentWorksheet.SetRowHeight(0, 20);                                                   // Set row height
                workbook.CurrentWorksheet.SetRowHeight(1, 30);

                workbook.Save();
            }
        }
        [TestMethod]
        public void Zellenbereiche()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_04.xlsx");

            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                List<object> values = new List<object>() { "Header1", "Header2", "Header3" };               // Create a List of values
                workbook.CurrentWorksheet.SetActiveStyle(Style.BasicStyles.BorderFrameHeader);              // Assign predefined basic style as active style
                workbook.CurrentWorksheet.AddCellRange(values, "A1:C1");                                    // Add cell range

                values = new List<object>() { "Cell A2", "Cell B2", "Cell C2" };                            // Create a List of values
                workbook.CurrentWorksheet.SetActiveStyle(Style.BasicStyles.BorderFrame);                    // Assign predefined basic style as active style
                workbook.CurrentWorksheet.AddCellRange(values, "A2:C2");                                    // Add cell range (using active style)

                values = new List<object>() { "Cell A3", "Cell B3", "Cell C3" };                            // Create a List of values
                workbook.CurrentWorksheet.AddCellRange(values, "A3:C3");                                    // Add cell range (using active style)

                values = new List<object>() { "Cell A4", "Cell B4", "Cell C4" };                            // Create a List of values
                workbook.CurrentWorksheet.ClearActiveStyle();                                               // Clear the active style 
                workbook.CurrentWorksheet.AddCellRange(values, "A4:C4");                                    // Add cell range (without style)

                workbook.WorkbookMetadata.Title = "Test 5";                                                 // Add meta data to workbook
                workbook.WorkbookMetadata.Subject = "This is the 5th PicoXLSX test";                        // Add meta data to workbook
                workbook.WorkbookMetadata.Creator = "PicoXLSX";                                             // Add meta data to workbook
                workbook.WorkbookMetadata.Keywords = "Keyword1;Keyword2;Keyword3";                          // Add meta data to workbook

                workbook.Save();
            }
        }

        [TestMethod]
        public void ZusammenfügensVonZellen()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_05.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddNextCell("Merged1");                                           // Add cell A1
                workbook.CurrentWorksheet.MergeCells("A1:C1");                                              // Merge cells from A1 to C1
                workbook.CurrentWorksheet.GoToNextRow();                                                    // Go to next row
                workbook.CurrentWorksheet.AddNextCell(false);                                               // Add cell A2
                workbook.CurrentWorksheet.MergeCells("A2:D2");                                              // Merge cells from A2 to D1
                workbook.CurrentWorksheet.GoToNextRow();                                                    // Go to next row
                workbook.CurrentWorksheet.AddNextCell("22.2d");                                             // Add cell A3
                workbook.CurrentWorksheet.MergeCells("A3:E4");                                              // Merge cells from A3 to E4
                workbook.AddWorksheet("Protected");                                                         // Add a new worksheet
                workbook.CurrentWorksheet.AddAllowedActionOnSheetProtection(Worksheet.SheetProtectionValue.sort);               // Allow to sort sheet (worksheet is automatically set as protected)
                workbook.CurrentWorksheet.AddAllowedActionOnSheetProtection(Worksheet.SheetProtectionValue.insertRows);         // Allow to insert rows
                workbook.CurrentWorksheet.AddAllowedActionOnSheetProtection(Worksheet.SheetProtectionValue.selectLockedCells);  // Allow to select cells (locked cells caused automatically to select unlocked cells)
                workbook.CurrentWorksheet.AddNextCell("Cell A1");                                           // Add cell A1
                workbook.CurrentWorksheet.AddNextCell("Cell B1");                                           // Add cell B1
                workbook.CurrentWorksheet.Cells["A1"].SetCellLockedState(false, true);                      // Set the locking state of cell A1 (not locked but value is hidden when cell selected)
                workbook.AddWorksheet("PWD-Protected");                                                     // Add a new worksheet
                workbook.CurrentWorksheet.AddCell("This worksheet is password protected. The password is:", 0, 0);  // Add cell A1
                workbook.CurrentWorksheet.AddCell("test123", 0, 1);                                         // Add cell A2
                workbook.CurrentWorksheet.SetSheetProtectionPassword("test123");                            // Set the password "test123"
                workbook.SetWorkbookProtection(true, true, true, null);
                workbook.Save();
            }
        }

        [TestMethod]
        public void AusblendensZeilenSpaltenAutofilter()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_06.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                string invalidSheetName = "Sheet?1";                                                        // ? is not allowed in the names of worksheets
                string sanitizedSheetName = Worksheet.SanitizeWorksheetName(invalidSheetName, workbook);    // Method to sanitize a worksheet name (replaces ? with _)
                workbook.AddWorksheet(sanitizedSheetName);                                                  // Add new worksheet
                Worksheet ws = workbook.CurrentWorksheet;                                                   // Create reference (shortening)
                List<object> values = new List<object>() { "Cell A1", "Cell B1", "Cell C1", "Cell D1" };    // Create a List of values
                ws.AddCellRange(values, "A1:D1");                                                           // Insert cell range
                values = new List<object>() { "Cell A2", "Cell B2", "Cell C2", "Cell D2" };                 // Create a List of values
                ws.AddCellRange(values, "A2:D2");                                                           // Insert cell range
                values = new List<object>() { "Cell A3", "Cell B3", "Cell C3", "Cell D3" };                 // Create a List of values
                ws.AddCellRange(values, "A3:D3");                                                           // Insert cell range
                ws.AddHiddenColumn("C");                                                                    // Hide column C
                ws.AddHiddenRow(1);                                                                         // Hider row 2 (zero-based: 1)
                ws.SetAutoFilter(1, 3);                                                                     // Set auto-filter for column B to D
                workbook.Save();

            }
        }

        [TestMethod]
        public void ZellUndArbeitsblattauswahl()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_07.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddNextCell("Test");                                              // Add cell A1
                workbook.CurrentWorksheet.AddSelectedCells("A5:B10");                                       // Set the selection to the range A5:B10
                workbook.CurrentWorksheet.AddSelectedCells("D2:D2");                                        // Add another cell as selected on this worksheet
                workbook.AddWorksheet("Sheet2");                                                            // Create new worksheet
                workbook.CurrentWorksheet.AddNextCell("Test2");                                             // Add cell A1
                Cell.Range range = new Cell.Range(new Cell.Address(1, 1), new Cell.Address(3, 3));          // Create a cell range for the selection B2:D4
                workbook.CurrentWorksheet.SetSelectedCells(range);                                          // Set the selection to the range (deprecated method to clear all previous definitions)
                workbook.AddWorksheet("Sheet2", true);                                                      // Create new worksheet with already existing name; The name will be changed to Sheet21 due to auto-sanitizing (appending of 1)
                workbook.CurrentWorksheet.AddNextCell("Test3");                                             // Add cell A1
                workbook.CurrentWorksheet.AddSelectedCells(new Cell.Address(2, 2), new Cell.Address(4, 4)); // Set the selection to the range C3:E5
                workbook.CurrentWorksheet.AddSelectedCells(new Cell.Address(5, 1), new Cell.Address(5, 1)); // Set the selection to F2 as range
                workbook.SetSelectedWorksheet(1);                                                           // Set the second Tab as selected (zero-based: 1)
                workbook.Save();
            }
        }

        [TestMethod]
        public void GrundlegendenExcelFormeln()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_08.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                List<object> numbers = new List<object> { 1.15d, 2.225d, 13.8d, 15d, 15.1d, 17.22d, 22d, 107.5d, 128d }; // Create a list of numbers
                List<object> texts = new List<object>() { "value 1", "value 2", "value 3", "value 4", "value 5", "value 6", "value 7", "value 8", "value 9" }; // Create a list of strings (for vlookup)

                workbook.WS.Value("Numbers", Style.BasicStyles.Bold);                                       // Add a header with a basic style
                workbook.WS.Value("Values", Style.BasicStyles.Bold);                                        // Add a header with a basic style
                workbook.WS.Value("Formula type", Style.BasicStyles.Bold);                                  // Add a header with a basic style
                workbook.WS.Value("Formula value", Style.BasicStyles.Bold);                                 // Add a header with a basic style
                workbook.WS.Value("(See also worksheet2)");                                                 // Add a note

                workbook.CurrentWorksheet.AddCellRange(numbers, "A2:A10");                                  // Add the numbers as range
                workbook.CurrentWorksheet.AddCellRange(texts, "B2:B10");                                    // Add the values as range

                workbook.CurrentWorksheet.SetCurrentCellAddress("D2");                                      // Set the "cursor" to D2
                Cell c;                                                                                     // Create an empty cell object (reusable)
                c = Cell.BasicFormulas.Average(new Cell.Range("A2:A10"));                                   // Define an average formula
                workbook.CurrentWorksheet.AddCell("Average", "C2");                                         // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D2");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Ceil(new Cell.Address("A2"), 0);                                     // Define a ceil formula
                workbook.CurrentWorksheet.AddCell("Ceil", "C3");                                            // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D3");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Floor(new Cell.Address("A2"), 0);                                    // Define a floor formula
                workbook.CurrentWorksheet.AddCell("Floor", "C4");                                           // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D4");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Round(new Cell.Address("A3"), 1);                                    // Define a round formula with one digit after the comma
                workbook.CurrentWorksheet.AddCell("Round", "C5");                                           // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D5");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Max(new Cell.Range("A2:A10"));                                       // Define a max formula
                workbook.CurrentWorksheet.AddCell("Max", "C6");                                             // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D6");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Min(new Cell.Range("A2:A10"));                                       // Define a min formula
                workbook.CurrentWorksheet.AddCell("Min", "C7");                                             // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D7");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Median(new Cell.Range("A2:A10"));                                    // Define a median formula
                workbook.CurrentWorksheet.AddCell("Median", "C8");                                          // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D8");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.Sum(new Cell.Range("A2:A10"));                                       // Define a sum formula
                workbook.CurrentWorksheet.AddCell("Sum", "C9");                                             // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D9");                                                 // Add the formula to the worksheet

                c = Cell.BasicFormulas.VLookup(13.8d, new Cell.Range("A2:B10"), 2, true);                   // Define a vlookup formula (look for the value of the number 13.8) 
                workbook.CurrentWorksheet.AddCell("Vlookup", "C10");                                        // Add the description of the formula to the worksheet
                workbook.CurrentWorksheet.AddCell(c, "D10");                                                // Add the formula to the worksheet

                workbook.AddWorksheet("sheet2");                                                            // Create a new worksheet
                c = Cell.BasicFormulas.VLookup(workbook.Worksheets[0], new Cell.Address("B4"), workbook.Worksheets[0], new Cell.Range("B2:C10"), 2, true); // Define a vlookup formula in worksheet1 (look for the text right of the (value of) cell B4) 
                workbook.WS.Value(c);                                                                       // Add the formula to the worksheet

                c = Cell.BasicFormulas.Median(workbook.Worksheets[0], new Cell.Range("A2:A10"));            // Define a median formula in worksheet1
                workbook.WS.Value(c);                                                                       // Add the formula to the worksheet

                workbook.Save();
            }
        }

        [TestMethod]
        public void StyleAppending()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_09.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                Style style = new Style();                                                                  // Create a new style
                style.Append(Style.BasicStyles.Bold);                                                       // Append a basic style (bold) 
                style.Append(Style.BasicStyles.Underline);                                                  // Append a basic style (underline) 
                style.Append(Style.BasicStyles.Font("Arial Black", 20));                                    // Append a basic style (custom font) 

                workbook.WS.Value("THIS IS A TEST", style);                                                       // Add text and the appended style
                workbook.WS.Down();                                                                               // Go to a new row

                Style chainedStyle = new Style()                                                            // Create a new style...
                    .Append(Style.BasicStyles.Underline)                                                    // ... and append another part (chaining underline)
                    .Append(Style.BasicStyles.ColorizedText("FF00FF"))                                      // ... and append another part (chaining colorized text)
                    .Append(Style.BasicStyles.ColorizedBackground("AAFFAA"));                               // ... and append another part (chaining colorized background)

                workbook.WS.Value("Another test", chainedStyle);                                                  // Add text and the appended style
                workbook.Save();
            }
        }

        [TestMethod]
        public void SetStyleMethoden()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_10.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                Style style = new Style();                                                            // Create a new style
                style.Append(Style.BasicStyles.ColorizedBackground("FF0000"));                        // Append a visible style component

                workbook.CurrentWorksheet.AddCell("Test", "C3", Style.BasicStyles.Bold);                    // Define a cell with a style (will be replaced)
                workbook.CurrentWorksheet.SetStyle("A1", style);                                                              // Set style based on a string address
                workbook.CurrentWorksheet.SetStyle("A3:B6", style);                                                           // Set style based on a string address range
                workbook.CurrentWorksheet.SetStyle(new Cell.Address(0, 7), style);                                            // Set style based on a address object
                workbook.CurrentWorksheet.SetStyle(new Cell.Range(new Cell.Address("C1"), new Cell.Address(4, 8)), style);    // Set style based on a range object (overwrites style on C3)
                workbook.CurrentWorksheet.SetStyle(new Cell.Address("F6"), new Cell.Address("F10"), style);                   // Set style based on a two address objects as range
                workbook.Save();
            }
        }

        [TestMethod]
        public void AufteilensEinfrierensArbeitsblatts()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_11.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.SetVerticalSplit(30f, new Cell.Address("D1"), Worksheet.WorksheetPane.topRight);       // Split worksheet vertically by characters
                workbook.AddWorksheet("SplitXcols");                                                                             // Create new worksheet
                workbook.CurrentWorksheet.SetVerticalSplit(4, false, new Cell.Address("E1"), Worksheet.WorksheetPane.topRight);  // Split worksheet vertically by columns
                workbook.CurrentWorksheet.SetColumnWidth(0, 15f);                                                                // Define column width
                workbook.CurrentWorksheet.SetColumnWidth(1, 20f);                                                                // Define column width
                workbook.CurrentWorksheet.SetColumnWidth(2, 35f);                                                                // Define column width

                workbook.AddWorksheet("SplitYchars");                                                                             // Create new worksheet
                workbook.CurrentWorksheet.SetHorizontalSplit(20f, new Cell.Address("C1"), Worksheet.WorksheetPane.bottomLeft);    // Split worksheet horizontally by characters
                workbook.AddWorksheet("SplitYcols");                                                                              // Create new worksheet
                workbook.CurrentWorksheet.SetHorizontalSplit(5, false, new Cell.Address("A6"), Worksheet.WorksheetPane.bottomLeft);// Split worksheet horizontally by rows
                workbook.CurrentWorksheet.SetRowHeight(0, 10f);                                                                   // Define row height
                workbook.CurrentWorksheet.SetRowHeight(3, 35f);                                                                   // Define row height
                workbook.CurrentWorksheet.SetRowHeight(2, 25f);                                                                   // Define row height

                workbook.AddWorksheet("SplitXYchars");                                                                            // Create new worksheet
                workbook.CurrentWorksheet.SetSplit(30f, 20f, new Cell.Address("D3"), Worksheet.WorksheetPane.bottomRight);        // Split worksheet horizontally and vertically by characters

                workbook.AddWorksheet("SplitXYColRow");                                                                           // Create new worksheet
                workbook.CurrentWorksheet.SetSplit(3, 10, false, new Cell.Address("D11"), Worksheet.WorksheetPane.bottomRight);    // Split worksheet horizontally and vertically by rows and columns

                workbook.AddWorksheet("FreezeXcols");                                                                              // Create new worksheet
                workbook.CurrentWorksheet.SetVerticalSplit(4, true, new Cell.Address("E1"), Worksheet.WorksheetPane.topRight);     // Split and freeze worksheet vertically by columns

                workbook.AddWorksheet("FreezeYcols");                                                                              // Create new worksheet
                workbook.CurrentWorksheet.SetHorizontalSplit(1, true, new Cell.Address("A2"), Worksheet.WorksheetPane.bottomLeft); // Split and freeze worksheet horizontally by rows

                workbook.AddWorksheet("FreezeXYColRow");                                                                           // Create new worksheet
                workbook.CurrentWorksheet.SetSplit(3, 10, true, new Cell.Address("D11"), Worksheet.WorksheetPane.bottomRight);     // Split and freeze worksheet horizontally and vertically by rows and columns

                workbook.AddWorksheet("FreezeXYColRow1");                                                                           // Create new worksheet
                workbook.CurrentWorksheet.SetSplit(1, 1, true, new Cell.Address("D2"), Worksheet.WorksheetPane.bottomRight);     // Split and freeze worksheet horizontally and vertically by rows and columns
                workbook.CurrentWorksheet.AddCell("Id", "A1", Style.BasicStyles.Bold);
                workbook.CurrentWorksheet.AddCell("Name", "B1", Style.BasicStyles.Bold);
                workbook.Save();
            }
        }

        [TestMethod]
        public void AusblendensArbeitsmappenArbeitsblättern()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_12.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddNextCell("Hidden Workbook");
                workbook.Hidden = true;                                                               // Set the workbook hidden (Set visible again in another, visible workbook)
                workbook.Save();                                                                      // Save the workbook

                ExcelWriter wb2 = new ExcelWriter("Demo_12(hidden_worksheet).xlsx", "visible");        // Create a new workbook
                wb2.CurrentWorksheet.AddNextCell("Visible Worksheet");
                wb2.AddWorksheet("hidden");                                                     // Create new worksheet
                wb2.CurrentWorksheet.AddNextCell("Hidden Worksheet");
                wb2.CurrentWorksheet.Hidden = true;                                             // Set the current worksheet hidden
                wb2.Save();
            }
        }

        [TestMethod]
        public void BasicDemoA()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_13.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddNextCell("Test");              // Add cell A1
                workbook.CurrentWorksheet.AddNextCell(55.2);                // Add cell B1
                workbook.CurrentWorksheet.AddNextCell(DateTime.Now);        // Add cell C1
                workbook.Save();
            }
        }

        [TestMethod]
        public void BasicDemoB()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_13.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.WS.Value("Some Text");                                                    // Add cell A1
                workbook.WS.Value(58.55, Style.BasicStyles.DoubleUnderline);                       // Add a formatted value to cell B1
                workbook.WS.Right(2);                                                              // Move to cell E1   
                workbook.WS.Value(true);                                                           // Add cell E1
                workbook.AddWorksheet("Sheet2");                                                   // Add a new worksheet
                workbook.CurrentWorksheet.CurrentCellDirection = Worksheet.CellDirection.RowToRow; // Change the cell direction
                workbook.WS.Value("This is another text");                                         // Add cell A1
                workbook.WS.Formula("=A1");                                                        // Add a formula in Cell A2
                workbook.WS.Down();                                                                // Go to cell A4
                workbook.WS.Value("Formatted Text", Style.BasicStyles.Bold);                       // Add a formatted value to cell A4
                workbook.Save();
            }
        }

        [TestMethod]
        public void BasicDemoCSaveAsStream()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_14.xlsx");
            using (ExcelWriter workbook = new ExcelWriter(pathFileName, "Sheet1"))
            {
                workbook.CurrentWorksheet.AddNextCell("This is an example");           // Add cell A1
                workbook.CurrentWorksheet.AddNextCellFormula("=A1");                   // Add formula in cell B1
                workbook.CurrentWorksheet.AddNextCell(123456789);                      // Add cell C1            }

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAsStream(ms, true);                                   // Save the workbook into the MemoryStream; IMPORTANT: Leave stream open (2nd parameter = true)
                    ms.Position = 0;                                                   // Reset the stream position
                    using (StreamReader sr = new StreamReader(ms))                     // Pass MemoryStream to StreamReader
                    {
                        string binaryData = sr.ReadToEnd();                            // Write Stream to a string 
                        Trace("Number of symbols: " + binaryData.Length);  // Write some "useful" data
                    }
                }

                string pathFileNameA = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelWriter\\DemoData\\Demo_15(Stream).xlsx");
                using (FileStream fs = new FileStream(pathFileNameA, FileMode.Create)) // Create a FileStream
                {
                    workbook.SaveAsStream(fs);                                         // Save the workbook into the FileStream and close the stream after writing
                }
            }
        }
    }
}