namespace blqw.ConvertServices
{
    /// <summary>
    /// 命名服务提供程序
    /// </summary>
    public interface INamedServiceProvider
    {
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        object GetService(string serviceName);
        /// <summary>
        /// 添加命名服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="service"></param>
        void AddService(string name, object service);
    }
}
