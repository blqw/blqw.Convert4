using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    /// <summary>
    /// 泛型代理转换器
    /// </summary>
    internal sealed class GenericProxyConvertor<T> : BaseConvertor<T>, IConvertor
    {
        private IConvertor _convertor;

        public GenericProxyConvertor(IConvertor convertor)
            => _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));

        public override ConvertResult<T> ChangeType(ConvertContext context, object input)
            => _convertor.ChangeType(context, input);

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input)
            => _convertor.ChangeType(context, input);

        public override IConvertor GetConvertor(Type outputType)
            => _convertor.GetConvertor(outputType);
    }
}
