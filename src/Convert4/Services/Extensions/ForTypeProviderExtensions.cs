using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Linq;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 关于 命名服务提供程序 <seealso cref="IForTypeProvider"/> 的扩展方法
    /// </summary>
    public static class ForTypeProviderExtensions
    {
        /// <summary>
        /// 获取提供给指定类型的标准服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static object GetForTypeService(this IServiceProvider provider, Type forType, Type serviceType) =>
            provider == null ||  forType == null || serviceType == null
            ? null
            : provider.GetDedicatedServiceProvider(forType)?.GetService(serviceType);

        /// <summary>
        /// 获取提供给指定类型的命名服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        //public static object GetForTypeNamedService(this IServiceProvider provider, Type forType, string serviceName) =>
        //    forType == null || string.IsNullOrWhiteSpace(serviceName)
        //    ? null
        //    : provider.GetForTypeServiceProvider(forType)?.GetNamedService(serviceName);

        /// <summary>
        /// 获取提供给指定类型的命名服务, 并转为指定类型, 如果获取失败或转换失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">服务名称</param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetForTypeNamedService<T>(this IServiceProvider provider, Type forType, string name, T defaultValue = default) =>
            provider.GetDedicatedServiceProvider(forType).GetNamedService(name, defaultValue);

        /// <summary>
        /// 获取提供给指定类型的标准服务, 如果获取失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider"></param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider provider, Type forType, T defaultValue = default)
            => provider?.GetForTypeService(forType, typeof(T)) is T t ? t : defaultValue;

        public static void AddForTypeSingleton(this IServiceCollection services) =>
            services.AddForTypeSingleton(null, null);

        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        public static void AddForTypeSingleton(this IServiceCollection services, Type forType, object service)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var engine = services.GetTargetedServiceProviderEngine<Type>();

            if (forType != null && service != null)
            {
                engine.AddService(forType, service.GetType(), service);
            }
        }

        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        public static void AddForTypeService(this IServiceProvider provider, Type forType, object service)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var engine = provider.GetService<DedicatedServiceProviderEngine<Type>>();

            if (forType != null && service != null)
            {
                engine.AddService(forType, service.GetType(), service);
            }
        }
    }
}
