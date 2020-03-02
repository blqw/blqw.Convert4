using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;

namespace blqw.Kanai.Dynamic
{
    public static class DynamicFactory
    {
        public static dynamic Create(object value, ConvertSettings settings)
        {
            switch (value)
            {
                case null:
                    return DynamicPrimitive.Null;
                case IDynamicMetaObjectProvider dyc:
                    return dyc;
                case string str:
                    return new DynamicPrimitive(str, settings);
                case DataRow row:
                    return new DynamicDataRow(row, settings);
                case DataRowView view:
                    return new DynamicDataRow(view, settings);
                case NameValueCollection nv:
                    return new DynamicNameValueCollection(nv, settings);
                case IDataReader reader:
                    return new DynamicDictionary(reader.To<IDictionary>(), settings);
                case IDictionary dict:
                    return new DynamicDictionary(dict, settings);
                case IList list:
                    return new DynamicList(list, settings);
                case IEnumerator e1:
                    return new DynamicEnumerator(e1, settings);
                case IEnumerable e2:
                    return new DynamicEnumerator(e2.GetEnumerator(), settings);
            }
            if ("System".Equals(value.GetType().Namespace, StringComparison.Ordinal))
            {
                return new DynamicPrimitive(value, settings);
            }
            return new DynamicEntity(value, settings);
        }
    }
}
