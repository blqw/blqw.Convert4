namespace blqw.Kanai.Interface
{
    /// <summary>
    /// 转换器选择器接口
    /// </summary>
    public interface IConvertorSelector
    {
        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <param name="context">转换上下文</param>
        /// <returns></returns>
        IConvertor<T> GetConvertor<T>(ConvertContext context);
    }
}
