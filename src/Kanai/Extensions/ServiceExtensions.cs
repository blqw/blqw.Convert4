using blqw.Kanai.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace blqw.Kanai
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddKanai(this IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(IConvertorSelector) && x.ImplementationType == typeof(ConvertorSelector)))
            {
                return services;
            }
            var types = typeof(Convert).Assembly.SafeGetTypes();
            foreach (var type in types.Where(x => typeof(IConvertorFactory).IsAssignableFrom(x) && x.IsClass && x.Instantiable()))
            {
                services.AddSingleton(typeof(IConvertorFactory), type);
            }
            foreach (var type in types.Where(x => typeof(IConvertor).IsAssignableFrom(x) && x.IsClass && x.Instantiable()))
            {
                var genericArguments = type.GetGenericArguments(typeof(IConvertor<>));
                if (genericArguments != null)
                {
                    var factoryType = typeof(InstantiatedConvertorFactory<>).MakeGenericType(genericArguments);
                    services.AddSingleton(type, type);
                    services.AddSingleton(typeof(IConvertorFactory), p =>
                            ActivatorUtilities.CreateInstance(p, factoryType, p.GetService(type))
                    );
                }
            }
            services.AddSingleton<IConvertorSelector, ConvertorSelector>();
            return services;
        }

    }
}
