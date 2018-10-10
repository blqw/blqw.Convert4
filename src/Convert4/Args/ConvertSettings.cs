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

        private Dictionary<(Type, string), object> _services;


        public object DefaultValue { get; set; } = _unSet;

        public bool Throwable => ReferenceEquals(DefaultValue, _unSet);

        /// <summary>
        /// 获取设置
        /// </summary>
        public T Get<T>(Type forType, string name)
        {
            var value = Get(forType, name ?? typeof(T).GetFriendlyName())
                     ?? Get(null, name ?? typeof(T).GetFriendlyName());
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
        private object Get(Type forType, string name) => _services != null && _services.TryGetValue((forType, name), out var value) ? value : null;


        public ConvertSettings Set<T>(object value, Type forType = null) =>
            Set(typeof(T).GetFriendlyName(), value, forType);

        public ConvertSettings Set(string name, object value, Type forType = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                _services?.Remove((forType, name));
            }
            else if (_services == null)
            {
                _services = new Dictionary<(Type, string), object>()
                {
                    [(forType, name)] = value
                };
            }
            else
            {
                _services[(forType, name)] = value;
            }
            return this;
        }
    }
}
