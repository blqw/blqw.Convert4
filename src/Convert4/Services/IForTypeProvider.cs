using System;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 提供获取指定类型的服务提供程序的接口
    /// </summary>
    public interface IForTypeProvider
    {
        /// <summary>
        /// 获取提供指定类型的服务提供程序
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <returns></returns>
        IServiceProvider GetServiceProvider(Type forType);
        /// <summary>
        /// 获取提供指定类型服务提供程序
        /// </summary>
        /// <param name="forType">指定类型</param>
        /// <returns></returns>
        INamedServiceProvider GetNamedServiceProvider(Type forType);
    }
}
