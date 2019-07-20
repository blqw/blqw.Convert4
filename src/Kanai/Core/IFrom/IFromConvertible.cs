using System;

namespace blqw.Kanai
{
    /// <summary>
    /// 处理 <seealso cref="IConvertible"/> 类型的转换接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromConvertible<T>
        : IFrom<bool, T>
        , IFrom<char, T>
        , IFrom<sbyte, T>
        , IFrom<byte, T>
        , IFrom<short, T>
        , IFrom<ushort, T>
        , IFrom<int, T>
        , IFrom<uint, T>
        , IFrom<long, T>
        , IFrom<ulong, T>
        , IFrom<float, T>
        , IFrom<double, T>
        , IFrom<decimal, T>
        , IFrom<DateTime, T>
        , IFrom<string, T>
    {
    }
}
