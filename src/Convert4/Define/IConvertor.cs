using System;

namespace blqw
{
    /// <summary>
    /// 转换器接口
    /// </summary>
    public interface IConvertor
    {
        /// <summary>
        /// 转换器的输出类型
        /// </summary>
        Type OutputType { get; }

        /// <summary>
        /// 优先级
        /// </summary>
        uint Priority { get; }

        /// <summary>
        /// 返回指定类型的对象，其值等效于指定对象。
        /// </summary>
        /// <param name="context"> 上下文 </param>
        /// <param name="input"> 需要转换类型的对象 </param>
        ConvertResult ChangeType(ConvertContext context, object input);

        /// <summary>
        /// 获取子转换器
        /// </summary>
        /// <param name="outputType"></param>
        /// <returns></returns>
        IConvertor GetConvertor(Type outputType);
    }
}