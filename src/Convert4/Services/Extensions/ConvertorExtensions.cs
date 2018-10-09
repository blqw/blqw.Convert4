using blqw.Convertors;
using System;

namespace blqw.ConvertServices
{
    public static class ConvertorExtensions
    {
        public static IConvertor Proxy(this IConvertor convertor, Type outputType) =>
            (IConvertor)Activator.CreateInstance(typeof(ProxyConvertor<>).MakeGenericType(outputType), convertor);

    }
}
