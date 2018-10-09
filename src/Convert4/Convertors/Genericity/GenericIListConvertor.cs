using blqw.ConvertServices;
using blqw.DI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace blqw.Convertors
{
    sealed class GenericIListConvertor : BaseConvertor<IList<object>>
    {
        public override Type OutputType => typeof(IList<>);

        public override IConvertor GetConvertor(Type outputType)
        {
            var genericArgs = outputType.GetGenericArguments(typeof(IList<>));

            if (genericArgs == null)
            {
                //如果无法得道泛型参数, 判断output是否与 List<object> 兼容, 如果是返回 List<object> 的转换器
                if (outputType.IsAssignableFrom(typeof(List<object>)))
                {
                    return new InnerConvertor<List<object>, object>(this);
                }
                return null;
            }
            var args = new Type[genericArgs.Length + 1];
            args[0] = outputType;
            genericArgs.CopyTo(args, 1);
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<,>).MakeGenericType(args), this);
        }

        class InnerConvertor<TList, TValue> : BaseConvertor<TList>
                                            , IFrom<string, TList>
                                            , IFrom<IEnumerator, TList>
            where TList : class, IList<TValue>
        {
            static readonly char[] _separator = new[] { ',' };
            private readonly GenericIListConvertor _parent;

            public InnerConvertor(GenericIListConvertor parent) => _parent = parent;

            public TList From(ConvertContext context, string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    var list = context.CreateInstance<List<TValue>>(typeof(TList));
                }
                var separator = context.GetStringSeparators();

                var arr = separator is string[] s
                            ? input.Split(s, context.GetStringSplitOptions())
                            : input.Split(separator as char[] ?? _separator, context.GetStringSplitOptions());

                return From(context, arr.GetEnumerator());
            }

            public TList From(ConvertContext context, IEnumerator input)
            {
                if (input == null)
                {
                    return null;
                }
                var list = (TList)context.CreateInstance<List<TValue>>(typeof(TList));
                while (input.MoveNext())
                {
                    var result = context.Convert<TValue>(input.Current);
                    if (!result.Success)
                    {
                        return null;
                    }
                    list.Add(result.OutputValue);
                }
                return list;
            }

            public override IConvertor GetConvertor(Type outputType) => _parent.GetConvertor(outputType);
        }

    }
}
