using System;
using System.Collections.Concurrent;


namespace blqw.Kanai
{
    /// <summary>
    /// 超级转换器v5
    /// </summary>
    public static class Convert
    {
        public static ConvertResult<T> Cast<T>(this object input, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                return context.ChangeType<T>(input);
            }
        }


        private readonly static ConcurrentDictionary<Type, Func<object, ConvertSettings, ConvertResult<object>>> _cache =
            new ConcurrentDictionary<Type, Func<object, ConvertSettings, ConvertResult<object>>>();

        private static ConvertResult<object> CastToObject<T>(object input, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                var result = context.ChangeType<T>(input);
                return new ConvertResult<object>(result.Success, result.OutputValue, result.Exception);
            }
        }

        public static ConvertResult<object> Cast(this object input, Type outputType, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                var cast = _cache.GetOrAdd(outputType, x => (Func<object, ConvertSettings, ConvertResult<object>>)
                                                            ((Func<object, ConvertSettings, ConvertResult<object>>)CastToObject<object>)
                                                            .Method
                                                            .GetGenericMethodDefinition()
                                                            .MakeGenericMethod(outputType)
                                                            .CreateDelegate(typeof(Func<object, ConvertSettings, ConvertResult<object>>)));
                return cast(input, settings);
            }
        }
    }
}
