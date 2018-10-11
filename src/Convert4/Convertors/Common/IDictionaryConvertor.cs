using blqw.ConvertServices;
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

        public ConvertResult<IDictionary> From(ConvertContext context, object input)
        {
            if (input is null || input is DBNull)
            {
                return default;
            }

            var builder = new DictionaryBuilder(OutputType, context);
            var ex = builder.Exception + Mapper.Build(context, OutputType, input, builder.InstanceCreated, builder.Add);
            return ex ?? Result(builder.Instance);
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
                try
                {
                    Instance = (IDictionary)_context.CreateInstance<Hashtable>(_type);
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
            public IDictionary Instance { get; private set; }

            /// <summary>
            /// 返回是否已经实例化
            /// </summary>
            /// <returns> </returns>
            public bool InstanceCreated => Exception == null;

            public ConvertException Add(object key, object value)
            {
                try
                {
                    Instance.Add(key, value);
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
