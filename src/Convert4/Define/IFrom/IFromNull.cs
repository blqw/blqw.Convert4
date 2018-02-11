namespace blqw
{
    /// <summary>
    /// 处理 <seealso cref="null"/> 和 <seealso cref="DBNull"/> 转换的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromNull<T>
    {
        /// <summary>
        /// 将 <seealso cref="null"/> 转为指定类型
        /// </summary>
        /// <param name="context">转换上下文</param>
        T FromNull(ConvertContext context);

        /// <summary>
        /// 将 <seealso cref="DBNull"/> 转为指定类型
        /// </summary>
        /// <param name="context">转换上下文</param>
        T FromDBNull(ConvertContext context);
    }
}
