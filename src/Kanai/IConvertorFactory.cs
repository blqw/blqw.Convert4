using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary>
    /// 转换器创建接口
    /// </summary>
    public interface IConvertorFactory
    {
        /// <summary>
        /// 编译转换器
        /// </summary>
        IConvertor Build(Type type);
        /// <summary>
        /// 判断是否可以构造指定类型的转换器
        /// </summary>
        bool CanBuild(Type type);
    }
}
