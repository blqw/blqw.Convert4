using blqw.ConvertServices;
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
        /// <param name="context">转换上下文</param>
        /// <param name="target">属性实例对象</param>
        /// <param name="value">属性值</param>
        /// <returns></returns>
        public bool SetValue(ConvertContext context, object target, object value)
        {
            var result = context.ChangeType(PropertyType, value);
            if (result.Success == false)
            {
                context.InvalidCastException($"{"属性"}{Property.Name:!}{"值"}{"转换失败"}");
                return false;
            }
            try
            {
                Set(target, result.OutputValue);
                return true;
            }
            catch (Exception ex)
            {
                context.Error.AddException(ex);
                return false;
            }
        }
    }
}