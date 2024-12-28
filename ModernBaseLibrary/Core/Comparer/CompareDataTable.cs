//-----------------------------------------------------------------------
// <copyright file="CompareDataTable.cs" company="Lifeprojects.de">
//     Class: CompareDataTable
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>04.11.2019</date>
//
// <summary>Class for CompareDataTable Result</summary>
// <example>
//  DataTable changes = CompareDataTable.CompareDifferences(p1, p2);
// </example>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Comparer
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [DebuggerStepThrough]
    [Serializable]
    public static class CompareDataTable
    {
        public static DataTable CompareDifferences(DataTable sourceTable, DataTable compareTable)
        {
            sourceTable.IsArgumentNull(nameof(sourceTable));
            compareTable.IsArgumentNull(nameof(compareTable));

            DataTable returnTable = new DataTable("returnTable");

            using (DataSet ds = new DataSet())
            {
                ds.Tables.AddRange(new DataTable[] { sourceTable.Copy(), compareTable.Copy() });

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

                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                for (int i = 0; i < sourceTable.Columns.Count; i++)
                {
                    returnTable.Columns.Add(sourceTable.Columns[i].ColumnName, sourceTable.Columns[i].DataType);
                }

                returnTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        returnTable.LoadDataRow(parentrow.ItemArray, true);
                }

                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        returnTable.LoadDataRow(parentrow.ItemArray, true);
                }

                returnTable.EndLoadData();
            }

            return returnTable;
        }
    }
}
