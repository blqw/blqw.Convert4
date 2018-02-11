using System;

namespace blqw.Convertors
{
    /// <summary>
    /// 代理转换器
    /// </summary>
    internal sealed class ProxyConvertor<T> : BaseConvertor<T>, IConvertor
    {
        private IConvertor _convertor;

        public ProxyConvertor(IConvertor convertor)
            => _convertor = convertor ?? (FailConvertor<T>)new EntryPointNotFoundException(SR.GetString($"转换器未找到"));

        public override ConvertResult<T> ChangeType(ConvertContext context, object input)
            => _convertor.ChangeType(context, input);

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input)
            => _convertor.ChangeType(context, input);

        public override IConvertor GetConvertor(Type outputType)
            => _convertor.GetConvertor(outputType);

        public override uint Priority => _convertor.Priority;

        public override IConvertor<T1> GetConvertor<T1>() => (IConvertor<T1>)_convertor.GetConvertor(typeof(T1));


    }
}
