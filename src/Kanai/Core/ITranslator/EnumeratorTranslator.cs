using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace blqw.Kanai
{
    class EnumeratorTranslator : ITranslator
    {
        public Type InputType { get; } = typeof(IEnumerator);

        public object Translate(object input)
            => (input as IEnumerable)?.GetEnumerator()
                            ?? input as IEnumerator
                            ?? (input as DataTable)?.Rows?.GetEnumerator()
                            ?? (input as DataRow)?.ItemArray?.GetEnumerator()
                            ?? (input as DataRowView)?.Row?.ItemArray?.GetEnumerator()
                            ?? (input as IListSource)?.GetList()?.GetEnumerator()
                            ?? new object[] { input }.GetEnumerator();
    }
}
