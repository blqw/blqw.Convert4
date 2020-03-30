using blqw.Kanai.Interface;
using System;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// 代理转换器
    /// </summary>
    /// <remarks>
    /// 转换器输出类型可兼容,但不可直接协变时, 可使用代理方式执行
    /// </remarks>
    public sealed class ProxyConvertor<TProxy, TOutput> : BaseConvertor<TOutput>
    {
        public ProxyConvertor(IServiceProvider serviceProvider, IConvertor<TProxy> convertor)
             : base(serviceProvider)
        {
            _innerConvertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
        }

        public override ConvertResult<TOutput> Convert(ConvertContext context, object input)
        {
            var result = _innerConvertor.Convert(context, input);
            if (!result.Success)
            {
                return result.Exception;
            }
            return new ConvertResult<TOutput>((TOutput)(object)result.OutputValue);
        }

        public override uint Priority => _innerConvertor.Priority;

        private IConvertor<TProxy> _innerConvertor;
    }
}
