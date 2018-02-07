using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    [Serializable]
    public enum TypeMark
    {
        Empty = 0,
        Object = 1,
        DBNull = 2,
        Boolean = 3,
        Char = 4,
        SByte = 5,
        Byte = 6,
        Int16 = 7,
        UInt16 = 8,
        Int32 = 9,
        UInt32 = 10,
        Int64 = 11,
        UInt64 = 12,
        Single = 13,
        Double = 14,
        Decimal = 15,
        DateTime = 16,
        String = 18,

        Guid = 10001,
        TimeSpan = 10002,
        IntPtr = 10003,
        UIntPtr = 10004,
        Uri = 10005,
        Type = 10006,
        Enum = 10007,
        Array = 10008,
        DataRow = 10009,
        DataTable = 10010,
        NameObjectCollectionBase = 10011,
        StringDictionary = 10012,
    }
}
