using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    public class InstantiatedConvertorFactory : IConvertorFactory
    {
        private readonly IConvertor _convertor;

        public InstantiatedConvertorFactory(IConvertor convertor) => _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
        public IConvertor Build(Type type) => CanBuild(type) ? _convertor : null;
        public bool CanBuild(Type type) => _convertor.OutputType?.IsAssignableFrom(type) == true;
    }
}
