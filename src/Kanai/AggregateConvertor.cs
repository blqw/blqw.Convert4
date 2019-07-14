using blqw.ConvertServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    /// <summary>
    /// 聚合转换器
    /// </summary>
    public class AggregateConvertor : IConvertor
    {
        public Type OutputType => throw new NotImplementedException();

        public uint Priority => throw new NotImplementedException();

        public ConvertResult ChangeType(ConvertContext context, object input) => throw new NotImplementedException();
    }
}
