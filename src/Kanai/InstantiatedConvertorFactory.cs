using blqw.Kanai;
using System;

namespace blqw.Convertors
{
    public class InstantiatedConvertorFactory : IConvertorFactory
    {
        private readonly object _convertor;

        public InstantiatedConvertorFactory(object convertor) => _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
        public IConvertor<T> Build<T>() => _convertor as IConvertor<T>;
        public bool CanBuild<T>() => _convertor is IConvertor<T>;
    }
}
