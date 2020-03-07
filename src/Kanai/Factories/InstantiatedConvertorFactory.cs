using blqw.Kanai.Convertors;
using blqw.Kanai.Interface;
using System;

namespace blqw.Kanai.Factories
{
    public sealed class InstantiatedConvertorFactory<T> : IConvertorFactory
    {
        private readonly IConvertor<T> _convertor;
        public InstantiatedConvertorFactory(IServiceProvider serviceProvider, IConvertor<T> convertor)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
        }

        public IServiceProvider ServiceProvider { get; }

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
            return new ProxyConvertor<T, TOutput>(ServiceProvider, _convertor);
        }

        public bool CanBuild<TOutput>() =>
            typeof(T).IsAssignableFrom(typeof(TOutput));

    }
}
