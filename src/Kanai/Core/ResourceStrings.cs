namespace blqw.Kanai.Core
{
    public class ResourceStrings
    {
        /// <summary>
        /// `{0}` 无法转换为 {1}
        /// </summary>
        public string CANT_CONVERT { get; private set; } = "`{0}` 无法转换为 {1}";
        /// <summary>
        /// 值:`{0}`({1}) 无法转换为 {2}
        /// </summary>
        public string VALUE_CANT_CONVERT { get; private set; } = "值:`{0}`({1}) 无法转换为 {2}";
        /// <summary>
        /// 无法生成 {0} 类型的转换器: 泛型定义类型
        /// </summary>
        public string CANT_BUILD_CONVERTOR_BECAUSE_GENERIC_DEFINITION_TYPE { get; private set; } = "无法生成 {0} 类型的转换器: 泛型定义类型";
        /// <summary>
        /// 无法生成 {0} 类型的转换器: 静态类型
        /// </summary>
        public string CANT_BUILD_CONVERTOR_BECAUSE_STATIC_TYPE { get; private set; } = "无法生成 {0} 类型的转换器: 静态类型";
        /// <summary>
        /// 无法生成 {0} 类型的转换器: 未找到合适的转换器
        /// </summary>
        public string CANT_BUILD_CONVERTOR_BECAUSE_NOTFOUND { get; private set; } = "无法生成 {0} 类型的转换器: 未找到合适的转换器";
        /// <summary>
        /// 转换器{0} 转换失败: {1}
        /// </summary>
        public string CONVERTOR_FAIL { get; private set; } = "转换器{0} 转换失败: {1}";
        /// <summary>
        /// 对于{0}类型, 值 {1} 超过限制
        /// </summary>
        public string VALUE_OVERFLOW { get; private set; } = "对于{0}类型, 值 {1} 超过限制";
    }
}
