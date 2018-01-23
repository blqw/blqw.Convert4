using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public interface IFrom<TOutput, TInput>
    {
        TOutput From(ConvertContext context, TInput input);
    }
}
