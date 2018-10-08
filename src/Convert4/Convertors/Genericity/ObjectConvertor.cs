using blqw.ConvertServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class ObjectConvertor : BaseConvertor<object>
    {
        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType == null)
            {
                throw new ArgumentNullException(nameof(outputType));
            }

            var args = new Type[] { outputType };
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<>).MakeGenericType(args));
        }

        class InnerConvertor<T> : BaseConvertor<T>
                                , IFrom<string, T>
        {
            public T From(ConvertContext context, string input)
            {
                var serializer = context.GetSerializer();
                if (serializer == null)
                {
                    context.InvalidOperationException($"缺少序列化服务");
                    return default;
                }
                return serializer.ToObject<T>(input);
            }
        }
    }
}
