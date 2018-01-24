using System.Runtime.InteropServices.ComTypes;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;

namespace blqw
{
    public sealed class ConvertorSelector : IConvertorSelector
    {
        private readonly ConcurrentDictionary<Type, IConvertor> _convertors;

        public ConvertorSelector(IEnumerable<IConvertor> convertors)
        {
            _convertors = new ConcurrentDictionary<Type, IConvertor>();
            foreach (var convertor in convertors)
            {
                _convertors.AddOrUpdate(convertor.OutputType, convertor, (t, c) => c.Priority < convertor.Priority ? convertor : c);
            }
        }

        public IConvertor<T> Get<T>(ConvertContext context) =>
            (IConvertor<T>)Get(typeof(T), context);

        public IConvertor Get(Type outputType, ConvertContext context)
        {
            if(_convertors.TryGetValue(outputType,out var conv))
            {
                return conv;
            }
            throw new NotImplementedException();
        }
    }
}
