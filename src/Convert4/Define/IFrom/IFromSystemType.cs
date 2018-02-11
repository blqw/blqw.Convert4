using System;

namespace blqw
{
    /// <summary>
    /// 处理系统类型转换的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromSystemType<T>
        : IFrom<T, Guid>
        , IFrom<T, TimeSpan>
        , IFrom<T, Uri>
        , IFrom<T, Type>
        , IFrom<T, IntPtr>
        , IFrom<T, UIntPtr>
    {

    }
}
