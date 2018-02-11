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
    public sealed class ConvertSettings : IServiceProvider, INamedServiceProvider, IForTypeProvider
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
            if (serviceType.IsInstanceOfType(this))
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
        public ConvertSettings AddForType<T>(object service) =>
            AddForType(typeof(T), service);
        /// <summary>
        /// 添加提供给指定类型的命名服务
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="name">服务名称</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddForType<T>(string name, object service) =>
            AddForType(typeof(T), name, service);
        /// <summary>
        /// 添加提供给指定类型的标准服务
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <param name="service">服务实例</param>
        /// <returns></returns>
        public ConvertSettings AddForType(Type forType, object service)
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
        public ConvertSettings AddForType(Type forType, string name, object service)
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

        /// <summary>
        /// 获取提供指定类型的服务提供程序
        /// </summary>
        /// <param name="forType">指定类型</param>
        public IServiceProvider GetServiceProvider(Type forType) =>
            _specialServices?[forType] as IServiceProvider;

        /// <summary>
        /// 获取提供指定类型服务提供程序
        /// </summary>
        /// <param name="forType">指定类型</param>
        public INamedServiceProvider GetNamedServiceProvider(Type forType) =>
            _specialServices?[forType] as INamedServiceProvider;

    }
}
