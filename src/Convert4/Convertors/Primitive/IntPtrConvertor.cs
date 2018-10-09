using System;
using blqw.ConvertServices;

namespace blqw.Convertors
{
    public class IntPtrConvertor : BaseConvertor<IntPtr>
                                 , IFrom<int, IntPtr>
                                 , IFrom<long, IntPtr>
                                 , IFrom<object, IntPtr>
    {
        public IntPtr From(ConvertContext context, long input) => new IntPtr(input);
        public IntPtr From(ConvertContext context, int input) => new IntPtr(input);
        public IntPtr From(ConvertContext context, object input)
        {
            var result = context.Convert<long>(input);
            if (!result.Success)
            {
                context.Error.AddError(result.Error);
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return new IntPtr(result.OutputValue);
        }
    }
}
