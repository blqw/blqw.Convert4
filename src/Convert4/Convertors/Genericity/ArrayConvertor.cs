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

        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType.IsInterface)
            {
                return new InnerConvertor<object>(this);
            }
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<>).MakeGenericType(outputType.GetElementType()), this);
        }

        class InnerConvertor<T> : BaseConvertor<T[]>,
                                  IFrom<string, T[]>,
                                  IFrom<IEnumerator, T[]>
        {
            static readonly char[] _separator = new[] { ',' };
            private readonly ArrayConvertor _parent;

            public InnerConvertor(ArrayConvertor parent) => _parent = parent;

            public override IConvertor GetConvertor(Type outputType) => _parent.GetConvertor(outputType);

            public T[] From(ConvertContext context, string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return Array.Empty<T>();
                }

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
                var result = context.ChangeType<List<T>>(input);
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
