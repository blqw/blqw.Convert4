using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace blqw.Services
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// 判断是否为16进制格式的字符串,如果为true,将参数s的前缀(0x/&amp;h)去除
        /// </summary>
        /// <param name="str"> 需要判断的字符串 </param>
        /// <returns> </returns>
        public static string ToHex(this string str)
        {
            if ((str == null) || (str.Length == 0))
            {
                return null;
            }
            var c = str[0];
            if (char.IsWhiteSpace(c)) //有空格去空格
            {
                str = str.TrimStart();
            }
            if (str.Length > 2) //判断是否是0x 或者 &h 开头
            {
                switch (c)
                {
                    case '0':
                        switch (str[1])
                        {
                            case 'x':
                            case 'X':
                                str = str.Remove(0, 2);
                                return str;
                            default:
                                return null;
                        }
                    case '&':
                        switch (str[1])
                        {
                            case 'h':
                            case 'H':
                                str = str.Remove(0, 2);
                                return str;
                            default:
                                return null;
                        }
                    default:
                        return null;
                }
            }
            return null;
        }

    }
}
