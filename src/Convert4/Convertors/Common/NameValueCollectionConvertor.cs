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

        public ConvertResult<NameValueCollection> From(ConvertContext context, object input)
        {
            if (input is null || input is DBNull)
            {
                return default;
            }

            var builder = new NVCollectiontBuilder(context, OutputType);
            var ex = builder.Exception + Mapper.Build(context, OutputType, input, builder.InstanceCreated, builder.Add);
            return ex ?? Result(builder.Instance);
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

                try
                {
                    Instance = (NameValueCollection)_context.CreateInstance<NameValueCollection>(_type);
                    Exception = null;
                }
                catch (Exception ex)
                {
                    Instance = default;
                    Exception = ex as ConvertException ?? new ConvertException(ex);
                }
            }

            public ConvertException Exception { get; }
            /// <summary>
            /// 被构造的实例
            /// </summary>
            public NameValueCollection Instance { get; private set; }

            /// <summary>
            /// 返回是否已经实例化
            /// </summary>
            /// <returns> </returns>
            public bool InstanceCreated => Exception == null;

            public ConvertException Add(object key, object value)
            {
                var conv = _context.GetConvertor<string>();
                var rkey = conv.ChangeType(_context, key);
                if (rkey.Success == false)
                {
                    return rkey.Exception + _context.InvalidCastException($"Key{"转换失败"}");
                }
                var rval = conv.ChangeType(_context, value);
                if (rval.Success == false)
                {
                    return rval.Exception + _context.InvalidCastException($"Key={rkey.OutputValue:!} {"转换失败"}");
                }
                try
                {
                    Instance.Add(rkey.OutputValue, rval.OutputValue);
                    return null;
                }
                catch (Exception ex)
                {
                    return ex as ConvertException ?? new ConvertException(ex);
                }
            }
        }
    }
}
