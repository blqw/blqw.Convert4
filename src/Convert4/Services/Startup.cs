using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using blqw.Services;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using blqw.Define;

namespace blqw
{
    static class Startup
    {
        /// <summary>
        /// 注入的转换器
        /// </summary>
        private static ServiceProvider _serviceProvider;
        /// <summary>
        /// 非注入的转换器
        /// </summary>
        private static ServiceProvider _internalProvider;

        private static event EventHandler<ServiceProvider> _changed;

        /// <summary>
        /// 注入组件
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany(TypeExtensions.SafeGetTypes)
                     .Where(x => x.IsClass && x.Instantiable() && typeof(IConvertor).IsAssignableFrom(x))
                     .ForEach(x => services.AddSingleton(typeof(IConvertor), x));
            services.AddSingleton<IConvertorSelector>(provider => new ConvertorSelector(provider));
        }

        /// <summary>
        /// 获取转换器
        /// </summary>
        public static ServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider != null)
                {
                    return _serviceProvider;
                }

                if (_internalProvider == null)
                {
                    var services = new ServiceCollection();
                    ConfigureServices(services);
                    _internalProvider = services.BuildServiceProvider();
                    OnChanged(_internalProvider);
                }

                return _internalProvider;
            }
        }

        /// <summary>
        /// 获取已注入的组件
        /// </summary>
        /// <param name="convertors"></param>
        public static void Configure(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            OnChanged(serviceProvider);
        }

        /// <summary>
        /// 更新组件事件
        /// </summary>
        public static event EventHandler<ServiceProvider> Changed
        {
            add
            {
                _changed -= value;
                _changed += value;
            }
            remove
            {
                _changed -= value;
            }
        }

        /// <summary>
        /// 触发 <seealso cref="Changed"/> 事件
        /// </summary>
        /// <param name="convertors"></param>
        private static void OnChanged(ServiceProvider serviceProvider) =>
            _changed?.Invoke(null, serviceProvider);
    }
}
