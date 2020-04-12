using blqw.Kanai.Core;
using blqw.Kanai.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;

namespace blqw.Kanai
{
    struct KeyValueEnumerator<TKey, TValue>
    {

        private static readonly Func<NameObjectCollectionBase, int, string> BaseGetKey =
            (Func<NameObjectCollectionBase, int, string>)typeof(NameObjectCollectionBase).GetMethod("BaseGetKey", BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(typeof(Func<NameObjectCollectionBase, int, string>));
        private static readonly Func<NameObjectCollectionBase, int, object> BaseGet =
            (Func<NameObjectCollectionBase, int, object>)typeof(NameObjectCollectionBase).GetMethod("BaseGet", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null).CreateDelegate(typeof(Func<NameObjectCollectionBase, int, object>));


        private readonly IDataReader _reader;
        private readonly IDataRecord _record;
        private readonly DataRow _row;
        private readonly DataSet _dataSet;
        private readonly DataTable _table;
        private readonly IEnumerator _enumerator;
        private readonly IEnumerator<TValue> _enumeratorT;
        private readonly NameObjectCollectionBase _nameObjectCollection;
        private readonly PropertyHandler[] _properties;
        private readonly object _object;

        private readonly IDictionary _dictionary;
        private readonly IEnumerator _keyEnumerator;
        private readonly IDictionary<TKey, TValue> _dictionaryT;
        private readonly IEnumerator<TKey> _keyEnumeratorT;

        private readonly InputType _type;
        private int _index;
        private readonly ConvertContext _context;
        private readonly IConvertor<TKey> _keyConvertor;
        private readonly IConvertor<TValue> _valueConvertor;
        private TValue _single;

        enum InputType
        {
            Undefined,
            DataReader,
            DataRecord,
            DataRow,
            DataSet,
            DataTable,
            Dictionary,
            Enumerator,
            DictionaryT,
            EnumeratorT,
            NameObjectCollection,
            Object,
            Single,
        }

        class DataRecord : IDataRecord
        {
            private readonly IDataRecord _record;

            public DataRecord(IDataRecord record) => _record = record;

            public bool GetBoolean(int i) => _record.GetBoolean(i);
            public byte GetByte(int i) => _record.GetByte(i);
            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => _record.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
            public char GetChar(int i) => _record.GetChar(i);
            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => _record.GetChars(i, fieldoffset, buffer, bufferoffset, length);
            public IDataReader GetData(int i) => _record.GetData(i);
            public string GetDataTypeName(int i) => _record.GetDataTypeName(i);
            public DateTime GetDateTime(int i) => _record.GetDateTime(i);
            public decimal GetDecimal(int i) => _record.GetDecimal(i);
            public double GetDouble(int i) => _record.GetDouble(i);
            public Type GetFieldType(int i) => _record.GetFieldType(i);
            public float GetFloat(int i) => _record.GetFloat(i);
            public Guid GetGuid(int i) => _record.GetGuid(i);
            public short GetInt16(int i) => _record.GetInt16(i);
            public int GetInt32(int i) => _record.GetInt32(i);
            public long GetInt64(int i) => _record.GetInt64(i);
            public string GetName(int i) => _record.GetName(i);
            public int GetOrdinal(string name) => _record.GetOrdinal(name);
            public string GetString(int i) => _record.GetString(i);
            public object GetValue(int i) => _record.GetValue(i);
            public int GetValues(object[] values) => _record.GetValues(values);
            public bool IsDBNull(int i) => _record.IsDBNull(i);

            public int FieldCount => _record.FieldCount;

            public object this[int i] => _record[i];

            public object this[string name] => _record[name];
        }

        public KeyValueEnumerator(ConvertContext context, object input)
        {
            _context = context;
            _keyConvertor = typeof(TKey) == typeof(object) ? null : context.GetConvertor<TKey>();
            _valueConvertor = typeof(TValue) == typeof(object) ? null : context.GetConvertor<TValue>();

            _type = InputType.Undefined;
            _index = -1;
            IsEmpty = false;
            _reader = null;
            _record = null;
            _row = null;
            _dataSet = null;
            _table = null;
            _dictionary = null;
            _enumerator = null;
            _dictionaryT = null;
            _enumeratorT = null;
            _nameObjectCollection = null;
            _properties = null;
            _keyEnumerator = null;
            _keyEnumeratorT = null;
            _object = null;
            _single = default;
            HasStringKey = false;


            switch (input)
            {
                case NameObjectCollectionBase nameObjectCollection:
                    _nameObjectCollection = nameObjectCollection;
                    _type = InputType.NameObjectCollection;
                    HasStringKey = true;
                    break;
                case DataRow row:
                    _row = row;
                    _table = row.Table;
                    _type = InputType.DataRow;
                    HasStringKey = true;
                    break;
                case DataSet dataSet:
                    _dataSet = dataSet;
                    _type = InputType.DataSet;
                    HasStringKey = true;
                    break;
                case DataTable table:
                    _table = table;
                    _type = InputType.DataTable;
                    break;
                case IDataReader reader:
                    _reader = reader;
                    _record = new DataRecord(reader);
                    _type = InputType.DataReader;
                    break;
                case IDataRecord record:
                    _record = record;
                    _type = InputType.DataRecord;
                    HasStringKey = true;
                    break;
                case IDictionary<TKey, TValue> dict:
                    _dictionaryT = dict;
                    _type = InputType.DictionaryT;
                    HasStringKey = true;
                    break;
                case IDictionary dict:
                    _dictionary = dict;
                    _type = InputType.Dictionary;
                    HasStringKey = true;
                    break;
                case IEnumerable<TValue> enumerable:
                    _enumeratorT = enumerable.GetEnumerator();
                    _type = InputType.EnumeratorT;
                    break;
                case IEnumerable enumerable:
                    _enumerator = enumerable.GetEnumerator();
                    _type = InputType.Enumerator;
                    break;
                case IEnumerator<TValue> enumerator:
                    _enumeratorT = enumerator;
                    _type = InputType.EnumeratorT;
                    break;
                case IEnumerator enumerator:
                    _enumerator = enumerator;
                    _type = InputType.Enumerator;
                    break;
                default:
                    if (input is TValue value)
                    {
                        _single = value;
                        _type = InputType.Single;
                    }
                    else
                    {
                        IsEmpty = true;
                    }
                    break;
            }

        }

        public bool MoveNext()
        {
            //var result = _type switch
            //{
            //    InputType.DataReader => _reader.Read(),
            //    InputType.DataRecord => _index + 1 < _reader.FieldCount,
            //    InputType.DataRow => _index + 1 < _table.Columns.Count,
            //    InputType.DataSet => _index + 1 < _dataSet.Tables.Count,
            //    InputType.DataTable => _index + 1 < _table.Rows.Count,
            //    InputType.Dictionary => _index + 1 < _dictionary.Count,
            //    InputType.Enumerator => _enumerator.MoveNext(),
            //    InputType.DictionaryT => _index + 1 < _dictionaryT.Count,
            //    InputType.EnumeratorT => _enumeratorT.MoveNext(),
            //    InputType.NameObjectCollection => _index + 1 < _nameObjectCollection.Count,
            //    InputType.Object => _index + 1 < _properties.Length,
            //    _ => false,
            //};
            var result = TypeSwitchMoveNext();

            if (result)
            {
                _index++;
            }
            return result;
        }

        private bool TypeSwitchMoveNext()
        {
            switch (_type)
            {
                case InputType.DataReader: return _reader.Read();
                case InputType.DataRecord: return _index + 1 < _reader.FieldCount;
                case InputType.DataRow: return _index + 1 < _table.Columns.Count;
                case InputType.DataSet: return _index + 1 < _dataSet.Tables.Count;
                case InputType.DataTable: return _index + 1 < _table.Rows.Count;
                case InputType.Dictionary: return _index + 1 < _dictionary.Count;
                case InputType.Enumerator: return _enumerator.MoveNext();
                case InputType.DictionaryT: return _index + 1 < _dictionaryT.Count;
                case InputType.EnumeratorT: return _enumeratorT.MoveNext();
                case InputType.NameObjectCollection: return _index + 1 < _nameObjectCollection.Count;
                case InputType.Object: return _index + 1 < _properties.Length;
                case InputType.Single: return _index == -1;
                default: return false;
            }
        }

        public ConvertResult<TKey> GetKey()
        {
            if (_keyConvertor is null)
            {
                return (TKey)OriginalKey;
            }
            switch (_type)
            {
                case InputType.Enumerator:
                case InputType.EnumeratorT:
                case InputType.DataReader:
                case InputType.Single:
                    return _index is TKey i ? i : _keyConvertor.Convert(_context.NewContext(typeof(TKey)), _index);
                default:
                    break;
            }
            var key = OriginalKey;
            return key is TKey t ? t : _keyConvertor.Convert(_context.NewContext(typeof(TKey)), key);
        }

        public ConvertResult<TValue> GetValue()
        {
            if (_valueConvertor is null)
            {
                return (TValue)OriginalValue;
            }
            switch (_type)
            {
                case InputType.DataRecord:
                    var result = DataRecordHelper.Convert<TValue>(_record, _index);
                    if (result.Success)
                    {
                        return result;
                    }
                    return _valueConvertor.Convert(_context.NewContext(typeof(TValue)), OriginalValue);
                case InputType.Single:
                    return _single;
                default:
                    var value = OriginalValue;
                    return value is TValue t ? t : _valueConvertor.Convert(_context.NewContext(typeof(TValue)), OriginalValue);
            }
        }

        //public object OriginalKey => _type switch
        //{
        //    InputType.DataRecord => _record.GetName(_index),
        //    InputType.DataRow => _table.Columns[_index].ColumnName,
        //    InputType.DataSet => _dataSet.Tables[_index].TableName ?? "table" + _index,
        //    InputType.Enumerator => _index,
        //    InputType.EnumeratorT => _index,
        //    InputType.DataTable => _index,
        //    InputType.DataReader => _index,
        //    InputType.NameObjectCollection => BaseGetKey(_nameObjectCollection, _index),
        //    InputType.Dictionary => _keyEnumerator.Current,
        //    InputType.DictionaryT => _keyEnumeratorT.Current,
        //    InputType.Object => _properties[_index].Name,
        //    _ => throw new NotImplementedException(),
        //};

        public object OriginalKey
        {
            get
            {
                switch (_type)
                {
                    case InputType.DataRecord: return _record.GetName(_index);
                    case InputType.DataRow: return _table.Columns[_index].ColumnName;
                    case InputType.DataSet: return _dataSet.Tables[_index].TableName ?? "table" + _index;
                    case InputType.Enumerator: return _index;
                    case InputType.EnumeratorT: return _index;
                    case InputType.DataTable: return _index;
                    case InputType.DataReader: return _index;
                    case InputType.NameObjectCollection: return BaseGetKey(_nameObjectCollection, _index);
                    case InputType.Dictionary: return _keyEnumerator.Current;
                    case InputType.DictionaryT: return _keyEnumeratorT.Current;
                    case InputType.Object: return _properties[_index].Name;
                    case InputType.Single: return 0;
                    default: throw new NotImplementedException();
                }
            }
        }

        public object OriginalValue
        {
            get
            {
                switch (_type)
                {
                    case InputType.DataRecord: return _record.GetValue(_index);
                    case InputType.DataRow: return _row[_index];
                    case InputType.DataSet: return _dataSet.Tables[_index];
                    case InputType.Enumerator: return _enumerator.Current;
                    case InputType.EnumeratorT: return _enumeratorT.Current;
                    case InputType.DataTable: return _table.Rows[_index];
                    case InputType.DataReader: return _record;
                    case InputType.NameObjectCollection: return BaseGet(_nameObjectCollection, _index);
                    case InputType.Dictionary: return _dictionary[_keyEnumerator.Current];
                    case InputType.DictionaryT: return _dictionaryT[_keyEnumeratorT.Current];
                    case InputType.Object: return _properties[_index].Get(_object);
                    case InputType.Single: return _single;
                    default: throw new NotImplementedException();
                }
            }
        }

        //public object OriginalValue => _type switch
        //{
        //    InputType.DataRecord => _record.GetValue(_index),
        //    InputType.DataRow => _row[_index],
        //    InputType.DataSet => _dataSet.Tables[_index],
        //    InputType.Enumerator => _enumerator.Current,
        //    InputType.EnumeratorT => _enumeratorT.Current,
        //    InputType.DataReader => _record,
        //    InputType.NameObjectCollection => BaseGet(_nameObjectCollection, _index),
        //    InputType.Dictionary => _dictionary[_keyEnumerator.Current],
        //    InputType.DictionaryT => _dictionaryT[_keyEnumeratorT.Current],
        //    InputType.Object => _properties[_index].Get(_object),
        //    _ => throw new NotImplementedException(),
        //};

        public bool IsEmpty { get; }
        public bool HasStringKey { get; }
    }
}
