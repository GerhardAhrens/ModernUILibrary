//-----------------------------------------------------------------------
// <copyright file="DataTableExtensions.cs" company="Lifeprojects.de">
//     Class: DataTableExtensions
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Extension Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Xml;

    public static class DataTableExtensions
    {
        public static bool IsNullOrEmpty(this DataTable @this)
        {
            return @this == null && @this.AsEnumerable().Any() == false;
        }

        public static bool IsNotNullOrEmpty(this DataTable @this)
        {
            return @this != null && @this.AsEnumerable().Any() == true;
        }

        public static void RenameColumn(this DataTable @this, string oldName, string newName)
        {
            if (@this != null && !string.IsNullOrEmpty(oldName) && !string.IsNullOrEmpty(newName) && oldName != newName)
            {
                int idx = @this.Columns.IndexOf(oldName);
                if (idx > 0)
                {
                    @this.Columns[idx].ColumnName = newName;
                    @this.AcceptChanges();
                }
                else
                {
                    throw new ArgumentException($"Column '{oldName}' not found!");
                }
            }
        }

        public static void RemoveColumn(this DataTable @this, string columnName)
        {
            if (@this != null && !string.IsNullOrEmpty(columnName) && @this.Columns.IndexOf(columnName) >= 0)
            {
                int idx = @this.Columns.IndexOf(columnName);
                @this.Columns.RemoveAt(idx);
                @this.AcceptChanges();
            }
        }

        public static Dictionary<string, Type> ColumnsToDictionary(this DataTable @this)
        {
            return @this.Columns
                .Cast<DataColumn>()
                .AsEnumerable<DataColumn>()
                .ToDictionary<DataColumn, string, Type>(col => col.ColumnName, col => col.DataType);
        }

        /// <summary>
        /// A DataTable extension method that return the first row.
        /// </summary>
        /// <param name="this">The table to act on.</param>
        /// <returns>The first row of the table.</returns>
        public static DataRow FirstRow(this DataTable @this)
        {
            return @this.Rows[0];
        }

        /// <summary>
        /// the DataTable extension method returns a row that matches the criterion
        /// </summary>
        /// <param name="this">The table to act on.</param>
        /// <returns>The first row of the table.</returns>
        public static DataRow FindRow(this DataTable @this, Func<DataRow, bool> filterCondition)
        {
            return @this.AsEnumerable().Where(filterCondition).FirstOrDefault();
        }

        /// <summary>
        /// the DataTable extension method returns a row that matches the criterion
        /// </summary>
        /// <param name="this">The table to act on.</param>
        /// <returns>The first row of the table.</returns>
        public static DataRow[] FindRows(this DataTable @this, Func<DataRow, bool> filterCondition)
        {
            return @this.AsEnumerable().Where(filterCondition).ToArray();
        }

        /// <summary>A DataTable extension method that last row.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DataRow.</returns>
        public static DataRow LastRow(this DataTable @this)
        {
            return @this.Rows[@this.Rows.Count - 1];
        }

        public static DataTable AsDataTable<T>(this IEnumerable<T> @this)
        {
            var table = new DataTable();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in @this)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static List<T> ToListOf<T>(this DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        #region ToSorting
        public static DataTable ToSorting(this DataTable dt, ListSortDirection direction, string colName)
        {
            DataTable dataTableOut = null;

            string sortOrder = string.Empty;

            if (direction == ListSortDirection.Ascending)
            {
                sortOrder = "ASC";
            }
            else
            {
                sortOrder = "DESC";
            }

            try
            {
                dt.DefaultView.Sort = $"{colName} {sortOrder}";
                dataTableOut = dt.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw new Exception("Sort: \n" + ex.Message);
            }

            return dataTableOut;
        }

        public static DataTable ToSorting(this DataTable @this, ListSortDirection direction, params string[] colName)
        {
            DataTable dataTableOut = null;

            string sortOrder = string.Empty;

            if (direction == ListSortDirection.Ascending)
            {
                sortOrder = "ASC";
            }
            else
            {
                sortOrder = "DESC";
            }

            try
            {
                string sortColumns = $"{string.Join(",", colName)} {sortOrder}";

                @this.DefaultView.Sort = sortColumns;
                dataTableOut = @this.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw new Exception("Sort: \n" + ex.Message);
            }

            return dataTableOut;
        }
        #endregion ToSorting

        public static Dictionary<string, string> GetColumnsName(this DataTable @this)
        {
            Dictionary<string, string> columnNames = null;

            if (@this != null)
            {
                columnNames = new Dictionary<string, string>();

                foreach (DataColumn item in @this.Columns)
                {
                    if (item.ColumnName.ToLower() == "flags")
                    {
                        columnNames.Add(item.ColumnName, string.Format("{{{{{0}}}}}", item.ColumnName));
                    }
                    else
                    {
                        columnNames.Add(item.ColumnName, item.DataType.Name);
                    }
                }
            }

            return columnNames;
        }

        public static DataTable ToDataTable<T>(IList<T> @this)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in @this)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;

        }

        public static DataTable ToDataTable(this IEnumerable<dynamic> @this, string tableName, IDictionary<string, Type> fields)
        {
            DataTable tbl = ToDataTableIntern<dynamic>(@this, fields);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        public static DataTable ToDataTable(this IEnumerable<dynamic> @this, string tableName)
        {
            DataTable tbl = ToDataTable(@this);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        public static DataTable ToDataTable(this IEnumerable<dynamic> @this)
        {
            DataTable tbl = ToDataTable(@this);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> @this, string tableName, IDictionary<string, Type> fields)
        {
            DataTable tbl = ToDataTableIntern<T>(@this, fields);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> @this, string tableName)
        {
            DataTable tbl = ToDataTableIntern<T>(@this);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> @this)
        {
            DataTable tbl = ToDataTableIntern<T>(@this);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        public static DataTable ToDataTableFromColumn<T>(this IEnumerable<T> @this, string tableName, IDictionary<string, Type> fields)
        {
            DataTable tbl = ToDataTableFromColumnIntern<T>(@this, fields);
            if (tbl != null)
            {
                tbl.TableName = @this.GetType().Name;
            }

            return tbl;
        }

        /// <summary>
        /// Renove doublicate entry in DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <example>
        /// DataTable resultTable = mainTable.DistinctDataTable();
        /// </example>
        public static DataTable DistinctDataTable(this DataTable table)
        {
            var resultTable = table.Clone();
            IEnumerable<DataRow> uniqueElements = table.AsEnumerable().Distinct(DataRowComparer.Default);
            foreach (var row in uniqueElements)
            {
                resultTable.ImportRow(row);
            }
            return resultTable;
        }

        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        #region Select Distinct
        /// <summary>
        /// "SELECT DISTINCT" over a DataTable
        /// </summary>
        /// <param name="SourceTable">Input DataTable</param>
        /// <param name="FieldNames">Fields to select (distinct)</param>
        /// <returns></returns>
        /// <example>
        /// DataTable dt2 = dt.SelectDistinct("Column1, Column2");
        /// </example>
        public static DataTable SelectDistinct(this DataTable SourceTable, string FieldName)
        {
            return SelectDistinct(SourceTable, FieldName, string.Empty);
        }

        /// <summary>
        ///"SELECT DISTINCT" over a DataTable
        /// </summary>
        /// <param name="SourceTable">Input DataTable</param>
        /// <param name="FieldNames">Fields to select (distinct)</param>
        /// <param name="Filter">Optional filter to be applied to the selection</param>
        /// <returns></returns>
        public static DataTable SelectDistinct(this DataTable SourceTable, string FieldNames, string Filter)
        {
            DataTable dt = new DataTable();
            string[] arrFieldNames = FieldNames.Replace(" ", "").Split(',');
            foreach (string s in arrFieldNames)
            {
                if (SourceTable.Columns.Contains(s))
                    dt.Columns.Add(s, SourceTable.Columns[s].DataType);
                else
                    throw new Exception(string.Format("The column {0} does not exist.", s));
            }

            object[] LastValues = null;
            foreach (DataRow dr in SourceTable.Select(Filter, FieldNames))
            {
                object[] NewValues = GetRowFields(dr, arrFieldNames);
                if (LastValues == null || !(ObjectComparison(LastValues, NewValues)))
                {
                    LastValues = NewValues;
                    dt.Rows.Add(LastValues);
                }
            }

            return dt;
        }


        #endregion Select Distinct

        #region Interne Methoden
        private static DataTable ToDataTableIntern(this IEnumerable<dynamic> @this)
        {
            var firstRecord = @this.FirstOrDefault();
            if (firstRecord == null)
            {
                return null;
            }

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();
            if (infos == null || infos.Count() == 0)
            {
                return null;
            }

            DataTable table = new DataTable();

            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(info.Name, info.PropertyType);
                }
            }

            DataRow row;

            foreach (var record in @this)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }

        private static DataTable ToDataTableIntern<T>(this IEnumerable<dynamic> @this, IDictionary<string, Type> fields)
        {
            var firstRecord = @this.FirstOrDefault();
            if (firstRecord == null)
            {
                return null;
            }

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();
            if (infos == null || infos.Count() == 0)
            {
                return null;
            }

            List<PropertyInfo> infoList = infos.ToList<PropertyInfo>();

            DataTable table = new DataTable();

            foreach (PropertyInfo info in infoList)
            {
                if (fields.Any(p => p.Key == info.Name))
                {
                    Type propType = info.PropertyType;

                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                    }
                    else
                    {
                        table.Columns.Add(info.Name, info.PropertyType);
                    }
                }
            }

            DataRow row = null;
            foreach (T record in @this)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    PropertyInfo info = infoList.SingleOrDefault<PropertyInfo>(p => p.Name == table.Columns[i].ColumnName);

                    row[i] = info.GetValue(record) != null ? info.GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }

        private static DataTable ToDataTableIntern<T>(this IEnumerable<T> @this)
        {
            var firstRecord = @this.FirstOrDefault();
            if (firstRecord == null)
            {
                return null;
            }

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();
            if (infos == null || infos.Count() == 0)
            {
                return null;
            }

            DataTable table = new DataTable();

            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(info.Name, info.PropertyType);
                }
            }

            DataRow row;

            foreach (var record in @this)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }

        private static DataTable ToDataTableIntern<T>(this IEnumerable<T> @this, IDictionary<string, Type> fields)
        {
            var firstRecord = @this.FirstOrDefault();
            if (firstRecord == null)
            {
                return null;
            }

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();
            if (infos == null || infos.Count() == 0)
            {
                return null;
            }

            List<PropertyInfo> infoList = infos.ToList<PropertyInfo>();

            DataTable table = new DataTable();

            foreach (PropertyInfo info in infoList)
            {
                if (fields.Any(p => p.Key == info.Name))
                {
                    Type propType = info.PropertyType;

                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                    }
                    else
                    {
                        table.Columns.Add(info.Name, info.PropertyType);
                    }
                }
            }

            DataRow row = null;
            foreach (T record in @this)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    PropertyInfo info = infoList.SingleOrDefault<PropertyInfo>(p => p.Name == table.Columns[i].ColumnName);

                    row[i] = info.GetValue(record) != null ? info.GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }

        private static DataTable ToDataTableFromColumnIntern<T>(this IEnumerable<T> @this, IDictionary<string, Type> fields)
        {
            var firstRecord = @this.FirstOrDefault();
            if (firstRecord == null)
            {
                return null;
            }

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();
            if (infos == null || infos.Count() == 0)
            {
                return null;
            }

            List<PropertyInfo> infoList = infos.ToList<PropertyInfo>();

            DataTable table = new DataTable();

            foreach (KeyValuePair<string, Type> column in fields)
            {
                if (infoList.Any(p => p.Name == column.Key))
                {
                    PropertyInfo propInfo = infoList.Single(p => p.Name == column.Key);
                    if (column.Value.IsGenericType && column.Value.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        table.Columns.Add(column.Key, Nullable.GetUnderlyingType(column.Value));
                    }
                    else
                    {
                        table.Columns.Add(column.Key, column.Value);
                    }
                }
            }

            DataRow row = null;
            foreach (T record in @this)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    PropertyInfo info = infoList.SingleOrDefault<PropertyInfo>(p => p.Name == table.Columns[i].ColumnName);

                    row[i] = info.GetValue(record) != null ? info.GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }

        private static object[] GetRowFields(DataRow dr, string[] arrFieldNames)
        {
            if (arrFieldNames.Length == 1)
                return new object[] { dr[arrFieldNames[0]] };
            else
            {
                ArrayList itemArray = new ArrayList();
                foreach (string field in arrFieldNames)
                {
                    itemArray.Add(dr[field]);
                }

                return itemArray.ToArray();
            }
        }

        /// <summary>
        /// Compares two values to see if they are equal. Also compares DBNULL.Value.
        /// </summary>
        /// <param name="A">Object A</param>
        /// <param name="B">Object B</param>
        /// <returns></returns>
        private static bool ObjectComparison(object a, object b)
        {
            if (a == DBNull.Value && b == DBNull.Value) //  both are DBNull.Value
            {
                return true;
            }

            if (a == DBNull.Value || b == DBNull.Value) //  only one is DBNull.Value
            {
                return false;
            }

            return (a.Equals(b));  // value type standard comparison
        }

        /// <summary>
        /// Compares two value arrays to see if they are equal. Also compares DBNULL.Value.
        /// </summary>
        /// <param name="A">Object Array A</param>
        /// <param name="B">Object Array B</param>
        /// <returns></returns>
        private static bool ObjectComparison(object[] a, object[] b)
        {
            Boolean retValue = true;
            Boolean singleCheck = false;

            if (a.Length == b.Length)
                for (int i = 0; i < a.Length; i++)
                {
                    if (!(singleCheck = ObjectComparison(a[i], b[i])))
                    {
                        retValue = false;
                        break;
                    }

                    retValue = retValue && singleCheck;
                }

            return retValue;
        }
        #endregion Interne Methoden

        #region AddRange
        public static void AddRange(this DataRowCollection rc, IEnumerable<object[]> tuples)
        {
            foreach (object[] data in tuples)
            {
                rc.Add(tuples);
            }
        }
        #endregion AddRange

        #region GetColumnDataType

        public static Type GetColumnDataType(this DataTable tbl, string ColumnName)
        {
            try
            {
                return tbl.Columns[ColumnName].DataType;
            }
            catch (Exception ex)
            {
                throw new Exception("GetColumnDataType: \n" + ex.Message);
            }
        }

        public static Type GetColumnDataType(this DataTable tbl, int ColumnIndex)
        {
            try
            {
                return tbl.Columns[ColumnIndex].DataType;
            }
            catch (Exception ex)
            {
                throw new Exception("GetColumnDataType: \n" + ex.Message);
            }
        }

        #endregion GetColumnDataType

        #region GetColumnValue
        public static T GetColumnValue<T>(this DataTable tbl, int ColInd, int RowInd)
        {
            try
            {
                object column = tbl.Rows[RowInd][ColInd];
                return column == DBNull.Value ? default(T) : (T)Convert.ChangeType(column, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetColumnValue<T>(this DataTable tbl, string ColumnName, int RowInd)
        {
            try
            {
                object column = tbl.Rows[RowInd][ColumnName];
                return column == DBNull.Value ? default(T) : (T)Convert.ChangeType(column, typeof(T));

            }
            catch
            {
                return default(T);
            }
        }
        #endregion GetColumnValue

        #region CopyToDataTable
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
        {
            return new ObjectDataTableConverter<T>().Shred(source, null, null);
        }
        #endregion CopyToDataTable

        #region CompareTo
        public static bool CompareTo(this DataTable firstDataTable, DataTable secondDataTable)
        {
            DataTable dt;
            dt = GetDifferentRecords(firstDataTable, secondDataTable);

            if (dt.Rows.Count == 0)
                return true;
            else
                return false;
        }

        public static DataTable GetDifferentRecords(this DataTable firstDataTable, DataTable secondDataTable)
        {
            //Create Empty Table     
            DataTable ResultDataTable = new DataTable("ResultDataTable");

            //use a Dataset to make use of a DataRelation object     
            using (DataSet ds = new DataSet())
            {
                //Add tables     
                ds.Tables.AddRange(new DataTable[] { firstDataTable.Copy(), secondDataTable.Copy() });

                //Get Columns for DataRelation     
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = ds.Tables[1].Columns[i];
                }

                //Create DataRelation     
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                //Create columns for return table     
                for (int i = 0; i < firstDataTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(firstDataTable.Columns[i].ColumnName, firstDataTable.Columns[i].DataType);
                }

                //If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.     
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.     
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }
                ResultDataTable.EndLoadData();
            }

            return ResultDataTable;
        }
        #endregion CompareTo    }
    }
}