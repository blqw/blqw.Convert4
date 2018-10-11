﻿using blqw.ConvertServices;
using System;
using System.Collections;
using System.Data;
using System.Net;
using System.Text;

namespace blqw.Convertors
{
    class StringConvertor : BaseConvertor<string>,
        IFrom<Type, string>,
        IFrom<byte[], string>,
        IFrom<bool, string>,
        IFrom<DataRow, string>,
        IFrom<DataTable, string>,
        IFrom<Uri, string>,
        IFrom<StringBuilder, string>,
        IFrom<IPAddress, string>,
        IFrom<object, string>,
        IFrom<IFormattable, string>,
        IFrom<IConvertible, string>,
        IFrom<IDataReader, string>,
        IFrom<IDataRecord, string>,
        IFrom<IEnumerator, string>,
        IFrom<IEnumerable, string>,
        IFromNull<string>
    {
        public ConvertResult<string> From(ConvertContext context, Type input) =>
            input.GetFriendlyName();
        public ConvertResult<string> FromNull(ConvertContext context) => default;

        public ConvertResult<string> FromDBNull(ConvertContext context) => default;

        public ConvertResult<string> From(ConvertContext context, IConvertible input) =>
            input?.ToString(context.GetFormatProvider(input?.GetType()));

        public ConvertResult<string> From(ConvertContext context, IFormattable input) =>
            input?.ToString(context.GetFormat(input.GetType()), context.GetFormatProvider(input.GetType()));

        public ConvertResult<string> From(ConvertContext context, byte[] input) =>
            context.GetEncoding().GetString(input);
        public ConvertResult<string> From(ConvertContext context, bool input) => input ? "true" : "false";

        private string ToString(ConvertContext context, object input) =>
            context.GetSerializer()?.ToString(input) ?? input?.ToString();

        public ConvertResult<string> From(ConvertContext context, IDataReader input) => ToString(context, input);

        public ConvertResult<string> From(ConvertContext context, IDataRecord input) => ToString(context, input);

        public ConvertResult<string> From(ConvertContext context, DataRow input) => ToString(context, input);

        public ConvertResult<string> From(ConvertContext context, DataTable input) => ToString(context, input);

        public ConvertResult<string> From(ConvertContext context, IEnumerable input) =>
            context.GetSerializer()?.ToString(input) ?? From(context, input?.GetEnumerator());

        public ConvertResult<string> From(ConvertContext context, object input)
        {
            if (input.GetType().GetProperties().Length > 0)
            {
                return input?.ToString();
            }
            return input.ToString();
        }

        public ConvertResult<string> From(ConvertContext context, Uri input) => input?.ToString();
        public ConvertResult<string> From(ConvertContext context, StringBuilder input) => input?.ToString();
        public ConvertResult<string> From(ConvertContext context, IEnumerator input)
        {
            if(input?.MoveNext() ?? false)
            {
                return default;
            }
            var s = context.Convert<string>(input.Current);
            if (!s.Success)
            {
                return s;
            }
            var separator = context.GetStringSeparator() ?? ",";
            var sb = new StringBuilder(s.OutputValue);
            do
            {
                sb.Append(separator);
                s = context.Convert<string>(input.Current);
                if (!s.Success)
                {
                    return s;
                }
            } while (input.MoveNext());

            return sb.ToString() ;
        }

        public ConvertResult<string> From(ConvertContext context, IPAddress input) => input.ToString();
    }
}
