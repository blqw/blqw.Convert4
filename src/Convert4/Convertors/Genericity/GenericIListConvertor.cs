using blqw.ConvertServices;
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
                return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<,>).MakeGenericType(new Type[] { typeof(List<object>), typeof(object) }));
            }
            var args = new Type[genericArgs.Length + 1];
            args[0] = outputType;
            genericArgs.CopyTo(args, 1);
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<,>).MakeGenericType(args));
        }

        class InnerConvertor<TList, TValue> : BaseConvertor<TList>
                                            , IFrom<string, TList>
                                            , IFrom<IEnumerator, TList>
            where TList : class, IList<TValue>
        {
            static readonly char[] _separator = new[] { ',' };

            public TList From(ConvertContext context, string input)
            {
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
                var list = OutputType.IsInterface ? new List<TValue>() : (IList<TValue>)CreateOutputInstance(OutputType);
                while (input.MoveNext())
                {
                    var result = context.ChangeType<TValue>(input.Current);
                    if (!result.Success)
                    {
                        context.Exception = context.InvalidCastException(input, TypeFriendlyName) + result.Error;
                        return null;
                    }
                    list.Add(result.OutputValue);
                }
                return (TList)list;
            }
        }

    }
}
