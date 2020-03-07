using blqw.Kanai.Interface.From;
using System;

namespace blqw.Kanai.Convertors
{
    public class IntPtrConvertor : BaseConvertor<IntPtr>
                                 , IFrom<int, IntPtr>
                                 , IFrom<long, IntPtr>
                                 , IFrom<object, IntPtr>
    {
        public IntPtrConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<IntPtr> From(ConvertContext context, long input) => new IntPtr(input);
        public ConvertResult<IntPtr> From(ConvertContext context, int input) => new IntPtr(input);
        public ConvertResult<IntPtr> From(ConvertContext context, object input)
        {
            var result = context.Convert<long>(input);
            if (!result.Success)
            {
                return result.Exception;
            }
            return new IntPtr(result.OutputValue);
        }
    }
}
