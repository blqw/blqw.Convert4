using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace blqw
{
    /// <summary>
    /// 启动器, 用于注入和获取服务
    /// <para></para> 可由 https://github.com/blqw/blqw.Startup 启动, 也可以独立执行
    /// </summary>
    static class Startup
    {
        /// <summary>
        /// 注入的服务提供程序
        /// </summary>
        private static ServiceProvider _serviceProvider;
        /// <summary>
        /// 非注入的服务提供程序
        /// </summary>
        private static ServiceProvider _internalProvider;
        /// <summary>
        /// 服务提供程序更新事件
        /// </summary>
        private static event EventHandler<ServiceProvider> _changed;

        /// <summary>
        /// 注入组件
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            var types = Assembly.GetAssembly(typeof(Convert4)).SafeGetTypes();
            types.Where(x => x.IsClass && x.Instantiable() && typeof(IConvertor).IsAssignableFrom(x))
                 .ForEach(x => services.AddSingleton(typeof(IConvertor), x));
            services.AddSingleton(typeof(IConvertorSelector), typeof(ConvertorSelector));
        }

        /// <summary>
        /// 服务提供程序, 优先获取注入的服务转换器, 如果没有注入则获取内部实现的服务提供程序
        /// </summary>
        public static ServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider != null)
                {
                    return _serviceProvider;
                }

                //获取服务提供程序时, 如果没有注入服务, 则创建标准服务提供程序
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
