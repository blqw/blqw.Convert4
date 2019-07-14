using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Kanai.Convertors
{
    internal sealed class WarpperConvertor<T> : Convertor, IConvertor<T>
    {
        private readonly IConvertor<T> _convertor;

        public uint Priority => _convertor.Priority;

        ConvertResult<T> IConvertor<T>.ChangeType(ConvertContext context, object input) => _convertor.ChangeType(context, input);

        protected override ConvertResult<object> ChangeType(ConvertContext context, object input)
        {
            var result = ChangeType(context, input);
            return new ConvertResult<object>(result.Success, result.OutputValue, result.Exception);
        }
    }
}
