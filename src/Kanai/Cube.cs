using blqw.Kanai.Dynamic;
using System;
using System.Collections.Concurrent;


namespace blqw.Kanai
{
    /// <summary>
    /// 超级转换器
    /// </summary>
    public static partial class Cube
    {
        private readonly static ConcurrentDictionary<Type, Func<object, ConvertContext, ConvertResult<object>>> _cache =
            new ConcurrentDictionary<Type, Func<object, ConvertContext, ConvertResult<object>>>();


        private static Func<object, ConvertContext, ConvertResult<object>> BuildCastDelegate(Type outputType)
        {
            if (outputType is null)
            {
                throw new ArgumentNullException(nameof(outputType));
            }

            if (!outputType.IsConstructedGenericType)
            {
                return (object input, ConvertContext context) =>
                {
                    if (context.OutputType.IsAbstract && context.OutputType.IsSealed)
                    {
                        var text = string.Format(context.ResourceStrings.CANT_BUILD_CONVERTOR_BECAUSE_STATIC_TYPE, context.OutputType.GetFriendlyName());
                        return new NotSupportedException(text);
                    }
                    var text2 = string.Format(context.ResourceStrings.CANT_BUILD_CONVERTOR_BECAUSE_GENERIC_DEFINITION_TYPE, context.OutputType.GetFriendlyName());
                    return new NotSupportedException(text2);
                };
            }

            var method = ((Func<object, ConvertContext, ConvertResult<object>>)CastToObject<object>);
            var def = method.Method.GetGenericMethodDefinition();
            var make = def.MakeGenericMethod(outputType);
            var function = make.CreateDelegate(typeof(Func<object, ConvertContext, ConvertResult<object>>));

            return (Func<object, ConvertContext, ConvertResult<object>>)function;

            ConvertResult<object> CastToObject<T>(object input, ConvertContext context)
            {
                var result = context.Convert<T>(input);
                return new ConvertResult<object>(result.Success, result.OutputValue, result.Exception);
            }
        }

        public static ConvertResult<object> Convert(this object input, Type outputType, ConvertContext context)
            => _cache.GetOrAdd(outputType, BuildCastDelegate).Invoke(input, context);

        public static ConvertResult<object> Convert(this object input, Type outputType, ConvertSettings settings)
        {
            var context = new ConvertContext(outputType, settings);
            var cast = _cache.GetOrAdd(outputType, BuildCastDelegate);
            return cast(input, context);
        }

        public static ConvertResult<object> Convert(this object input, Type outputType)
            => Convert(input, outputType, null);

        public static ConvertResult<T> Convert<T>(this object input, ConvertContext context)
            => context.Convert<T>(input);

        public static ConvertResult<T> Convert<T>(this object input, ConvertSettings settings)
        {
            var context = new ConvertContext(typeof(T), settings);
            return context.Convert<T>(input);
        }

        public static ConvertResult<T> Convert<T>(this object input) => Convert<T>(input, null);

        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input, T defaultValue)
        {
            var result = input.Convert<T>(null);
            return result.Success ? result.OutputValue : defaultValue;
        }

        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input)
        {
            var result = input.Convert<T>(null);
            result.ThrowIfExceptional();
            return result.OutputValue;
        }


        /// <summary>
        /// 尝试对指定对象进行类型转换,返回是否转换成功
        /// </summary>
        /// <param name="input">需要转换类型的对象</param>
        /// <param name="outputType">转换后的类型</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryChangeType(object input, Type outputType, out object result)
        {
            var r = input.Convert(outputType, null);
            result = r.OutputValue;
            return r.Success;
        }

        /// <summary>
        /// 尝试对指定对象进行类型转换,返回是否转换成功
        /// </summary>
        /// <param name="input">需要转换类型的对象</param>
        /// <param name="outputType">转换后的类型</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryChangeType<T>(object input, out T result)
        {
            var r = input.Convert<T>(null);
            result = r.OutputValue;
            return r.Success;
        }


        public static object ChangeType(this object input, Type outputType)
        {
            var result = input.Convert(outputType, null);
            result.ThrowIfExceptional();
            return result.OutputValue;
        }

        public static object ChangeType(this object input, Type outputType, object defaultValue)
        {
            var result = input.Convert(outputType, null);
            return result.Success ? result.OutputValue : defaultValue;
        }


        /// <summary>
        /// 转为动态类型
        /// </summary>
        public static dynamic ToDynamic(this object obj) => DynamicFactory.Create(obj, null);
        public static dynamic ToDynamic(this object obj, ConvertSettings settings) => DynamicFactory.Create(obj, settings);
    }
}
