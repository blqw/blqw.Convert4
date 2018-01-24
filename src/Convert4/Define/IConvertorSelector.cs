using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary>
    /// 转换器选择器
    /// </summary>
    public interface IConvertorSelector
    {
        IConvertor<T> Get<T>(ConvertContext context);
        IConvertor Get(Type outputType, ConvertContext context);
    }
}
