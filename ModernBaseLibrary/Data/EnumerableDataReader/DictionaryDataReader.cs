namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using System.Collections;
    using System.Data;
    using System.Data.Common;

    public class DictionaryDataReader : DbDataReader
    {
        private IEnumerable<IDictionary<string, object>> _Dictionary;
        private IEnumerator<IDictionary<string, object>> _Iterator;
        private IDictionary<string, object> _CurrentItem;
        private IDictionary<int, string> _IndexToNameMapping;
        private IDictionary<string, int> _NameToIndexMapping;

        /// <summary>
        /// creating new dictionary datareader,name list is read from first element.
        /// </summary>
        /// <remarks>
        /// <para>field info(ex. FieldCount,GetName(),etc.) is not enabled until Read() executed</para>
        /// <para>all records are expected to have same keys and types.</para>
        /// </remarks>
        /// <param name="dic">dictionary list.</param>
        public DictionaryDataReader(IEnumerable<IDictionary<string, object>> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }

            _Dictionary = dic;
        }

        public override object this[string name]
        {
            get
            {
                return _CurrentItem[name];
            }
        }

        public override object this[int i]
        {
            get
            {
                return _CurrentItem[_IndexToNameMapping[i]];
            }
        }

        public override int Depth
        {
            get
            {
                return 1;
            }
        }

        public override int FieldCount
        {
            get
            {
                return _IndexToNameMapping.Count();
            }
        }

        public override bool IsClosed
        {
            get
            {
                return _Iterator == null;
            }
        }

        public override int RecordsAffected
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool HasRows
        {
            get
            {
                return true;
            }
        }

        public new void Dispose()
        {
            Close();
            base.Dispose();
        }

        public override bool GetBoolean(int i)
        {
            return (bool)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override byte GetByte(int i)
        {
            return (byte)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var data = _CurrentItem[_IndexToNameMapping[i]] as IEnumerable<byte>;
            long count = 0;
            foreach (var x in data.Skip((int)fieldOffset).Take(length).Select((b, idx) => new { b, idx }))
            {
                buffer[bufferoffset + x.idx] = x.b;
                count++;
            }
            return count;
        }

        public override char GetChar(int i)
        {
            return (char)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var data = _CurrentItem[_IndexToNameMapping[i]] as IEnumerable<char>;
            long count = 0;
            foreach (var x in data.Skip((int)fieldoffset).Take(length).Select((c, idx) => new { c, idx }))
            {
                buffer[bufferoffset + x.idx] = x.c;
                count++;
            }
            return count;
        }

        public override string GetDataTypeName(int i)
        {
            return _CurrentItem[_IndexToNameMapping[i]].GetType().ToString();
        }

        public override DateTime GetDateTime(int i)
        {
            return (DateTime)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override decimal GetDecimal(int i)
        {
            return (decimal)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override double GetDouble(int i)
        {
            return (double)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override Type GetFieldType(int i)
        {
            return _CurrentItem[_IndexToNameMapping[i]].GetType();
        }

        public override float GetFloat(int i)
        {
            return (float)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override Guid GetGuid(int i)
        {
            return (Guid)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override short GetInt16(int i)
        {
            return (short)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override int GetInt32(int i)
        {
            return (int)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override long GetInt64(int i)
        {
            return (long)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override string GetName(int i)
        {
            return _IndexToNameMapping[i];
        }

        public override int GetOrdinal(string name)
        {
            return _NameToIndexMapping[name];
        }

        public override string GetString(int i)
        {
            return (string)_CurrentItem[_IndexToNameMapping[i]];
        }

        public override object GetValue(int i)
        {
            return this[i];
        }

        public override int GetValues(object[] values)
        {
            int count = 0;
            for (int i = 0; i < this.FieldCount && i < values.Length; i++)
            {
                values[i] = this[i];
                count++;
            }

            return count;
        }

        public override bool IsDBNull(int i)
        {
            return _CurrentItem[_IndexToNameMapping[i]] == null
                || _CurrentItem[_IndexToNameMapping[i]] is DBNull;
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            if (_Iterator == null)
            {
                _Iterator = _Dictionary.GetEnumerator();
            }

            var ret = _Iterator.MoveNext();

            if (ret)
            {
                _CurrentItem = _Iterator.Current;
                if (_IndexToNameMapping == null || _NameToIndexMapping == null)
                {
                    _IndexToNameMapping = _CurrentItem.Keys.Select((x, i) => new { x, i }).ToDictionary(x => x.i, x => x.x);
                    _NameToIndexMapping = _IndexToNameMapping.ToDictionary(kv => kv.Value, kv => kv.Key);
                }
            }

            return ret;
        }

        public override IEnumerator GetEnumerator()
        {
            return _Iterator;
        }

        new private void Close()
        {
            _CurrentItem = null;
            _Iterator.Dispose();
            _Iterator = null;
        }
    }
}
