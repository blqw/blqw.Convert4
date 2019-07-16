using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Kanai.Convertors
{
    class Int32Convertor : IConvertor<int>
    {
        public uint Priority => 100;

        public ConvertResult<int> ChangeType(ConvertContext context, object input)
        {
            throw new NotImplementedException();
        }
    }
}
