﻿using System;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 关于服务提供程序 <seealso cref="IServiceProvider"/> 的扩展方法
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 获取指定类型的服务, 如果获取失败, 返回 <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="provider"></param>
        /// <param name="defaultValue">默认服务</param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider provider, T defaultValue = default) =>
            provider?.GetService(typeof(T)) is T t ? t : defaultValue;
    }
}
