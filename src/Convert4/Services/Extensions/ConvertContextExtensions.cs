﻿using blqw.ConvertServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public static class ConvertContextExtensions
    {

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">转换后的类型</typeparam>
        /// <param name="input">待转换的实例</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"><typeparamref name="T"/>为静态类型</exception>
        /// <exception cref="ArgumentOutOfRangeException"><typeparamref name="T"/>为泛型定义类型</exception>
        /// <exception cref="EntryPointNotFoundException">找不到<typeparamref name="T"/>类型的转换器</exception>
        public static ConvertResult<T> ChangeType<T>(this ConvertContext context, object input)
        {
            var outputType = typeof(T);
            if (outputType.IsGenericTypeDefinition)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = context.GetConvertor<T>();
            if (conv == null)
            {
                return new EntryPointNotFoundException(SR.GetString($"未找到适合的转换器"));
            }
            return conv.ChangeType(context, input);
        }


        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="outputType">转换后的类型</param>
        /// <param name="input">待转换的实例</param>
        /// <returns></returns>
        public static ConvertResult ChangeType(this ConvertContext context, Type outputType, object input)
        {
            if (outputType.IsGenericTypeDefinition)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = context.GetConvertor(outputType);
            if (conv == null)
            {
                return new EntryPointNotFoundException(SR.GetString($"未找到适合的转换器"));
            }
            return conv.ChangeType(context, input);
        }


        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="value">待转换的值</param>
        /// <param name="outputTypeName">转换后的类型</param>
        /// <returns></returns>
        public static Exception InvalidCastException(this ConvertContext context, object value, string outputTypeName)
        {
            var text = (value is DBNull ? "`DBNull`" : null)
                       ?? (value as IConvertible)?.ToString(null)
                       ?? (value as IFormattable)?.ToString(null, null)
                       ?? (value == null ? "`null`" : null);

            if (text == null)
            {
                return context.Exception = context.InvalidCastException($"`{value.GetType().GetFriendlyName():!}` {"无法转换为"} {outputTypeName:!}");
            }
            return context.Exception = context.InvalidCastException($"{"值"}:`{value.ToString():!}`({value.GetType().GetFriendlyName():!}) {"无法转换为"}; {outputTypeName:!}");
        }

        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="str">格式化消息文本</param>
        /// <returns></returns>
        public static Exception InvalidCastException(this ConvertContext context, FormattableString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            return context.Exception = new InvalidCastException(SR.GetString(context.GetCultureInfo(), str));
        }

    }
}
