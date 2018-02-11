using System;

namespace blqw
{
    /// <summary>
    /// 转换器选择器接口
    /// </summary>
    public interface IConvertorSelector
    {
        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="context">转换上下文</param>
        /// <returns></returns>
        IConvertor<T> Get<T>(ConvertContext context);
        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <param name="outputType">指定类型</param>
        /// <param name="context">转换上下文</param>
        /// <returns></returns>
        IConvertor Get(Type outputType, ConvertContext context);
    }
}
