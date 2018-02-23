using Microsoft.Extensions.DependencyInjection;
using System;

namespace blqw.ConvertServices
{
    static class DedicatedServiceExtensions
    {
        public static DedicatedServiceProviderEngine<T> GetTargetedServiceProviderEngine<T>(this IServiceCollection services)
        {
            var engine = services.GetSingletonService<DedicatedServiceProviderEngine<T>>();
            if (engine == null)
            {
                var func = services.GetSingletonServiceDescriptor<Func<T, IServiceProvider>>();
                if (func != null)
                {
                    services.Remove(func);
                }
                engine = new DedicatedServiceProviderEngine<T>((Func<T, IServiceProvider>)func?.ImplementationInstance);
                services.AddSingleton(engine);
                services.AddScoped(p => new DedicatedServiceProviderEngine<T>(engine));
                services.AddScoped<Func<T, IServiceProvider>>(p => p.GetService<DedicatedServiceProviderEngine<T>>().GetServiceProvider);
            }
            return engine;
        }

        public static DedicatedServiceProviderEngine<T> GetTargetedServiceProviderEngine<T>(this IServiceProvider provider) =>
            provider?.GetService<DedicatedServiceProviderEngine<T>>();


        /// <summary>
        /// 获取专属服务提供程序
        /// </summary>
        /// <typeparam name="T"> 专属对象 类型</typeparam>
        /// <param name="provider">服务提供程序</param>
        /// <param name="target"> 专属对象 </param>
        /// <returns></returns>
        public static IServiceProvider GetDedicatedServiceProvider<T>(this IServiceProvider provider, T target) =>
                    (provider == null || target == null) ? null :
                    provider.GetService<Func<T, IServiceProvider>>()?.Invoke(target) ??
                    provider.GetTargetedServiceProviderEngine<T>()?.GetServiceProvider(target);

        public static IServiceProvider GetOrAddGetDedicatedServiceProvider<T>(this IServiceProvider provider, T target, Func<T, object> factor)
        {
            if (provider == null)
            {
                return null;
            }
            var engine = provider.GetTargetedServiceProviderEngine<T>();
            return engine.GetServiceProvider(target) ?? engine.AddService(target, null, factor(target)); ;
        }

    }
}
