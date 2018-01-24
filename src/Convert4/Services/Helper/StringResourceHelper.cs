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
        public static string GetString(string str) =>
            GetString(CultureInfo.CurrentCulture, str);

        /// <summary>
        /// 获取当前语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string GetString(CultureInfo culture, FormattableString str)
            => string.Format(new TranslationFormatter(culture), str.Format, str.GetArguments());

        class TranslationFormatter : IFormatProvider, ICustomFormatter
        {
            private CultureInfo culture;

            public TranslationFormatter(CultureInfo culture) => this.culture = culture;

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (format == "!")
                {
                    return arg?.ToString();
                }
                var culture = string.IsNullOrWhiteSpace(format) ? this.culture : CultureInfo.GetCultureInfo(format);
                return GetString(culture, arg?.ToString());
            }

            public object GetFormat(Type formatType) =>
                formatType == typeof(ICustomFormatter) ? this : null;
        }

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
        public static string GetString(CultureInfo culture, string str) =>
            str != null && str != " " && (_languageMapper.TryGetValue(culture, out var map) && map.TryGetValue(str, out var str1)) ? str1 : str;

        /// <summary>
        /// 获取特定语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="culture">语言区域</param>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string Concat(CultureInfo culture, params string[] strs) =>
            strs == null ? null : strs.Length == 0 ? "" : string.Concat(strs.Select(x => GetString(culture, x)));
    }
}