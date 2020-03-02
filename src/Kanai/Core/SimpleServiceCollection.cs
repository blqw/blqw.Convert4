using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace blqw.Kanai.Core
{
    internal class SimpleServiceCollection : List<ServiceDescriptor>, IServiceCollection, IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            foreach (var descriptor in this)
            {
                if (descriptor.ServiceType == serviceType)
                {
                    return CreateInstance(descriptor);
                }
            }

            if (typeof(IEnumerable).IsAssignableFrom(serviceType) && serviceType.IsGenericType && serviceType.GenericTypeArguments.Length == 1)
            {
                var srvType = serviceType.GenericTypeArguments[0];
                var arr = new ArrayList();

                foreach (var descriptor in this)
                {
                    if (descriptor.ServiceType == srvType)
                    {
                        arr.Add(CreateInstance(descriptor));
                    }
                }

                return arr.ToArray(srvType);
            }

            return null;
        }

        private object CreateInstance(ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }
            if (descriptor.ImplementationType != null)
            {
                return ActivatorUtilities.CreateInstance(this, descriptor.ImplementationType);
            }
            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(this);
            }
            return null;
        }
    }
}
