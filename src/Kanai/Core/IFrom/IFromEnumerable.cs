using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace blqw.Kanai.Froms
{
    /// <summary>
    /// 处理枚举类型的转换接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromEnumerable<T>
        : IFrom<IList, T>
        , IFrom<DataRow, T>
        , IFrom<DataTable, T>
        , IFrom<IDictionary, T>
        , IFrom<NameObjectCollectionBase, T>
        , IFrom<StringDictionary, T>
        , IFrom<Array, T>
    {

    }
}
