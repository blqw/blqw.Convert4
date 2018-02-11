using blqw.ConvertServices;
using System;
using System.Collections;
using System.Data;
using System.Net;
using System.Text;

namespace blqw.Convertors
{
    class StringConvertot : BaseConvertor<string>,
        IFrom<string, Type>,
        IFrom<string, byte[]>,
        IFrom<string, bool>,
        IFrom<string, DataRow>,
        IFrom<string, DataTable>,
        IFrom<string, Uri>,
        IFrom<string, StringBuilder>,
        IFrom<string, IPAddress>,
        IFrom<string, object>,
        IFrom<string, IFormattable>,
        IFrom<string, IConvertible>,
        IFrom<string, IDataReader>,
        IFrom<string, IDataRecord>,
        IFrom<string, IEnumerator>,
        IFrom<string, IEnumerable>,
        IFromNull<string>
    {
        public string From(ConvertContext context, Type input) =>
            input.GetFriendlyName();
        public string FromNull(ConvertContext context) => null;

        public string FromDBNull(ConvertContext context) => null;

        public string From(ConvertContext context, IConvertible input) =>
            input?.ToString(context.GetFormatProvider(input?.GetType()));

        public string From(ConvertContext context, IFormattable input) =>
            input?.ToString(context.GetFormat(input.GetType()), context.GetFormatProvider(input?.GetType()));

        public string From(ConvertContext context, byte[] input) =>
            context.GetEncoding().GetString(input);
        public string From(ConvertContext context, bool input) => input ? "true" : "false";

        private string ToString(ConvertContext context, object input) =>
            context.GetSerialization()?.ToString(context, input) ?? input?.ToString();

        public string From(ConvertContext context, IDataReader input) => ToString(context, input);

        public string From(ConvertContext context, IDataRecord input) => ToString(context, input);

        public string From(ConvertContext context, DataRow input) => ToString(context, input);

        public string From(ConvertContext context, DataTable input) => ToString(context, input);

        public string From(ConvertContext context, IEnumerable input) =>
            context.GetSerialization()?.ToString(context, input) ?? From(context, input?.GetEnumerator());

        public string From(ConvertContext context, object input)
        {
            if (input.GetType().GetProperties().Length > 0)
            {
                return input?.ToString();
            }
            return input.ToString();
        }

        public string From(ConvertContext context, Uri input) => input?.ToString();
        public string From(ConvertContext context, StringBuilder input) => input?.ToString();
        public string From(ConvertContext context, IEnumerator input)
        {
            if(input?.MoveNext() ?? false)
            {
                return null;
            }
            var s = context.ChangeType<string>(input.Current);
            if (!s.Success)
            {
                context.Exception = context.InvalidCastException(input, TypeFriendlyName) + s.Error;
                return null;
            }
            var separator = context.GetStringSeparator() ?? ",";
            var sb = new StringBuilder(s.OutputValue);
            do
            {
                sb.Append(separator);
                s = context.ChangeType<string>(input.Current);
                if (!s.Success)
                {
                    context.Exception = context.InvalidCastException(input, TypeFriendlyName) + s.Error;
                    return null;
                }
            } while (input.MoveNext());

            return sb.ToString() ;
        }

        public string From(ConvertContext context, IPAddress input) => input.ToString();
    }
}
