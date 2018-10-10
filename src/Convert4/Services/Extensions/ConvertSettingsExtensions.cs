using blqw.Convertors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using static blqw.ConvertServices.NamedServiceNames;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 常用的公开的拓展方法
    /// </summary>
    public static class ConvertSettingsExtensions
    {
        /// <summary>
        /// 设置指定类型的格式化字符串
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="format">格式化字符串</param>
        /// <returns></returns>
        public static ConvertSettings SetFormat(this ConvertSettings settings, Type forType, string format) =>
            settings?.Set(FORMAT, format, forType);

        public static string GetFormat(this ConvertContext context, Type forType) =>
            context?.GetSetting<string>(forType, FORMAT);

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings SetStringSeparator(this ConvertSettings settings, params char[] separator) =>
            settings?.Set(STRING_SEPARATOR, separator);

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings SetStringSeparator(this ConvertSettings settings, params string[] separator) =>
            settings?.Set(STRING_SEPARATOR, separator);

        /// <summary>
        /// 获取字符串分隔符, 如果有多个返回一个 <see cref="string"/> 数组 或 <see cref="char"/> 数组
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static object GetStringSeparators(this ConvertContext context)
        {
            var separator = context.GetSetting<object>(STRING_SEPARATOR);
            switch (separator)
            {
                case char[] r:
                    return r;
                case char r:
                    return new char[] { r };
                case string r:
                    return new string[] { r };
                case string[] r:
                    return r;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取字符串分隔符, 如果有多个返回第一个, 并转为string
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static string GetStringSeparator(this ConvertContext context)
        {
            var separator = context.GetSetting<object>(STRING_SEPARATOR);
            switch (separator)
            {
                case char[] r:
                    return r.Length > 0 ? r.ToString() : null;
                case char r:
                    return r.ToString();
                case string r:
                    return r;
                case string[] r:
                    return r.Length > 0 ? r[0] : null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 设置字符串分割选项
        /// </summary>
        /// <param name="separator">字符串分隔选项</param>
        public static ConvertSettings SetStringSplitOptions(this ConvertSettings settings, StringSplitOptions options) =>
            settings?.Set<StringSplitOptions>(options);

        /// <summary>
        /// 获取字符串分割选项 <seealso cref="StringSplitOptions"/>, 默认返回 <seealso cref="StringSplitOptions.None"/>
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static StringSplitOptions GetStringSplitOptions(this ConvertContext context) =>
            context?.GetSettingOrService<StringSplitOptions>() ?? StringSplitOptions.None;

        public static ConvertSettings SetSerializer(this ConvertSettings settings, Func<object, string> toString, Func<string, Type, object> toObject) =>
            settings.Set<ISerializationService>(new Serializer(null, toString, toObject));

        public static ConvertSettings SetSerializer(this ConvertSettings settings, ISerializationService serializer) =>
            settings?.Set<ISerializationService>(serializer);

        public static ConvertSettings SetSerializer(this ConvertSettings settings, string protocol) =>
            settings?.Set(SERIALIZATION_PROTOCOL, protocol);

        /// <summary>
        /// 获取序列化服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static ISerializationService GetSerializer(this ConvertContext context)
        {
            if (context == null)
            {
                return null;
            }
            var serializer = context.GetSetting<ISerializationService>();
            if (serializer != null)
            {
                return serializer;
            }
            var protocol = context.GetSetting<string>(SERIALIZATION_PROTOCOL);
            if (protocol != null)
            {
                return context.GetServices<ISerializationService>().FirstOrDefault(x => x.Protocol == protocol);
            }
            return context.GetService<ISerializationService>();
        }


        public static ConvertSettings SetCultureInfo(this ConvertSettings settings, CultureInfo cultureInfo) =>
             settings.Set<IFormatProvider>(cultureInfo);

        public static ConvertSettings SetCultureInfo(this ConvertSettings settings, Type forType, CultureInfo cultureInfo) =>
             settings.Set<IFormatProvider>(cultureInfo, forType);

        /// <summary>
        /// 获取 区域对象 <seealso cref="CultureInfo"/>, 默认返回当前区域 <seealso cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static CultureInfo GetCultureInfo(this ConvertContext context) =>
            context?.GetSettingOrService<CultureInfo>() ?? CultureInfo.CurrentCulture;


        public static ConvertSettings SetEncoding(this ConvertSettings settings, Encoding encoding) =>
            settings.Set<Encoding>(encoding);

        /// <summary>
        /// 获取编码对象 <seealso cref="Encoding"/>, 默认返回 <seealso cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(this ConvertContext context) =>
            context?.GetSettingOrService<Encoding>() ?? Encoding.UTF8;

        public static ConvertSettings SetFormatProvider(this ConvertSettings settings, Type forType, IFormatProvider formatProvider) =>
            settings.Set<IFormatProvider>(formatProvider, forType);

        public static IFormatProvider GetFormatProvider(this ConvertContext context, Type forType) =>
            context?.GetSettingOrService<IFormatProvider>(forType);

    }
}
