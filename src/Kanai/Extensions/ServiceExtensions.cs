using blqw.Kanai.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace blqw.Kanai
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddKanai(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (services.Any(x => x.ServiceType == typeof(IConvertorSelector) && x.ImplementationType == typeof(ConvertorSelector)))
            {
                return services;
            }
            //枚举所有可实例化的类型
            var types = assemblies.Distinct().SelectMany(x => x.SafeGetTypes().Where(y => y.IsClass && y.Instantiable()));
            foreach (var type in types)
            {
                // 将指定接口实现类添加到服务
                if (typeof(IConvertorFactory).IsAssignableFrom(type))
                {
                    services.AddSingleton(typeof(IConvertorFactory), type);
                }
                if (typeof(ITranslator).IsAssignableFrom(type))
                {
                    services.AddSingleton(typeof(ITranslator), type);
                }
                // 添加转换器
                var genericArguments = type.GetGenericArguments(typeof(IConvertor<>));
                if (genericArguments != null)
                {
                    // 将转换器转为 IConvertorFactory 后添加到服务
                    var factoryType = typeof(InstantiatedConvertorFactory<>).MakeGenericType(genericArguments);
                    services.AddSingleton(typeof(IConvertorFactory), p => ActivatorUtilities.CreateInstance(p, factoryType, ActivatorUtilities.CreateInstance(p, type)));
                }
            }
            services.AddSingleton<IConvertorSelector, ConvertorSelector>();
            return services;
        }

        public static IServiceCollection AddKanai(this IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(IConvertorSelector) && x.ImplementationType == typeof(ConvertorSelector)))
            {
                return services;
            }
            return AddKanai(services, AppDomain.CurrentDomain.GetAssemblies().Append(typeof(ServiceExtensions).Assembly));
        }

    }
}
