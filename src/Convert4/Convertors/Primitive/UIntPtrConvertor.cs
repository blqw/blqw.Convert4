using System;
using blqw.ConvertServices;

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
            var result = context.Convert<ulong>(input);
            if (!result.Success)
            {
                context.Error.AddError(result.Error);
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return new UIntPtr(result.OutputValue);
        }
    }
}
