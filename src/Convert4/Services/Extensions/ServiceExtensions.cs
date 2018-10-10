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
    internal static class ServiceExtensions
    {
        /// <summary>
        /// 获取 区域对象 <seealso cref="CultureInfo"/>, 默认返回当前区域 <seealso cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <returns></returns>
        public static CultureInfo GetCultureInfo(this IServiceProvider provider) =>
            provider?.GetService(typeof(CultureInfo)) as CultureInfo ?? CultureInfo.CurrentCulture;

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
            var separator = (provider?.GetService(typeof(ConvertSettings)) as ConvertSettings)?.GetNamedService(STRING_SEPARATOR);
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
            var separator = (provider?.GetService(typeof(ConvertSettings)) as ConvertSettings)?.GetNamedService(STRING_SEPARATOR);
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
        public static ISerializationService GetSerializer(this IServiceProvider provider)
        {
            if (provider == null)
            {
                return null;
            }
            if (provider.GetService(typeof(ConvertSettings)) is ConvertSettings settings)
            {
                var contract = settings.GetNamedService(SERIALIZATION_CONTRACT) as string;
                if (string.IsNullOrWhiteSpace(contract))
                {
                    return (ISerializationService)settings.GetService(typeof(ISerializationService));
                }
                return provider.GetServices<ISerializationService>().FirstOrDefault(x => x.Contract == contract);
            }
            return provider.GetService<ISerializationService>();
        }

        /// <summary>
        /// 获取指定类型的格式化字符串
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <returns></returns>
        public static string GetFormat(this IServiceProvider provider, Type forType)
        {
            if (forType == null || provider == null)
            {
                return null;
            }
            if (provider.GetService(typeof(ConvertSettings)) is ConvertSettings settings)
            {
                return settings.GetNamedServiceForType(forType, FORMAT) as string;
            }
            return null;
        }

        public static IFormatProvider GetFormatProvider(this IServiceProvider provider, Type forType)
        {
            if (forType == null || provider == null)
            {
                return null;
            }
            if (provider.GetService(typeof(ConvertSettings)) is ConvertSettings settings)
            {
                return settings.GetServiceForType(forType, typeof(IFormatProvider)) as IFormatProvider;
            }
            return provider.GetService(typeof(IFormatProvider)) as IFormatProvider;
        }

        /// <summary>
        /// 获取指定类型的服务, 如果获取失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider"></param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider provider, T defaultValue = default) =>
            provider?.GetService(typeof(T)) is T t ? t : defaultValue;


        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="context">转换上下文</param>
        /// <returns></returns>
        public static IConvertor<T> Get<T>(this IConvertorSelector selector, ConvertContext context) =>
            (IConvertor<T>)selector.Get(typeof(T), context);
    }
}
