using System;

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
        public static IForTypeProvider GetForTypeServiceProvider(this IServiceProvider provider) =>
            provider as IForTypeProvider ?? provider.GetService<IForTypeProvider>();

        /// <summary>
        /// 获取提供给指定类型的标准服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static object GetForTypeService(this IServiceProvider provider, Type forType, Type serviceType) =>
            forType == null || serviceType == null
            ? null
            : provider.GetForTypeServiceProvider()?.GetServiceProvider(forType)?.GetService(serviceType);

        /// <summary>
        /// 获取提供给指定类型的命名服务
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        /// <param name="forType">指定类型</param>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static object GetForTypeNamedService(this IServiceProvider provider, Type forType, string serviceName) =>
            forType == null || string.IsNullOrWhiteSpace(serviceName)
            ? null
            : provider.GetForTypeServiceProvider()?.GetNamedServiceProvider(forType)?.GetService(serviceName);

        /// <summary>
        /// 获取提供给指定类型的命名服务, 并转为指定类型, 如果获取失败或转换失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        /// <param name="name">服务名称</param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetForTypeNamedService<T>(this IServiceProvider provider, Type forType, string name, T defaultValue = default)
            => provider.GetForTypeNamedService(forType, name) is T t ? t : defaultValue;

        /// <summary>
        /// 获取提供给指定类型的标准服务, 如果获取失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider"></param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider provider, Type forType, T defaultValue = default)
            => provider?.GetForTypeService(forType, typeof(T)) is T t ? t : defaultValue;
    }
}
