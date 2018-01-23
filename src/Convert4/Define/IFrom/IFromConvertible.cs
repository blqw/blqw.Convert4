using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public interface IFromConvertible<T>
        : IFrom<T, bool>
        , IFrom<T, char>
        , IFrom<T, sbyte>
        , IFrom<T, byte>
        , IFrom<T, short>
        , IFrom<T, ushort>
        , IFrom<T, int>
        , IFrom<T, uint>
        , IFrom<T, long>
        , IFrom<T, ulong>
        , IFrom<T, float>
        , IFrom<T, double>
        , IFrom<T, decimal>
        , IFrom<T, DateTime>
        , IFrom<T, string>
    {
    }
}
