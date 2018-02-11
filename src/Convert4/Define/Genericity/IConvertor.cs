namespace blqw
{
    /// <summary>
    /// 泛型转换器接口
    /// </summary>
    public interface IConvertor<T> : IConvertor
    {
        /// <summary>
        /// 返回指定类型的对象，其值等效于指定对象。
        /// </summary>
        /// <param name="context"> 上下文 </param>
        /// <param name="input"> 需要转换类型的对象 </param>
        new ConvertResult<T> ChangeType (ConvertContext context, object input);

        /// <summary>
        /// 获取泛型子转换器
        /// </summary>
        /// <returns></returns>
        IConvertor<T1> GetConvertor<T1>();
    }
}