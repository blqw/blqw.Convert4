using blqw.ConvertServices;
using blqw.DI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace blqw.Convertors
{
    class NameValueCollectionConvertor : BaseConvertor<NameValueCollection>,
                                         IFrom<object, NameValueCollection>
    {
        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType.IsInterface)
            {
                return null;
            }
            return outputType == OutputType ? this : new IListConvertor(outputType).Proxy(outputType);
        }

        public NameValueCollection From(ConvertContext context, object input)
        {
            if (input is null || input is DBNull)
            {
                return null;
            }

            var builder = new NVCollectiontBuilder(context, OutputType);
            if (Mapper.Build(context, OutputType, input, builder.TryCreateInstance, builder.Add))
            {
                return builder.Instance;
            }
            return null;
        }


        /// <summary>
        /// <seealso cref="NameValueCollection" /> 转换器
        /// </summary>
        private struct NVCollectiontBuilder
        {
            private readonly ConvertContext _context;
            private readonly Type _type;
            /// <summary>
            /// 键转换器
            /// </summary>
            private readonly IConvertor<string> _stringConvertor;

            public NVCollectiontBuilder(ConvertContext context, Type type)
            {
                _context = context;
                _type = type;
                _stringConvertor = context.GetConvertor<string>();
                Instance = null;
            }

            /// <summary>
            /// 被构造的实例
            /// </summary>
            public NameValueCollection Instance { get; private set; }

            /// <summary>
            /// 尝试构造实例,返回是否成功
            /// </summary>
            /// <returns> </returns>
            public bool TryCreateInstance()
            {
                try
                {
                    Instance = (NameValueCollection)_context.CreateInstance<NameValueCollection>(_type);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Error.AddException(ex);
                    return false;
                }
            }

            public bool Add(object key, object value)
            {
                var conv = _context.GetConvertor<string>();
                var rkey = conv.ChangeType(_context, key);
                if (rkey.Success == false)
                {
                    _context.InvalidCastException($"Key{"转换失败"}");
                    return false;
                }
                var rval = conv.ChangeType(_context, value);
                if (rval.Success == false)
                {
                    _context.InvalidCastException($"{rkey.OutputValue:!} {"转换失败"}");
                    return false;
                }
                try
                {
                    Instance.Add(rkey.OutputValue, rval.OutputValue);
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
