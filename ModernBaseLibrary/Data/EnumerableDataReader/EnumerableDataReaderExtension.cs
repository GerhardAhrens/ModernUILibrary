using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernBaseLibrary.Reader
{
    using System.Data;
    using System.Collections;
    using System.Data.Common;
    public static class EnumerableDataReaderExtension
    {
        /// <summary>
        /// create DbDataReader from IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="list">data source</param>
        /// <returns>DbDataReader instance contains passed list elements</returns>
        /// <remarks>you can ignore or custom name by set attributes</remarks>
        /// <seealso cref="IgnoreFieldAttribute"/>
        /// <seealso cref="FieldNameAsAttribute"/>
        public static DbDataReader AsDataReader<T>(this IEnumerable<T> list)
        {
            return new TypedEnumerableDataReader<T>(list);
        }

        /// <summary>
        /// create DbDataReader from IEnumerable,you must pass element type
        /// </summary>
        /// <param name="list">data list you want to create datareader</param>
        /// <param name="t">data source</param>
        /// <returns>DbDataReader instance contains passed list elements</returns>
        /// <remarks>you can ignore or custom name by set attributes</remarks>
        /// <seealso cref="IgnoreFieldAttribute"/>
        /// <seealso cref="FieldNameAsAttribute"/>
        public static DbDataReader AsDataReader(this IEnumerable list, Type t)
        {
            return new EnumerableDataReader(t, list);
        }

        /// <summary>
        /// create DbDataReader from dictionary
        /// </summary>
        /// <remarks>all elements are expected to have same key</remarks>
        /// <param name="list">data source</param>
        /// <returns>DbDataReader instance contains passed list elements</returns>
        public static DbDataReader AsDataReaderFromDictionary(this IEnumerable<IDictionary<string, object>> list)
        {
            return new DictionaryDataReader(list);
        }
    }
}
