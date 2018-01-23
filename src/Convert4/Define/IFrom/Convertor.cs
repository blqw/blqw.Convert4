using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{

    public delegate TOutput Convertor<out TOutput, in TInput>(ConvertContext context, TInput input, out bool result);
}
