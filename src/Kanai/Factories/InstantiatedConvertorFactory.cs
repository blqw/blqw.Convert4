using blqw.Kanai.Convertors;
using System;

namespace blqw.Kanai.Factories
{
    public sealed class InstantiatedConvertorFactory<T> : IConvertorFactory
    {
        private readonly IConvertor<T> _convertor;

        public InstantiatedConvertorFactory(IConvertor<T> convertor) =>
            _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));

        public IConvertor<TOutput> Build<TOutput>()
        {
            if (_convertor is IConvertor<TOutput> conv)
            {
                return conv;
            }
            if (!CanBuild<T>())
            {
                return null;
            }
            return new ProxyConvertor<T, TOutput>(_convertor);
        }

        public bool CanBuild<TOutput>() =>
            typeof(T).IsAssignableFrom(typeof(TOutput));
    }
}
