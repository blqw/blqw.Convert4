﻿using Microsoft.Extensions.DependencyInjection;
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
        /// 添加Convert4服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConvert4(this IServiceCollection services)
        {
            var types = typeof(Convert4).Assembly.SafeGetTypes();
            types.Where(x => typeof(IConvertor).IsAssignableFrom(x) && x.IsClass && x.Instantiable())
                 .ForEach(x => services.AddSingleton(typeof(IConvertor), x));
            services.AddSingleton<IConvertorSelector, ConvertorSelector>();
            services.AddSingleton<ConvertSettings>();
            return services;
        }

    }
}
