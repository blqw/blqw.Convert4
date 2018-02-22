namespace blqw
{
    /// <summary>
    /// 处理指定类型的转换接口
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    public interface IFrom<TInput, TOutput>
    {
        TOutput From(ConvertContext context, TInput input);
    }
}
