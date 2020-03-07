namespace blqw.Kanai.Core
{
    public class ResourceStrings
    {
        /// <summary>
        /// `{0}` 无法转换为 {1}
        /// </summary>
        public string CANT_CONVERT { get; private set; } = "`{0}`({1}) 无法转换为 {2}";
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
        public string CONVERTOR_CAST_FAIL { get; private set; } = "转换失败: {1}; 转换器:{0}";

        /// <summary>
        /// 属性: {0}.{1} 转换失败
        /// </summary>
        public string PROPERTY_CAST_FAIL { get; private set; } = "属性: {0}.{1} 转换失败";


        /// <summary>
        /// 属性: {0}.{1} 设置失败, 值: {2}
        /// </summary>
        public string PROPERTY_SET_FAIL { get; private set; } = "属性: {0}.{1} 设置失败, 值: {2}";

        /// <summary>
        /// {0} 实例化失败
        /// </summary>
        public string INSTANTIATION_FAIL { get; private set; } = "{0} 实例化失败";

        /// <summary>
        /// 集合插值失败,{0}[{1}]={2}
        /// </summary>
        public string COLLECTION_ADD_FAIL { get; private set; } = "集合插值失败,{0}[{1}]={2}";

        /// <summary>
        /// 属性: {0}.{1} 超过限制
        /// </summary>
        public string VALUE_OVERFLOW { get; private set; } = "对于{0}类型, 值 {1} 超过限制";
    }
}
