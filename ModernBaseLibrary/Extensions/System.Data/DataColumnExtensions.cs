//-----------------------------------------------------------------------
// <copyright file="DataColumnExtensions.cs" company="Lifeprojects.de">
//     Class: DataColumnExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>Extension Class für DataColumn und DataColumnCollection</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Data;
    using System.Linq;

    public static class DataColumnExtensions
    {
        /// <summary>
        ///     A DataColumnCollection extension method that adds a range to 'columns'.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="columns">A variable-length parameters list containing columns.</param>
        public static void AddRange(this DataColumnCollection @this, params string[] columns)
        {
            foreach (string column in columns)
            {
                @this.Add(column);
            }
        }

        public static bool IsNumeric(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
           typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
           typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};

            return numericTypes.Contains(col.DataType);
        }

        public static bool IsBool(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            var boolTypes = new[] { typeof(bool), typeof(Boolean), typeof(bool?), typeof(Boolean?)};

            return boolTypes.Contains(col.DataType);
        }

        public static bool IsString(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            var stringTypes = new[] { typeof(string), typeof(String), typeof(char), typeof(Char) };

            return stringTypes.Contains(col.DataType);
        }

        public static bool IsDateTime(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }


            var stringTypes = new[] { typeof(DateTime), typeof(DateTime?) };

            return stringTypes.Contains(col.DataType);
        }

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
    }
}