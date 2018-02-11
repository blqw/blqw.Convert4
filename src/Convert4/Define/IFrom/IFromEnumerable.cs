using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace blqw
{
    /// <summary>
    /// 处理枚举类型的转换接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromEnumerable<T>
        : IFrom<T, IList>
        , IFrom<T, DataRow>
        , IFrom<T, DataTable>
        , IFrom<T, IDictionary>
        , IFrom<T, NameObjectCollectionBase>
        , IFrom<T, StringDictionary>
        , IFrom<T, Array>
    {

    }
}
