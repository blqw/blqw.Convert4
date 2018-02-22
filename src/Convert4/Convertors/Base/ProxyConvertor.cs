using System;

namespace blqw.Convertors
{
    /// <summary>
    /// 代理转换器
    /// </summary>
    internal sealed class ProxyConvertor<T> : BaseConvertor<T>, IConvertor
    {
        public ProxyConvertor(IConvertor convertor) =>
            InnerConvertor = convertor ?? (FailConvertor<T>)new EntryPointNotFoundException(SR.GetString($"转换器未找到"));

        public override ConvertResult<T> ChangeType(ConvertContext context, object input) =>
            InnerConvertor.ChangeType(context, input);

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input) =>
            InnerConvertor.ChangeType(context, input);

        public override IConvertor GetConvertor(Type outputType) => InnerConvertor.GetConvertor(outputType);

        public override uint Priority => InnerConvertor.Priority;

        protected IConvertor InnerConvertor { get; }

        public override IConvertor<T1> GetConvertor<T1>() => (IConvertor<T1>)InnerConvertor.GetConvertor(typeof(T1));
    }
}
