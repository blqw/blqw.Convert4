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
    internal class SR
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

            lock (_languageMapper)
            {
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
        }


        /// <summary>
        /// 获取特定语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="culture">语言区域</param>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string Localize(CultureInfo culture, string str) =>
            str != null && str != " " && (_languageMapper.TryGetValue(culture, out var map) && map.TryGetValue(str, out var str1)) ? str1 : str;


        /// <summary>
        /// 获取特定语言区域的字符串映射，如果没有找到返回str
        /// </summary>
        /// <param name="culture">语言区域</param>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string Concat(CultureInfo culture, params string[] strs) =>
            strs == null ? null : strs.Length == 0 ? "" : string.Concat(strs.Select(x => Localize(culture, x)));


        public static readonly SR CANT_CONVERT = "`{0}` 无法转换为 {1}";

        public static readonly SR VALUE_CANT_CONVERT = "值:`{0}`({1}) 无法转换为 {2}";

        public static readonly SR CANT_BUILD_CONVERTOR = "无法生成 {0} 类型的转换器";


        public static readonly SR CONVERTOR_FAIL = "转换器{0} 转换失败";

        public static readonly SR VALUE_OVERFLOW = "值超过限制";

        private readonly string _str;
        public SR(string str) => _str = str ?? throw new ArgumentNullException(nameof(str));
        /// <summary>
        /// 本地化
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Localize(CultureInfo culture, params object[] args) =>
           string.Format(SR.Localize(culture, _str), args);

        public static implicit operator SR(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return new SR(str);
        }
    }
}