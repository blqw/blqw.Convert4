namespace blqw.Kanai
{
    /// <summary>
    /// 处理指定类型的转换接口
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    public interface IFrom<TInput, TOutput>
    {
        ConvertResult<TOutput> From(ConvertContext context, TInput input);
    }
}
