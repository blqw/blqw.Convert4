using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 聚合服务提供程序
    /// </summary>
    public class AggregateServicesProvider : IServiceProvider, IServiceScopeFactory, IDisposable
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        private IServiceProvider[] _serviceProviders;

        /// <summary>
        /// 初始化聚合服务提供程序
        /// </summary>
        /// <param name="serviceProviders">一组服务提供程序</param>
        public AggregateServicesProvider(params IServiceProvider[] serviceProviders) =>
            _serviceProviders = serviceProviders ?? throw new ArgumentNullException(nameof(serviceProviders));

        /// <summary>
        /// 获取服务, 循环服务提供程序, 并返回第一个得到的服务, 如果全部提供程序均没有返回服务, 则返回null
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof(IServiceScopeFactory))
            {
                return this;
            }
            return _serviceProviders.Select(x => x.GetService(serviceType)).FirstOrDefault(x => x != null);
        }

        /// <summary>
        /// 创建一个服务范围的实例
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope() =>
            new Scope(new AggregateServicesProvider(_serviceProviders.Select(x => x.GetService<IServiceScopeFactory>()?.CreateScope()?.ServiceProvider ?? x).ToArray()));

        /// <summary>
        /// 服务范围对象
        /// </summary>
        private class Scope : IServiceScope
        {
            public Scope(IServiceProvider provider) => ServiceProvider = provider;

            public IServiceProvider ServiceProvider { get; }

            public void Dispose() => (ServiceProvider as IDisposable)?.Dispose();
        }

        /// <summary>
        /// 施放资源
        /// </summary>
        public void Dispose()
        {
            var providers = _serviceProviders;
            _serviceProviders = null;
            providers.SafeForEach<IDisposable>(x => x.Dispose());
        }
    }
}
