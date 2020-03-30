namespace blqw.Kanai.Interface
{
    /// <summary>
    /// 转换器接口
    /// </summary>
    public interface IConvertor<T>
    {
        /// <summary>
        /// 优先级
        /// </summary>
        uint Priority { get; }

        /// <summary>
        /// 返回指定类型的对象，其值等效于指定对象。
        /// </summary>
        /// <param name="context"> 上下文 </param>
        /// <param name="input"> 需要转换类型的对象 </param>
        ConvertResult<T> Convert(ConvertContext context, object input);
    }
}
