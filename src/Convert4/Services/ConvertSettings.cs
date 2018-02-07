using System.Collections.Specialized;
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
        /// <summary>
        /// 标准服务
        /// </summary>
        private IList<object> _services;
        /// <summary>
        /// 特别服务
        /// </summary>
        private IDictionary _specialServices;
        /// <summary>
        /// 获取提供给指定类型的服务设置
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <returns></returns>
        private ConvertSettings GetForTypeSettings(Type forType) =>
            _specialServices?[forType] as ConvertSettings;
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
            return _services?.FirstOrDefault(serviceType.IsInstanceOfType);
        }
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public object GetService(string serviceName) =>
                    _specialServices?[serviceName];
        /// <summary>
        /// 获取提供给指定类型的标准服务
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetServiceForType<T>(Type serviceType) =>
            GetServiceForType(typeof(T), serviceType);
        /// <summary>
        /// 获取提供给指定类型的命名服务
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public object GetServiceForType<T>(string serviceName) =>
                    GetForTypeSettings(typeof(T))?.GetService(serviceName);
        /// <summary>
        /// 获取提供给指定类型的标准服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetServiceForType(Type forType, Type serviceType)
        {
            if (serviceType == null || forType == null)
            {
                return null;
            }
            if (serviceType == typeof(ConvertSettings))
            {
                return this;
            }
            return GetForTypeSettings(forType)?.GetService(serviceType);
        }
        /// <summary>
        /// 获取提供给指定类型的命名服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public object GetServiceForType(Type forType, string serviceName) =>
                    GetForTypeSettings(forType)?.GetService(serviceName);
        /// <summary>
        /// 添加标准服务
        /// </summary>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddService(object service)
        {
            if (_services == null)
            {
                _services = new List<object>();
            }
            _services.Add(service);
            return this;
        }
        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddService(string name, object service)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (_specialServices == null)
                {
                    _specialServices = new Hashtable();
                }
                _specialServices[name] = service;
            }
            return this;
        }
        /// <summary>
        /// 添加提供给指定类型的标准服务
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddServiceForType<T>(object service) =>
            AddServiceForType(typeof(T), service);
        /// <summary>
        /// 添加提供给指定类型的命名服务
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddServiceForType<T>(string name, object service) =>
            AddServiceForType(typeof(T), name, service);
        /// <summary>
        /// 添加提供给指定类型的标准服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddServiceForType(Type forType, object service)
        {
            if (service == null)
            {
                return this;
            }
            if (_specialServices == null)
            {
                _specialServices = new Hashtable();
            }
            _specialServices.Add(forType, new ConvertSettings().AddService(service));
            return this;
        }
        /// <summary>
        /// 添加提供给指定类型的命名服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddServiceForType(Type forType, string name, object service)
        {
            if (service == null)
            {
                return this;
            }
            if (_specialServices == null)
            {
                _specialServices = new Hashtable();
            }
            _specialServices.Add(forType, new ConvertSettings().AddService(name, service));
            return this;
        }
    }
}
