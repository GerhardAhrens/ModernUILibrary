//-----------------------------------------------------------------------
// <copyright file="CompareDataRow.cs" company="Lifeprojects.de">
//     Class: CompareDataRow
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>04.11.2019</date>
//
// <summary>Class for CompareDataRow Result</summary>
// <example>
//  List<CompareResult> changes = CompareObject.CompareDifferences(dr1, dr2);
// </example>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [DebuggerStepThrough]
    [Serializable]
    public static class CompareDataRow
    {
        public static List<CompareResult> CompareDifferences(DataRow sourceRow, DataRow compareRow)
        {
            sourceRow.IsArgumentNull(nameof(sourceRow));
            compareRow.IsArgumentNull(nameof(compareRow));

            List<CompareResult> differenceInfoList = new List<CompareResult>();

            for (int i = 0; i < sourceRow.ItemArray.Length; i++)
            {
                object firstValue = sourceRow.ItemArray[i];
                object secondValue = compareRow.ItemArray[i];
                string tableName = sourceRow.Table.TableName;
                string fieldName = sourceRow.Table.Columns[i].ColumnName;
                Type type = sourceRow.Table.Columns[i].DataType;

                if (object.Equals(firstValue, secondValue) == false)
                {
                    differenceInfoList.Add(new CompareResult(tableName, fieldName, type.Name, firstValue, secondValue));
                }

            }

            return differenceInfoList;
        }

        public static List<CompareResult> CompareDifferences(DataRow sourceRow, DataRow compareRow, params string[] ignoreColumn)
        {
            sourceRow.IsArgumentNull(nameof(sourceRow));
            compareRow.IsArgumentNull(nameof(compareRow));

            List<string> ignoreList = null;
            if (ignoreColumn != null)
            {
                ignoreList = new List<string>(ignoreColumn);
            }

            List<CompareResult> differenceInfoList = new List<CompareResult>();

            for (int i = 0; i < sourceRow.ItemArray.Length; i++)
            {
                object firstValue = sourceRow.ItemArray[i];
                object secondValue = compareRow.ItemArray[i];
                string tableName = sourceRow.Table.TableName;
                string fieldName = sourceRow.Table.Columns[i].ColumnName;
                Type type = sourceRow.Table.Columns[i].DataType;

                if (ignoreList == null)
                {
                    if (object.Equals(firstValue, secondValue) == false)
                    {
                        differenceInfoList.Add(new CompareResult(tableName, fieldName, type.Name, firstValue, secondValue));
                    }
                }
                else
                {
                    if (ignoreList.Contains(fieldName) == false)
                    {
                        if (object.Equals(firstValue, secondValue) == false)
                        {
                            differenceInfoList.Add(new CompareResult(tableName, fieldName, type.Name, firstValue, secondValue));
                        }
                    }
                }

            }

            return differenceInfoList;
        }

        public static List<CompareResult> CompareDifferences(DataRow sourceRow, DataRow compareRow, List<string> ignoreColumn)
        {
            sourceRow.IsArgumentNull(nameof(sourceRow));
            compareRow.IsArgumentNull(nameof(compareRow));

            List<CompareResult> differenceInfoList = new List<CompareResult>();

            for (int i = 0; i < sourceRow.ItemArray.Length; i++)
            {
                object firstValue = sourceRow.ItemArray[i];
                object secondValue = compareRow.ItemArray[i];
                string tableName = sourceRow.Table.TableName;
                string fieldName = sourceRow.Table.Columns[i].ColumnName;
                Type type = sourceRow.Table.Columns[i].DataType;

                if (ignoreColumn == null)
                {
                    if (object.Equals(firstValue, secondValue) == false)
                    {
                        differenceInfoList.Add(new CompareResult(tableName, fieldName, type.Name, firstValue, secondValue));
                    }
                }
                else
                {
                    if (ignoreColumn.Contains(fieldName) == false)
                    {
                        if (object.Equals(firstValue, secondValue) == false)
                        {
                            differenceInfoList.Add(new CompareResult(tableName, fieldName, type.Name, firstValue, secondValue));
                        }
                    }
                }

            }

            return differenceInfoList;
        }
    }
}
