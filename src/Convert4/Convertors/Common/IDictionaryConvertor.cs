﻿using blqw.ConvertServices;
using blqw.DI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class IDictionaryConvertor : BaseConvertor<IDictionary>,
                               IFrom<object, IDictionary>
    {
        public IDictionaryConvertor()
        {
        }

        public IDictionaryConvertor(Type outputType) : base(outputType)
        {
        }

        public IDictionary From(ConvertContext context, object input)
        {
            if (input is null || input is DBNull)
            {
                return null;
            }

            var builder = new DictionaryBuilder(OutputType, context);
            if (Mapper.Build(context, OutputType, input, builder.TryCreateInstance, builder.Add))
            {
                return builder.Instance;
            }
            return null;
        }

        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType == typeof(IEnumerable) || outputType == typeof(ICollection))
            {
                return null;
            }
            return outputType == OutputType ? this : new IDictionaryConvertor(outputType).Proxy(outputType);
        }


        /// <summary>
        /// <seealso cref="IDictionary" /> 构造器
        /// </summary>
        private struct DictionaryBuilder
        {
            private readonly Type _type;
            private readonly ConvertContext _context;

            public DictionaryBuilder(Type type, ConvertContext context)
            {
                _type = type;
                _context = context;
                Instance = null;
            }

            /// <summary>
            /// 被构造的实例
            /// </summary>
            public IDictionary Instance { get; private set; }

            public bool Add(object key, object value)
            {
                try
                {
                    Instance.Add(key, value);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Error.AddException(ex);
                    return false;
                }
            }

            /// <summary>
            /// 尝试构造实例,返回是否成功
            /// </summary>
            /// <returns> </returns>
            public bool TryCreateInstance()
            {
                try
                {
                    Instance = (IDictionary)_context.CreateInstance<Hashtable>(_type);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Error.AddException(ex);
                    return false;
                }
            }
        }
    }
}
