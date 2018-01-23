using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public interface IFromNull<T>
    {
        T FromNull(ConvertContext context);
        T FromDBNull(ConvertContext context);
    }
}
