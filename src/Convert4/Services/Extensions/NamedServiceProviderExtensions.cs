using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 关于 命名服务提供程序 <seealso cref="INamedServiceProvider"/> 的扩展方法
    /// </summary>
    public static class NamedServiceProviderExtensions
    {
        /// <summary>
        /// 获取指定名称的服务, 并转为指定类型, 如果获取失败或转换失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">服务名称</param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static INamedServiceProvider GetNamedServiceProvider(this IServiceProvider provider)
            => provider?.GetService<INamedServiceProvider>();


        /// <summary>
        /// 获取指定名称的服务, 并转为指定类型, 如果获取失败或转换失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">服务名称</param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetNamedService<T>(this IServiceProvider provider, string name, T defaultValue = default)
            => provider?.GetNamedService(name) is T t ? t : defaultValue;


        /// <summary>
        /// 获取指定名称的服务, 并转为指定类型, 如果获取失败或转换失败, 返回 null
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static object GetNamedService(this IServiceProvider provider, string name)
            => string.IsNullOrWhiteSpace(name) ? null : provider?.GetNamedServiceProvider()?.GetService(name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public static void AddNamedSingleton(this IServiceCollection services, string name, object service)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var named = services.OfType<INamedServiceProvider>().FirstOrDefault();
            if (named == null)
            {
                named = new ConvertSettings();
                services.AddSingleton(named);
            }
            named.AddService(name, service);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public static void AddNamedService(this IServiceProvider provider, string name, object service)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            var named = provider.GetNamedServiceProvider();
            if (named == null)
            {
                throw new NotSupportedException(SR.GetString($"未找到命名服务提供程序"));
            }
            named.AddService(name, service);
        }


    }
}
