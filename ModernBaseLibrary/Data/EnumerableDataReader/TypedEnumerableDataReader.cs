namespace ModernBaseLibrary.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Data;
    using System.Collections.Concurrent;

    using System.Collections;
    using System.Data.Common;

    public class TypedEnumerableDataReader<T> : DbDataReader
    {

        private static readonly Lazy<TypedFunctionMap<T>> _StaticFunctionMap = new Lazy<TypedFunctionMap<T>>();
        private IEnumerable<T> _DataList;
        private IEnumerator<T> _CurrentItem;

        public TypedEnumerableDataReader(IEnumerable<T> dataList)
        {
            _DataList = dataList;
            _CurrentItem = dataList.GetEnumerator();
        }

        private TypedFunctionMap<T> _FunctionMap
        {
            get
            {
                return _StaticFunctionMap.Value;
            }
        }

        public new void Dispose()
        {
            Close();
            base.Dispose();
        }

        public override object this[string name]
        {
            get
            {
                return _FunctionMap.ObjectGetters[_FunctionMap.NameIndexMapping[name]](_CurrentItem.Current);
            }
        }

        public override object this[int i]
        {
            get
            {
                return _FunctionMap.ObjectGetters[i](_CurrentItem.Current);
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
                return _CurrentItem == null;
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
                return _CurrentItem != null || _DataList.Any();
            }
        }

        public override bool GetBoolean(int i)
        {
            return _FunctionMap.BoolGetters[i](_CurrentItem.Current);
        }

        public override byte GetByte(int i)
        {
            return _FunctionMap.ByteGetters[i](_CurrentItem.Current);
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var data = ((IEnumerable<byte>)_FunctionMap.ObjectGetters[i](_CurrentItem.Current)).Skip((int)fieldOffset).Take(length).ToArray();
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
            return _FunctionMap.CharGetters[i](_CurrentItem.Current);
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var data = ((IEnumerable<char>)_FunctionMap.ObjectGetters[i](_CurrentItem.Current)).Skip((int)fieldoffset).Take(length).ToArray();
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
                return _FunctionMap.DateGetters[i](_CurrentItem.Current);
            }
            else
            {
                return DateTime.Parse(_FunctionMap.ObjectGetters[i](_CurrentItem.Current).ToString());
            }
        }

        public override decimal GetDecimal(int i)
        {
            if (_FunctionMap.DecimalGetters.ContainsKey(i))
            {
                return _FunctionMap.DecimalGetters[i](_CurrentItem.Current);
            }
            else if (_FunctionMap.IntGetters.ContainsKey(i))
            {
                return _FunctionMap.IntGetters[i](_CurrentItem.Current);
            }
            else if (_FunctionMap.LongGetters.ContainsKey(i))
            {
                return _FunctionMap.LongGetters[i](_CurrentItem.Current);
            }
            else if (_FunctionMap.ShortGetters.ContainsKey(i))
            {
                return _FunctionMap.ShortGetters[i](_CurrentItem.Current);
            }
            else if (_FunctionMap.ByteGetters.ContainsKey(i))
            {
                return _FunctionMap.ByteGetters[i](_CurrentItem.Current);
            }
            else
            {
                return (decimal)_FunctionMap.ObjectGetters[i](_CurrentItem.Current);
            }
        }

        public override double GetDouble(int i)
        {
            return _FunctionMap.DoubleGetters[i](_CurrentItem.Current);
        }

        public override Type GetFieldType(int i)
        {
            return _FunctionMap.MemberTypeMapping[_FunctionMap.IndexNameMapping[i]];
        }

        public override float GetFloat(int i)
        {
            return _FunctionMap.FloatGetters[i](_CurrentItem.Current);
        }

        public override Guid GetGuid(int i)
        {
            return _FunctionMap.GuidGetters[i](_CurrentItem.Current);
        }

        public override short GetInt16(int i)
        {
            return _FunctionMap.ShortGetters[i](_CurrentItem.Current);
        }

        public override int GetInt32(int i)
        {
            return _FunctionMap.IntGetters[i](_CurrentItem.Current);
        }

        public override long GetInt64(int i)
        {
            return _FunctionMap.LongGetters[i](_CurrentItem.Current);
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
            return _FunctionMap.ObjectGetters[i](_CurrentItem.Current).ToString();
        }

        public override object GetValue(int i)
        {
            return _FunctionMap.ObjectGetters[i](_CurrentItem.Current);
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
            var obj = _FunctionMap.ObjectGetters[i](_CurrentItem.Current);
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
            if (!_CurrentItem.MoveNext())
            {
                _CurrentItem = null;
                return false;
            }
            else
            {
                return true;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return _CurrentItem;
        }

        new public void Close()
        {
            _CurrentItem?.Dispose();
            _CurrentItem = null;
        }
    }
}
