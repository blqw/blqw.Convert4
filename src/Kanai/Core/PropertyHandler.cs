using blqw.Kanai;
using blqw.Kanai.Core;
using blqw.Kanai.Extensions;
using System;
using System.Reflection;

namespace blqw
{
    /// <summary>
    /// 用于操作属性的Get和Set
    /// </summary>
    internal class PropertyHandler
    {
        /// <summary>
        /// 初始化 <see cref="PropertyHandler"/>
        /// </summary>
        /// <param name="property">属性</param>
        private PropertyHandler(PropertyInfo property)
        {
            Property = property;
            PropertyType = property.PropertyType;
            Name = property.Name;
            Get = property.GetValue;
            Set = property.SetValue;
        }

        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static PropertyHandler Create(PropertyInfo property) => new PropertyHandler(property);

        /// <summary>
        /// 属性类型
        /// </summary>
        public Type PropertyType { get; }
        /// <summary>
        /// 属性的Get方法委托
        /// </summary>
        public Func<object, object> Get { get; }
        /// <summary>
        /// 属性的Set方法委托
        /// </summary>
        public Action<object, object> Set { get; }
        /// <summary>
        /// 属性
        /// </summary>
        public PropertyInfo Property { get; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="settings">转换设置</param>
        /// <param name="target">属性实例对象</param>
        /// <param name="value">属性值</param>
        /// <returns></returns>
        public Exception SetValue(ConvertSettings settings, object target, object value)
        {
            var context = new ConvertContext(PropertyType, settings);
            var result = Cube.Convert(value, PropertyType, context);
            if (result.Success == false)
            {
                var message = string.Format(context.ResourceStrings.PROPERTY_CAST_FAIL, Property.DeclaringType.GetFriendlyName(), Property.Name);
                return context.Fail(message, result.Exception);
            }
            try
            {
                Set(target, result.OutputValue);
                return null;
            }
            catch (Exception ex)
            {
                if (ex is ConvertException exception)
                {
                    return exception;
                }

                var rs = context.ResourceStrings ?? ResourceStringManager.ZH_CN;
                ExceptionCollection exceptions = null;
                exceptions += ex;
                return new ConvertException(string.Format(rs.PROPERTY_SET_FAIL, Property.DeclaringType.GetFriendlyName(), Property.Name), exceptions);
            }
        }
    }
}
