using blqw.Convertors;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public static ConvertSettings AddFormat(this ConvertSettings settings, Type formattableType, string format)
        {
            if (!typeof(IFormattable).IsAssignableFrom(formattableType))
            {
                return settings;
            }
            return settings?.AddNamedForType(formattableType, FORMAT, format);
        }

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        /// <returns></returns>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, string separator) =>
            settings?.AddNamedService(STRING_SEPARATOR, new[] { separator });

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, char separator) =>
            settings?.AddNamedService(STRING_SEPARATOR, new[] { separator });

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, params char[] separator) =>
            settings?.AddNamedService(STRING_SEPARATOR, separator);

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, params string[] separator) =>
            settings?.AddNamedService(STRING_SEPARATOR, separator);

        /// <summary>
        /// 设置字符串分割选项
        /// </summary>
        /// <param name="separator">字符串分隔选项</param>
        public static ConvertSettings AddStringSplitOptions(this ConvertSettings settings, StringSplitOptions options) =>
            settings?.AddService(options);

        public static ConvertSettings AddSerializer(this ConvertSettings settings, string contract, Func<object, string> toString, Func<string, Type, object> toObject)
        {
            settings.AddService<ISerializationService>(new Serializer(contract, toString, toObject));
            return settings;
        }

        public static ConvertSettings AddSerializationContract(this ConvertSettings settings, string contract)
        {
            settings.AddNamedService(SERIALIZATION_CONTRACT, contract);
            return settings;
        }

    }
}
