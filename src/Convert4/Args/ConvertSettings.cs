using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace blqw
{
    /// <summary>
    /// 转换器设置参数
    /// </summary>
    public sealed class ConvertSettings
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 全局设置
        /// </summary>
        public static readonly ConvertSettings Global = new ServiceCollection().AddConvert4().BuildServiceProvider().GetService<ConvertSettings>();


        public ConvertSettings(IServiceProvider provider = null) =>
            ServiceProvider = provider ?? Global?.ServiceProvider;

        private static readonly object _unSet = new object();

        private LinkedList<(Type, string, object)> _services;
        private LinkedListNode<(Type, string, object)> _firstForTypeService;

        public object DefaultValue { get; set; } = _unSet;

        public bool Throwable => ReferenceEquals(DefaultValue, _unSet);

        public LinkedList<(Type, string, object)> Services
        {
            get
            {
                if (_services != null)
                {
                    return _services;
                }
                var services = new LinkedList<(Type, string, object)>();
                services.AddFirst((null, null, null));
                _firstForTypeService = services.AddLast((null, null, null));
                return _services = services;
            }
        }

        /// <summary>
        /// 获取标准服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            if (serviceType == typeof(ConvertSettings))
            {
                return this;
            }
            if (Services == null)
            {
                return null;
            }
            return GetExact(null, serviceType, null, out _)
                ?? GetEnumerable(null, serviceType);
        }

        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public object GetNamedService(string name)
        {
            if (name == null || Services == null)
            {
                return null;
            }
            return GetExact(null, null, name, out _);
        }

        /// <summary>
        /// 获取类型专属的标准服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetServiceForType(Type forType, Type serviceType)
        {
            if (serviceType == null || Services == null)
            {
                return null;
            }
            return GetExact(forType, serviceType, null, out _)
                ?? GetEnumerable(forType, serviceType)
                ?? GetExact(null, serviceType, null, out _)
                ?? GetEnumerable(null, serviceType);
        }

        /// <summary>
        /// 获取类型专属的命名服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public object GetNamedServiceForType(Type forType, string name)
        {
            if (name == null || Services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetNamedServiceForType(forType, name);
            }
            return GetExact(forType, null, name, out _) ?? GetExact(null, null, name, out _);
        }

        /// <summary>
        /// 获取服务集合
        /// </summary>
        private object GetEnumerable(Type forType, Type serviceType)
        {
            if (serviceType.IsConstructedGenericType &&
               serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var itemType = serviceType.GenericTypeArguments.Single();

                var services = new ArrayList();
                for (var node = forType == null ? Services.First.Next : (_firstForTypeService?.Next ?? Services.Last);
                     node?.Value.Item3 != null;
                     node = node.Next)
                {
                    var (t, n, v) = node.Value;
                    if (forType == t
                        && n == null
                        && itemType.IsInstanceOfType(v))
                    {
                        services.Add(v);
                    }
                }
                return services.ToArray(itemType);
            }
            return null;
        }

        /// <summary>
        /// 获取精确服务
        /// </summary>
        /// <returns></returns>
        private object GetExact(Type forType, Type serviceType, string name, out LinkedListNode<(Type, string, object)> node)
        {
            for (node = forType == null ? Services.First.Next : (_firstForTypeService?.Next ?? Services.Last);
                 node?.Value.Item3 != null;
                 node = node.Next)
            {
                var (t, n, v) = node.Value;
                if (forType == t
                    && n == name
                    && (serviceType == null || serviceType.IsInstanceOfType(v)))
                {
                    return v;
                }
            }
            node = null;
            return null;
        }

        /// <summary>
        /// 添加标准服务
        /// </summary>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddService<TService>(TService service)
        {
            CheckParams(typeof(object), "name", service);
            GetExact(null, typeof(TService), null, out var node);
            Services.AddAfter(node?.Previous ?? _services.First, (null, null, service));
            return this;
        }

        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddNamedService(string name, object service)
        {
            CheckParams(typeof(object), name, service);
            GetExact(null, null, name, out var node);
            Services.AddAfter(node?.Previous ?? _services.First, (null, name, service));
            return this;
        }

        /// <summary>
        /// 添加类型专属的标准服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddForType<TService>(Type forType, TService service)
        {
            CheckParams(forType, "name", service);
            GetExact(forType, typeof(TService), null, out var node);
            Services.AddAfter(node?.Previous ?? _firstForTypeService, (forType, null, service));
            return this;
        }

        /// <summary>
        /// 添加类型专属的命名服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddNamedForType(Type forType, string name, object service)
        {
            CheckParams(forType, name, service);
            GetExact(forType, null, name, out var node);
            Services.AddAfter(node?.Previous ?? _firstForTypeService, (forType, name, service));
            return this;
        }

        private void CheckParams(Type forType, string name, object service)
        {
            if (forType == null)
            {
                throw new ArgumentNullException(nameof(forType));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
        }
    }
}
