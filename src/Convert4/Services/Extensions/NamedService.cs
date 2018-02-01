using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Services
{
    class NamedService : IServiceProvider
    {
        private readonly IDictionary _services;

        public NamedService(IDictionary services) =>
            _services = services ?? throw new ArgumentNullException(nameof(services));

        public object GetService(Type serviceType) =>
            serviceType == typeof(NamedService) ? this : _services[serviceType];

        public object GetService(string serviceName) =>
                    _services[serviceName];
    }
}
