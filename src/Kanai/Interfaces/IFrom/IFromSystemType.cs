using System;

namespace blqw.Kanai.Interface.From
{
    /// <summary>
    /// 处理系统类型转换的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromSystemType<T>
        : IFrom<Guid, T>
        , IFrom<TimeSpan, T>
        , IFrom<Uri, T>
        , IFrom<Type, T>
        , IFrom<IntPtr, T>
        , IFrom<UIntPtr, T>
    {

    }
}
