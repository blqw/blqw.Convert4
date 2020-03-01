using blqw.Kanai.Factories;
using Microsoft.Extensions.DependencyInjection;
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
            var types = typeof(ServiceExtensions).Assembly.SafeGetTypes();
            foreach (var type in types.Where(x => x.IsClass && x.Instantiable()))
            {
                if (typeof(IConvertorFactory).IsAssignableFrom(type))
                {
                    services.AddSingleton(typeof(IConvertorFactory), type);
                }
                if (typeof(ITranslator).IsAssignableFrom(type))
                {
                    services.AddSingleton(typeof(ITranslator), type);
                }
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
