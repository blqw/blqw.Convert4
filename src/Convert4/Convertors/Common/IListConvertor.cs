using blqw.ConvertServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class IListConvertor : BaseConvertor<IList>,
                           IFrom<string, IList>,
                           IFrom<IEnumerator, IList>
    {
        static readonly char[] _separator = new[] { ',' };

        public IList From(ConvertContext context, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            var separator = context.GetStringSeparators();

            var arr = separator is string[] s
                        ? input.Split(s, context.GetStringSplitOptions())
                        : input.Split(separator as char[] ?? _separator, context.GetStringSplitOptions());

            return From(context, arr.GetEnumerator());
        }

        public IList From(ConvertContext context, IEnumerator input)
        {
            if (input is null)
            {
                return null;
            }
            var list = new ArrayList();
            while (input.MoveNext())
            {
                var result = context.ChangeType<object>(input.Current);
                if (!result.Success)
                {
                    context.Exception = context.InvalidCastException(input, TypeFriendlyName) + result.Error;
                    return null;
                }
                list.Add(input.Current);
            }
            return list;
        }
    }
}
