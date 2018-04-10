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

            public TDictionary From(ConvertContext context, object input)
            {
                if (input is null || input is DBNull)
                {
                    return null;
                }

                var builder = new DictionaryBuilder(OutputType, context);
                if (builder.TryCreateInstance() == false)
                {
                    return null;
                }

                var mapper = new Mapper(input);

                if (mapper.Error != null)
                {
                    context.InvalidCastException(mapper.Error);
                    return null;
                }

                while (mapper.MoveNext())
                {
                    if (builder.Add(mapper.Key, mapper.Value) == false)
                    {
                        return null;
                    }
                }
                return (TDictionary)builder.Instance;
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
                    Instance = null;
                }

                /// <summary>
                /// 被构造的实例
                /// </summary>
                public IDictionary<TKey, TValue> Instance { get; private set; }

                public bool Add(object key, object value)
                {
                    var rkey = _keyConvertor.ChangeType(_context, key);
                    if (rkey.Success == false)
                    {
                        _context.Exception = new InvalidOperationException($"添加到字典{_type.GetFriendlyName()}失败") + rkey.Error;
                        return false;
                    }

                    var rval = _valueConvertor.ChangeType(_context, value);
                    if (rval.Success == false)
                    {
                        _context.Exception = new InvalidOperationException($"向字典{_type.GetFriendlyName()}中添加元素 {key} 失败") + rval.Error;
                        return false;
                    }
                    try
                    {
                        Instance.Add(rkey.OutputValue, rval.OutputValue);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _context.Exception = new InvalidOperationException($"向字典{_type.GetFriendlyName()}中添加元素 {key} 失败,原因:{ex.Message}", ex);
                        return false;
                    }
                }

                /// <summary>
                /// 尝试构造实例,返回是否成功
                /// </summary>
                /// <returns> </returns>
                public bool TryCreateInstance()
                {
                    if (_type.IsInterface)
                    {
                        Instance = new Dictionary<TKey, TValue>();
                        return true;
                    }
                    try
                    {
                        Instance = (IDictionary<TKey, TValue>)Activator.CreateInstance(_type);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _context.Exception = ex;
                        return false;
                    }
                }

                /// <summary>
                /// 设置对象值
                /// </summary>
                /// <param name="obj"> 待设置的值 </param>
                /// <returns> </returns>
                public bool Set(DictionaryEntry obj) => Add(obj.Key, obj.Value);
            }
        }
    }
}
