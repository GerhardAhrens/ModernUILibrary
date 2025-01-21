//-----------------------------------------------------------------------
// <copyright file="ExportExcelCSV.cs" company="Lifeprojects.de">
//     Class:   
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>02.03.2023</date>
//
// <summary>
// Export DataTable oder List<T> zu einer CSV/XML-Datei
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.ExcelCSV
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using ModernBaseLibrary.Core;

    public class ExportExcelCSV : DisposableCoreBase
    {
        public Encoding csvExportEncoding { get; set; } = Encoding.Default;

        public string Sheetname { get; set; } = "Tabelle1";

        public bool IsDateOnly { get; set; } = true;

        public char TextQualifier { get; set; } = '"';

        public string DateFormat { get; set; } = "dd.MM.yyyy";

        public char DecimalSeparator { get; set; } = ',';

        public Action<int,int> CallbackName = null;

        public void ListToWorksheet(DataTable dataSource, string fileName, List<string> excludeField, bool withHeader, Dictionary<string, string> headerTranslation = null, char separator = ';')
        {

            try
            {
                if (File.Exists(fileName) == true)
                {
                    File.Delete(fileName);
                }

                if (excludeField != null && excludeField.Count > 0)
                {
                    foreach (string column in excludeField)
                    {
                        dataSource.Columns.Remove(column);
                    }
                }

                if (Path.GetExtension(fileName).ToUpper() == ".XML")
                {
                    DataTableToWorksheetXML(dataSource, fileName, withHeader, headerTranslation);
                }
                else if (Path.GetExtension(fileName).ToUpper() == ".CSV")
                {
                    DataTableToWorksheetCSV(dataSource, fileName, withHeader, headerTranslation, separator);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListToWorksheet(DataTable dataSource, string fileName)
        {

            try
            {
                if (File.Exists(fileName) == true)
                {
                    File.Delete(fileName);
                }

                if (Path.GetExtension(fileName).ToUpper() == ".XML")
                {
                    DataTableToWorksheetXML(dataSource, fileName, true, null);
                }
                else if (Path.GetExtension(fileName).ToUpper() == ".CSV")
                {
                    DataTableToWorksheetCSV(dataSource, fileName, true, null, ';');
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListToWorksheet<T>(IList<T> dataSource, string pFilename, List<string> pExcludeField, bool pWithHeader, Dictionary<string, string> pHeaderTranslation = null, char separator = ';')
        {

            try
            {
                DataTable dtSource = ToDataTable(dataSource, pExcludeField);

                if (File.Exists(pFilename) == true)
                {
                    File.Delete(pFilename);
                }

                if (Path.GetExtension(pFilename).ToUpper() == ".XML")
                {
                    DataTableToWorksheetXML(dtSource, pFilename, pWithHeader, pHeaderTranslation);
                }
                else if (Path.GetExtension(pFilename).ToUpper() == ".CSV")
                {
                    DataTableToWorksheetCSV(dtSource, pFilename, pWithHeader, pHeaderTranslation, separator);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListToWorksheet<T>(IList<T> dataSource, string pFilename, List<string> pExcludeField, Dictionary<string, string> pHeaderTranslation = null, char separator = ';')
        {

            try
            {
                DataTable dtSource = ToDataTable(dataSource, pExcludeField);

                if (File.Exists(pFilename) == true)
                {
                    File.Delete(pFilename);
                }

                if (Path.GetExtension(pFilename).ToUpper() == ".XML")
                {
                    DataTableToWorksheetXML(dtSource, pFilename, true, pHeaderTranslation);
                }
                else if (Path.GetExtension(pFilename).ToUpper() == ".CSV")
                {
                    DataTableToWorksheetCSV(dtSource, pFilename, true, pHeaderTranslation, separator);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListToWorksheet(List<string> dataSource, string pFilename, Dictionary<string, string> pHeaderTranslation = null, char separator = ';')
        {

            try
            {
                DataTable dtSource = ToDataTable(dataSource);

                if (File.Exists(pFilename) == true)
                {
                    File.Delete(pFilename);
                }

                if (Path.GetExtension(pFilename).ToUpper() == ".XML")
                {
                    DataTableToWorksheetXML(dtSource, pFilename, false, pHeaderTranslation);
                }
                else if (Path.GetExtension(pFilename).ToUpper() == ".CSV")
                {
                    DataTableToWorksheetCSV(dtSource, pFilename, true, pHeaderTranslation, separator);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Die Methode gibt die übergebene Liste als DataTable zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns>DataTable</returns>
        private DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            table.TableName = typeof(T).Name;
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data.AsParallel())
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        private DataTable ToDataTable<T>(IList<T> data, List<string> pExcludeField)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            table.TableName = typeof(T).Name;

            try
            {
                foreach (PropertyDescriptor prop in properties)
                {
                    if (FieldExclude(pExcludeField, prop.Name) == true)
                    {
                        if (prop.PropertyType.BaseType.Name == "Enum")
                        {
                            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(typeof(string)) ?? typeof(string));
                        }
                        else
                        {
                            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        }
                    }
                }

                foreach (T item in data.AsParallel())
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (FieldExclude(pExcludeField, prop.Name) == true)
                        {
                            if (prop.GetValue(item) != null)
                            {
                                if (prop.GetValue(item).GetType().BaseType.Name == "Enum")
                                {
                                    row[prop.Name] = prop.GetValue(item).ToString();
                                }
                                else
                                {
                                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                                }
                            }
                            else
                            {
                                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                            }
                        }
                    }
                    table.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }
            return table;
        }

        private List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        pro.SetValue(objT, row[pro.Name], null);
                    }
                }

                return objT;
            }).ToList();

        }

        /// <summary>
        /// Erzeugt aus einer DataTable ein Excel-XML-Dokument mit SpreadsheetML.
        /// </summary>
        /// <param name="pDataSource">The p data source.</param>
        /// <param name="pFileName">Name of the p file.</param>
        /// <param name="pWithHeader">if set to <c>true</c> [p with header].</param>
        private void DataTableToWorksheetXML(DataTable pDataSource, string pFileName, bool pWithHeader = false, Dictionary<string, string> pHeaderTranslation = null)
        {
            int maxRows = 0;

            if (pDataSource == null || pDataSource.Rows.Count <= 0)
            {
                return;
            }

            try
            {
                maxRows = pDataSource.Rows.Count;

                // XML-Schreiber erzeugen
                XmlTextWriter writer = new XmlTextWriter(pFileName, Encoding.UTF8);

                // Ausgabedatei für bessere Lesbarkeit formatieren (einrücken etc.)
                writer.Formatting = Formatting.Indented;

                // <?xml version="1.0"?>
                writer.WriteStartDocument();

                // <?mso-application progid="Excel.Sheet"?>
                writer.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                // <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet >"
                writer.WriteStartElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet");

                // Definition der Namensräume schreiben 
                writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
                writer.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
                writer.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
                writer.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");

                // <DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
                writer.WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office");

                // Dokumenteingeschaften schreiben
                writer.WriteElementString("Author", Environment.UserName);
                writer.WriteElementString("LastAuthor", Environment.UserName);
                writer.WriteElementString("Created", DateTime.Now.ToString("u") + "Z");
                writer.WriteElementString("Company", "Unknown");
                writer.WriteElementString("Version", "14.00");

                // </DocumentProperties>
                writer.WriteEndElement();

                // <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
                writer.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");

                // Arbeitsmappen-Einstellungen schreiben
                writer.WriteElementString("WindowHeight", "10110");
                writer.WriteElementString("WindowWidth", "17100");
                writer.WriteElementString("WindowTopX", "120");
                writer.WriteElementString("WindowTopY", "60");
                writer.WriteElementString("TabRatio", "500");
                writer.WriteElementString("MaxIterations", "1");
                writer.WriteElementString("ProtectStructure", "False");
                writer.WriteElementString("ProtectWindows", "False");

                // </ExcelWorkbook>
                writer.WriteEndElement();

                // <Styles>
                writer.WriteStartElement("Styles");

                // <Style ss:ID="Default" ss:Name="Normal">
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "Default");
                writer.WriteAttributeString("ss", "Name", null, "Normal");

                // <Alignment ss:Vertical="Bottom"/>
                writer.WriteStartElement("Alignment");
                writer.WriteAttributeString("ss", "Vertical", null, "Bottom");
                writer.WriteEndElement();

                // Verbleibende Sytle-Eigenschaften leer schreiben
                writer.WriteElementString("Borders", null);
                writer.WriteElementString("Font", null);
                writer.WriteElementString("Interior", null);
                writer.WriteElementString("NumberFormat", null);
                writer.WriteElementString("Protection", null);

                // </Style>
                writer.WriteEndElement();

                // <Style> NumberFormat
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "s63");
                writer.WriteStartElement("NumberFormat");
                writer.WriteAttributeString("ss", "Format", null, "Fixed");
                writer.WriteEndElement();
                // </Style> NumberFormat
                writer.WriteEndElement();

                // <Style> Date Short
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "s64");
                writer.WriteStartElement("NumberFormat");
                writer.WriteAttributeString("ss", "Format", null, "Short Date");
                writer.WriteEndElement();
                // </Style> Date Short
                writer.WriteEndElement();

                // <Style> Currency
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "s65");
                writer.WriteStartElement("NumberFormat");
                writer.WriteAttributeString("ss", "Format", null, string.Format("{0}\\ €", "#,##0.00"));
                writer.WriteEndElement();
                // </Style> Currency
                writer.WriteEndElement();

                // <Style> Integer
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "s67");
                writer.WriteStartElement("NumberFormat");
                writer.WriteAttributeString("ss", "Format", null, "0");
                writer.WriteEndElement();
                // </Style> Integer
                writer.WriteEndElement();

                // <Style> styleHeader
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "styleHeader");
                writer.WriteStartElement("Font");
                writer.WriteAttributeString("ss", "FontName", null, "Arial");
                writer.WriteAttributeString("x", "Family", null, "Swiss");
                writer.WriteAttributeString("ss", "Bold", null, "1");
                writer.WriteEndElement();
                // </Style> styleHeader
                writer.WriteEndElement();

                // </Styles>
                writer.WriteEndElement();

                // <Worksheet ss:Name="xxx">
                writer.WriteStartElement("Worksheet");
                if (string.IsNullOrEmpty(Sheetname) == true)
                {
                    writer.WriteAttributeString("ss", "Name", null, pDataSource.TableName);
                }
                else
                {
                    writer.WriteAttributeString("ss", "Name", null, this.Sheetname);
                }

                // <Table ss:ExpandedColumnCount="2" ss:ExpandedRowCount="3" x:FullColumns="1" x:FullRows="1" ss:DefaultColumnWidth="60">
                writer.WriteStartElement("Table");
                writer.WriteAttributeString("ss", "ExpandedColumnCount", null, pDataSource.Columns.Count.ToString());
                int excelRowCount = pDataSource.Rows.Count;
                if (pWithHeader == false)
                {
                    writer.WriteAttributeString("ss", "ExpandedRowCount", null, excelRowCount.ToString());
                }
                else
                {
                    excelRowCount++;
                    writer.WriteAttributeString("ss", "ExpandedRowCount", null, excelRowCount.ToString());
                }
                writer.WriteAttributeString("x", "FullColumns", null, "1");
                writer.WriteAttributeString("x", "FullRows", null, "1");
                writer.WriteAttributeString("ss", "DefaultColumnWidth", null, "60");

                if (pWithHeader == true)
                {
                    string header = string.Empty;
                    writer.WriteStartElement("Row");
                    foreach (DataColumn col in pDataSource.Columns)
                    {
                        // <Cell>
                        writer.WriteStartElement("Cell");
                        writer.WriteAttributeString("ss", "StyleID", null, "styleHeader");

                        // <Data ss:Type="String">xxx</Data>
                        writer.WriteStartElement("Data");
                        writer.WriteAttributeString("ss", "Type", null, "String");

                        // Headertranslation
                        if (pHeaderTranslation != null)
                        {
                            header = (pHeaderTranslation.ContainsKey(col.ColumnName)) ? pHeaderTranslation[col.ColumnName] : col.ColumnName;
                        }
                        else
                        {
                            header = col.ColumnName;
                        }

                        writer.WriteValue(header);

                        // </Data>
                        writer.WriteEndElement();

                        // </Cell>
                        writer.WriteEndElement();
                    }
                    // </Row>
                    writer.WriteEndElement();

                }

                int currentRow = 0;

                // Alle Zeilen der Datenquelle durchlaufen
                foreach (DataRow row in pDataSource.Rows)
                {
                    // <Row>
                    writer.WriteStartElement("Row");

                    // Alle Zellen der aktuellen Zeile durchlaufen
                    foreach (DataColumn cellType in row.Table.Columns)
                    {

                        object cellValue = row[cellType.Ordinal];

                        // Zelleninhakt schreiben
                        if (cellValue != null)
                        {
                            if (cellValue.ToString() == "01.01.0001 00:00:00" || cellValue.ToString() == "01.01.1900 00:00:00")
                            {
                                // <Cell>
                                writer.WriteStartElement("Cell");
                                // <Data ss:Type="String">xxx</Data>
                                writer.WriteStartElement("Data");
                                writer.WriteAttributeString("ss", "Type", null, "String");
                                writer.WriteValue(string.Empty);
                                // </Data>
                                writer.WriteEndElement();
                                // </Cell>
                                writer.WriteEndElement();
                            }
                            else
                            {
                                if (cellType.DataType.Name == "DateTime")
                                {
                                    // <Cell>
                                    writer.WriteStartElement("Cell");
                                    writer.WriteAttributeString("ss", "StyleID", null, "s64");
                                    // <Data ss:Type="String">xxx</Data>
                                    writer.WriteStartElement("Data");
                                    writer.WriteAttributeString("ss", "Type", null, "DateTime");
                                    if (this.IsDateOnly == false)
                                    {
                                        if (string.IsNullOrEmpty(cellValue.ToString()) == false)
                                        {
                                            string dt = Convert.ToDateTime(cellValue).ToString("yyyy-MM-dd");
                                            writer.WriteValue(dt);
                                        }
                                        else
                                        {
                                            writer.WriteValue(string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(cellValue.ToString()) == false)
                                        {
                                            string dt = Convert.ToDateTime(cellValue).ToString("yyyy-MM-ddThh:mm:ss.fff");
                                            writer.WriteValue(dt);
                                        }
                                        else
                                        {
                                            writer.WriteValue(string.Empty);
                                        }
                                    }
                                    // </Data>
                                    writer.WriteEndElement();
                                    // </Cell>
                                    writer.WriteEndElement();
                                }
                                else if (cellType.DataType.Name == "Guid")
                                {
                                    // <Cell>
                                    writer.WriteStartElement("Cell");
                                    // <Data ss:Type="String">xxx</Data>
                                    writer.WriteStartElement("Data");
                                    writer.WriteAttributeString("ss", "Type", null, "String");
                                    string dt = cellValue.ToString();
                                    writer.WriteValue(dt);
                                    // </Data>
                                    writer.WriteEndElement();
                                    // </Cell>
                                    writer.WriteEndElement();
                                }
                                else if (cellType.DataType.Name == "Decimal")
                                {
                                    if (cellValue.ToString() == "0" || string.IsNullOrEmpty(cellValue.ToString()) == true)
                                    {
                                        writer.WriteValue(string.Empty);
                                        writer.WriteStartElement("Cell");
                                        // <Data ss:Type="String">xxx</Data>
                                        writer.WriteStartElement("Data");
                                        writer.WriteAttributeString("ss", "Type", null, "String");
                                        writer.WriteValue(string.Empty);
                                        // </Data>
                                        writer.WriteEndElement();
                                        // </Cell>
                                        writer.WriteEndElement();
                                    }
                                    else
                                    {
                                        // <Cell>
                                        writer.WriteStartElement("Cell");
                                        writer.WriteAttributeString("ss", "StyleID", null, "s65");
                                        // <Data ss:Type="Number">xxx</Data>
                                        writer.WriteStartElement("Data");
                                        writer.WriteAttributeString("ss", "Type", null, "Number");
                                        string dt = cellValue.ToString().Replace(',', '.');
                                        writer.WriteValue(dt);
                                        // </Data>
                                        writer.WriteEndElement();
                                        // </Cell>
                                        writer.WriteEndElement();
                                    }

                                }
                                else if (cellType.DataType.Name == "Double")
                                {
                                    if (cellValue.ToString() == "0" || string.IsNullOrEmpty(cellValue.ToString()) == true)
                                    {
                                        writer.WriteValue(string.Empty);
                                        writer.WriteStartElement("Cell");
                                        // <Data ss:Type="String">xxx</Data>
                                        writer.WriteStartElement("Data");
                                        writer.WriteAttributeString("ss", "Type", null, "String");
                                        writer.WriteValue(string.Empty);
                                        // </Data>
                                        writer.WriteEndElement();
                                        // </Cell>
                                        writer.WriteEndElement();
                                    }
                                    else
                                    {
                                        // <Cell>
                                        writer.WriteStartElement("Cell");
                                        writer.WriteAttributeString("ss", "StyleID", null, "s63");
                                        // <Data ss:Type="Number">xxx</Data>
                                        writer.WriteStartElement("Data");
                                        writer.WriteAttributeString("ss", "Type", null, "Number");
                                        string dt = cellValue.ToString().Replace(',', '.');
                                        writer.WriteValue(dt);
                                        // </Data>
                                        writer.WriteEndElement();
                                        // </Cell>
                                        writer.WriteEndElement();
                                    }

                                }
                                else if (cellType.DataType.Name == "Int32" || cellType.DataType.Name == "Int64")
                                {
                                    if (cellValue.ToString() == "0" || string.IsNullOrEmpty(cellValue.ToString()) == true)
                                    {
                                        writer.WriteValue(string.Empty);
                                        writer.WriteStartElement("Cell");
                                        // <Data ss:Type="String">xxx</Data>
                                        writer.WriteStartElement("Data");
                                        writer.WriteAttributeString("ss", "Type", null, "String");
                                        writer.WriteValue(string.Empty);
                                        // </Data>
                                        writer.WriteEndElement();
                                        // </Cell>
                                        writer.WriteEndElement();
                                    }
                                    else
                                    {
                                        // <Cell>
                                        writer.WriteStartElement("Cell");
                                        writer.WriteAttributeString("ss", "StyleID", null, "s67");
                                        // <Data ss:Type="Number">xxx</Data>
                                        writer.WriteStartElement("Data");
                                        writer.WriteAttributeString("ss", "Type", null, "Number");
                                        string dt = cellValue.ToString();
                                        writer.WriteValue(dt);
                                        // </Data>
                                        writer.WriteEndElement();
                                        // </Cell>
                                        writer.WriteEndElement();
                                    }
                                }
                                else
                                {
                                    // <Cell>
                                    writer.WriteStartElement("Cell");
                                    // <Data ss:Type="String">xxx</Data>
                                    writer.WriteStartElement("Data");
                                    writer.WriteAttributeString("ss", "Type", null, "String");
                                    writer.WriteValue(cellValue.ToString());
                                    // </Data>
                                    writer.WriteEndElement();
                                    // </Cell>
                                    writer.WriteEndElement();
                                }
                            }
                        }

                    }
                    // </Row>
                    writer.WriteEndElement();

                    currentRow++;

                    if (this.CallbackName != null)
                    {
                        this.CallbackName(maxRows, currentRow);
                    }

                }
                // </Table>
                writer.WriteEndElement();

                // <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
                writer.WriteStartElement("WorksheetOptions", "urn:schemas-microsoft-com:office:excel");

                // Seiteneinstellungen schreiben
                writer.WriteStartElement("PageSetup");
                writer.WriteStartElement("Header");
                writer.WriteAttributeString("x", "Margin", null, "0.4921259845");
                writer.WriteEndElement();
                writer.WriteStartElement("Footer");
                writer.WriteAttributeString("x", "Margin", null, "0.4921259845");
                writer.WriteEndElement();
                writer.WriteStartElement("PageMargins");
                writer.WriteAttributeString("x", "Bottom", null, "0.984251969");
                writer.WriteAttributeString("x", "Left", null, "0.78740157499999996");
                writer.WriteAttributeString("x", "Right", null, "0.78740157499999996");
                writer.WriteAttributeString("x", "Top", null, "0.984251969");
                writer.WriteEndElement();
                writer.WriteEndElement();

                // <Selected/>
                writer.WriteElementString("Selected", null);

                // <Panes>
                writer.WriteStartElement("Panes");

                // <Pane>
                writer.WriteStartElement("Pane");

                // Bereichseigenschaften schreiben
                writer.WriteElementString("Number", "1");
                writer.WriteElementString("ActiveRow", "1");
                writer.WriteElementString("ActiveCol", "1");

                // </Pane>
                writer.WriteEndElement();

                // </Panes>
                writer.WriteEndElement();

                // <ProtectObjects>False</ProtectObjects>
                writer.WriteElementString("ProtectObjects", "False");

                // <ProtectScenarios>False</ProtectScenarios>
                writer.WriteElementString("ProtectScenarios", "False");

                // </WorksheetOptions>
                writer.WriteEndElement();

                // </Worksheet>
                writer.WriteEndElement();

                // </Workbook>
                writer.WriteEndElement();

                // Datei auf Festplatte schreiben
                writer.Flush();
                writer.Close();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }

        }

        private void DataTableToWorksheetCSV(DataTable pDataSource, string pFileName, bool pWithHeader, Dictionary<string, string> pHeaderTranslation = null, char separator = ';')
        {
            StringBuilder sb = null;
            int maxRows = 0;

            if (pDataSource == null || pDataSource.Rows.Count <= 0)
            {
                return;
            }

            try
            {
                maxRows = pDataSource.Rows.Count;

                sb = new StringBuilder();
                if (pWithHeader == true)
                {
                    string[] columnNames = pDataSource.Columns.Cast<DataColumn>().
                                                      Select(column => column.ColumnName).
                                                      ToArray();
                    if (pHeaderTranslation != null)
                    {
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            string header = columnNames[i];
                            columnNames[i] = (pHeaderTranslation.ContainsKey(header)) ? pHeaderTranslation[header] : header;
                        }

                    }
                    sb.AppendLine(string.Join(separator.ToString(), columnNames));
                }

                int currentRow = 0;
                foreach (DataRow row in pDataSource.Rows.AsParallel())
                {
                    object[] fields = row.ItemArray.Select(field => field).ToArray();

                    for (int i = 0; i < fields.Count(); i++)
                    {

                        if (fields[i].GetType() == typeof(double) || fields[i].GetType() == typeof(decimal))
                        {
                            char decimalSeparator = Convert.ToChar(CultureInfo.GetCultureInfo("de").NumberFormat.NumberDecimalSeparator);
                            fields[i] = fields[i].ToString().Replace(decimalSeparator,this.DecimalSeparator);
                        }
                        else if (fields[i].GetType() == typeof(DateTime))
                        {
                            fields[i] = ((DateTime)fields[i]).ToString(this.DateFormat);
                        }
                        else if (fields[i].GetType() == typeof(string))
                        {
                            if (this.TextQualifier != ' ')
                            {
                                fields[i] = $"{this.TextQualifier}{fields[i]}{this.TextQualifier}";
                            }
                        }
                    }

                    sb.AppendLine(string.Join(separator.ToString(), fields));

                    currentRow++;

                    if (this.CallbackName != null)
                    {
                        this.CallbackName(maxRows, currentRow);
                    }
                }

                File.WriteAllText(pFileName, sb.ToString(), this.csvExportEncoding);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        private bool FieldExclude(List<string> pExcludeField, string pField)
        {
            bool result = false;

            if (pExcludeField == null)
            {
                result = true;
            }
            else
            {
                var count = pExcludeField.FindAll(s => s.Equals(pField.Trim(), StringComparison.OrdinalIgnoreCase) == true).Count;
                if (count == 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return (result);
        }

        private bool FieldInclude(List<string> pExcludeField, string pField)
        {
            bool result = false;

            if (pExcludeField == null)
            {
                result = true;
            }
            else
            {
                var count = pExcludeField.FindAll(s => s.IndexOf(pField.Trim(), StringComparison.OrdinalIgnoreCase) >= 0).Count;
                if (count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return (result);
        }
    }
}