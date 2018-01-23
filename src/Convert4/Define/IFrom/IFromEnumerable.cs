using System.Collections.Specialized;
using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
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
