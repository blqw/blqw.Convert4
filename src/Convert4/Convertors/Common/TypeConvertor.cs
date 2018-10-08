using System;
using System.Collections.Specialized;

namespace blqw.Convertors
{

    /// <summary>
    /// <seealso cref="Type" /> 转换器
    /// </summary>
    class TypeConvertor : BaseConvertor<Type>, IFrom<string, Type>, IFrom<object, Type>
    {
        /// <summary>
        /// 系统关键字映射
        /// </summary>
        private static readonly StringDictionary _keywords = new StringDictionary
        {
            ["bool"] = "System.Boolean",
            ["byte"] = "System.Byte",
            ["char"] = "System.Char",
            ["decimal"] = "System.Decimal",
            ["double"] = "System.Double",
            ["short"] = "System.Int16",
            ["int"] = "System.Int32",
            ["long"] = "System.Int64",
            ["sbyte"] = "System.SByte",
            ["float"] = "System.Single",
            ["string"] = "System.String",
            ["object"] = "System.Object",
            ["ushort"] = "System.UInt16",
            ["uint"] = "System.UInt32",
            ["ulong"] = "System.UInt64",
        };

        public Type From(ConvertContext context, string input)
        {
            if (input == null)
            {
                return null;
            }
            var result = Type.GetType(_keywords[input] ?? input, false, false) ??
                         Type.GetType("System." + input, false, false);

            if (result == null)
            {
                context.Error.AddException(new TypeLoadException($"{input:!} {"并不是一个类型"}"));
            }
            return result;
        }

        public Type From(ConvertContext context, object input) => input?.GetType();
    }
}
