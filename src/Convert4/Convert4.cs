using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using blqw.Dynamic;

namespace blqw
{
    /// <summary>
    /// 超级转换器v4
    /// </summary>
    public static class Convert4
    {
        public static ConvertResult<T> Convert<T>(this object input, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                return context.ChangeType<T>(input);
            }
        }

        public static ConvertResult Convert(this object input, Type outputType, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                return context.ChangeType(outputType, input);
            }
        }

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

        /// <summary>
        /// 转为动态类型
        /// </summary>
        public static dynamic ToDynamic(this object obj) => DynamicFactory.Create(obj);


        /// <summary>
        /// 获取一个 <see cref="IFormatterConverter"> 类型的简单转换器
        /// </summary>
        /// <returns></returns>
        public static IFormatterConverter GetFormatterConverter() =>
            throw new NotImplementedException(); //TODO:未实现
    }
}
