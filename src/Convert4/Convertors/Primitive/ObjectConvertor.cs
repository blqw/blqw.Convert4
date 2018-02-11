using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class ObjectConvertor : BaseConvertor<object>
    {
        public override ConvertResult<object> ChangeType(ConvertContext context, object input) => new ConvertResult<object>(input);
    }
}
