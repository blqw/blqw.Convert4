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
            var types = typeof(Convert).Assembly.SafeGetTypes();
            foreach (var type in types.Where(x => typeof(IConvertorFactory).IsAssignableFrom(x) && x.IsClass && x.Instantiable()))
            {
                services.AddSingleton(typeof(IConvertorFactory), type);
            }
            foreach (var type in types.Where(x => typeof(IConvertor<>).IsAssignableFrom(x) && x.IsClass && x.Instantiable()))
            {
                var genericArguments = type.GetGenericArguments(typeof(IConvertor<>));
                var factory = Activator.CreateInstance(typeof(InstantiatedConvertorFactory<>).MakeGenericType(genericArguments));
                services.AddSingleton(typeof(IConvertorFactory), factory);
            }
            services.AddSingleton<IConvertorSelector, ConvertorSelector>();
            return services;
        }

    }
}
