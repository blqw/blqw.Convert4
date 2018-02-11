using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static blqw.ConvertServices.NamedServiceNames;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 获取服务的扩展方法
    /// </summary>
    public static class GetServiceExtensions
    {
        /// <summary>
        /// 获取 区域对象 <seealso cref="CultureInfo"/>, 默认返回当前区域 <seealso cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static CultureInfo GetCultureInfo(this IServiceProvider provider) =>
            provider?.GetService(typeof(CultureInfo)) as CultureInfo ?? CultureInfo.CurrentCulture;

        /// <summary>
        /// 获取提供给指定类型的 <seealso cref="IFormatProvider"/> 对象
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <returns></returns>
        public static IFormatProvider GetFormatProvider(this IServiceProvider provider, Type forType) =>
            provider?.GetService<IForTypeProvider>()?.GetServiceProvider(forType)?.GetService<IFormatProvider>()
            ?? provider?.GetService<IFormatProvider>();

        /// <summary>
        /// 获取编码对象 <seealso cref="Encoding"/>, 默认返回 <seealso cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(this IServiceProvider provider) =>
                   provider?.GetService(typeof(Encoding)) as Encoding ?? Encoding.UTF8;

        /// <summary>
        /// 获取序列化上下文 <seealso cref="StreamingContextStates"/> , 默认返回 <seealso cref="StreamingContextStates.All"/>
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static StreamingContextStates GetStreamingContextStates(this IServiceProvider provider) =>
            provider?.GetService(typeof(StreamingContextStates)) is StreamingContextStates v ? v : StreamingContextStates.All;

        /// <summary>
        /// 获取字符串分割选项 <seealso cref="StringSplitOptions"/>, 默认返回 <seealso cref="StringSplitOptions.None"/>
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static StringSplitOptions GetStringSplitOptions(this IServiceProvider provider) =>
            provider?.GetService(typeof(StringSplitOptions)) is StringSplitOptions v ? v : StringSplitOptions.None;

        /// <summary>
        /// 获取字符串分隔符, 如果有多个返回一个 <see cref="string"/> 数组 或 <see cref="char"/> 数组
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static object GetStringSeparators(this IServiceProvider provider)
        {
            var separator = provider?.GetNamedService(StringSeparator);
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
        public static string GetStringSeparator(this IServiceProvider provider)
        {
            var separator = provider?.GetNamedService(StringSeparator);
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
        /// 获取序列化服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static ISerializationService GetSerialization(this IServiceProvider provider)
        {
            var contract = provider?.GetNamedService<string>(SerializationContract);
            if (string.IsNullOrWhiteSpace(contract))
            {
                return null;
            }
            return provider.GetServices<ISerializationService>()?.First(x => x.Contract == contract);
        }

        /// <summary>
        /// 获取指定类型的格式化字符串
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        public static string GetFormat<T>(this IServiceProvider provider)
          where T : IFormattable =>
            provider?.GetFormat(typeof(T));

        /// <summary>
        /// 获取指定类型的格式化字符串
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <returns></returns>
        public static string GetFormat(this IServiceProvider provider, Type forType) =>
            forType == null ? null : provider?.GetForTypeNamedService<string>(forType, Format, null);





    }
}
