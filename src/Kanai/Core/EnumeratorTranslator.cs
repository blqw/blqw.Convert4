using blqw.Kanai.Interface;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace blqw.Kanai
{
    class EnumeratorTranslator : ITranslator
    {

        public bool CanTranslate(Type type) =>
            typeof(IEnumerable).IsAssignableFrom(type)
                || typeof(DataTable) == type
                || typeof(DataRow) == type
                || typeof(DataRowView) == type
                || typeof(IListSource) == type
                || type.IsArray;

        public object Translate(ConvertContext context, object input)
            => (input as IEnumerable)?.GetEnumerator()
                            ?? input as IEnumerator
                            ?? (input as DataTable)?.Rows?.GetEnumerator()
                            ?? (input as DataRow)?.ItemArray?.GetEnumerator()
                            ?? (input as DataRowView)?.Row?.ItemArray?.GetEnumerator()
                            ?? (input as IListSource)?.GetList()?.GetEnumerator()
                            ?? new object[] { input }.GetEnumerator();

    }
}
