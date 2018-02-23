using System;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 专属服务提供程序引擎
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDedicatedServiceProviderEngine
    {
        /// <summary>
        /// 获取专属服务提供程序
        /// </summary>
        /// <param name="target"> 专属服务对象 </param>
        /// <returns></returns>
        IServiceProvider GetServiceProvider(object target);
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="target"> 专属服务对象 </param>
        /// <param name="serviceType"></param>
        /// <param name="service"></param>
        void AddService(object target, Type serviceType, object service);
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        object GetServiceProviderService();
    }
}
