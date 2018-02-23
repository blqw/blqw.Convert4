using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public static T GetNamedService<T>(this IServiceProvider provider, string name, T defaultValue = default) =>
            provider.GetDedicatedServiceProvider(name)?.GetService(typeof(T)) is T t ? t : defaultValue;


        /// <summary>
        /// 获取指定名称的服务, 并转为指定类型, 如果获取失败或转换失败, 返回 null
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        //public static object GetNamedService(this IServiceProvider provider, string name)
        //    => string.IsNullOrWhiteSpace(name) ? null : provider?.GetNamedServiceProvider()?.GetServiceProvider(name).get;

        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddNamedSingleton(this IServiceCollection services) =>
            services.AddNamedSingleton<object>(null, null);

        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        public static void AddNamedSingleton<T>(this IServiceCollection services, string name, T service)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var engine = services.GetTargetedServiceProviderEngine<string>();

            if (!string.IsNullOrWhiteSpace(name) && service != null)
            {
                engine.AddService(name, typeof(T), service);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public static void AddNamedService<T>(this IServiceProvider provider, string name, T service)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (!string.IsNullOrWhiteSpace(name) && service != null)
            {
                var engine = provider?.GetService<DedicatedServiceProviderEngine<string>>();

                if (engine == null)
                {
                    throw new NotSupportedException(SR.GetString($"未找到命名服务提供程序"));
                }

                engine.AddService(name, typeof(T), service);
            }
        }


    }
}
