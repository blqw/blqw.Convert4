using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Kanai.Convertors
{
    class Int32Convertor : BaseConvertor<int>, IFrom<string, int>
    {
        public ConvertResult<int> From(ConvertContext context, string input) => throw new NotImplementedException();
    }
}
