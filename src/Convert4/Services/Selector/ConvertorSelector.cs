using blqw.Define;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public sealed class ConvertorSelector : IConvertorSelector
    {
        private IServiceProvider provider;

        public ConvertorSelector(IServiceProvider provider) => this.provider = provider;
    }
}
