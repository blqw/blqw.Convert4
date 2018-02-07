using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Services
{
    interface SerializationService
    {
        string ToString(ConvertContext context, object value);
        object ToObject(ConvertContext context, string value, Type type);
        T ToObject<T>(ConvertContext context, string value);
    }
}
