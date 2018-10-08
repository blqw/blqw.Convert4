using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
        internal static T GetService<T>(this IServiceProvider provider, T defaultValue = default) =>
            provider?.GetService(typeof(T)) is T t ? t : defaultValue;


        internal static IServiceProvider Join(this IServiceProvider provider, IServiceProvider provider2)
        {
            if (provider == null || provider2 == null)
            {
                return provider ?? provider2;
            }
            return new AggregateServicesProvider(provider, provider2);
        }

        /// <summary>
        /// 添加Convert4服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConvert4(this IServiceCollection services)
        {
            var types = typeof(Convert4).Assembly.SafeGetTypes();
            types.Where(x => x.IsClass && x.Instantiable() && typeof(IConvertor).IsAssignableFrom(x))
                 .ForEach(x => services.AddSingleton(typeof(IConvertor), x));
            services.AddSingleton(typeof(IConvertorSelector), typeof(ConvertorSelector));
            services.AddSingleton<ConvertSettings>();
            return services;
        }

    }
}
