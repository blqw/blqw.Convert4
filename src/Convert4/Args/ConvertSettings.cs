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
        //public ConvertSettings()
        //{
        //    AddService(new DedicatedServiceProviderEngine<Type>());
        //    AddService(new DedicatedServiceProviderEngine<string>());
        //}
        /// <summary>
        /// 标准服务
        /// </summary>
        private IList<object> _services;

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
            var service = _services?.FirstOrDefault(serviceType.IsInstanceOfType);
            if (service == null)
            {
                if (serviceType.IsGenericType && serviceType.IsConstructedGenericType && serviceType.GetGenericTypeDefinition() == typeof(DedicatedServiceProviderEngine<>))
                {
                    service = Activator.CreateInstance(serviceType);
                    AddService(service);
                    //if (service is IDedicatedServiceProviderEngine engine)
                    //{
                    //    AddService(engine.GetServiceProviderService());
                    //}
                }
            }
            return service;
        }

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
            this.AddNamedService(name, service);
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
            this.AddForTypeService(forType, service);
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
            var provider = this.GetOrAddGetDedicatedServiceProvider(forType, t => new DedicatedServiceProviderEngine<string>());
            provider.AddNamedService(name, service);
            return this;
        }

        /// <summary>
        /// 是否抛出异常
        /// </summary>
        public bool Throwable { get; set; } = true;

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
    }
}
