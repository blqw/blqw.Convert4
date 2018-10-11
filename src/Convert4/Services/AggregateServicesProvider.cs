﻿using System.Collections;
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
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var itemServiceType = serviceType.GetGenericArguments().Single();
                return _serviceProviders.SelectMany(x => x.GetServices(itemServiceType)).ToList();
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
            private IServiceProvider _serviceProvider;

            public Scope(IServiceProvider provider) => _serviceProvider = provider;

            public IServiceProvider ServiceProvider => _serviceProvider;
            public void Dispose() => DisposeHelper.DisposeAndRemove(ref _serviceProvider);
        }

        /// <summary>
        /// 施放资源
        /// </summary>
        public void Dispose() => DisposeHelper.DisposeAndRemove(ref _serviceProviders);

        public override bool Equals(object obj)
        {
            if (obj is AggregateServicesProvider aggregate)
            {
                if (aggregate._serviceProviders.Length != _serviceProviders.Length)
                {
                    return false;
                }
                for (var i = 0; i < _serviceProviders.Length; i++)
                {
                    if (_serviceProviders[i] != aggregate._serviceProviders[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 0;
                foreach (var provider in _serviceProviders)
                {
                    hash = (hash << 1) ^ provider.GetHashCode();
                }
                return hash;
            }
        }
    }
}
