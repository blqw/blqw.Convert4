using System;
using System.Collections.Generic;

namespace blqw.Kanai.Core
{
    class FormatProviderSelector
    {
        Dictionary<Type, IFormatProvider> _items = new Dictionary<Type, IFormatProvider>();

        public FormatProviderSelector(IFormatProvider defaultFormatProvider) => DefaultFormatProvider = defaultFormatProvider;

        public IFormatProvider DefaultFormatProvider { get; }

        public IFormatProvider Get(Type type)
            => _items.TryGetValue(type, out var value) ? value : DefaultFormatProvider;

    }
}
