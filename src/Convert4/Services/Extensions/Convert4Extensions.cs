using System.Security.Cryptography;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static blqw.ConvertServices.NamedServiceNames;

namespace blqw
{
    /// <summary>
    /// 常用的公开的拓展方法
    /// </summary>
    public static class Convert4Extensions
    {
        /// <summary>
        /// 在一个可枚举的对象集合中循环执行指定的动作
        /// </summary>
        /// <typeparam name="T">集合项的类型</typeparam>
        /// <param name="enumerable">可枚举对象集合</param>
        /// <param name="action">执行动作</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null || action == null)
            {
                return;
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// 在一个可枚举的对象集合中循环执行指定的动作
        /// </summary>
        /// <param name="enumerable">可枚举对象集合</param>
        /// <param name="action">执行动作</param>
        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            if (enumerable == null || action == null)
            {
                return;
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// 在一个可枚举的对象集合中循环 安全的执行指定的动作(执行<paramref name="action"/>不会产生异常)
        /// </summary>
        /// <param name="enumerable">可枚举对象集合</param>
        /// <param name="action">执行动作</param>
        public static void SafeForEach<T>(this IEnumerable enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                if (item is T t)
                {
                    try
                    {
                        action(t);
                    }
                    catch
                    {
                    }
                }

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

        public static string ToBin(this string str)
        {
            throw new NotImplementedException();
        }

        public static string ToOct(this string str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Fill(this byte[] bytes, int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), SR.GetString($"{"不能小于"} 0"));
            }

            if (bytes == null || size <= 0)
            {
                return new byte[size];
            }
            if (bytes.Length == size)
            {
                return bytes;
            }
            Array.Resize(ref bytes, size);
            return bytes;
        }

        /// <summary>
        /// 设置指定类型的格式化字符串
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="format">格式化字符串</param>
        /// <returns></returns>
        public static ConvertSettings AddFormat<T>(this ConvertSettings settings, string format)
          where T : IFormattable =>
            settings?.AddForType(typeof(T), Format, format);

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        /// <returns></returns>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, string separator) =>
            settings?.AddService(StringSeparator, new[] { separator });

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, char separator) =>
            settings?.AddService(StringSeparator, new [] { separator });

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, params char[] separator) =>
            settings?.AddService(StringSeparator, separator);

        /// <summary>
        /// 设置字符串分隔符
        /// </summary>
        /// <param name="separator">字符串分隔符</param>
        public static ConvertSettings AddStringSeparator(this ConvertSettings settings, params string[] separator) =>
            settings?.AddService(StringSeparator, separator);

        /// <summary>
        /// 设置字符串分割选项
        /// </summary>
        /// <param name="separator">字符串分隔选项</param>
        public static ConvertSettings AddStringSplitOptions(this ConvertSettings settings, StringSplitOptions options) =>
            settings?.AddService(options);
    }
}
