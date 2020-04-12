using blqw.Kanai.Extensions;
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

            if (genericArgs != null)
            {
                return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(DictionaryConvertor<,,>).MakeGenericType(typeof(T), genericArgs[0], genericArgs[1]));
            }

            if (typeof(T) == typeof(IDictionary) || typeof(T) == typeof(Hashtable))
            {
                return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(HashtableConvertor<>).MakeGenericType(typeof(T)));
            }

            return null;

        }
        public bool CanBuild<T>() => typeof(T).GetGenericArguments(typeof(IDictionary<,>)) != null || typeof(T) == typeof(IDictionary) || typeof(T) == typeof(Hashtable);

        private class HashtableConvertor<T> : BaseConvertor<T>,
                                              IFrom<object, T>
                                    where T : IDictionary
        {
            public HashtableConvertor(IServiceProvider serviceProvider)
                           : base(serviceProvider)
            {
            }

            public ConvertResult<T> From(ConvertContext context, object input)
            {
                var enumerator = new KeyValueEnumerator<object, object>(context, input);
                var hashtable = new Hashtable();
                while (enumerator.MoveNext())
                {
                    hashtable.Add(enumerator.OriginalKey, enumerator.OriginalValue);
                }
                return (T)(object)hashtable;
            }
        }

        private class DictionaryConvertor<TDictionary, TKey, TValue> : BaseConvertor<TDictionary>,
                                                                       IFrom<object, TDictionary>
                                                   where TDictionary : class, IDictionary<TKey, TValue>
        {
            public DictionaryConvertor(IServiceProvider serviceProvider)
                           : base(serviceProvider)
            {
            }

            public ConvertResult<TDictionary> From(ConvertContext context, object input)
            {
                var enumerator = new KeyValueEnumerator<TKey, TValue>(context, input);
                var dict = ActivatorUtilities.CreateInstance<TDictionary>(context.ServiceProvider);
                while (enumerator.MoveNext())
                {
                    var key = enumerator.GetKey();
                    if (!key.Success)
                    {
                        var message = string.Format(context.ResourceStrings.COLLECTION_KEY_FAIL, typeof(TDictionary).GetFriendlyName(), enumerator.OriginalKey);
                        return context.Fail(message, key.Exception);
                    }

                    var value = enumerator.GetValue();
                    if (!value.Success)
                    {
                        var message = string.Format(context.ResourceStrings.COLLECTION_ADD_FAIL, typeof(TDictionary).GetFriendlyName(), key.OutputValue, enumerator.OriginalValue);
                        return context.Fail(message, value.Exception);
                    }

                    dict.Add(key.OutputValue, value.OutputValue);
                }
                return dict;
            }
        }

    }
}
