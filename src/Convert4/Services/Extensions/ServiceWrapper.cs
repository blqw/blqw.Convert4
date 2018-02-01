using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Services
{
    class ServiceWrapper : IServiceProvider
    {
        private readonly IServiceProvider[] _serviceProviders;

        public ServiceWrapper(params IServiceProvider[] serviceProviders) =>
            _serviceProviders = serviceProviders ?? throw new ArgumentNullException(nameof(serviceProviders));

        public object GetService(Type serviceType) =>
            _serviceProviders.Select(x => x.GetService(serviceType)).Where(x => x != null).FirstOrDefault();
    }
}
