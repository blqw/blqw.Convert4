using blqw.ConvertServices;
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
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为静态类型</exception>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为泛型定义类型</exception>
        /// <exception cref="EntryPointNotFoundException">找不到<typeparamref name="T"/>类型的转换器</exception>
        public static ConvertResult<T> ChangeType<T>(this ConvertContext context, object input)
        {
            var outputType = typeof(T);
            if (outputType.IsGenericTypeDefinition)
            {
                return new NotSupportedException(context.Localize($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new NotSupportedException(context.Localize($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = context.GetConvertor<T>();
            if (conv == null)
            {
                return new EntryPointNotFoundException(context.Localize($"未找到适合的转换器"));
            }
            return conv.ChangeType(context, input);
        }


        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="outputType">转换后的类型</param>
        /// <param name="input">待转换的实例</param>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为静态类型</exception>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为泛型定义类型</exception>
        /// <exception cref="EntryPointNotFoundException">找不到<typeparamref name="T"/>类型的转换器</exception>
        /// <returns></returns>
        public static ConvertResult ChangeType(this ConvertContext context, Type outputType, object input)
        {
            if (outputType.IsGenericTypeDefinition)
            {
                return new NotSupportedException(context.Localize($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new NotSupportedException(context.Localize($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = context.GetConvertor(outputType);
            if (conv == null)
            {
                return new EntryPointNotFoundException(context.Localize($"未找到适合的转换器"));
            }
            return conv.ChangeType(context, input);
        }


        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="value">待转换的值</param>
        /// <param name="outputTypeName">转换后的类型</param>
        /// <returns></returns>
        internal static void InvalidCastException(this ConvertContext context, object value, string outputTypeName) =>
            context.Error.AddException(GetInvalidCastException(context, value, outputTypeName));

        internal static Exception GetInvalidCastException(this ConvertContext context, object value, string outputTypeName)
        {
            var text = (value as IConvertible)?.ToString(null)
                       ?? (value as IFormattable)?.ToString(null, null);

            if (value == null)
            {
                text = context.Localize($"`null` {"无法转换为"} {outputTypeName:!}");
            }
            else if (text == null)
            {
                text = context.Localize($"`{value.GetType().GetFriendlyName():!}` {"无法转换为"} {outputTypeName:!}");
            }
            else
            {
                text = context.Localize($"{"值"}:`{text:!}`({value.GetType().GetFriendlyName():!}) {"无法转换为"}; {outputTypeName:!}");
            }
            return new InvalidCastException(text);
        }


        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="str">格式化消息文本</param>
        /// <returns></returns>
        public static void InvalidCastException(this ConvertContext context, FormattableString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            context.Error.AddException(new InvalidCastException(context.Localize(str)));
        }

        public static void InvalidOperationException(this ConvertContext context, FormattableString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            context.Error.AddException(new InvalidOperationException(context.Localize(str)));
        }

        /// <summary>
        /// 值超过限制
        /// </summary>
        public static void OverflowException(this ConvertContext context, string value) =>
          context.Error.AddException(new OverflowException(context.Localize($"{"值超过限制"}:{value:!}")));

        /// <summary>
        /// 本地化字符串
        /// </summary>
        /// <param name="str">格式化消息文本</param>
        /// <returns></returns>
        public static string Localize(this ConvertContext context, FormattableString str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return SR.GetString(context.GetCultureInfo(), str);
        }


    }
}
