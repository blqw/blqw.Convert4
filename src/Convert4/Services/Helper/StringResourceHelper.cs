using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace blqw
{
    /// <summary>
    /// 字符资源帮助类
    /// </summary>
    internal static class SR
    {
        /// <summary>
        /// 语言翻译映射
        /// </summary>
        /// <returns></returns>
        private static readonly Dictionary<CultureInfo, ReadOnlyDictionary<string, string>> _languageMapper = new Dictionary<CultureInfo, ReadOnlyDictionary<string, string>>();

        /// <summary>
        /// 注册资源
        /// </summary>
        /// <param name="culture">语言区域</param>
        /// <param name="map">字符串映射</param>
        public static void Register(CultureInfo culture, IDictionary<string, string> map)
        {
            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (_languageMapper.TryGetValue(culture, out var oldMap))
            {
                var newMap = new Dictionary<string, string>(oldMap);
                foreach (var item in map)
                {
                    newMap[item.Key] = item.Value;
                }
                map = newMap;
            }

            _languageMapper[culture] = map as ReadOnlyDictionary<string, string> ?? new ReadOnlyDictionary<string, string>(map);
        }

        /// <summary>
        /// 获取当前语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string GetString(FormattableString str) =>
            GetString(CultureInfo.CurrentCulture, str);

        /// <summary>
        /// 获取当前语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string GetString(CultureInfo culture, FormattableString str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            if (str.ArgumentCount == 0)
            {
                if (string.IsNullOrWhiteSpace(str.Format))
                {
                    return str.Format;
                }
                return GetStringImpl(culture, str.Format);
            }
            return string.Format(new TranslationFormatter(culture), str.Format, str.GetArguments());
        }

        /// <summary>
        /// 翻译文本组件
        /// </summary>
        class TranslationFormatter : IFormatProvider, ICustomFormatter
        {
            /// <summary>
            /// 区域信息
            /// </summary>
            private CultureInfo _culture;

            /// <summary>
            /// 初始化翻译文本
            /// </summary>
            /// <param name="culture">区域信息</param>
            public TranslationFormatter(CultureInfo culture) => this._culture = culture;
            /// <summary>
            /// 翻译文本
            /// </summary>
            /// <param name="language">翻译语言</param>
            /// <param name="text">待翻译的文本</param>
            /// <param name="formatProvider"></param>
            /// <returns></returns>
            public string Format(string language, object text, IFormatProvider formatProvider)
            {
                if (language == "!")
                {
                    return text?.ToString();
                }
                var culture = string.IsNullOrWhiteSpace(language) ? _culture : CultureInfo.GetCultureInfo(language);
                return GetStringImpl(culture, text?.ToString());
            }
            /// <summary>
            /// 标准实现
            /// </summary>
            object IFormatProvider.GetFormat(Type formatType) =>
                formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <summary>
        /// 获取特定语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="culture">语言区域</param>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        private static string GetStringImpl(CultureInfo culture, string str) =>
            str != null && str != " " && (_languageMapper.TryGetValue(culture, out var map) && map.TryGetValue(str, out var str1)) ? str1 : str;

        /// <summary>
        /// 获取当前语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string Concat(params string[] strs) =>
            Concat(CultureInfo.CurrentCulture, strs);

        /// <summary>
        /// 获取特定语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="culture">语言区域</param>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string Concat(CultureInfo culture, params string[] strs) =>
            strs == null ? null : strs.Length == 0 ? "" : string.Concat(strs.Select(x => GetStringImpl(culture, x)));


    }
}