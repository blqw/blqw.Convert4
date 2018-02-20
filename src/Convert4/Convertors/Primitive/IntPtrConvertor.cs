using System;
using System.Collections.Generic;
using System.Text;

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
            var result = context.ChangeType<long>(input);
            if (!result.Success)
            {
                context.Exception = context.InvalidCastException(input, TypeFriendlyName) + result.Error;
                return default;
            }
            return new IntPtr(result.OutputValue);
        }
    }
}
