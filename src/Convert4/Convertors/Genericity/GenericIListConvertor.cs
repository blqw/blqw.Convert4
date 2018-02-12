using blqw.ConvertServices;
using System;
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
                    return new InnerConvertor<List<object>, object>();
                }
                return null;
            }
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
                var list = OutputType.IsInterface ? new List<TValue>() : (IList<TValue>)CreateOutputInstance(OutputType);
                var separator = context.GetStringSeparators();

                var arr = separator is string[] s
                            ? input.Split(s, context.GetStringSplitOptions())
                            : input.Split(separator as char[] ?? _separator, context.GetStringSplitOptions());

                foreach (var item in arr)
                {
                    var result = context.ChangeType<TValue>(item);
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
