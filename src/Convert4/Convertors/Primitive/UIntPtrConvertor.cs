using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class UIntPtrConvertor : BaseConvertor<UIntPtr>
                                 , IFrom<uint, UIntPtr>
                                 , IFrom<ulong, UIntPtr>
                                 , IFrom<object, UIntPtr>
    {
        public UIntPtr From(ConvertContext context, ulong input) => new UIntPtr(input);
        public UIntPtr From(ConvertContext context, uint input) => new UIntPtr(input);
        public UIntPtr From(ConvertContext context, object input)
        {
            var result = context.ChangeType<ulong>(input);
            if (!result.Success)
            {
                context.Exception = context.InvalidCastException(input, TypeFriendlyName) + result.Error;
                return default;
            }
            return new UIntPtr(result.OutputValue);
        }
    }
}
