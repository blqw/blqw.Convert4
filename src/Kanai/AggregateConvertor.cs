using blqw.Kanai;
using System;

namespace blqw.Convertors
{
    /// <summary>
    /// 聚合转换器
    /// </summary>
    public class AggregateConvertor<T> : IConvertor<T>
    {
        public Type OutputType => throw new NotImplementedException();

        public uint Priority => throw new NotImplementedException();

        public ConvertResult<T> ChangeType(ConvertContext context, object input) => throw new NotImplementedException();
    }
}
