using System;

namespace blqw.Kanai.Interface
{
    /// <summary>
    /// 翻译器
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// 是否可翻译
        /// </summary>
        bool CanTranslate(Type type);

        /// <summary>
        /// 翻译
        /// </summary>
        object Translate(ConvertContext context, object input);
    }
}
