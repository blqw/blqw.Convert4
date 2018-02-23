using System.Collections.Specialized;
using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace blqw.ConvertServices
{
    class DedicatedServiceProviderEngine<T> : IDedicatedServiceProviderEngine
    {
        private readonly ConcurrentDictionary<T, ServiceProvider> _serviceProviders = new ConcurrentDictionary<T, ServiceProvider>();
        private readonly Func<T, IServiceProvider> _getter;
        public DedicatedServiceProviderEngine() { }

        public DedicatedServiceProviderEngine(Func<T, IServiceProvider> getter) => _getter = getter;

        public DedicatedServiceProviderEngine(DedicatedServiceProviderEngine<T> engine)
        {
            if (engine != null)
            {
                _getter = engine._getter;
                foreach (var kv in engine._serviceProviders)
                {
                    _serviceProviders.TryAdd(kv.Key, kv.Value.Clone());
                }
            }
        }

        public IServiceProvider GetServiceProvider(T t) =>
            _serviceProviders.TryGetValue(t, out var provider) ? provider : null;

        public IServiceProvider AddService(T t, Type serviceType, object service) =>
            _serviceProviders.GetOrAdd(t, x => new ServiceProvider(_getter?.Invoke(x), null)).AddService(serviceType, service);

        class ServiceProvider : IServiceProvider
        {
            private readonly List<(Type type, object service)> _services;
            private readonly IServiceProvider _provider;

            public ServiceProvider(IServiceProvider serviceProvider, List<(Type type, object service)> services)
            {
                _provider = serviceProvider;
                _services = services ?? new List<(Type type, object service)>();
            }

            public object GetService(Type serviceType)
            {
                object substitute = null;
                for (var i = 0; i < _services.Count; i++)
                {
                    var (type, service) = _services[i];
                    if (serviceType == type)
                    {
                        return service;
                    }
                    else if (substitute == null && serviceType.IsInstanceOfType(service))
                    {
                        substitute = service;
                    }
                }
                return substitute ?? _provider?.GetService(serviceType);
            }

            public ServiceProvider AddService(Type serviceType, object service)
            {
                if (serviceType == null)
                {
                    if (service == null)
                    {
                        return this;
                    }
                    serviceType = service.GetType();
                }
                for (var i = 0; i < _services.Count; i++)
                {
                    var (type, _) = _services[i];
                    if (serviceType == type)
                    {
                        _services[i] = (type, service);
                        if (serviceType == typeof(object) && service.GetType() != typeof(object))
                        {
                            AddService(service.GetType(), service);
                        }
                        return this;
                    }
                }
                _services.Add((serviceType, service));
                if (serviceType == typeof(object) && service.GetType() != typeof(object))
                {
                    AddService(service.GetType(), service);
                }
                return this;
            }

            public ServiceProvider Clone() => new ServiceProvider(_provider, new List<(Type type, object service)>(_services));
        }

        IServiceProvider IDedicatedServiceProviderEngine.GetServiceProvider(object target) => GetServiceProvider((T)target);
        void IDedicatedServiceProviderEngine.AddService(object target, Type serviceType, object service) => AddService((T)target, serviceType, service);
        object IDedicatedServiceProviderEngine.GetServiceProviderService() => (Func<T, IServiceProvider>)GetServiceProvider;
    }
}
