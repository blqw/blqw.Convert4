using blqw.Kanai.Extensions;
using blqw.Kanai.Interface;
using blqw.Kanai.Interface.From;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace blqw.Kanai.Convertors
{
    sealed class ListConvertorFactory : IConvertorFactory
    {
        public ListConvertorFactory(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public IServiceProvider ServiceProvider { get; }

        public bool CanBuild<T>() => typeof(T).GetGenericArguments(typeof(IList<>)) != null || typeof(T) == typeof(IList) || typeof(T) == typeof(ArrayList) || typeof(T).IsArray;

        public IConvertor<T> Build<T>()
        {
            if (typeof(T).IsArray)
            {
                return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(ArrayConvertor<>).MakeGenericType(typeof(T).GetElementType()));
            }
            var genericArgs = typeof(T).GetGenericArguments(typeof(IList<>));

            if (genericArgs != null)
            {
                return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(ListConvertor<,>).MakeGenericType(typeof(T), genericArgs[0]));
            }

            // 如果无法得道泛型参数, 判断output是否与 List<object> 兼容, 如果是返回 List<object> 的转换器
            if (typeof(T) == typeof(ArrayList) || typeof(T) == typeof(IList))
            {
                return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(ArrayListConvertor<>).MakeGenericType(typeof(T)));
            }
            return null;
        }


        class ArrayListConvertor<T> : BaseConvertor<T>,
                                      IFrom<string, T>,
                                      IFrom<object, T>
                            where T : IList
        {
            public ArrayListConvertor(IServiceProvider serviceProvider)
                : base(serviceProvider)
            {
            }

            public ConvertResult<T> From(ConvertContext context, string input)
            {
                if (input is null)
                {
                    return Ok(default);
                }

                if (string.IsNullOrEmpty(input))
                {
                    return (T)(object)new ArrayList();
                }

                var arr = input.Split(context.StringSeparators.CharArray, context.StringSplitOptions);
                return (T)(object)new ArrayList(arr);
            }

            public ConvertResult<T> From(ConvertContext context, object input)
            {
                if (input is null)
                {
                    return Ok(default);
                }
                var enumerator = new KeyValueEnumerator<object, object>(context, input);

                var list = new ArrayList();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.OriginalValue);
                }
                return (T)(object)list;

            }
        }

        class ListConvertor<TList, TValue> : BaseConvertor<TList>,
                                             IFrom<string, TList>,
                                             IFrom<object, TList>
            where TList : IList<TValue>
        {
            public ListConvertor(IServiceProvider serviceProvider)
                : base(serviceProvider)
            {
            }

            public ConvertResult<TList> From(ConvertContext context, string input)
            {
                if (input is null)
                {
                    return typeof(TList).IsValueType ? this.Fail(context, input) : Ok(default);
                }

                if (string.IsNullOrEmpty(input))
                {
                    return (TList)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(TList)); ;
                }

                var arr = input.Split(context.StringSeparators.CharArray, context.StringSplitOptions);
                if (arr is TList result)
                {
                    return result;
                }
                return From(context, arr.GetEnumerator());
            }

            public ConvertResult<TList> From(ConvertContext context, object input)
            {
                if (input is null)
                {
                    return typeof(TList).IsValueType ? this.Fail(context, input) : Ok(default);
                }

                var enumerator = new KeyValueEnumerator<object, TValue>(context, input);

                if (enumerator.IsEmpty)
                {
                    return this.Fail(context, input);
                }

                var list = (TList)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(TList).IsInterface ? typeof(List<TValue>) : typeof(TList));
                while (enumerator.MoveNext())
                {
                    var result = enumerator.GetValue();
                    if (!result.Success)
                    {
                        var message = string.Format(context.ResourceStrings.COLLECTION_ADD_FAIL, typeof(TList).GetFriendlyName(), list.Count, enumerator.OriginalValue);
                        return context.Fail(message, result.Exception);
                    }
                    list.Add(result.OutputValue);
                }
                return list;
            }
        }



        class ArrayConvertor<T> : BaseConvertor<T[]>,
                                  IFrom<string, T[]>,
                                  IFrom<object, T[]>
        {
            public ArrayConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }

            public ConvertResult<T[]> From(ConvertContext context, string input)
            {
                if (input is null)
                {
                    return Ok(null);
                }
                if (string.IsNullOrEmpty(input))
                {
                    return Array.Empty<T>();
                }

                //var serializer = context.StringSerializer;
                //if (serializer != null)
                //{
                //    try
                //    {
                //        return (T[])serializer.ToObject(input, typeof(T[]));
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                var arr = input.Split(context.StringSeparators.CharArray, context.StringSplitOptions);
                if (arr is T[] result)
                {
                    return result;
                }
                return From(context, arr.GetEnumerator());
            }

            public ConvertResult<T[]> From(ConvertContext context, IEnumerator input)
            {
                if (input is null)
                {
                    return Ok(null);
                }
                var result = context.Convert<List<T>>(input);
                if (!result.Success)
                {
                    return this.Fail(context, input, result.Exception);
                }
                return result.OutputValue.ToArray();
            }

            public ConvertResult<T[]> From(ConvertContext context, object input)
            {
                if (input is null)
                {
                    return Ok(default);
                }

                var enumerator = new KeyValueEnumerator<object, T>(context, input);

                if (enumerator.IsEmpty)
                {
                    return this.Fail(context, input);
                }

                var list = new List<T>();
                while (enumerator.MoveNext())
                {
                    var result = enumerator.GetValue();
                    if (!result.Success)
                    {
                        var message = string.Format(context.ResourceStrings.COLLECTION_ADD_FAIL, $"List<{typeof(T).GetFriendlyName()}>", list.Count, enumerator.OriginalValue);
                        return context.Fail(message, result.Exception);
                    }
                    list.Add(result.OutputValue);
                }
                return list.ToArray();
            }
        }

    }

}
