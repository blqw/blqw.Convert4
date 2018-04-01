using blqw.ConvertServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace blqw
{
    /// <summary>
    /// 转换器设置参数
    /// </summary>
    public sealed class ConvertSettings : IServiceProvider
    {
        public static readonly ConvertSettings Global = new ConvertSettings();

        /// <summary>
        /// 标准服务
        /// </summary>
        private IList<(Type type, string name, object service)> _services;

        private int _boundary = 0;

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
            if (_services == null)
            {
                return null;
            }
            var (i, s) = Get(null, serviceType);
            if (i >= 0)
            {
                return s;
            }
            return null;
        }

        /// <summary>
        /// 获取标准服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetOwnService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            if (serviceType == typeof(ConvertSettings))
            {
                return this;
            }
            if (_services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetOwnService(serviceType);
            }
            var (i, s) = Get(null, serviceType);
            if (i >= 0)
            {
                return s;
            }
            return ReferenceEquals(Global, this) ? null : Global.GetOwnService(serviceType);
        }

        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public object GetNamedService(string name)
        {
            if (name == null || _services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetNamedService(name);
            }
            var (i, s) = Get(null, name);
            if (i >= 0)
            {
                return s;
            }
            return ReferenceEquals(Global, this) ? null : Global.GetNamedService(name);
        }

        /// <summary>
        /// 获取标准服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetService<TService>()
        {
            if (typeof(TService) == typeof(ConvertSettings))
            {
                return this;
            }
            if (_services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetService<TService>();
            }
            var (i, s) = Get<TService>(null);
            if (i >= 0)
            {
                return s;
            }
            return ReferenceEquals(Global, this) ? null : Global.GetService<TService>();
        }

        /// <summary>
        /// 获取类型专属的标准服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetServiceForType(Type forType, Type serviceType)
        {
            if (serviceType == null || _services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetServiceForType(forType, serviceType);
            }
            var (i, s) = Get(forType, serviceType);
            if (i >= 0)
            {
                return s;
            }
            (i, s) = Get(null, serviceType);
            if (i >= 0)
            {
                return s;
            }
            return ReferenceEquals(Global, this) ? null : Global.GetServiceForType(forType, serviceType);
        }

        /// <summary>
        /// 获取类型专属的命名服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public object GetNamedServiceForType(Type forType, string name)
        {
            if (name == null || _services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetNamedServiceForType(forType, name);
            }
            var (i, s) = Get(forType, name);
            if (i >= 0)
            {
                return s;
            }
            (i, s) = Get(null, name);
            if (i >= 0)
            {
                return s;
            }
            return ReferenceEquals(Global, this) ? null : Global.GetNamedServiceForType(forType, name);
        }

        /// <summary>
        /// 获取类型专属的标准服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetServiceForType<TService>(Type forType)
        {
            if (_services == null)
            {
                return ReferenceEquals(Global, this) ? null : Global.GetServiceForType<TService>(forType);
            }
            var (i, s) = Get<TService>(forType);
            if (i >= 0)
            {
                return s;
            }
            (i, s) = Get<TService>(null);
            if (i >= 0)
            {
                return s;
            }
            return ReferenceEquals(Global, this) ? null : Global.GetServiceForType<TService>(forType);
        }

        private (int, object) Get(Type forType, string name)
        {
            var start = 0;
            var end = _boundary;
            if (forType == null)
            {
                start = _boundary;
                end = _services.Count;
            }

            for (var i = start; i < end; i++)
            {
                var (t, n, v) = _services[i];
                if (forType == t && name == n)
                {
                    return (i, v);
                }
            }
            return (-1, null);
        }

        private (int, object) Get<T>(Type forType)
        {
            var start = 0;
            var end = _boundary;
            if (forType == null)
            {
                start = _boundary;
                end = _services.Count;
            }

            for (var i = start; i < end; i++)
            {
                var (t, n, v) = _services[i];
                if (forType == t && n == null && v is T)
                {
                    return (i, v);
                }
            }
            return (-1, null);
        }

        private (int, object) Get(Type forType, Type serviceType)
        {
            var start = 0;
            var end = _boundary;
            if (forType == null)
            {
                start = _boundary;
                end = _services.Count;
            }

            for (var i = start; i < end; i++)
            {
                var (t, n, v) = _services[i];
                if (forType == t && n == null && serviceType.IsInstanceOfType(v))
                {
                    return (i, v);
                }
            }
            return (-1, null);
        }

        /// <summary>
        /// 添加标准服务
        /// </summary>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddService<TService>(TService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (_services == null)
            {
                _services = new List<(Type, string, object)>() { (null, null, service) };
                return this;
            }
            var (i, s) = Get(null, typeof(TService));
            if (i <= 0)
            {
                _services.Add((null, null, service));
            }
            else
            {
                _services[i] = (null, null, service);
            }
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
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (_services == null)
            {
                _services = new List<(Type, string, object)>() { (null, name, service) };
                return this;
            }
            var (i, s) = Get(null, name);
            if (i <= 0)
            {
                _services.Add((null, name, service));
            }
            else
            {
                _services[i] = (null, name, service);
            }
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
            if (forType == null)
            {
                throw new ArgumentNullException(nameof(forType));
            }
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (_services == null)
            {
                _services = new List<(Type, string, object)>() { (forType, null, service) };
                _boundary++;
                return this;
            }
            var (i, s) = Get(forType, typeof(TService));
            if (i <= 0)
            {
                _services.Add((forType, null, service));
                _boundary++;

            }
            else
            {
                _services[i] = (forType, null, service);
            }
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
            if (forType == null)
            {
                throw new ArgumentNullException(nameof(forType));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (_services == null)
            {
                _services = new List<(Type, string, object)>() { (forType, name, service) };
                _boundary++;
                return this;
            }
            var (i, s) = Get(forType, name);
            if (i <= 0)
            {
                _services.Add((forType, name, service));
                _boundary++;
            }
            else
            {
                _services[i] = (forType, name, service);
            }
            return this;
        }
    }
}
