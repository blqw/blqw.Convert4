﻿using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace blqw
{
    /// <summary>
    /// 超级转换器v4
    /// </summary>
    public static class Convert4
    {
        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input, T defaultValue)
        {
            using (var context = new ConvertContext())
            {
                var result = context.ChangeType<T>(input);
                return result.Success ? result.OutputValue : defaultValue;
            }
        }

        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input)
        {
            using (var context = new ConvertContext())
            {
                var result = context.ChangeType<T>(input);
                result.ThrowIfExceptional();
                return result.OutputValue;
            }
        }

        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                var result = context.ChangeType<T>(input);
                if (!result.Success)
                {
                    if (settings?.Throwable ?? true)
                    {
                        result.ThrowIfExceptional();
                    }
                    return settings?.DefaultValue is T t ? t : default;
                }
                return result.OutputValue;
            }
        }


        public static object ChangeType(this object input, Type type, ConvertSettings settings)
        {
            using (var context = new ConvertContext(settings))
            {
                var result = context.ChangeType(type, input);
                if (!result.Success)
                {
                    if (settings?.Throwable ?? true)
                    {
                        result.ThrowIfExceptional();
                    }
                    return settings?.DefaultValue;
                }
                return result.OutputValue;
            }
        }


        /// <summary>
        /// 获取一个 <see cref="IFormatterConverter"> 类型的简单转换器
        /// </summary>
        /// <returns></returns>
        public static IFormatterConverter GetFormatterConverter() =>
            throw new NotImplementedException(); //TODO:未实现
    }
}
