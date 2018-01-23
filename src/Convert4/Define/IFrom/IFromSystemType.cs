using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public interface IFromSystemType<T>
        : IFrom<T, Guid>
        , IFrom<T, TimeSpan>
        , IFrom<T, Uri>
        , IFrom<T, Type>
        , IFrom<T, IntPtr>
        , IFrom<T, UIntPtr>
    {

    }
}
