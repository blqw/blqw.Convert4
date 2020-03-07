using blqw.Kanai.Interface;
using blqw.Kanai.Interface.From;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace blqw.Kanai.Convertors
{
    class DictionaryConvertorFactory : IConvertorFactory
    {
        public IServiceProvider ServiceProvider { get; }

        public DictionaryConvertorFactory(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        public IConvertor<T> Build<T>()
        {
            var genericArgs = typeof(T).GetGenericArguments(typeof(IDictionary<,>));

            if (genericArgs == null)
            {
                return null;
            }

            var args = new Type[genericArgs.Length + 1];
            args[0] = typeof(T);
            genericArgs.CopyTo(args, 1);
            return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(InnerConvertor<,,>).MakeGenericType(args));

        }
        public bool CanBuild<T>() => typeof(T).GetGenericArguments(typeof(IDictionary<,>)) != null;

        private class InnerConvertor<TDictionary, TKey, TValue> : BaseConvertor<TDictionary>,
                                                          IFrom<object, TDictionary>
                                      where TDictionary : class, IDictionary<TKey, TValue>
        {
            public InnerConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
            {

            }

            public ConvertResult<TDictionary> From(ConvertContext context, object input)
            {
                if (input is null || input is DBNull)
                {
                    return default;
                }

                var builder = new DictionaryBuilder(context, ServiceProvider);
                if (builder.Exception != null)
                {
                    return builder.Exception;
                }
                var ex = Mapper.Build(context, input, builder.InstanceCreated, builder.Add);
                return builder.Instance;
            }



            /// <summary>
            /// <seealso cref="IDictionary" /> 构造器
            /// </summary>
            private struct DictionaryBuilder
            {
                /// <summary>
                /// 键转换器
                /// </summary>
                private readonly IConvertor<TKey> _keyConvertor;

                /// <summary>
                /// 值转换器
                /// </summary>
                private readonly IConvertor<TValue> _valueConvertor;

                public DictionaryBuilder(ConvertContext context, IServiceProvider serviceProvider)
                {
                    _keyConvertor = context.GetConvertor<TKey>();
                    _valueConvertor = context.GetConvertor<TValue>();
                    try
                    {
                        Instance = (TDictionary)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, context.OutputType);
                        Exception = null;
                    }
                    catch (Exception ex)
                    {
                        Instance = default;
                        Exception = ex;
                    }
                }

                public Exception Exception { get; }

                /// <summary>
                /// 被构造的实例
                /// </summary>
                public TDictionary Instance { get; }

                public ConvertException Add(ConvertContext context, object key, object value)
                {
                    var rkey = _keyConvertor.ChangeType(context, key);
                    if (rkey.Success == false)
                    {
                        return rkey.Exception + context.InvalidCastException($"Key{"转换失败"}");
                    }

                    var rval = _valueConvertor.ChangeType(context, value);
                    if (rval.Success == false)
                    {
                        return rval.Exception + context.InvalidCastException($"Key={rkey.OutputValue:!} {"值"}{"转换失败"}");
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
