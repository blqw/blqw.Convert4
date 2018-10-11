using blqw.ConvertServices;
using blqw.DI;
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


        public override IConvertor GetConvertor(Type outputType) =>
            outputType == OutputType ? this : new IListConvertor(outputType).Proxy(outputType);

        static readonly char[] _separator = new[] { ',' };

        public IListConvertor()
        {
        }

        public IListConvertor(Type outputType) : base(outputType)
        {
        }

        public ConvertResult<IList> From(ConvertContext context, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Array.Empty<object>();
            }

            var separator = context.GetStringSeparators();

            var arr = separator is string[] s
                        ? input.Split(s, context.GetStringSplitOptions())
                        : input.Split(separator as char[] ?? _separator, context.GetStringSplitOptions());

            return From(context, arr.GetEnumerator());
        }

        public ConvertResult<IList> From(ConvertContext context, IEnumerator input)
        {
            if (input is null)
            {
                return default;
            }
            var list = (IList)context.CreateInstance<ArrayList>(OutputType);
            while (input.MoveNext())
            {
                var result = context.Convert<object>(input.Current);
                if (!result.Success)
                {
                    return result.Exception + context.InvalidOperationException($"{OutputType.GetFriendlyName()} {"填充元素失败"}");
                }
                list.Add(input.Current);
            }
            return Result(list);
        }
    }
}
