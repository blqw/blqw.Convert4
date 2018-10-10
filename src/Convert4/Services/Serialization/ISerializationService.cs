using System;

namespace blqw.ConvertServices
{
    /// <summary>
    /// 序列化服务接口
    /// </summary>
    public interface ISerializationService
    {
        /// <summary>
        /// 序列化协议
        /// </summary>
        string Protocol { get; }
        /// <summary>
        /// 对象转为字符串
        /// </summary>
        /// <param name="context">转换上下文</param>
        /// <param name="value">对象实例</param>
        /// <returns></returns>
        string ToString(object value);
        /// <summary>
        /// 字符串转为泛型对象
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="context">转换上下文</param>
        /// <param name="value">对象实例</param>
        /// <returns></returns>
        object ToObject(string value, Type type);
    }
}
