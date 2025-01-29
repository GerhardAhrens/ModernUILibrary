namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using System.Data;

    public static class DataReaderExtension
    {
        /// <summary>
        /// convert datareader to dictionary list
        /// </summary>
        /// <param name="dr">data source</param>
        /// <remarks>this function do not enumerate next result,so dr includes multiple results,you must execute multiple times per NextResult()</remarks>
        /// <returns>string(name)-object(value) key-value dictionary of datareader</returns>
        public static IEnumerable<IDictionary<string, object>> ToDictionary(this IDataReader dr)
        {
            int fieldCount = -1;
            string[] names = null;
            while (dr.Read())
            {
                if (fieldCount < 0)
                {
                    fieldCount = dr.FieldCount;
                    names = new string[fieldCount];
                }
                var dic = new Dictionary<string, object>();
                for (int i = 0; i < fieldCount; i++)
                {
                    if(names[i] == null)
                    {
                        names[i] = dr.GetName(i);
                    }
                    dic[names[i]] = dr.GetValue(i);
                }
                yield return dic;
            }
        }
    }
}
