using System;

namespace blqw.Kanai.Factories
{
    public sealed class InstantiatedConvertorFactory<outputT> : IConvertorFactory
    {
        private readonly IConvertor<outputT> _convertor;

        public InstantiatedConvertorFactory(IConvertor<outputT> convertor) => 
            _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));

        public IConvertor<T> Build<T>()
        {
            if (!CanBuild<T>())
            {
                throw new NotSupportedException(SR.CANT_BUILD_CONVERTOR.Localize(null, typeof(T).GetFriendlyName()));
            }
            return (IConvertor<T>)_convertor;
        }

        public bool CanBuild<T>() =>
            typeof(outputT).IsAssignableFrom(typeof(T));
    }
}
