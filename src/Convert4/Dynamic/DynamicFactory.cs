using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Text;

namespace blqw.Dynamic
{
    public static class DynamicFactory
    {
        public static dynamic Create(object value)
        {
            switch (value)
            {
                case null:
                    return DynamicPrimitive.Null;
                case IDynamicMetaObjectProvider dyc:
                    return dyc;
                case string str:
                    return new DynamicPrimitive(str);
                case DataRow row:
                    return new DynamicDataRow(row);
                case DataRowView view:
                    return new DynamicDataRow(view);
                case NameValueCollection nv:
                    return new DynamicNameValueCollection(nv);
                case IDataReader reader:
                    return new DynamicDictionary(reader.To<IDictionary>());
                case IDictionary dict:
                    return new DynamicDictionary(dict);
                case IList list:
                    return new DynamicList(list);
                case IEnumerator e1:
                    return new DynamicEnumerator(e1);
                case IEnumerable e2:
                    return new DynamicEnumerator(e2.GetEnumerator());
            }
            if ("System".Equals(value.GetType().Namespace, StringComparison.Ordinal))
            {
                return new DynamicPrimitive(value);
            }
            return new DynamicEntity(value);
        }
    }
}
