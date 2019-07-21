using blqw.Kanai.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace blqw.Kanai.Convertors
{
    class Int32Convertor2 : IConvertor<int>
    {
        public uint Priority => 99;

        public ConvertResult<int> ChangeType(ConvertContext context, object input)
        {
            if (int.TryParse(input?.ToString(), out var i))
            {
                return new ConvertResult<int>(i);
            }
            return this.Fail(input, context.CultureInfo);
        }
    }
}
