using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public static class Convert4
    {
        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input, T defaultValue)
        {
            using (var context = new ConvertContext())
            {
                var result = context.ChangeType<T>(context, input);
                return result.Success ? result.OutputValue : defaultValue;
            }
        }

        /// <summary>
        /// 返回一个指定类型的对象，该对象的值等效于指定的对象。转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"> 要返回的对象类型的泛型 </typeparam>
        /// <param name="input"> 需要转换类型的对象 </param>
        /// <param name="defaultValue"> 转换失败时返回的默认值 </param>
        public static T To<T>(this object input)
        {
            using (var context = new ConvertContext())
            {
                var result = context.ChangeType<T>(context, input);
                result.ThrowIfExceptional();
                return result.OutputValue;
            }
        }
    }
}
