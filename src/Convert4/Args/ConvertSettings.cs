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

        public ConvertSettings(IServiceProvider provider = null) => ServiceProvider = provider ?? Global?.ServiceProvider;

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
        /// 获取设置
        /// </summary>
        public T Get<T>(Type forType, string name)
        {
            var value = Get(forType, name ?? typeof(T).GetFriendlyName(), out _)
                     ?? Get(null, name ?? typeof(T).GetFriendlyName(), out _);
            return value is T t ? t : (default);
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        public T Get<T>(string name) => Get<T>(null, name);

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        private object Get(Type forType, string name, out LinkedListNode<(Type, string, object)> node)
        {
            for (node = forType == null ? Services.First.Next : (_firstForTypeService?.Next ?? Services.Last);
                 node?.Value.Item3 != null;
                 node = node.Next)
            {
                var (t, n, v) = node.Value;
                if (forType == t && n == name)
                {
                    return v;
                }
            }
            node = null;
            return null;
        }


        public ConvertSettings Set<T>(object value, Type forType = null) =>
            Set(typeof(T).GetFriendlyName(), value, forType);

        public ConvertSettings Set(string name, object value, Type forType = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Get(forType, name, out var node);
            if (value == null)
            {
                if (node != null)
                {
                    Services.Remove(node);
                }
            }
            else if (node != null)
            {
                Services.AddAfter(node.Previous, (forType, name, value));
            }
            else if (forType == null)
            {
                Services.AddAfter(_services.First, (forType, name, value));
            }
            else
            {
                Services.AddAfter(_firstForTypeService, (forType, name, value));
            }
            return this;
        }
    }
}
