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
    /// ������, ����ע��ͻ�ȡ����
    /// <para></para> ���� https://github.com/blqw/blqw.Startup ����, Ҳ���Զ���ִ��
    /// </summary>
    internal static class ServiceContainer
    {
        /// <summary>
        /// ע��ķ����ṩ����
        /// </summary>
        private static ServiceProvider _injectedServices;
        /// <summary>
        /// ��ע��ķ����ṩ����
        /// </summary>
        private static ServiceProvider _inbuiltServices;
        /// <summary>
        /// �����ṩ��������¼�
        /// </summary>
        private static event EventHandler<ServiceProvider> _changed;

        /// <summary>
        /// ���÷���
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
        /// �����ṩ����, ���Ȼ�ȡע��ķ���ת����, ���û��ע�����ȡ�ڲ�ʵ�ֵķ����ṩ����
        /// </summary>
        public static ServiceProvider ServiceProvider
        {
            get
            {
                if (_injectedServices != null)
                {
                    return _injectedServices;
                }

                //��ȡ�����ṩ����ʱ, ���û��ע�����, �򴴽���׼�����ṩ����
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
        /// ��װ����
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
        /// ��������¼�
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
        /// ���� <seealso cref="Changed"/> �¼�
        /// </summary>
        /// <param name="convertors"></param>
        private static void OnChanged(ServiceProvider serviceProvider) =>
            _changed?.Invoke(null, serviceProvider);
    }
}
