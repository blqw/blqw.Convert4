using System;

namespace blqw
{
    /// <summary>
    /// 处理 <seealso cref="IConvertible"/> 类型的转换接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromConvertible<T>
        : IFrom<T, bool>
        , IFrom<T, char>
        , IFrom<T, sbyte>
        , IFrom<T, byte>
        , IFrom<T, short>
        , IFrom<T, ushort>
        , IFrom<T, int>
        , IFrom<T, uint>
        , IFrom<T, long>
        , IFrom<T, ulong>
        , IFrom<T, float>
        , IFrom<T, double>
        , IFrom<T, decimal>
        , IFrom<T, DateTime>
        , IFrom<T, string>
    {
    }
}
