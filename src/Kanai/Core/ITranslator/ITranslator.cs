using System;

namespace blqw.Kanai
{
    /// <summary>
    /// 翻译器
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// 输入类型
        /// </summary>
        Type InputType { get; }

        /// <summary>
        /// 翻译
        /// </summary>
        object Translate(object input);
    }
}
