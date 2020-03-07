using blqw.Kanai.Interface.From;
using System;

namespace blqw.Kanai.Convertors
{
    class UIntPtrConvertor : BaseConvertor<UIntPtr>
                                 , IFrom<uint, UIntPtr>
                                 , IFrom<ulong, UIntPtr>
                                 , IFrom<object, UIntPtr>
    {
        public UIntPtrConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

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
