using blqw.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    sealed class IListTConvertor : BaseConvertor<IList<object>>
    {
        public override Type OutputType => typeof(IList<>);

        public override IConvertor GetConvertor(Type outputType)
        {
            var genericArgs = outputType.GetGenericArguments(typeof(IList<>));
            var args = new Type[genericArgs.Length + 1];
            args[0] = outputType;
            genericArgs.CopyTo(args, 1);
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<,>).MakeGenericType(args));
        }

        class InnerConvertor<TList, TValue> : BaseConvertor<TList>, IFrom<TList, string>
            where TList : class, IList<TValue>
        {
            static readonly char[] _separator = new[] { ',' };
            public TList From(ConvertContext context, string input)
            {
                var list = CreateOutput();
                var separator = context.GetStringSeparator();

                var arr = separator is string[] s
                            ? input.Split(s, context.GetStringSplitOptions())
                            : input.Split(separator as char[] ?? _separator, context.GetStringSplitOptions());

                foreach (var item in arr)
                {
                    var result = context.ChangeType<TValue>(item);
                    if (!result.Success)
                    {
                        context.Exception = context.Error(input, TypeFriendlyName) + result.Error;
                        return null;
                    }
                    list.Add(result.OutputValue);
                }

                return list;
            }
        }

    }
}
