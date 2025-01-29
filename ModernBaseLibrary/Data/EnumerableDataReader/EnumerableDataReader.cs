namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using System.Data;
    using System.Data.Common;
    using System.Collections.Concurrent;
    using System.Collections;

    public class EnumerableDataReader : DbDataReader
    {
        private static readonly ConcurrentDictionary<Type, FunctionMap> _StaticFunctionMap = new ConcurrentDictionary<Type, FunctionMap>();
        private IEnumerable _DataList;
        private IEnumerator _Current;
        private Type _Type;
        private object _Lock = new object();
        private FunctionMap _FunctionMap;

        public EnumerableDataReader(Type t, IEnumerable dataList)
        {
            lock (_Lock)
            {
                if (!_StaticFunctionMap.ContainsKey(t))
                {
                    _StaticFunctionMap[t] = new FunctionMap(t);
                }

                _FunctionMap = _StaticFunctionMap[t];
            }

            _DataList = dataList;
            _Type = t;
            _Current = dataList.GetEnumerator();
        }

        public override object this[string name]
        {
            get
            {
                return _FunctionMap.ObjectGetters[_FunctionMap.NameIndexMapping[name]](_Current.Current);
            }
        }

        public override object this[int i]
        {
            get
            {
                return _FunctionMap.ObjectGetters[i](_Current.Current);
            }
        }

        public override int Depth
        {
            get
            {
                return 0;
            }
        }

        public override int FieldCount
        {
            get
            {
                return _FunctionMap.FieldNum;
            }
        }

        public override bool IsClosed
        {
            get
            {
                return _Current == null;
            }
        }

        public override int RecordsAffected
        {
            get
            {
                return 0;
            }
        }

        public override bool HasRows
        {
            get
            {
                return _Current != null || _DataList.Cast<object>().Any();
            }
        }

        new private void Close()
        {
            _Current = null;
        }

        public new void Dispose()
        {
            Close();
            base.Dispose();
        }

        public override bool GetBoolean(int i)
        {
            return _FunctionMap.BoolGetters[i](_Current.Current);
        }

        public override byte GetByte(int i)
        {
            return _FunctionMap.ByteGetters[i](_Current.Current);
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var data = ((IEnumerable<byte>)_FunctionMap.ObjectGetters[i](_Current.Current)).Skip((int)fieldOffset).Take(length).ToArray();
            long readLength = 0;
            for (int idx = 0; idx < data.Length && idx < length; idx++)
            {
                buffer[bufferoffset + idx] = data[idx];
                readLength += 1;
            }
            return readLength;
        }

        public override char GetChar(int i)
        {
            return _FunctionMap.CharGetters[i](_Current.Current);
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var data = ((IEnumerable<char>)_FunctionMap.ObjectGetters[i](_Current.Current)).Skip((int)fieldoffset).Take(length).ToArray();
            var readlength = 0;
            for (int idx = 0; idx < data.Length; idx++)
            {
                buffer[bufferoffset + idx] = data[idx];
                readlength += 1;
            }
            return readlength;
        }

        public override string GetDataTypeName(int i)
        {
            return _FunctionMap.MemberTypeMapping[_FunctionMap.IndexNameMapping[i]].ToString();
        }

        public override DateTime GetDateTime(int i)
        {
            if (_FunctionMap.DateGetters.ContainsKey(i))
            {
                return _FunctionMap.DateGetters[i](_Current.Current);
            }
            else
            {
                return DateTime.Parse(_FunctionMap.ObjectGetters[i](_Current.Current).ToString());
            }
        }

        public override decimal GetDecimal(int i)
        {
            if (_FunctionMap.DecimalGetters.ContainsKey(i))
            {
                return _FunctionMap.DecimalGetters[i](_Current.Current);
            }
            else if (_FunctionMap.IntGetters.ContainsKey(i))
            {
                return _FunctionMap.IntGetters[i](_Current.Current);
            }
            else if (_FunctionMap.LongGetters.ContainsKey(i))
            {
                return _FunctionMap.LongGetters[i](_Current.Current);
            }
            else if (_FunctionMap.ShortGetters.ContainsKey(i))
            {
                return _FunctionMap.ShortGetters[i](_Current.Current);
            }
            else if (_FunctionMap.ByteGetters.ContainsKey(i))
            {
                return _FunctionMap.ByteGetters[i](_Current.Current);
            }
            else
            {
                return (decimal)_FunctionMap.ObjectGetters[i](_Current.Current);
            }
        }

        public override double GetDouble(int i)
        {
            return _FunctionMap.DoubleGetters[i](_Current.Current);
        }

        public override Type GetFieldType(int i)
        {
            return _FunctionMap.MemberTypeMapping[_FunctionMap.IndexNameMapping[i]];
        }

        public override float GetFloat(int i)
        {
            return _FunctionMap.FloatGetters[i](_Current.Current);
        }

        public override Guid GetGuid(int i)
        {
            return _FunctionMap.GuidGetters[i](_Current.Current);
        }

        public override short GetInt16(int i)
        {
            return _FunctionMap.ShortGetters[i](_Current.Current);
        }

        public override int GetInt32(int i)
        {
            return _FunctionMap.IntGetters[i](_Current.Current);
        }

        public override long GetInt64(int i)
        {
            return _FunctionMap.LongGetters[i](_Current.Current);
        }

        public override string GetName(int i)
        {
            return _FunctionMap.IndexNameMapping[i];
        }

        public override int GetOrdinal(string name)
        {
            return _FunctionMap.NameIndexMapping[name];
        }

        public override string GetString(int i)
        {
            return _FunctionMap.ObjectGetters[i](_Current.Current).ToString();
        }

        public override object GetValue(int i)
        {
            return _FunctionMap.ObjectGetters[i](_Current.Current);
        }

        public override int GetValues(object[] values)
        {
            for (int i = 0; i < values.Length && i < _FunctionMap.FieldNum; i++)
            {
                values[i] = _FunctionMap.ObjectGetters[i];
            }
            return values.Length < _FunctionMap.FieldNum ? values.Length : _FunctionMap.FieldNum;
        }

        public override bool IsDBNull(int i)
        {
            var obj = _FunctionMap.ObjectGetters[i](_Current.Current);
            if (obj == null || obj is DBNull)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            if (!_Current.MoveNext())
            {
                _Current = null;
                return false;
            }
            else
            {
                return true;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return _Current;
        }
    }
}
