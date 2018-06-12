using System.Collections;
using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using blqw;
using System.Threading;

[assembly: AssemblyStartup(typeof(blqw.ServiceContainer))]
namespace blqw
{
    /// <summary>
    /// 启动器, 用于注入和获取服务
    /// <para></para> 可由 https://github.com/blqw/blqw.Startup 启动, 也可以独立执行
    /// </summary>
    internal static class ServiceContainer
    {
        /// <summary>
        /// 注入的服务提供程序
        /// </summary>
        private static ServiceProvider _injectedServices;
        /// <summary>
        /// 非注入的服务提供程序
        /// </summary>
        private static ServiceProvider _inbuiltServices;
        /// <summary>
        /// 服务提供程序更新事件
        /// </summary>
        private static event EventHandler<ServiceProvider> _changed;

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            var types = typeof(Convert4).Assembly.SafeGetTypes();
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
                if (_injectedServices != null)
                {
                    return _injectedServices;
                }

                //获取服务提供程序时, 如果没有注入服务, 则创建标准服务提供程序
                if (_inbuiltServices == null)
                {
                    var services = new ServiceCollection();
                    ConfigureServices(services);
                    _inbuiltServices = services.BuildServiceProvider();
                    OnChanged(_inbuiltServices);
                }

                return _inbuiltServices;
            }
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        public static void Configure(ServiceProvider serviceProvider)
        {
            var prev = Interlocked.Exchange(ref _injectedServices, serviceProvider);
            if (prev != serviceProvider)
            {
                OnChanged(serviceProvider);
            }
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
