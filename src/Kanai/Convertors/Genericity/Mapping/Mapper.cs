﻿using blqw.Kanai;
using blqw.Kanai.Convertors.Mappings;
using blqw.Kanai.Extensions;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace blqw
{
    /// <summary>
    /// 用于枚举对象或集合的键值
    /// </summary>
    public struct Mapper : IDictionaryEnumerator
    {
        private readonly DataReaderEnumerator _reader;
        private readonly NameValueEnumerator _nv;
        private readonly DataRowEnumerator _row;
        private readonly DataSetEnumerator _dataSet;
        private readonly IDictionaryEnumerator _enumerator;
        private readonly PairEnumerator _pair;
        private readonly PropertyEnumerator _property;
        private readonly int _index;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="input"></param>
        public Mapper(object input)
            : this()
        {
            if (input is IDataReader reader)
            {
                _reader = new DataReaderEnumerator(reader);
                Error = _reader.Error;
                _index = 1;
                return;
            }

            if (input is NameObjectCollectionBase no)
            {
                _nv = new NameValueEnumerator(no);
                Error = _nv.Error;
                _index = 2;
                return;
            }

            var row = (input as DataRowView)?.Row ?? input as DataRow;
            if (row?.Table != null)
            {
                _row = new DataRowEnumerator(row);
                Error = _row.Error;
                _index = 3;
                return;
            }

            if (input is DataSet dataset)
            {
                _dataSet = new DataSetEnumerator(dataset);
                Error = _dataSet.Error;
                _index = 4;
                return;
            }

            if (input is IDictionary dict)
            {
                _enumerator = dict.GetEnumerator();
                _index = 5;
                return;
            }

            var ee = (input as IEnumerable)?.GetEnumerator() ?? input as IEnumerator;
            if (ee != null)
            {
                _pair = new PairEnumerator(ee);
                Error = _pair.Error;
                _index = 6;
                return;
            }

            var ps = PropertyHelper.GetByType(input.GetType());
            if (ps.Length > 0)
            {
                _property = new PropertyEnumerator(input, ps);
                Error = _property.Error;
                _index = 7;
            }
        }



        /// <summary>
        /// 异常文本
        /// </summary>
        public FormattableString Error { get; }

        /// <summary>
        /// 键
        /// </summary>
        public object Key
        {
            get
            {
                switch (_index)
                {
                    case 1:
                        return _reader.GetKey();
                    case 2:
                        return _nv.GetKey();
                    case 3:
                        return _row.GetKey();
                    case 4:
                        return _dataSet.GetKey();
                    case 5:
                        return _enumerator.Key;
                    case 6:
                        return _pair.GetKey();
                    case 7:
                        return _property.GetKey();
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                switch (_index)
                {
                    case 1:
                        return _reader.GetValue();
                    case 2:
                        return _nv.GetValue();
                    case 3:
                        return _row.GetValue();
                    case 4:
                        return _dataSet.GetValue();
                    case 5:
                        return _enumerator.Value;
                    case 6:
                        return _pair.GetValue();
                    case 7:
                        return _property.GetValue();
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public DictionaryEntry Entry =>
            _enumerator?.Entry ?? new DictionaryEntry(Key, Value);

        object IEnumerator.Current => Entry;

        public bool MoveNext()
        {
            switch (_index)
            {
                case 1:
                    return _reader.MoveNext();
                case 2:
                    return _nv.MoveNext();
                case 3:
                    return _row.MoveNext();
                case 4:
                    return _dataSet.MoveNext();
                case 5:
                    return _enumerator.MoveNext();
                case 6:
                    return _pair.MoveNext();
                case 7:
                    return _property.MoveNext();
                default:
                    throw new NotSupportedException();
            }
        }

        public void Reset()
        {
            switch (_index)
            {
                case 1:
                    _reader.Reset();
                    break;
                case 2:
                    _nv.Reset();
                    break;
                case 3:
                    _row.Reset();
                    break;
                case 4:
                    _dataSet.Reset();
                    break;
                case 5:
                    _enumerator.Reset();
                    break;
                case 6:
                    _pair.Reset();
                    break;
                case 7:
                    _property.Reset();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        internal static Exception Build(object input, Func<ConvertContext, object, object, Exception> add)
        {
            var mapper = new Mapper(input);

            if (mapper.Error != null)
            {
                return new ConvertException(mapper.Error.ToString(), null);
            }

            while (mapper.MoveNext())
            {
                if (add(context, mapper.Key, mapper.Value) is ConvertException ex)
                {
                    var message = string.Format(context.ResourceStrings.PROPERTY_SET_FAIL, context.OutputType.GetFriendlyName(), mapper.Key, mapper.Value);
                    return context.Fail(message, ex);
                }
            }

            return null;
        }

    }
}
