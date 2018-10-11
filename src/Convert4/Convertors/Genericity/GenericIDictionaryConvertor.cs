using blqw.ConvertServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class GenericIDictionaryConvertor : BaseConvertor<IDictionary<object, object>>
    {
        public override Type OutputType => typeof(IDictionary<,>);

        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType == typeof(IEnumerable))
            {
                return null;
            }
            var genericArgs = outputType.GetGenericArguments(typeof(IDictionary<,>));

            if (genericArgs == null)
            {
                //如果无法得道泛型参数, 判断output是否与 List<object> 兼容, 如果是返回 List<object> 的转换器
                if (outputType.IsAssignableFrom(typeof(IDictionary<string, object>)))
                {
                    return new InnerConvertor<IDictionary<string, object>, string, object>(this);
                }
                return null;
            }
            var args = new Type[genericArgs.Length + 1];
            args[0] = outputType;
            genericArgs.CopyTo(args, 1);
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<,,>).MakeGenericType(args), this);
        }

        class InnerConvertor<TDictionary, TKey, TValue> : BaseConvertor<TDictionary>,
                                                          IFrom<object, TDictionary>
            where TDictionary : class, IDictionary<TKey, TValue>
        {
            private readonly GenericIDictionaryConvertor _parent;

            public InnerConvertor(GenericIDictionaryConvertor parent) => _parent = parent;

            public override IConvertor GetConvertor(Type outputType) => _parent.GetConvertor(outputType);

            public ConvertResult<TDictionary> From(ConvertContext context, object input)
            {
                if (input is null || input is DBNull)
                {
                    return default;
                }

                var builder = new DictionaryBuilder(OutputType, context);
                var ex = builder.Exception + Mapper.Build(context, OutputType, input, builder.InstanceCreated, builder.Add);
                return ex ?? Result(builder.Instance);
            }



            /// <summary>
            /// <seealso cref="IDictionary" /> 构造器
            /// </summary>
            private struct DictionaryBuilder
            {
                private readonly Type _type;
                private readonly ConvertContext _context;
                /// <summary>
                /// 键转换器
                /// </summary>
                private readonly IConvertor<TKey> _keyConvertor;

                /// <summary>
                /// 值转换器
                /// </summary>
                private readonly IConvertor<TValue> _valueConvertor;

                public DictionaryBuilder(Type type, ConvertContext context)
                {
                    _type = type;
                    _context = context;
                    _keyConvertor = context.GetConvertor<TKey>();
                    _valueConvertor = context.GetConvertor<TValue>();
                    try
                    {
                        Instance = (TDictionary)_context.CreateInstance<Dictionary<TKey, TValue>>(_type);
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
                public TDictionary Instance { get; }

                public ConvertException Add(object key, object value)
                {
                    var rkey = _keyConvertor.ChangeType(_context, key);
                    if (rkey.Success == false)
                    {
                        return rkey.Exception + _context.InvalidCastException($"Key{"转换失败"}");
                    }

                    var rval = _valueConvertor.ChangeType(_context, value);
                    if (rval.Success == false)
                    {
                        return rval.Exception + _context.InvalidCastException($"Key={rkey.OutputValue:!} {"值"}{"转换失败"}");
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

                /// <summary>
                /// 返回是否已经实例化
                /// </summary>
                /// <returns> </returns>
                public bool InstanceCreated => Instance != null;
            }
        }
    }
}
