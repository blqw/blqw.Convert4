using System;
using blqw.ConvertServices;

namespace blqw.Convertors
{
    class UIntPtrConvertor : BaseConvertor<UIntPtr>
                                 , IFrom<uint, UIntPtr>
                                 , IFrom<ulong, UIntPtr>
                                 , IFrom<object, UIntPtr>
    {
        public ConvertResult<UIntPtr> From(ConvertContext context, ulong input) => new UIntPtr(input);
        public ConvertResult<UIntPtr> From(ConvertContext context, uint input) => new UIntPtr(input);
        public ConvertResult<UIntPtr> From(ConvertContext context, object input)
        {
            var result = context.Convert<ulong>(input);
            if (!result.Success)
            {
                return result.Exception;
            }
            return new UIntPtr(result.OutputValue);
        }
    }
}
