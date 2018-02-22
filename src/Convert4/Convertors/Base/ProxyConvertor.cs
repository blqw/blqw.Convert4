using System;

namespace blqw.Convertors
{
    /// <summary>
    /// 代理转换器
    /// </summary>
    internal sealed class ProxyConvertor<T> : BaseConvertor<T>, IConvertor
    {
        public ProxyConvertor(IConvertor convertor) =>
            _innerConvertor = convertor ?? (FailConvertor<T>)new EntryPointNotFoundException(SR.GetString($"转换器未找到"));

        public override ConvertResult<T> ChangeType(ConvertContext context, object input) =>
            _innerConvertor.ChangeType(context, input);

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input) =>
            _innerConvertor.ChangeType(context, input);

        public override IConvertor GetConvertor(Type outputType) => _innerConvertor.GetConvertor(outputType);

        public override uint Priority => _innerConvertor.Priority;

        private IConvertor _innerConvertor;

        public override IConvertor<T1> GetConvertor<T1>() => (IConvertor<T1>)_innerConvertor.GetConvertor(typeof(T1));
    }
}
