using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Services
{
    class MultiServicesProvider : IServiceProvider
    {
        private readonly IServiceProvider[] _serviceProviders;

        public MultiServicesProvider(params IServiceProvider[] serviceProviders) =>
            _serviceProviders = serviceProviders ?? throw new ArgumentNullException(nameof(serviceProviders));

        public object GetService(Type serviceType) =>
            _serviceProviders.Select(x => x.GetService(serviceType)).FirstOrDefault(x => x != null);
    }
}
