using blqw.ConvertServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class ArrayConvertor : BaseConvertor<Array>
    {
        public override Type OutputType => typeof(Array);

        public override IConvertor GetConvertor(Type outputType) =>
            (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<>).MakeGenericType(outputType.GetElementType()));

        class InnerConvertor<T> : BaseConvertor<T[]>,
                                  IFrom<string, T[]>,
                                  IFrom<IEnumerator, T[]>
        {
            static readonly char[] _separator = new[] { ',' };

            public T[] From(ConvertContext context, string input)
            {
                var separator = context.GetStringSeparators();

                var arr = separator is string[] s
                            ? input.Split(s, context.GetStringSplitOptions())
                            : input.Split(separator as char[] ?? _separator, context.GetStringSplitOptions());

                return From(context, arr.GetEnumerator());
            }

            public T[] From(ConvertContext context, IEnumerator input)
            {
                if (input == null)
                {
                    return null;
                }
                var result = context.ChangeType<List<T>>(input.Current);
                if (!result.Success)
                {
                    context.Exception = context.InvalidCastException(input, TypeFriendlyName) + result.Error;
                    return null;
                }
                return result.OutputValue.ToArray();
            }
        }
    }
}
